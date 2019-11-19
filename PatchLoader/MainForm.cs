﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatchLoader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            TbPatchLocation.Text = Properties.Settings.Default.LastSavedDir;
            ResizeForm();
        }

        PatchUtils patchUtils = new PatchUtils(
            Properties.Settings.Default.RemoteRoot,
            Properties.Settings.Default.RemoteLinkRoot,
            Properties.Settings.Default.BaseLocation);

        private void RefreshList(DirectoryInfo patchDir)
        {
            Regex addToRepRegex = new Regex(Properties.Settings.Default.AddToRep);
            Regex addToPatchRegex = new Regex(Properties.Settings.Default.AddToPatch);
            Regex notAddToRepRegex = new Regex(Properties.Settings.Default.NotAddToRep);
            Regex notAddToPatchRegex = new Regex(Properties.Settings.Default.NotAddToPatch);

            DgvFileList.Rows.Clear();
            int i = 0;
            foreach (FileInfo fileInfo in patchDir.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                if (!fileInfo.Name.Equals("vssver2.scc", StringComparison.InvariantCultureIgnoreCase))
                {
                    DgvFileList.Rows.Add();
                    DataGridViewRow currRow = DgvFileList.Rows[i++];
                    string fromSelectedPath = fileInfo.FullName.Substring(patchDir.FullName.Length + 1);

                    string schema = "";
                    bool schemaFound = false;

                    if (fromSelectedPath.Count(x => x == '\\') > 1)
                    {
                        int schemaStart = fromSelectedPath.IndexOf('\\');
                        int schemaEnd = fromSelectedPath.IndexOf('\\', schemaStart + 1);

                        schema = fromSelectedPath.Substring(schemaStart + 1, schemaEnd - schemaStart - 1);
                        schemaFound = true;
                    }

                    currRow.Cells[0].Value = fromSelectedPath;

                    bool addToPatch = addToPatchRegex.IsMatch(fileInfo.Name) && !notAddToPatchRegex.IsMatch(fileInfo.Name);
                    bool addToRep =
                        schemaFound &&
                        addToRepRegex.IsMatch(fromSelectedPath) &&
                        !notAddToRepRegex.IsMatch(fromSelectedPath) &&
                        //папка со скриптами, и подпапка есть в списке допустимых
                        (PatchUtils.IsAcceptableDir(fileInfo.Directory, Properties.Settings.Default.ScriptsSubdir, schema, patchDir, Properties.Settings.Default.RepStructureScripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList())
                         ||
                         PatchUtils.IsAcceptableDir(fileInfo.Directory, Properties.Settings.Default.InfaSubdir, schema, patchDir, Properties.Settings.Default.RepStructureInfa.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList()));

                    currRow.Cells[1].Value = addToPatch && addToRep;
                    currRow.Cells[2].Value = addToPatch;
                }
            }
        }

        private void BtRefreshList_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(TbPatchLocation.Text))
            {
                DirectoryInfo patchDir = new DirectoryInfo(TbPatchLocation.Text);
                RefreshList(patchDir);
            }
            else
            {
                MessageBox.Show("Папка с патчем не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtPatchLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog
            {
                SelectedPath = Properties.Settings.Default.LastSavedDir
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.LastSavedDir = fbd.SelectedPath;
                Properties.Settings.Default.Save();

                TbPatchLocation.Text = fbd.SelectedPath;

                if (Directory.Exists(fbd.SelectedPath))
                {
                    DirectoryInfo patchDir = new DirectoryInfo(fbd.SelectedPath);
                    RefreshList(patchDir);
                }
                else
                {
                    MessageBox.Show("Папка с патчем не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm();

            if(sf.ShowDialog() == DialogResult.OK)
            {
                patchUtils = new PatchUtils(
                    Properties.Settings.Default.RemoteRoot,
                    Properties.Settings.Default.RemoteLinkRoot,
                    Properties.Settings.Default.BaseLocation);
            }

            patchUtils = new PatchUtils(
                Properties.Settings.Default.RemoteRoot,
                Properties.Settings.Default.RemoteLinkRoot,
                Properties.Settings.Default.BaseLocation);

        }

        private bool CheckPatch(DirectoryInfo patchDir, List<FileInfoWithPatchOptions> patchFiles)
        {
            bool res = true;
            string errLog = patchUtils.CheckPatch(patchDir, patchFiles);
            if (errLog != "")
            {
                if (MessageBox.Show(errLog, "Ошибки при проверке", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Retry)
                {
                    res = CheckPatch(patchDir, patchFiles);
                }
                else
                {
                    return false;
                }
            }
            return res;
        }

        private void BtPush_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(TbPatchLocation.Text))
            {
                DirectoryInfo patchDir = new DirectoryInfo(TbPatchLocation.Text);

                List<FileInfoWithPatchOptions> patchFiles =
                    DgvFileList.Rows.Cast<DataGridViewRow>()
                    .Where(x => x.Cells[0].Value != null)
                    .Select(x => new FileInfoWithPatchOptions(
                        new FileInfo(
                            Path.Combine(patchDir.FullName, (string)x.Cells[0].Value)), 
                            (bool)x.Cells[1].Value, 
                            (bool)x.Cells[2].Value))
                    .ToList();

                if (CheckPatch(patchDir, patchFiles) && !patchUtils.PushPatch(patchDir, patchFiles, out List<string> vssPathCheckedOutToAnotherUser))
                {
                    ErrorForm ef = new ErrorForm("Файлы checked out другим пользователем. Невозможно добавить:", string.Join(Environment.NewLine, vssPathCheckedOutToAnotherUser));
                    ef.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Папка с патчем не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResizeForm()
        {
            DgvFileList.Width = ClientRectangle.Width - 2 * 8;
            DgvFileList.Height = ClientRectangle.Height - DgvFileList.Top - BtPush.Height - 2 * 8;

            BtPush.Top = ClientRectangle.Height - BtPush.Height - 8;
            BtInstallToTest.Top = ClientRectangle.Height - BtInstallToTest.Height - 8;

            BtRefreshList.Left = ClientRectangle.Width - BtRefreshList.Width - 8;
            BtPatchLocation.Left = BtRefreshList.Left - BtRefreshList.Width - 8;
            TbPatchLocation.Width = BtPatchLocation.Left - 8 - TbPatchLocation.Left;

            DgvFileList.Columns[0].Width = DgvFileList.Width - DgvFileList.Columns[1].Width - DgvFileList.Columns[2].Width - 8;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        private void CreateScriptsRepositoryDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnterValueForm evf = new EnterValueForm("Добавление папки скриптов в репозиторий");
            if(evf.ShowDialog() == DialogResult.OK)
            {
                patchUtils.CreateStructure(
                    evf.Value,
                    Properties.Settings.Default.ScriptsSubdir,
                    Properties.Settings.Default.RepStructureScripts.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
        }

        private void CreateInfaRepositoryDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnterValueForm evf = new EnterValueForm("Добавление папки информатики в репозиторий");
            if (evf.ShowDialog() == DialogResult.OK)
            {
                patchUtils.CreateStructure(
                    evf.Value,
                    Properties.Settings.Default.InfaSubdir,
                    Properties.Settings.Default.RepStructureInfa.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList());
            }
        }

        private void BtInstallToTest_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(TbPatchLocation.Text))
            {
                string fileScPath = Path.Combine(TbPatchLocation.Text, "file_sc.txt");
                if (File.Exists(fileScPath))
                {
                    System.Diagnostics.Process.Start("CMD.exe", $"/C \"{Properties.Settings.Default.PatchInstallerPath}\" STDEV11 1 \"{fileScPath}\" 1");
                }
                else
                {
                    MessageBox.Show("Файл сценария не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Папка с патчем не найдена!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
