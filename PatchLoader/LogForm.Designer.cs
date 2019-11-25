namespace PatchLoader
{
    partial class LogForm
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
            this.GbMain = new System.Windows.Forms.GroupBox();
            this.TbMain = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.GbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbMain
            // 
            this.GbMain.Controls.Add(this.TbMain);
            this.GbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbMain.Location = new System.Drawing.Point(0, 0);
            this.GbMain.Name = "GbMain";
            this.GbMain.Size = new System.Drawing.Size(800, 450);
            this.GbMain.TabIndex = 0;
            this.GbMain.TabStop = false;
            this.GbMain.Text = "Лог";
            // 
            // TbMain
            // 
            this.TbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbMain.Location = new System.Drawing.Point(3, 16);
            this.TbMain.Multiline = true;
            this.TbMain.Name = "TbMain";
            this.TbMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TbMain.Size = new System.Drawing.Size(794, 431);
            this.TbMain.TabIndex = 0;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.GbMain);
            this.Name = "LogForm";
            this.Text = "Лог выкладывания";
            this.GbMain.ResumeLayout(false);
            this.GbMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbMain;
        private System.Windows.Forms.TextBox TbMain;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}