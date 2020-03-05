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
        }

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

                Thread th = new Thread(() =>
                {
                    BtTest.Invoke(new Action(() => BtTest.Enabled = false));
                    foreach (string item in TbPatchList.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string dir = item.Split('/').Last();
                        DirectoryInfo patchDir = Directory.CreateDirectory(Path.Combine(localDir.FullName, dir));
                        vss.Pull(item, patchDir);

                        vss.TestPatchDir(item, out string errDesc, patchDir);
                        TbErrors.Invoke(new Action(() => TbErrors.AppendText(errDesc)));
                        lf.AddToLog("Проверка завершена!");
                    }

                    BtTest.Invoke(new Action(() => BtTest.Enabled = true));
                });
                th.Start();
            }

        }

        private void TestPatch_Resize(object sender, EventArgs e)
        {
            ScMain.Width = ClientRectangle.Width - 8 * 2;
            ScMain.Height = ClientRectangle.Height - 8 * 3 - BtTest.Height;

            BtTest.Top = ScMain.Bottom + 8;
        }
    }
}
