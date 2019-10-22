namespace PatchLoader
{
    partial class EnterValueForm
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
            this.TbValue = new System.Windows.Forms.TextBox();
            this.BtSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TbValue
            // 
            this.TbValue.Location = new System.Drawing.Point(12, 12);
            this.TbValue.Name = "TbValue";
            this.TbValue.Size = new System.Drawing.Size(160, 20);
            this.TbValue.TabIndex = 0;
            this.TbValue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TbValue_KeyDown);
            // 
            // BtSubmit
            // 
            this.BtSubmit.Location = new System.Drawing.Point(178, 10);
            this.BtSubmit.Name = "BtSubmit";
            this.BtSubmit.Size = new System.Drawing.Size(75, 23);
            this.BtSubmit.TabIndex = 1;
            this.BtSubmit.Text = "OK";
            this.BtSubmit.UseVisualStyleBackColor = true;
            this.BtSubmit.Click += new System.EventHandler(this.BtSubmit_Click);
            // 
            // EnterValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 46);
            this.Controls.Add(this.BtSubmit);
            this.Controls.Add(this.TbValue);
            this.Name = "EnterValueForm";
            this.Text = "EnterValueForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TbValue;
        private System.Windows.Forms.Button BtSubmit;
    }
}