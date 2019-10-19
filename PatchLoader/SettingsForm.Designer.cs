namespace PatchLoader
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LbRemoteBase = new System.Windows.Forms.Label();
            this.TbRemoteBase = new System.Windows.Forms.TextBox();
            this.TbRemoteDir = new System.Windows.Forms.TextBox();
            this.LbRemoteDir = new System.Windows.Forms.Label();
            this.LbLinkDir = new System.Windows.Forms.Label();
            this.TbLinkDir = new System.Windows.Forms.TextBox();
            this.BtSubmit = new System.Windows.Forms.Button();
            this.LbToRep = new System.Windows.Forms.Label();
            this.TbToRep = new System.Windows.Forms.TextBox();
            this.TbToPatch = new System.Windows.Forms.TextBox();
            this.LbToPatch = new System.Windows.Forms.Label();
            this.TbNotToPatch = new System.Windows.Forms.TextBox();
            this.LbNotToPatch = new System.Windows.Forms.Label();
            this.TbNotToRep = new System.Windows.Forms.TextBox();
            this.LbNotToRep = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LbRemoteBase
            // 
            this.LbRemoteBase.AutoSize = true;
            this.LbRemoteBase.Location = new System.Drawing.Point(12, 12);
            this.LbRemoteBase.Name = "LbRemoteBase";
            this.LbRemoteBase.Size = new System.Drawing.Size(56, 13);
            this.LbRemoteBase.TabIndex = 0;
            this.LbRemoteBase.Text = "База VSS";
            // 
            // TbRemoteBase
            // 
            this.TbRemoteBase.Location = new System.Drawing.Point(134, 9);
            this.TbRemoteBase.Name = "TbRemoteBase";
            this.TbRemoteBase.Size = new System.Drawing.Size(197, 20);
            this.TbRemoteBase.TabIndex = 1;
            // 
            // TbRemoteDir
            // 
            this.TbRemoteDir.Location = new System.Drawing.Point(134, 35);
            this.TbRemoteDir.Name = "TbRemoteDir";
            this.TbRemoteDir.Size = new System.Drawing.Size(197, 20);
            this.TbRemoteDir.TabIndex = 3;
            // 
            // LbRemoteDir
            // 
            this.LbRemoteDir.AutoSize = true;
            this.LbRemoteDir.Location = new System.Drawing.Point(12, 38);
            this.LbRemoteDir.Name = "LbRemoteDir";
            this.LbRemoteDir.Size = new System.Drawing.Size(107, 13);
            this.LbRemoteDir.TabIndex = 2;
            this.LbRemoteDir.Text = "Папка с объектами";
            // 
            // LbLinkDir
            // 
            this.LbLinkDir.AutoSize = true;
            this.LbLinkDir.Location = new System.Drawing.Point(12, 64);
            this.LbLinkDir.Name = "LbLinkDir";
            this.LbLinkDir.Size = new System.Drawing.Size(93, 13);
            this.LbLinkDir.TabIndex = 4;
            this.LbLinkDir.Text = "Папка с патчами";
            // 
            // TbLinkDir
            // 
            this.TbLinkDir.Location = new System.Drawing.Point(134, 61);
            this.TbLinkDir.Name = "TbLinkDir";
            this.TbLinkDir.Size = new System.Drawing.Size(197, 20);
            this.TbLinkDir.TabIndex = 5;
            // 
            // BtSubmit
            // 
            this.BtSubmit.Location = new System.Drawing.Point(12, 206);
            this.BtSubmit.Name = "BtSubmit";
            this.BtSubmit.Size = new System.Drawing.Size(657, 35);
            this.BtSubmit.TabIndex = 6;
            this.BtSubmit.Text = "Применить";
            this.BtSubmit.UseVisualStyleBackColor = true;
            this.BtSubmit.Click += new System.EventHandler(this.BtSubmit_Click);
            // 
            // LbToRep
            // 
            this.LbToRep.AutoSize = true;
            this.LbToRep.Location = new System.Drawing.Point(12, 90);
            this.LbToRep.MaximumSize = new System.Drawing.Size(130, 0);
            this.LbToRep.Name = "LbToRep";
            this.LbToRep.Size = new System.Drawing.Size(112, 39);
            this.LbToRep.TabIndex = 7;
            this.LbToRep.Text = "Добавлять в общую папку (регулярное выражение)";
            // 
            // TbToRep
            // 
            this.TbToRep.Location = new System.Drawing.Point(134, 90);
            this.TbToRep.Multiline = true;
            this.TbToRep.Name = "TbToRep";
            this.TbToRep.Size = new System.Drawing.Size(197, 52);
            this.TbToRep.TabIndex = 8;
            // 
            // TbToPatch
            // 
            this.TbToPatch.Location = new System.Drawing.Point(134, 148);
            this.TbToPatch.Multiline = true;
            this.TbToPatch.Name = "TbToPatch";
            this.TbToPatch.Size = new System.Drawing.Size(197, 52);
            this.TbToPatch.TabIndex = 10;
            // 
            // LbToPatch
            // 
            this.LbToPatch.AutoSize = true;
            this.LbToPatch.Location = new System.Drawing.Point(12, 148);
            this.LbToPatch.MaximumSize = new System.Drawing.Size(120, 0);
            this.LbToPatch.Name = "LbToPatch";
            this.LbToPatch.Size = new System.Drawing.Size(100, 39);
            this.LbToPatch.TabIndex = 9;
            this.LbToPatch.Text = "Добавлять в патч (регулярное выражение)";
            // 
            // TbNotToPatch
            // 
            this.TbNotToPatch.Location = new System.Drawing.Point(466, 148);
            this.TbNotToPatch.Multiline = true;
            this.TbNotToPatch.Name = "TbNotToPatch";
            this.TbNotToPatch.Size = new System.Drawing.Size(197, 52);
            this.TbNotToPatch.TabIndex = 14;
            // 
            // LbNotToPatch
            // 
            this.LbNotToPatch.AutoSize = true;
            this.LbNotToPatch.Location = new System.Drawing.Point(344, 148);
            this.LbNotToPatch.MaximumSize = new System.Drawing.Size(120, 0);
            this.LbNotToPatch.Name = "LbNotToPatch";
            this.LbNotToPatch.Size = new System.Drawing.Size(114, 39);
            this.LbNotToPatch.TabIndex = 13;
            this.LbNotToPatch.Text = "Не добавлять в патч (регулярное выражение)";
            // 
            // TbNotToRep
            // 
            this.TbNotToRep.Location = new System.Drawing.Point(466, 90);
            this.TbNotToRep.Multiline = true;
            this.TbNotToRep.Name = "TbNotToRep";
            this.TbNotToRep.Size = new System.Drawing.Size(197, 52);
            this.TbNotToRep.TabIndex = 12;
            // 
            // LbNotToRep
            // 
            this.LbNotToRep.AutoSize = true;
            this.LbNotToRep.Location = new System.Drawing.Point(344, 90);
            this.LbNotToRep.MaximumSize = new System.Drawing.Size(130, 0);
            this.LbNotToRep.Name = "LbNotToRep";
            this.LbNotToRep.Size = new System.Drawing.Size(126, 39);
            this.LbNotToRep.TabIndex = 11;
            this.LbNotToRep.Text = "Не добавлять в общую папку (регулярное выражение)";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 249);
            this.Controls.Add(this.TbNotToPatch);
            this.Controls.Add(this.LbNotToPatch);
            this.Controls.Add(this.TbNotToRep);
            this.Controls.Add(this.LbNotToRep);
            this.Controls.Add(this.TbToPatch);
            this.Controls.Add(this.LbToPatch);
            this.Controls.Add(this.TbToRep);
            this.Controls.Add(this.LbToRep);
            this.Controls.Add(this.BtSubmit);
            this.Controls.Add(this.TbLinkDir);
            this.Controls.Add(this.LbLinkDir);
            this.Controls.Add(this.TbRemoteDir);
            this.Controls.Add(this.LbRemoteDir);
            this.Controls.Add(this.TbRemoteBase);
            this.Controls.Add(this.LbRemoteBase);
            this.Name = "SettingsForm";
            this.Text = "Настройки";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbRemoteBase;
        private System.Windows.Forms.TextBox TbRemoteBase;
        private System.Windows.Forms.TextBox TbRemoteDir;
        private System.Windows.Forms.Label LbRemoteDir;
        private System.Windows.Forms.Label LbLinkDir;
        private System.Windows.Forms.TextBox TbLinkDir;
        private System.Windows.Forms.Button BtSubmit;
        private System.Windows.Forms.Label LbToRep;
        private System.Windows.Forms.TextBox TbToRep;
        private System.Windows.Forms.TextBox TbToPatch;
        private System.Windows.Forms.Label LbToPatch;
        private System.Windows.Forms.TextBox TbNotToPatch;
        private System.Windows.Forms.Label LbNotToPatch;
        private System.Windows.Forms.TextBox TbNotToRep;
        private System.Windows.Forms.Label LbNotToRep;
    }
}