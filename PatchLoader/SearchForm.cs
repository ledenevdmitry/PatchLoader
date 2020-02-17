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
    public partial class SearchForm : Form
    {
        public SearchForm()
        {
            InitializeComponent();
            TbRoots.Text = Properties.Settings.Default.SearchRoots;
        }

        private void SearchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.SearchRoots = TbRoots.Text;
            Properties.Settings.Default.Save();
        }

        VSSUtils vss;

        private void BtFindFirst_Click(object sender, EventArgs e)
        {
            TbResult.Clear();
            List<string> roots = TbRoots.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            vss = new VSSUtils(Properties.Settings.Default.BaseLocation, Environment.UserName);
            
            lf = new LogForm();
            VSSUtils.sender = lf.AddToLog;
            lf.Show();

            Thread th = new Thread(() =>
            {
                EnableStopButton();
                DisableSearchButtons();

                bool found = false;

                foreach (string root in roots)
                {
                    if(vss.FirstInEntireBase(root, TbFileName.Text, (int)NudDepth.Value, out string match))
                    {
                        TbResult.Invoke(new Action(() => TbResult.AppendText(match)));
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    MessageBox.Show("Поиск завершен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                lf.Invoke(new Action(() => lf.Close()));

                EnableSearchButtons();
                DisableStopButton();
            });

            th.Start();
        }

        private void DisableSearchButtons()
        {
            BtFindFirst.Invoke(new Action(() => BtFindFirst.Enabled = false));
            BtFindAll.Invoke(new Action(() => BtFindAll.Enabled = false));
        }

        private void EnableSearchButtons()
        {
            BtFindFirst.Invoke(new Action(() => BtFindFirst.Enabled = true));
            BtFindAll.Invoke(new Action(() => BtFindAll.Enabled = true));
        }

        private void DisableStopButton()
        {
            BtStopSearch.Invoke(new Action(() => BtStopSearch.Enabled = false));
        }

        private void EnableStopButton()
        {
            BtStopSearch.Invoke(new Action(() => BtStopSearch.Enabled = true));
        }

        private void BtStopSearch_Click(object sender, EventArgs e)
        {
            vss.stopSearch = true;
            lf.Close();
        }

        LogForm lf;

        private void BtFindAll_Click(object sender, EventArgs e)
        {
            TbResult.Clear();
            List<string> roots = TbRoots.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            vss = new VSSUtils(Properties.Settings.Default.BaseLocation, Environment.UserName);
            
            lf = new LogForm();
            VSSUtils.sender = lf.AddToLog;
            lf.Show();

            Thread th = new Thread(() =>
            {
                EnableStopButton();
                DisableSearchButtons();

                bool found = false;
                foreach (string root in roots)
                {
                    try
                    {
                        foreach (string res in vss.AllInEntireBase(root, TbFileName.Text, (int)NudDepth.Value))
                        {
                            TbResult.Invoke(new Action(() => TbResult.AppendText(res + Environment.NewLine)));
                        }
                        found = true;
                    }
                    catch (FileNotFoundException) { }
                }

                if (found)
                {
                    MessageBox.Show("Поиск завершен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                EnableSearchButtons();
                DisableStopButton();
            });

            th.Start();
        }

        private void CbDepth_CheckedChanged(object sender, EventArgs e)
        {
            NudDepth.Enabled = CbDepth.Checked;
            if(!CbDepth.Checked)
            {
                NudDepth.Value = -1;
            }
        }

        private void SearchForm_Resize(object sender, EventArgs e)
        {
            NudDepth.Left = ClientRectangle.Width - NudDepth.Width - 8;
            CbDepth.Left = NudDepth.Left - CbDepth.Width - 8;
            TbFileName.Width = ClientRectangle.Width - LbFileName.Width - CbDepth.Width - NudDepth.Width - 8 * 5;

            GbSearch.Width = ClientRectangle.Width - 8 * 2;
            GbSearch.Height = ClientRectangle.Height - LbFileName.Bottom - BtFindFirst.Height - 8 * 3;

            BtFindFirst.Top = ClientRectangle.Height - BtFindFirst.Height - 8;
            BtFindAll.Top = ClientRectangle.Height - BtFindAll.Height - 8;
            BtStopSearch.Top = ClientRectangle.Height - BtStopSearch.Height - 8;
        }
    }
}
