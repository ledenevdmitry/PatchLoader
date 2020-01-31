using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatchLoader
{
    public partial class EditFileScForm : Form
    {
        FileInfo file;
        public EditFileScForm(FileInfo file)
        {
            InitializeComponent();

            this.file = file;

            using (StreamReader sr = new StreamReader(file.FullName, Encoding.GetEncoding("Windows-1251")))
            {
                string fileScText = sr.ReadToEnd();
                TbFileSc.Text = fileScText;
            }
        }

        private void ResizeForm()
        {
            TbFileSc.Width = ClientRectangle.Width - 8 * 2;
            TbFileSc.Height = ClientRectangle.Height - BtOk.Height - 8 * 3;

            BtOk.Left = ClientRectangle.Width / 2 - BtOk.Width / 2;
            BtOk.Top = TbFileSc.Bottom + 8;
        }

        private void BtOk_Click(object sender, EventArgs e)
        {
            File.WriteAllText(file.FullName, TbFileSc.Text);
            DialogResult = DialogResult.OK;

            Close();
        }

        private void EditFileScForm_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }
    }
}
