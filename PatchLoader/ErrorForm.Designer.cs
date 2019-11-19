namespace PatchLoader
{
    partial class ErrorForm
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
            this.BtRetry = new System.Windows.Forms.Button();
            this.BtCancel = new System.Windows.Forms.Button();
            this.GbMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbMain
            // 
            this.GbMain.Controls.Add(this.TbMain);
            this.GbMain.Location = new System.Drawing.Point(0, 0);
            this.GbMain.Name = "GbMain";
            this.GbMain.Size = new System.Drawing.Size(800, 395);
            this.GbMain.TabIndex = 0;
            this.GbMain.TabStop = false;
            this.GbMain.Text = "groupBox1";
            // 
            // TbMain
            // 
            this.TbMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbMain.Location = new System.Drawing.Point(3, 16);
            this.TbMain.Multiline = true;
            this.TbMain.Name = "TbMain";
            this.TbMain.Size = new System.Drawing.Size(794, 376);
            this.TbMain.TabIndex = 0;
            // 
            // BtRetry
            // 
            this.BtRetry.Location = new System.Drawing.Point(11, 401);
            this.BtRetry.Name = "BtRetry";
            this.BtRetry.Size = new System.Drawing.Size(105, 37);
            this.BtRetry.TabIndex = 1;
            this.BtRetry.Text = "Перепроверить";
            this.BtRetry.UseVisualStyleBackColor = true;
            this.BtRetry.Click += new System.EventHandler(this.BtRetry_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(122, 401);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(105, 37);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "Отменить";
            this.BtCancel.UseVisualStyleBackColor = true;
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // ErrorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtRetry);
            this.Controls.Add(this.GbMain);
            this.Name = "ErrorForm";
            this.Text = "Ошибка";
            this.Resize += new System.EventHandler(this.ErrorForm_Resize);
            this.GbMain.ResumeLayout(false);
            this.GbMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GbMain;
        private System.Windows.Forms.TextBox TbMain;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button BtRetry;
        private System.Windows.Forms.Button BtCancel;
    }
}