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
            this.TbMain = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // TbMain
            // 
            this.TbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbMain.Location = new System.Drawing.Point(0, 0);
            this.TbMain.Multiline = true;
            this.TbMain.Name = "TbMain";
            this.TbMain.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TbMain.Size = new System.Drawing.Size(800, 450);
            this.TbMain.TabIndex = 0;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TbMain);
            this.Name = "LogForm";
            this.Text = "Журнал";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TbMain;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}