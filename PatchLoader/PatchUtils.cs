using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatchLoader
{
    class PatchUtils
    {
        private readonly string remoteRoot;
        private readonly string remoteLinkRoot;
        private readonly string remoteBaseLocation;

        public PatchUtils(string remoteRoot, string remoteLinkRoot, string remoteBaseLocation)
        {
            this.remoteRoot = remoteRoot;
            this.remoteLinkRoot = remoteLinkRoot;
            this.remoteBaseLocation = remoteBaseLocation;
        }

        public bool PushPatch(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles, out List<string> vssPathCheckedOutToAnotherUser)
        {
            VSSUtils vss = new VSSUtils(remoteBaseLocation, Environment.UserName);
            return vss.PushDir(localDir, patchFiles, remoteRoot, remoteLinkRoot, out vssPathCheckedOutToAnotherUser);
        }

        public string CheckPatch(DirectoryInfo localDir, List<FileInfoWithPatchOptions> patchFiles)
        {
            VSSUtils vss = new VSSUtils(remoteBaseLocation, Environment.UserName);
            return vss.CheckDir(localDir, patchFiles, remoteRoot);

        }

        public void CreateStructure(string dirName, string subdir, List<string> repScripts)
        {
            VSSUtils vss = new VSSUtils(remoteBaseLocation, Environment.UserName);
            vss.CreateStructure(dirName, remoteRoot, subdir, repScripts);
        }


        public static bool IsAcceptableDir(DirectoryInfo dir, string scriptsOrInfaDir, string schema, DirectoryInfo patchDir, List<string> acceptableRemotePathes)
        {
            foreach(string acceptablePath in acceptableRemotePathes)
            {
                List<string> subpathes = new List<string>();

                string [] folders = acceptablePath.Split('/');
                string aggFolder = "";

                foreach(string folder in folders)
                {
                    aggFolder += folder;
                    subpathes.Add(aggFolder);

                    aggFolder += '/';
                }

                
                foreach(string subpath in subpathes)
                {
                    if (Path.Combine(patchDir.FullName, scriptsOrInfaDir, schema, subpath.Replace('/', '\\')).Equals(dir.FullName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public static bool CreateFPScenarioByFiles(DirectoryInfo patchDirectory)
        {
            SortedList<int, string> priorityLinePair = new SortedList<int, string>(new DuplicateKeyComparer<int>());

            foreach (FileInfo fileInfo in patchDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories))
            {
                string fromFPPath = fileInfo.FullName.Substring(patchDirectory.FullName.Length + 1);
                if (!WrongFiles.IsMatch(fromFPPath))
                {
                    if (CreateScenarioLineByFromFPDirPath(fromFPPath, out string scenarioLine))
                    {
                        priorityLinePair.Add(Priority(scenarioLine), scenarioLine);
                    }
                }
            }

            SaveFileSc(patchDirectory, priorityLinePair.Values);

            return true;
        }

        static Regex WrongFiles = new Regex(@"file_sc.*\.|RELEASE_NOTES\.|VSSVER2\.|\.xls|IVSS\.tmp|\\tablespace|\\user", RegexOptions.IgnoreCase);
        static Regex ORASchemaFromScenarioLine = new Regex(@"([^\\]+)@");

        //компаратор нужен для того, чтобы в сортированный список можно было добавлять пары с одинаковыми ключами
        public class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable
        {
            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return -1;   // обрабатываем равенство как "меньше", будет вставлять как в патче
                else
                    return result;
            }
        }

        public static int Priority(string line)
        {
            int priority = 0;
            //папки
            if (line.IndexOf("\\DWI", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 1;
            else if (line.IndexOf("\\SRC001_ODS", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 2;
            else if (line.IndexOf("\\DWS", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 3;
            else if (line.IndexOf("\\OP_USER", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 4;
            else if (line.IndexOf("\\STG", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 5;
            else if (line.IndexOf("\\DWH", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 6;
            else if (line.IndexOf("\\UM", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 7;
            else if (line.IndexOf("\\FLOW_CONTROL", StringComparison.InvariantCultureIgnoreCase) != -1)
                priority += 8;

            //скрипты
            if (line.IndexOf("||DB_SCRIPT", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                priority += 10000;

                //порядок в скриптах
                if (line.IndexOf("\\SEQUENCE", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 50;
                else if (line.IndexOf("\\TABLE", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 100;
                else if (line.IndexOf("\\INDEX", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 150;
                else if (line.IndexOf("\\VIEW", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 200;
                else if (line.IndexOf("\\FUNCTION", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 250;
                else if (line.IndexOf("\\PROCEDURE", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 300;
                else if (line.IndexOf("\\PACKAGE", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 350;
                else if (line.IndexOf("\\SCRIPT", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 400;

                //DBATOOLS в конец скриптов
                if (line.IndexOf("\\DBATOOLS", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    priority += 1000;
                }
            }

            //информатика
            else if (line.IndexOf("||INFA_XML", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                priority += 20000;

                //сначала shared
                if (line.IndexOf("\\SHARED", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 1000;
                else priority += 2000;

                //порядок в информатике 
                if (line.IndexOf("\\SOURCE", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 50;
                else if (line.IndexOf("\\TARGET", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 100;
                else if (line.IndexOf("\\USER-DEFINED FUNCTION", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 150;
                else if (line.IndexOf("\\EXP_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 200;
                else if (line.IndexOf("\\SEQ_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 250;
                else if (line.IndexOf("\\LKP_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 300;
                else if (line.IndexOf("\\MPL_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 350;
                else if (line.IndexOf("\\M_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 400;
                else if (line.IndexOf("\\CMD_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 450;
                else if (line.IndexOf("\\S_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 500;
                else if (line.IndexOf("\\WKLT_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 550;
                else if (line.IndexOf("\\WF_", StringComparison.InvariantCultureIgnoreCase) != -1)
                    priority += 600;
            }

            //старты потоков
            else if (line.IndexOf("||START_WF", StringComparison.InvariantCultureIgnoreCase) != -1)
            {
                priority += 30000;
            }

            return priority;
        }

        private static bool CreateScenarioLineByFromFPDirPath(string path, out string scenarioLine)
        {
            if (path.IndexOf(".sql", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                scenarioLine = $"ORA||{path}||{ORASchemaFromScenarioLine.Match(path).Groups[1].Value}";
                return true;
            }
            if (path.IndexOf(".xml", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                scenarioLine = $"IPC||{path}";
                return true;
            }
            if (path.IndexOf(".txt", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                scenarioLine = $"STWF||{path}";
                return true;
            }
            scenarioLine = null;
            return false;
        }

        public static void SaveFileSc(DirectoryInfo fixpackDir, IEnumerable<string> text)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "txt",
                Filter = "Текстовый файл|*.txt",
                FileName = "file_sc",
                InitialDirectory = fixpackDir.FullName
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Delete(sfd.FileName);
                using (File.Create(sfd.FileName)) { }
                using (StreamWriter tw = new StreamWriter(sfd.FileName, false, Encoding.GetEncoding("Windows-1251")))
                {
                    tw.WriteLine(fixpackDir.Name);
                    tw.WriteLine(fixpackDir.Name);
                    foreach (var line in text)
                    {
                        if (text != null)
                            tw.WriteLine(line);
                    }
                }
            }
        }
    }
}
