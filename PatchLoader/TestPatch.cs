using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

            Thread th = new Thread(() =>
            {
                BtTest.Invoke(new Action(() => BtTest.Enabled = false));

                foreach (string item in TbPatchList.Text.Split(new string[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
                {
                    vss.TestPatchDir(item, out List<string> versionMismatches);
                    lf.AddToLog("Проверка завершена!");
                    foreach(string mismatch in versionMismatches)
                    {
                        TbErrors.Invoke(new Action(() => TbErrors.AppendText($"Несоответствие версий для {mismatch}{Environment.NewLine}")));
                    }
                }

                BtTest.Invoke(new Action(() => BtTest.Enabled = true));

            });

            th.Start();
        }

        private void TestPatch_Resize(object sender, EventArgs e)
        {
            ScMain.Width = ClientRectangle.Width - 8 * 2;
            ScMain.Height = ClientRectangle.Height - 8 * 3 - BtTest.Height;

            BtTest.Top = ScMain.Bottom + 8;
        }
    }
}
