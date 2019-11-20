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

        public delegate void MoveDelegate(string movingDirName, VSSItem movingDir);
        public event MoveDelegate AfterMove;
        static ReaderWriterLockSlim rwl = new ReaderWriterLockSlim();

        public void Move(string destination, IEnumerable<string> items)
        {
            VSSItem destDir;
            try
            {
                destDir = VSSDB.get_VSSItem(destination, false);
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
                        VSSItem movingDir = VSSDB.get_VSSItem(item, false);
                        rwl.ExitReadLock();
                        movingDir.Move(destDir);
                        AfterMove(item, movingDir);
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
                VSSItem dir = VSSDB.get_VSSItem(oldName, false);
                dir.Name = newName;
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
                VSSItem dir = VSSDB.get_VSSItem(vssPath, false);
                if (!localPath.Exists)
                {
                    localPath.Create();
                }
                dir.Get(localPath.FullName, (int)(VSSFlags.VSSFLAG_RECURSYES | VSSFlags.VSSFLAG_REPREPLACE));
                DeleteLocalIfNotExistsInVSS(dir, localPath);
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

        private bool IsCheckedOutByAnotherUser(string vssDir, string localDir, string localFileName)
        {
            string fileName = $"{vssDir}/{localFileName}";
            try
            {
                IVSSItem item = VSSDB.get_VSSItem(fileName, false);

                if (item.IsCheckedOut == 1)
                {
                    return true;
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
            return false;
        }

        //Unpin + Checkout + Exception handling
        private bool PrepareToPushFile(string vssDir, string localDir, string localFileName)
        {
            string fileName = $"{vssDir}/{localFileName}";
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
                    item.Checkout("", Path.Combine(localDir, localFileName), (int)VSSFlags.VSSFLAG_GETNO);
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
        public bool PushFile(string vssDir, string localDir, string localFileName, out VSSItem item)
        {
            if(!PrepareToPushFile(vssDir, localDir, localFileName))
            {
                item = null;
                return false;
            }

            string vssPath = $"{vssDir}/{localFileName}";
            string localPath = Path.Combine(localDir, localFileName);

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
                    IVSSItem dir = VSSDB.get_VSSItem(vssDir, false);
                    item = dir.Add(localPath);
                    Pin(item, item.VersionNumber);
                }
            }

            return true;
        }

        public bool PushDir(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, string remotePath, string linkPath, out List<string> vssPathCheckedOutToAnotherUser)
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

            PushDirRec(localDir, patchFiles, remoteDir, linkDir, vssPathCheckedOutToAnotherUser);

            if (vssPathCheckedOutToAnotherUser.Count > 0)
            {
                return false;
            }

            return true;
        }

        public string CheckDir(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, string remotePath)
        {
            VSSItem remoteDir = VSSDB.get_VSSItem(remotePath);
            string res = "";

            CheckDirRec(localDir, patchFiles, remoteDir, ref res);

            return res;
        }

        public void CreateStructure(string dirName, string remoteRoot, string subdir, List<string> repStructure)
        {
            VSSItem repDir = VSSDB.get_VSSItem(remoteRoot);
            VSSItem repSubdir = repDir.Child[subdir];

            bool found = false;
            foreach (VSSItem currRemoteSubDir in repSubdir.Items)
            {
                if (currRemoteSubDir.Type == 0 && currRemoteSubDir.Name.Equals(dirName, StringComparison.InvariantCultureIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            VSSItem newDir;
            if (found)
            {
                newDir = repSubdir.Child[dirName];
            }
            else
            {
                newDir = repSubdir.NewSubproject(dirName);
            }

            foreach(string path in repStructure)
            {
                string[] dirs = path.Split('/');

                VSSItem currDir = newDir;

                foreach (string dir in dirs)
                {
                    found = false;
                    foreach (VSSItem currSubDir in currDir.Items)
                    {
                        if (currSubDir.Type == 0 && currSubDir.Name.Equals(dir, StringComparison.InvariantCultureIgnoreCase))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        currDir = currDir.NewSubproject(dir);
                    }
                }
            }

        }

        private void CheckDirRec(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, VSSItem remoteDir, ref string res)
        {
            foreach (FileInfoWithPatchOptions fi in patchFiles)
            {
                //определяем, что мы находимся на нужном уровне
                if (fi.FileInfo.Directory.FullName.Equals(localDir.FullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (fi.AddInRepDir)
                    {
                        if (remoteDir != null && IsCheckedOutByAnotherUser(remoteDir.Spec, localDir.FullName, fi.FileInfo.Name))
                        {
                            res += $"Файл checked out другим пользователем {remoteDir.Spec}/{fi.FileInfo.Name}" + Environment.NewLine;
                        }
                    }
                }
            }

            foreach (DirectoryInfo localSubDir in localDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                VSSItem remoteSubDir = null;

                bool found = false;
                foreach (VSSItem currSubDir in remoteDir.Items)
                {
                    if(currSubDir.Name.Equals(localSubDir.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        remoteSubDir = currSubDir;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    CheckDirRec(localSubDir, patchFiles, remoteSubDir, ref res);
                }
            }

        }

        public void PushDirRec
        (
            DirectoryInfo localDir, 
            List<FileInfoWithPatchOptions> patchFiles, 
            VSSItem remoteDir, 
            VSSItem linkDir,
            List<string> vssPathCheckedOutToAnotherUser
        )
        {
            foreach (FileInfoWithPatchOptions fi in patchFiles)
            {
                //определяем, что мы находимся на нужном уровне
                if (fi.FileInfo.Directory.FullName.Equals(localDir.FullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (fi.AddInRepDir)
                    {
                        if (remoteDir != null && PushFile(remoteDir.Spec, localDir.FullName, fi.FileInfo.Name, out VSSItem item))
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
                        if(linkDir != null && !PushFile(linkDir.Spec, localDir.FullName, fi.FileInfo.Name, out VSSItem item))
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
                    .Where(x => (x.FullName + "\\").StartsWith(localSubDir.FullName + "\\", StringComparison.InvariantCulture))
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

                    //все проверки адекватности пути должны быть перенесены на класс формы
                    if (!found)
                    {
                        remoteSubDir = remoteDir.NewSubproject(localSubDir.Name);
                    }

                }

                //проверяем только те папки, для которых добавляются файлы в патчи
                //создаем подпапку
                if (patchFiles.Where(x => x.AddToPatch)
                    .Select(x => x.FileInfo.Directory)
                    .Where(x => (x.FullName + "\\").StartsWith((localSubDir.FullName + "\\"), StringComparison.InvariantCulture))
                    .Count() > 0)
                {
                    linkSubDir = linkDir.NewSubproject(localSubDir.Name);
                }

                PushDirRec(localSubDir, patchFiles, remoteSubDir, linkSubDir, vssPathCheckedOutToAnotherUser);
            }
        }

        public void CreateLink(IVSSItem sourceItem, IVSSItem destDir)
        {
            destDir.Share((VSSItem)sourceItem, destDir.Name, (int)VSSFlags.VSSFLAG_GETNO);
        }

        public void Close()
        {
            VSSDB.Close();
        }
    }
}
