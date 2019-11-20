using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatchLoader
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(string caption, string text)
        {
            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            GbMain.Text = caption;
            TbMain.Text = text;
        }

        private void ErrorForm_Resize(object sender, EventArgs e)
        {
            GbMain.Width = ClientSize.Width;
            GbMain.Height = ClientSize.Height - BtRetry.Height - 2 * 8;

            BtRetry.Top = ClientSize.Height - BtRetry.Height - 8;
            BtCancel.Top = ClientSize.Height - BtCancel.Height - 8;
        }

        private void BtRetry_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            return;
        }

        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            return;
        }
    }
}
