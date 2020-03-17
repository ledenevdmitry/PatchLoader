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
    public partial class LogForm : Form
    {
        public LogForm()
        {
            InitializeComponent();
            if (File.Exists("log.txt"))
            {
                File.Delete("log.txt");
            }
        }        

        public void AddToLog(string entry)
        {
            try
            {
                TbMain.Invoke(new Action(() => TbMain.AppendText(entry + Environment.NewLine)));
            }
            catch { }
                       

            using (var sw = File.AppendText("log.txt"))
            {
                sw.WriteLine(entry);
            }
        }
    }
}
