namespace PatchLoader
{
    partial class EditFileScForm
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
            this.BtOk = new System.Windows.Forms.Button();
            this.TbFileSc = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(345, 395);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(88, 43);
            this.BtOk.TabIndex = 1;
            this.BtOk.Text = "OK";
            this.BtOk.UseVisualStyleBackColor = true;
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // TbFileSc
            // 
            this.TbFileSc.Location = new System.Drawing.Point(12, 12);
            this.TbFileSc.Multiline = true;
            this.TbFileSc.Name = "TbFileSc";
            this.TbFileSc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TbFileSc.Size = new System.Drawing.Size(776, 377);
            this.TbFileSc.TabIndex = 2;
            // 
            // EditFileScForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TbFileSc);
            this.Controls.Add(this.BtOk);
            this.Name = "EditFileScForm";
            this.Text = "EditFileSc";
            this.Resize += new System.EventHandler(this.EditFileScForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.TextBox TbFileSc;
    }
}