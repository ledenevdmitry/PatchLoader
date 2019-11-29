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

            Thread th = new Thread(() =>
            {
                EnableStopButton();
                DisableSearchButtons();

                LogForm lf = new LogForm();
                VSSUtils.sender = lf.AddToLog;

                foreach (string root in roots)
                {
                    if(vss.FirstInEntireBase(root, TbFileName.Text, (int)NudDepth.Value, out string match))
                    {
                        TbResult.AppendText(match);
                    }
                    else
                    {
                        MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    MessageBox.Show("Поиск завершен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
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
            BtStopSearch.Invoke(new Action(() => BtFindFirst.Enabled = false));
        }

        private void EnableStopButton()
        {
            BtStopSearch.Invoke(new Action(() => BtFindFirst.Enabled = true));
        }

        private void BtStopSearch_Click(object sender, EventArgs e)
        {
            vss.stopSearch = true;
        }

        private void BtFindAll_Click(object sender, EventArgs e)
        {
            TbResult.Clear();
            List<string> roots = TbRoots.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            vss = new VSSUtils(Properties.Settings.Default.BaseLocation, Environment.UserName);

            Thread th = new Thread(() =>
            {
                EnableStopButton();
                DisableSearchButtons();

                LogForm lf = new LogForm();
                VSSUtils.sender = lf.AddToLog;

                foreach (string root in roots)
                {
                    try
                    {
                        foreach (string res in vss.AllInEntireBase(root, TbFileName.Text, (int)NudDepth.Value))
                        {
                            TbResult.AppendText(res);
                        }
                        MessageBox.Show("Поиск завершен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    catch (FileNotFoundException exc)
                    {
                        MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
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
    }
}
