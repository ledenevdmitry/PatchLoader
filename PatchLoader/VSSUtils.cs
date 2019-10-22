using Microsoft.VisualStudio.SourceSafe.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PatchLoader
{
    public class VSS
    {
        public VSSDatabase VSSDB { get; private set; }

        public string Location { get; set; }
        public string Login { get; set; }

        public VSS(string location, string login)
        {
            this.Location = location;
            this.Login = login;
            Connect();
        }

        public void Connect()
        {
            if (VSSDB == null)
            {
                try
                {

                    VSSDB = new VSSDatabase();
                    VSSDB.Open(Location, Login, "");
                }
                catch (System.Runtime.InteropServices.COMException exc)
                {
                    if (exc.ErrorCode == -2147167977)
                    {
                        throw new ArgumentException("Wrong location or login");
                    }
                    else
                        throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
                }
                catch
                {
                    throw new Exception("Неопознанная ошибка");
                }
            }
        }

        public void Move(string source, string destination, IEnumerable<string> items)
        {
            try
            {
                VSSDB.get_VSSItem(source, false);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }
            items = items.Select(x => string.Join("/", source, x));
            Move(destination, items);
        }

        private string SpecToCorrectPath(string spec)
        {
            return spec.Insert(1, "/");
        }


        public IEnumerable<string> AllInEntireBase(string root, List<string> matches, Regex pattern, int depth)
        {
            Queue<Tuple<VSSItem, int>> queue = new Queue<Tuple<VSSItem, int>>();
            queue.Enqueue(new Tuple<VSSItem, int>(VSSDB.get_VSSItem(root, false), 0));
            while (queue.Count > 0)
            {
                Tuple<VSSItem, int> currItem = queue.Dequeue();
                if (IsMatch(pattern, currItem.Item1))
                {
                    matches.Add(currItem.Item1.Name);
                    yield return SpecToCorrectPath(currItem.Item1.Spec);
                }

                if (currItem.Item2 < depth)
                {
                    foreach (VSSItem subItem in currItem.Item1.Items)
                    {
                        if ((VSSItemType)subItem.Type == VSSItemType.VSSITEM_PROJECT)
                        {
                            queue.Enqueue(new Tuple<VSSItem, int>(subItem, depth + 1));
                        }
                    }
                }
            }
            throw new ArgumentException("File Not Found");
        }

        public string FirstInEntireBase(string root, out string match, Regex pattern, int depth)
        {
            Queue<Tuple<VSSItem, int>> queue = new Queue<Tuple<VSSItem, int>>();
            queue.Enqueue(new Tuple<VSSItem, int>(VSSDB.get_VSSItem(root, false), 0));
            while (queue.Count > 0)
            {
                Tuple<VSSItem, int> currItem = queue.Dequeue();

                if (IsMatch(pattern, currItem.Item1))
                {
                    match = currItem.Item1.Name;
                    return SpecToCorrectPath(currItem.Item1.Spec);
                }

                if (currItem.Item2 < depth)
                {
                    foreach (VSSItem subItem in currItem.Item1.Items)
                    {
                        if ((VSSItemType)subItem.Type == VSSItemType.VSSITEM_PROJECT)
                        {
                            queue.Enqueue(new Tuple<VSSItem, int>(subItem, depth + 1));
                        }
                    }
                }
            }
            throw new ArgumentException("File Not Found");
        }

        private bool IsMatch(Regex pattern, VSSItem item)
        {
            return pattern.IsMatch(item.Name);
        }

        public delegate void MoveDelegate(string movingFolderName, VSSItem movingFolder);
        public event MoveDelegate AfterMove;
        static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

        public void Move(string destination, IEnumerable<string> items)
        {
            VSSItem destFolder;
            try
            {
                destFolder = VSSDB.get_VSSItem(destination, false);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }

            foreach (string item in items)
            {
                Thread th = new Thread(() =>
                {
                    try
                    {
                        rwl.EnterReadLock();
                        VSSItem movingFolder = VSSDB.get_VSSItem(item, false);
                        rwl.ExitReadLock();
                        movingFolder.Move(destFolder);
                        AfterMove(item, movingFolder);
                    }

                    catch (System.Runtime.InteropServices.COMException exc)
                    {
                        throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
                    }
                    catch
                    {
                        throw new Exception("Неопознанная ошибка");
                    }
                });
                th.Start();
            }
        }

        public void Rename(string oldName, string newName)
        {
            try
            {
                VSSItem folder = VSSDB.get_VSSItem(oldName, false);
                folder.Name = newName;
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
            catch
            {
                throw new Exception("Неопознанная ошибка");
            }
        }        

        private void DeleteLocalIfNotExistsInVSS(VSSItem remote, DirectoryInfo local)
        {
            foreach (var subFile in local.EnumerateFiles("*", SearchOption.TopDirectoryOnly))
            {
                bool found = false;
                foreach (VSSItem subitem in remote.Items)
                {
                    //1 - файл
                    if (subitem.Type == 1 && subitem.Name.Equals(subFile.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    if (!subFile.Name.Equals("vssver2.scc", StringComparison.InvariantCultureIgnoreCase))
                    {
                        subFile.Delete();
                    }
                }
            }

            foreach (VSSItem subitem in remote.Items)
            {
                //0 - папка
                if (subitem.Type == 0)
                {
                    DirectoryInfo subdestination = new DirectoryInfo(Path.Combine(local.FullName, subitem.Name));
                    DeleteLocalIfNotExistsInVSS(subitem, subdestination);
                }
            }
        }

        public void Pull(string vssPath, DirectoryInfo localPath)
        {
            try
            {
                VSSItem folder = VSSDB.get_VSSItem(vssPath, false);
                if (!localPath.Exists)
                {
                    localPath.Create();
                }
                folder.Get(localPath.FullName, (int)(VSSFlags.VSSFLAG_RECURSYES | VSSFlags.VSSFLAG_REPREPLACE));
                DeleteLocalIfNotExistsInVSS(folder, localPath);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                throw new ArgumentException(VSSErrors.GetMessageByCode(exc.ErrorCode));
            }
        }

        private void Unpin(VSSItem objItem)
        {
            int version = 0;
            VSSItem objOldItem = objItem.Version[version];
            VSSItem objProject = objItem.Parent;

            objProject.Share(objOldItem, "", (int)VSSFlags.VSSFLAG_GETNO);
        }

        private void Pin(VSSItem objItem, int version)
        {
            VSSItem objOldItem = objItem.Version[version];
            VSSItem objProject = objItem.Parent;

            objProject.Share(objOldItem, "", (int)VSSFlags.VSSFLAG_GETNO);
        }

        //Unpin + Checkout + Exception handling
        private bool PrepareToPushFile(string vssFolder, string localFolder, string localFileName)
        {
            string fileName = $"{vssFolder}/{localFileName}";
            try
            {
                IVSSItem item = VSSDB.get_VSSItem(fileName, false);

                if (item.IsPinned)
                {
                    Unpin((VSSItem)item);
                }

                //file is not checked out
                if (item.IsCheckedOut == 0)
                {
                    item.Checkout("", Path.Combine(localFolder, localFileName), (int)VSSFlags.VSSFLAG_GETNO);
                }
                //file is checked out to another user
                else if (item.IsCheckedOut == 1)
                {
                    return false;
                }
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                //file not found
                if (!IsFileNotFoundError(exc))
                {
                    throw exc;
                }
            }
            return true;
        }

        private bool IsFileNotFoundError(System.Runtime.InteropServices.COMException exc)
        {
            if ((short)exc.ErrorCode == -10609 || (short)exc.ErrorCode == -10421)
                return true;
            return false;
        }

        //Unpin + Checkout (PrepareToPush) + Checkin + Pin or Add + Pin + + Exception handling
        public bool PushFile(string vssFolder, string localFolder, string localFileName, out VSSItem item)
        {
            if(!PrepareToPushFile(vssFolder, localFolder, localFileName))
            {
                item = null;
                return false;
            }

            string vssPath = $"{vssFolder}/{localFileName}";
            string localPath = Path.Combine(localFolder, localFileName);

            try
            {
                item = VSSDB.get_VSSItem(vssPath, false);
                item.Checkin("", localPath);
                Pin(item, item.VersionNumber);
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                if (!IsFileNotFoundError(exc))
                {
                    throw exc;
                }
                else
                {
                    IVSSItem folder = VSSDB.get_VSSItem(vssFolder, false);
                    item = folder.Add(localPath);
                    Pin(item, item.VersionNumber);
                }
            }

            return true;
        }

        public bool PushDir(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, string remotePath, string linkPath, out List<string> vssPathCheckedOutToAnotherUser, string scriptsSubdir, string infaSubdir, List<string> repStructureInfa, List<string> repStructureScripts)
        {
            VSSItem remoteDir = VSSDB.get_VSSItem(remotePath);
            VSSItem linkRootDir = VSSDB.get_VSSItem(linkPath);

            foreach (VSSItem item in linkRootDir.Items)
            {
                if (item.Type == 0 && item.Name.Equals(localDir.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    item.Destroy();
                }
            }
            VSSItem linkDir = linkRootDir.NewSubproject(localDir.Name);

            vssPathCheckedOutToAnotherUser = new List<string>();

            PushDirRec(localDir, patchFiles, remoteDir, linkDir, vssPathCheckedOutToAnotherUser, scriptsSubdir, infaSubdir, repStructureInfa, repStructureScripts, 0);

            if(vssPathCheckedOutToAnotherUser.Count > 0)
            {
                return false;
            }

            return true;
        }

        private Dictionary<int, List<string>> PathesToLevelNamePair(List<string> pathes)
        {
            Dictionary<int, List<string>> res = new Dictionary<int, List<string>>();
            foreach (string path in pathes)
            {
                string[] splitted = path.Split(new char[] { '/' });
                for (int i = 0; i < splitted.Length; ++i)
                {
                    if(res[i] == null)
                    {
                        res[i] = new List<string>();
                    }
                    res[i].Add(splitted[i]);
                }
            }
            return res;
        }

        public void PushDirRec(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, VSSItem remoteDir, VSSItem linkDir, List<string> vssPathCheckedOutToAnotherUser, string scriptsSubdir, string infaSubdir, List<string> repStructureInfa, List<string> repStructureScripts, int level)
        {
            Dictionary<int, List<string>> scriptsDirs = PathesToLevelNamePair(repStructureScripts);
            Dictionary<int, List<string>> infaDirs = PathesToLevelNamePair(repStructureInfa);

            foreach (FileInfoWithPatchOptions fi in patchFiles)
            {
                //определяем, что мы находимся на нужном уровне
                if (fi.FileInfo.Directory.FullName.Equals(localDir.FullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (fi.AddInRepDir)
                    {
                        if (PushFile(remoteDir.Spec, localDir.FullName, fi.FileInfo.Name, out VSSItem item))
                        {
                            CreateLink(item, linkDir);
                        }
                        else
                        {
                            vssPathCheckedOutToAnotherUser.Add($"{remoteDir.Spec}/{fi.FileInfo.Name}");
                        }
                    }
                    else if (fi.AddToPatch)
                    {
                        if(!PushFile(linkDir.Spec, localDir.FullName, fi.FileInfo.Name, out VSSItem item))
                        {
                            vssPathCheckedOutToAnotherUser.Add($"{linkDir.Spec}/{fi.FileInfo.Name}");
                        }
                    }
                }
            }

            foreach (DirectoryInfo localSubDir in localDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                VSSItem remoteSubDir = null;
                VSSItem linkSubDir = null;

                //проверяем только те папки, для который добавляются файлы в репозитории
                //если там не было папки, добавляем ее
                if (patchFiles.Where(x => x.AddInRepDir)
                    .Select(x => x.FileInfo.Directory)
                    .Where(x => x.FullName.StartsWith(localSubDir.FullName, StringComparison.InvariantCulture))
                    .Count() > 0)
                {
                    bool found = false;

                    foreach (VSSItem currRemoteSubDir in remoteDir.Items)
                    {
                        if (currRemoteSubDir.Type == 0 && currRemoteSubDir.Name.Equals(localSubDir.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            found = true;
                            remoteSubDir = currRemoteSubDir;
                        }
                    }

                    if (!found)
                    {
                        //первой папкой должна быть папка создания скриптов или информатики
                        if (level == 0 &&
                            (localSubDir.Name.Equals(scriptsSubdir, StringComparison.InvariantCultureIgnoreCase) ||
                             localSubDir.Name.Equals(infaSubdir, StringComparison.InvariantCultureIgnoreCase))
                           ||
                           //папка после папки источника должна совпадать с доступными путями
                           level > 1 &&
                           (localDir.Parent.Parent.Name.Equals(scriptsSubdir, StringComparison.InvariantCultureIgnoreCase) &&
                            scriptsDirs[level - 2].Contains(localSubDir.Name, StringComparer.InvariantCultureIgnoreCase) ||
                            localDir.Parent.Parent.Name.Equals(infaSubdir, StringComparison.InvariantCultureIgnoreCase) &&
                            scriptsDirs[level - 2].Contains(localSubDir.Name, StringComparer.InvariantCultureIgnoreCase)))
                        {
                            remoteSubDir = remoteDir.NewSubproject(localSubDir.Name);
                        }
                    }

                }

                //проверяем только те папки, для которых добавляются файлы в патчи
                //создаем подпапку
                if (patchFiles.Where(x => x.AddToPatch)
                    .Select(x => x.FileInfo.Directory)
                    .Where(x => x.FullName.StartsWith(localSubDir.FullName, StringComparison.InvariantCulture))
                    .Count() > 0)
                {
                    linkSubDir = linkDir.NewSubproject(localSubDir.Name);
                }

                PushDirRec(localSubDir, patchFiles, remoteSubDir, linkSubDir, vssPathCheckedOutToAnotherUser);
            }
        }

        public void CreateLink(IVSSItem sourceItem, IVSSItem destFolder)
        {
            destFolder.Share((VSSItem)sourceItem, destFolder.Name, (int)VSSFlags.VSSFLAG_GETNO);
        }

        public void Close()
        {
            VSSDB.Close();
        }
    }
}
