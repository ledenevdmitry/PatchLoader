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

            objProject.Share(objOldItem, "", 0);
        }

        //Unpin + Checkout + Exception handling
        private void PrepareToPushFile(string vssFolder, string localFileName)
        {
            string fileName = $"{vssFolder}/{localFileName}";
            try
            {
                IVSSItem item = VSSDB.get_VSSItem(fileName, false);

                if (item.IsPinned)
                {
                    Unpin((VSSItem)item);
                }

                //!(file is checked out to the current user)
                if (!(item.IsCheckedOut == 2))
                {
                    item.Checkout();
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
        }

        private bool IsFileNotFoundError(System.Runtime.InteropServices.COMException exc)
        {
            if ((short)exc.ErrorCode == -10609 || (short)exc.ErrorCode == -10421)
                return true;
            return false;
        }

        //Unpin + Checkout (PrepareToPush) + Checkin + Pin or Add + Pin + + Exception handling
        public VSSItem PushFile(string vssFolder, string localFolder, string localFileName)
        {
            PrepareToPushFile(vssFolder, localFileName);

            string vssPath = $"{vssFolder}/{localFileName}";
            string localPath = Path.Combine(localFolder, localFileName);

            try
            {
                VSSItem item = VSSDB.get_VSSItem(vssPath, false);
                item.Checkin("", localPath);
                Pin(item, item.VersionNumber);
                return item;
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
                    VSSItem item = folder.Add(localPath);
                    Pin(item, item.VersionNumber);

                    return item;
                }
            }
        }

        public void PushDir(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, string remotePath, string linkPath)
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

            PushDirRec(localDir, patchFiles, remoteDir, linkDir);
        }

        public void PushDirRec(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, VSSItem remoteDir, VSSItem linkDir)
        {
            foreach (FileInfoWithPatchOptions fi in patchFiles)
            {
                //определяем, что мы находимся на нужном уровне
                if (fi.FileInfo.Directory.FullName.Equals(localDir.FullName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (fi.AddInRepDir)
                    {
                        VSSItem item = PushFile(remoteDir.Spec, localDir.FullName, fi.FileInfo.Name);
                        CreateLink(item, linkDir);
                    }
                    else if (fi.AddToPatch)
                    {
                        PushFile(linkDir.Spec, localDir.FullName, fi.FileInfo.Name);
                    }
                }
            }

            foreach (DirectoryInfo localSubDir in localDir.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
            {
                //проверяем только те папки, для который добавляются файлы в репозитории
                if (patchFiles.Where(x => x.AddInRepDir)
                    .Select(x => x.FileInfo.Directory)
                    .Where(x => x.FullName.StartsWith(localSubDir.FullName, StringComparison.InvariantCulture))
                    .Count() > 0)
                {
                    bool found = false;
                    VSSItem remoteSubDir = null;
                    VSSItem linkSubDir = null;

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
                        remoteSubDir = remoteDir.NewSubproject(localSubDir.Name);
                    }
                    linkSubDir = linkDir.NewSubproject(localSubDir.Name);

                    PushDirRec(localSubDir, patchFiles, remoteSubDir, linkSubDir);
                }
            }
        }

        public void CreateLink(IVSSItem sourceItem, IVSSItem destFolder)
        {
            destFolder.Share((VSSItem)sourceItem);
        }

        public void Close()
        {
            VSSDB.Close();
        }
    }
}
