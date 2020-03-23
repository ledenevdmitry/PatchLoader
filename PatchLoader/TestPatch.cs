using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatchLoader
{
    public partial class TestPatch : Form
    {
        public TestPatch()
        {
            InitializeComponent();

            Icon = Properties.Resources.vsstest;

            TbPatchList.Text = Properties.Settings.Default.PatchesToTest;
        }

        Thread th;

        private void BtTest_Click(object sender, EventArgs e)
        {
            VSSUtils vss = new VSSUtils(Properties.Settings.Default.BaseLocation, Environment.UserName);

            LogForm lf = new LogForm();
            VSSUtils.sender = lf.AddToLog;
            lf.Show();

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Необходимо выбрать папку для хранения патчей";
            fbd.SelectedPath = Properties.Settings.Default.PatchesLocalDir;

            DirectoryInfo localDir;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                localDir = new DirectoryInfo(fbd.SelectedPath);
                Properties.Settings.Default.PatchesLocalDir = fbd.SelectedPath;
                Properties.Settings.Default.Save();

                th = new Thread(() =>
                {
                    BtStop.Invoke(new Action(() => BtStop.Enabled = true));
                    BtTest.Invoke(new Action(() => BtTest.Enabled = false));

                    foreach (string item in TbPatchList.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string dir = item.Split('/').Last();
                        DirectoryInfo patchDir = Directory.CreateDirectory(Path.Combine(localDir.FullName, dir));
                        vss.Pull(item, patchDir);
                        OSUtils.SetAttributesNormal(patchDir);

                        vss.TestPatchDir(item, out string errDesc, patchDir);
                        TbErrors.Invoke(new Action(() => TbErrors.AppendText(errDesc)));
                        lf.AddToLog("Проверка завершена!");
                    }

                    BtStop.Invoke(new Action(() => BtStop.Enabled = false));
                    BtTest.Invoke(new Action(() => BtTest.Enabled = true));
                    MessageBox.Show("Проверка завершена", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                });
                th.Start();


                Properties.Settings.Default.PatchesToTest = TbPatchList.Text;
                Properties.Settings.Default.Save();
            }

        }

        private void TestPatch_Resize(object sender, EventArgs e)
        {
            ScMain.Width = ClientRectangle.Width - 8 * 2;
            ScMain.Height = ClientRectangle.Height - 8 * 3 - BtTest.Height;

            BtTest.Top = ScMain.Bottom + 8;
            BtGetList.Top = ScMain.Bottom + 8;
        }

        private void BtGetList_Click(object sender, EventArgs e)
        {
            EnterValueForm evf = new EnterValueForm("Папка с патчами", Properties.Settings.Default.MainFolderToTest);
            if(evf.ShowDialog() == DialogResult.OK)
            {
                string dir = evf.Value;
                Properties.Settings.Default.MainFolderToTest = evf.Value;
                Properties.Settings.Default.Save();

                VSSUtils vss = new VSSUtils(Properties.Settings.Default.BaseLocation, Environment.UserName);
                List<string> subdirs = vss.GetSubdirs(dir);

                if(TbPatchList.Text != "")
                {
                    if(MessageBox.Show("Список патчей непустой. Очистить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        TbPatchList.Clear();
                    }
                }

                foreach(string subdir in subdirs)
                {
                    TbPatchList.AppendText(subdir + Environment.NewLine);
                }
            }
        }

        private void BtStop_Click(object sender, EventArgs e)
        {
            th.Abort();
            BtTest.Enabled = true;
            BtStop.Enabled = false;
        }
    }
}
