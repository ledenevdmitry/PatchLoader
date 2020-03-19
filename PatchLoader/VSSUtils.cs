using Microsoft.VisualStudio.SourceSafe.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatchLoader
{
    public class VSSUtils
    {
        public VSSDatabase VSSDB { get; private set; }

        public string Location { get; set; }
        public string Login { get; set; }
        private string basePath;
        private string SSExeFullName;

        public VSSUtils(string location, string login)
        {
            this.Location = location;
            this.Login = login;
            stopSearch = false;
            Connect();
            basePath = VSSDB.SrcSafeIni.ToUpper().Replace("SRCSAFE.INI", "");
            SSExeFullName = Path.Combine(Properties.Settings.Default.SSPath, "ss.exe");
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

        public bool stopSearch;

        public IEnumerable<string> AllInEntireBase(string root, string name, bool matchPart, int depth)
        {
            Queue<Tuple<VSSItem, int>> queue = new Queue<Tuple<VSSItem, int>>();
            queue.Enqueue(new Tuple<VSSItem, int>(VSSDB.get_VSSItem(root, false), 0));

            bool found = false;

            while (queue.Count > 0 && !stopSearch)
            {
                Tuple<VSSItem, int> currItem = queue.Dequeue();

                if (matchPart && currItem.Item1.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) != -1
                    ||
                    currItem.Item1.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    sender($"Найден файл {currItem.Item1.Spec} ...");
                    yield return currItem.Item1.Spec;
                    found = true;
                }

                if (currItem.Item2 < depth || depth == -1)
                {
                    if ((VSSItemType)currItem.Item1.Type == VSSItemType.VSSITEM_PROJECT)
                    {
                        sender($"Запланирован поиск в {currItem.Item1.Spec} ...");
                        foreach (VSSItem subItem in currItem.Item1.Items)
                        {
                            queue.Enqueue(new Tuple<VSSItem, int>(subItem, currItem.Item2 + 1));
                        }
                    }
                }
            }

            if (!found && !stopSearch)
            {
                throw new FileNotFoundException("File Not Found");
            }
        }

        public bool FirstInEntireBase(string root, string name, int depth, bool matchPart, out string match)
        {
            Queue<Tuple<VSSItem, int>> queue = new Queue<Tuple<VSSItem, int>>();
            queue.Enqueue(new Tuple<VSSItem, int>(VSSDB.get_VSSItem(root, false), 0));
            while (queue.Count > 0 && !stopSearch)
            {
                Tuple<VSSItem, int> currItem = queue.Dequeue();

                if (matchPart && currItem.Item1.Name.IndexOf(name, StringComparison.InvariantCultureIgnoreCase) != -1
                    ||
                    currItem.Item1.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    sender($"Найден файл {currItem.Item1.Spec} ...");
                    match = currItem.Item1.Spec;
                    return true;
                }

                if (currItem.Item2 < depth || depth == -1)
                {
                    if ((VSSItemType)currItem.Item1.Type == VSSItemType.VSSITEM_PROJECT)
                    {
                        sender($"Запланирован поиск в {currItem.Item1.Spec} ...");
                        foreach (VSSItem subItem in currItem.Item1.Items)
                        {
                            queue.Enqueue(new Tuple<VSSItem, int>(subItem, currItem.Item2 + 1));
                        }
                    }
                }
            }

            match = null;
            return false;
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

        public List<string> GetSubdirs(string dirPath)
        {
            List<string> subdirs = new List<string>();
            VSSItem dir = VSSDB.get_VSSItem(dirPath, false);
            foreach(VSSItem subItem in dir.Items)
            {
                if((VSSItemType)subItem.Type == VSSItemType.VSSITEM_PROJECT)
                {
                    subdirs.Add(subItem.Spec);
                }
            }
            return subdirs;
        }

        public void Pull(string vssPath, DirectoryInfo localPath)
        {
            try
            {
                VSSItem dir = VSSDB.get_VSSItem(vssPath, false);
                sender($"Папка {vssPath} найдена");
                if (!localPath.Exists)
                {
                    localPath.Create();
                    sender($"Папка {vssPath} найдена");
                }
                dir.Get(localPath.FullName, (int)(VSSFlags.VSSFLAG_RECURSYES/* | VSSFlags.VSSFLAG_REPREPLACE*/));
                //DeleteLocalIfNotExistsInVSS(dir, localPath);
                sender("Папка загружена!");
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
                    sender($"{item.Spec} unpinned");
                }

                //file is not checked out
                if (item.IsCheckedOut == 0)
                {
                    item.Checkout("", Path.Combine(localDir, localFileName), (int)VSSFlags.VSSFLAG_GETNO);
                    sender($"{item.Spec} checked out");
                }
                //file is checked out to another user
                else if (item.IsCheckedOut == 1)
                {
                    sender($"{item.Spec} checked out by another user!");
                    return false;
                }
            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                //file not found
                if (!IsFileNotFoundError(exc))
                {
                    sender($"{exc.Message}");
                    throw exc;
                }
                else
                {
                    sender($"{fileName} не найден");
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

        public delegate void SendLog(string log);
        public static SendLog sender;

        //Unpin + Checkout (PrepareToPush) + Checkin + Pin or Add + Pin + + Exception handling
        public bool PushFile(string vssDir, string localDir, string localFileName, string patchName, out VSSItem item)
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
                if(item.IsCheckedOut == 0)
                {
                    sender($"{item.Spec} checked in");
                    try
                    {
                        Pin(item, item.VersionNumber);
                        sender($"{item.Spec} pinned");
                    }
                    catch
                    {
                        sender($"Cannot pin {item.Spec}");
                    }
                }
                else
                {
                    if(MessageBox.Show($"Файл {localFileName} не зачекинился. Требуется повторить попытку или выложить патч заново", "Предупреждение", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                    {
                        return PushFile(vssDir, localDir, localFileName, patchName, out item);
                    }
                    /*
                    Process pr = new Process();
                    pr.StartInfo.FileName = "cmd.exe";
                    pr.StartInfo.UseShellExecute = false;
                    pr.StartInfo.RedirectStandardOutput = true;
                    pr.StartInfo.RedirectStandardInput = true;
                    pr.StartInfo.CreateNoWindow = true;
                    pr.Start();

                    string drive = Path.GetPathRoot(localDir).Replace("\\", "");
                    pr.StandardInput.WriteLine(drive);
                    pr.StandardInput.WriteLine($"set SSDIR={basePath}");
                    pr.StandardInput.WriteLine($"\"{SSExeFullName}\" checkin \"{item.Spec}\"");
                    pr.StandardInput.WriteLine($"\"{SSExeFullName}\" pin \"{item.Spec}\" -V{item.VersionNumber}");

                    sender($"{item.Spec} checked in and pinned via cmd");
                    MessageBox.Show($"Костыль. Запинили с помощью консоли файл {item.Spec}. Требуется проверить вручную.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    */
                }

            }
            catch (System.Runtime.InteropServices.COMException exc)
            {
                if (!IsFileNotFoundError(exc))
                {
                    throw exc;
                }
                else
                {
                    sender($"{vssPath} не найден. Добавление нового файла...");
                    IVSSItem dir = VSSDB.get_VSSItem(vssDir, false);

                    item = dir.Add(localPath);
                    sender($"{item.Spec} добавлен");

                    try
                    {
                        Pin(item, item.VersionNumber);
                        sender($"{item.Spec} pinned");
                    }
                    catch
                    {
                        sender($"Cannot pin {item.Spec}");
                    }
                }
            }

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();

            p.StandardInput.WriteLine($"set SSDIR={basePath}");
            p.StandardInput.WriteLine($"\"{SSExeFullName}\" Comment \"{item.Spec}\"");
            p.StandardInput.WriteLine($"Patch {patchName}");

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
                    sender($"{item.Spec} найдена в {linkRootDir.Spec} и удалена!");
                    break;
                }
            }
            VSSItem linkDir = linkRootDir.NewSubproject(localDir.Name);
            sender($"{linkDir.Spec} создана в {linkRootDir.Spec}");

            vssPathCheckedOutToAnotherUser = new List<string>();

            PushDirRec(localDir, localDir, patchFiles, remoteDir, linkDir, vssPathCheckedOutToAnotherUser);

            if (vssPathCheckedOutToAnotherUser.Count > 0)
            {
                return false;
            }

            return true;
        }

        public static bool CheckPatchErrors(DirectoryInfo localDir, ref string errDesc)
        {
            bool res = true;
            foreach (DirectoryInfo dir in localDir.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                if (dir.Name.Equals("TABLE", StringComparison.InvariantCultureIgnoreCase))
                {
                    foreach (FileInfo file in dir.EnumerateFiles("*.sql", SearchOption.TopDirectoryOnly))
                    {
                        sender($"Проверка файла {file.FullName} на insert-ы");
                        using (StreamReader sr = new StreamReader(file.FullName, Encoding.GetEncoding("Windows-1251")))
                        {
                            string fileText = sr.ReadToEnd();
                            if (fileText.IndexOf("insert into", StringComparison.InvariantCultureIgnoreCase) != -1)
                            {
                                errDesc += $"В файле {file.FullName} в папке Table идет insert" + Environment.NewLine;
                                res = false;
                            }
                        }
                    }
                }
                foreach (FileInfo file in dir.EnumerateFiles("*.sql", SearchOption.TopDirectoryOnly))
                {
                    using (StreamReader sr = new StreamReader(file.FullName, Encoding.GetEncoding("Windows-1251")))
                    {
                        string fileText =
                            sr.ReadToEnd()
                            .Replace(Environment.NewLine, " ")
                            .Replace(".", " . ")
                            .Replace(",", " , ")
                            .Replace(";", " ; ")
                            .Replace(")", " ) ")
                            .Replace("(", " ( ")
                            .Replace("'", " ' ")
                            .Replace(@"""", @" "" ")
                            .Replace(":", " : ")
                            .Replace("=", " = ")
                            .Replace("+", " + ")
                            .Replace("-", " - ")
                            .Replace("*", " * ")
                            .Replace("/", " / ")
                            .Replace(">", " > ")
                            .Replace("<", " < ")
                            .Replace("|", " | ");

                        string[] words = Regex.Split(fileText, "\\s+");

                        if (dir.Name.Equals("OP_USER@BI2DB5", StringComparison.InvariantCultureIgnoreCase))
                        {
                            for (int i = 0; i < words.Length; ++i)
                            {
                                if (words[i].Equals("CREATE", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    sender($"Проверка {file.FullName} CREATE на TABLESPACE TRASH...");
                                    int j = i + 1;
                                    while (!words[j].Equals(";", StringComparison.InvariantCultureIgnoreCase) &&
                                            !words[j].Equals("AS", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        ++j;
                                    }
                                    //до j должен быть задан TS

                                    bool TSFound = false;
                                    bool CorrectTSOpUser = true;
                                    bool schemaFound = false;

                                    for (int k = i; k < j; ++k)
                                    {
                                        //если TS нашли, то true
                                        if (words[k].Equals("TABLESPACE", StringComparison.InvariantCultureIgnoreCase))

                                        {
                                            TSFound = true;
                                            for (int m = i; m < k; ++m)
                                            {
                                                //если схема OP_USER, но TS не TRASH, то ошибка
                                                if (words[m].Equals("OP_USER", StringComparison.InvariantCultureIgnoreCase) &&
                                                   words[m + 1].Equals(".", StringComparison.InvariantCultureIgnoreCase) &&
                                                  !words[k + 1].Equals("TRASH", StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    CorrectTSOpUser = false;
                                                }
                                                if (words[m].Equals(".", StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    schemaFound = true;
                                                }
                                            }

                                            if (!schemaFound && !words[k + 1].Equals("TRASH", StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                CorrectTSOpUser = false;
                                            }

                                            break;
                                        }
                                    }

                                    if (!TSFound)
                                    {
                                        errDesc += $"В файле {file.FullName} в схеме для CREATE не найден TABLESPACE" + Environment.NewLine;
                                        res = false;
                                    }

                                    if (!CorrectTSOpUser)
                                    {
                                        errDesc += $"В файле {file.FullName} в схеме OP_USER для CREATE не найден TABLESPACE TRASH" + Environment.NewLine;
                                        res = false;
                                    }

                                    sender($"Проверка {file.FullName} CREATE на TABLESPACE TRASH завершена");
                                }
                            }
                        }

                        sender($"Проверка файла {file.FullName} неявное преобразование");
                        for (int i = 0; i < words.Length; ++i)
                        {
                            if (i >= 3 &&
                               (words[i].IndexOf("1900") != -1 || words[i].IndexOf("5999") != -1) &&
                               !words[i].StartsWith("P", StringComparison.InvariantCultureIgnoreCase) //не партиции
                               )
                            {
                                int k = i;

                                while (!words[k].Equals("'"))
                                {
                                    k--;
                                }

                                while (words[k].Equals("'"))
                                {
                                    k--;
                                }

                                if (k >= 1 && !(words[k].Equals("(") && words[k - 1].Equals("TO_DATE", StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    errDesc += $"Возможно неявное преобразование в файле {file.FullName}" + Environment.NewLine;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return res;
        }


        public string CheckDir(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, string remotePath)
        {
            VSSItem remoteDir = VSSDB.get_VSSItem(remotePath);
            string res = "";

            CheckDirRec(localDir, patchFiles, remoteDir, ref res);
            CheckPatchErrors(localDir, ref res);

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
                    sender($"{currRemoteSubDir.Spec} найдена в {repSubdir.Spec}");
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
                sender($"{newDir.Spec} создана в {repSubdir.Spec}");
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
                            sender($"{currSubDir.Spec} найдена в {currDir.Spec}");
                            break;
                        }
                    }

                    if (!found)
                    {
                        currDir = currDir.NewSubproject(dir);
                        sender($"{currDir.Spec} создана в {currDir.Spec}");
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
            DirectoryInfo patchDir,
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
                        sender($"{fi.FileInfo.Name} добавляется в {remoteDir.Spec}...");
                        if (/*remoteDir != null && */PushFile(remoteDir.Spec, localDir.FullName, fi.FileInfo.Name, patchDir.Name, out VSSItem item))
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
                        sender($"{fi.FileInfo.Name} добавляется в {linkDir.Spec}...");
                        if (/*linkDir != null && */!PushFile(linkDir.Spec, localDir.FullName, fi.FileInfo.Name, patchDir.Name, out VSSItem item))
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
                    .Where(x => (x.FullName + "\\").StartsWith(localSubDir.FullName + "\\", StringComparison.InvariantCultureIgnoreCase))
                    .Count() > 0)
                {
                    bool found = false;

                    foreach (VSSItem currRemoteSubDir in remoteDir.Items)
                    {
                        if (currRemoteSubDir.Type == 0 && currRemoteSubDir.Name.Equals(localSubDir.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            found = true;
                            sender($"{currRemoteSubDir.Spec} найдена в {remoteDir.Spec}");
                            remoteSubDir = currRemoteSubDir;
                        }
                    }

                    //все проверки адекватности пути должны быть перенесены на класс формы
                    if (!found)
                    {
                        remoteSubDir = remoteDir.NewSubproject(localSubDir.Name);
                        sender($"{remoteSubDir.Spec} создана в {remoteDir.Spec}");
                    }

                }

                //проверяем только те папки, для которых добавляются файлы в патчи
                //создаем подпапку
                if (patchFiles.Where(x => x.AddToPatch)
                    .Select(x => x.FileInfo.Directory)
                    .Where(x => (x.FullName + "\\").StartsWith((localSubDir.FullName + "\\"), StringComparison.InvariantCultureIgnoreCase))
                    .Count() > 0)
                {
                    linkSubDir = linkDir.NewSubproject(localSubDir.Name);
                    sender($"{linkSubDir.Spec} создана в {linkDir.Spec}");
                }

                PushDirRec(localSubDir, patchDir, patchFiles, remoteSubDir, linkSubDir, vssPathCheckedOutToAnotherUser);
            }
        }

        private void CreateLink(IVSSItem sourceItem, IVSSItem destDir)
        {
            destDir.Share((VSSItem)sourceItem, destDir.Name, (int)VSSFlags.VSSFLAG_GETNO);
            sender($"Создана ссылка для файла {sourceItem.Spec} в {destDir.Spec}");
        }

        public void Close()
        {
            VSSDB.Close();
        }

        private bool CompareItems(IVSSItem repItem, IVSSItem linkItem, string linkItemLocal, out IVSSItem lastItem)
        {
            int maxVersion = -1;
            int linkItemVersion = -1;
            lastItem = null;

            foreach (IVSSItem item in repItem.Links)
            {
                if(item.VersionNumber > maxVersion)
                {
                    maxVersion = item.VersionNumber;
                    lastItem = item;
                }

                if (item.Spec.Equals(linkItem.Spec, StringComparison.InvariantCultureIgnoreCase))
                {
                    linkItemVersion = item.VersionNumber;
                }
            }

            if (linkItemVersion == maxVersion)
            {
                return true;
            }

            if(!repItem.IsDifferent[linkItemLocal])
            {
                sender($"Файлы {repItem.Spec} и {linkItem.Spec} идентичны");
                return true;
            }

            return false;
        }       

        public bool TestPatchDir(string remotePath, out string errDesc, DirectoryInfo localDir)
        {
            sender($"Проверка патча {remotePath}");
            IVSSItem patchStartDir = VSSDB.get_VSSItem(remotePath);
            sender($"Патч {remotePath} найден");

            string startRepPath = Properties.Settings.Default.RemoteRoot;
            sender($"Папка {Properties.Settings.Default.RemoteRoot} найдена");
            List<string> versionMismatches = new List<string>();

            bool res = true;
            errDesc = "";

            string scriptsSubdir = Properties.Settings.Default.ScriptsSubdir;
            string infaSubdir = Properties.Settings.Default.InfaSubdir;

            List<IVSSItem> startItemsScripts = new List<IVSSItem>();
            List<IVSSItem> startItemsInfa = new List<IVSSItem>();

            sender($"Находим папку {startRepPath}/{scriptsSubdir}");
            IVSSItem scriptsRepDir = VSSDB.get_VSSItem($"{startRepPath}/{scriptsSubdir}");

            sender($"Находим папку {startRepPath}/{infaSubdir}");
            IVSSItem infaRepDir = VSSDB.get_VSSItem($"{startRepPath}/{infaSubdir}");

            try
            {
                foreach (string itemSpec in AllInEntireBase(patchStartDir.Spec, scriptsSubdir, false, -1))
                {
                    sender($"Нашли папку со скриптами в патче {itemSpec}");
                    startItemsScripts.Add(VSSDB.get_VSSItem(itemSpec));
                }
            }
            catch (FileNotFoundException)
            { }

            try
            {
                foreach (string itemSpec in AllInEntireBase(patchStartDir.Spec, infaSubdir, false, -1))
                {
                    sender($"Нашли папку информатики в патче {itemSpec}");
                    startItemsInfa.Add(VSSDB.get_VSSItem(itemSpec));
                }
            }
            catch (FileNotFoundException)
            { }

            foreach (IVSSItem subpatchItem in startItemsScripts)
            {
                int patchIndex = subpatchItem.Spec.IndexOf(localDir.Name, StringComparison.InvariantCultureIgnoreCase) + localDir.Name.Length;
                string subPath = subpatchItem.Spec.Substring(patchIndex).Replace('/', '\\');

                string currLocalDir = localDir.FullName + subPath;

                sender($"Локальная папка {currLocalDir} (Путь: {localDir.FullName}, подпуть: {subPath})");

                if ((VSSItemType)subpatchItem.Type == VSSItemType.VSSITEM_PROJECT)
                {
                    TestPatchDirRec(subpatchItem, currLocalDir, scriptsRepDir, ref res, ref errDesc);
                }
            }

            foreach (IVSSItem subpatchItem in startItemsInfa)
            {
                int patchIndex = subpatchItem.Spec.IndexOf(localDir.Name, StringComparison.InvariantCultureIgnoreCase) + localDir.Name.Length;
                string subPath = subpatchItem.Spec.Substring(patchIndex).Replace('/', '\\');

                string currLocalDir = localDir.FullName + subPath;

                sender($"Локальная папка {currLocalDir} (Путь: {localDir.FullName}, подпуть: {subPath})");

                if ((VSSItemType)subpatchItem.Type == VSSItemType.VSSITEM_PROJECT)
                {
                    TestPatchDirRec(subpatchItem, currLocalDir, infaRepDir, ref res, ref errDesc);
                }
            }

            foreach (string file in versionMismatches)
            {
                errDesc += "Несоответствие версий в файле " + file + Environment.NewLine;
            }
            
            res &= CheckPatchErrors(localDir, ref errDesc);

            return res;
        }

        private void TestPatchDirRec(IVSSItem patchCurrDir, string localPath, IVSSItem repCurrDir, ref bool res, ref string errDesc)
        {
            foreach (IVSSItem linkItem in patchCurrDir.Items)
            {
                if ((VSSItemType)linkItem.Type == VSSItemType.VSSITEM_PROJECT)
                {
                    try
                    {
                        sender($"Проверка папки {linkItem.Spec}");
                        IVSSItem repNextDir = VSSDB.get_VSSItem($"{repCurrDir.Spec}/{linkItem.Name}");
                        string localNextDir = Path.Combine(localPath, linkItem.Name);
                        sender($"Папка {repNextDir.Spec} найдена");
                        TestPatchDirRec(linkItem, localNextDir, repNextDir, ref res, ref errDesc);
                    }
                    catch (System.Runtime.InteropServices.COMException exc)
                    {
                        if (!IsFileNotFoundError(exc))
                        {
                            throw exc;
                        }
                        else
                        {
                            sender($"Папка {repCurrDir.Spec}/{linkItem.Name} не найдена");
                        }
                    }
                }
                else
                {
                    if (linkItem.Name.StartsWith("SCRIPT_", StringComparison.InvariantCultureIgnoreCase) ||
                        Regex.IsMatch(linkItem.Name, "WF_.*_FIX.xml", RegexOptions.IgnoreCase))
                    {
                        sender($"Файл {linkItem.Spec} пропущен");
                    }
                    else
                    {
                        try
                        {
                            IVSSItem repItem = VSSDB.get_VSSItem($"{repCurrDir.Spec}/{linkItem.Name}");
                            sender($"Файл {repItem.Spec} найден");

                            string localItem = Path.Combine(localPath, linkItem.Name);
                            sender($"Сравнение версий. Локальный файл {localItem}");

                            if (!CompareItems(repItem, linkItem, localItem, out IVSSItem lastItem))
                            {
                                res = false;
                                sender($"Разные версии у файлов {linkItem.Spec} и {repItem.Spec}. Последняя версия: {lastItem.Spec}");
                                errDesc += $"Несоответствие версии для файла {linkItem.Spec};{linkItem.VersionNumber}. Последняя версия: {lastItem.Spec};{lastItem.VersionNumber}" + Environment.NewLine;
                            }
                            else
                            {
                                sender($"Одинаковые версии у файлов {linkItem.Spec} и {repItem.Spec}");
                            }
                        }
                        catch (System.Runtime.InteropServices.COMException exc)
                        {
                            if (!IsFileNotFoundError(exc))
                            {
                                throw exc;
                            }
                            else
                            {
                                errDesc += $"Файл {linkItem.Spec} не найден в репозитории" + Environment.NewLine;
                                sender($"Файл {repCurrDir.Spec}/{linkItem.Name} не найден в репозитории");
                            }
                        }
                    }
                }
            }
        }
    }
}
