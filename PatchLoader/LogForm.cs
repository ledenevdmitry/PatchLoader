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
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
        }        

        public void AddToLog(string entry)
        {
            try
            {
                TbMain.Invoke(new Action(() => TbMain.AppendText(entry + Environment.NewLine)));
            }
            catch { }
        }
    }
}
