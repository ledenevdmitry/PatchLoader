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
    public partial class EnterValueForm : Form
    {
        public EnterValueForm(string caption, string value = null)
        {
            InitializeComponent();
            Text = caption;
            TbValue.Text = value;
        }

        public string Value { get; private set; }

        private void TbValue_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                Submit();
            }
        }

        private void Submit()
        {
            DialogResult = DialogResult.OK;
            Value = TbValue.Text;
        }

        private void BtSubmit_Click(object sender, EventArgs e)
        {
            Submit();
        }
    }
}
