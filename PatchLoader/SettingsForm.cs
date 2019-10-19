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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            TbLinkDir.Text = Properties.Settings.Default.RemoteLinkRoot;
            TbRemoteBase.Text = Properties.Settings.Default.BaseLocation;
            TbRemoteDir.Text = Properties.Settings.Default.RemoteRoot;
            TbToRep.Text = Properties.Settings.Default.AddToRep;
            TbToPatch.Text = Properties.Settings.Default.AddToPatch;
            TbNotToRep.Text = Properties.Settings.Default.NotAddToRep;
            TbNotToPatch.Text = Properties.Settings.Default.NotAddToPatch;
        }

        private void BtSubmit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            Properties.Settings.Default.RemoteLinkRoot = TbLinkDir.Text;
            Properties.Settings.Default.BaseLocation = TbRemoteBase.Text;
            Properties.Settings.Default.RemoteRoot = TbRemoteDir.Text;
            Properties.Settings.Default.AddToRep = TbToRep.Text;
            Properties.Settings.Default.AddToPatch = TbToPatch.Text;
            Properties.Settings.Default.NotAddToRep = TbNotToRep.Text;
            Properties.Settings.Default.NotAddToPatch = TbNotToPatch.Text;

            Properties.Settings.Default.Save();

            Close();
        }
    }
}
