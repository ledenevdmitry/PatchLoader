namespace PatchLoader
{
    partial class TestPatch
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
            this.GbPatchList = new System.Windows.Forms.GroupBox();
            this.TbPatchList = new System.Windows.Forms.TextBox();
            this.BtTest = new System.Windows.Forms.Button();
            this.GbErrors = new System.Windows.Forms.GroupBox();
            this.TbErrors = new System.Windows.Forms.TextBox();
            this.ScMain = new System.Windows.Forms.SplitContainer();
            this.BtGetList = new System.Windows.Forms.Button();
            this.GbPatchList.SuspendLayout();
            this.GbErrors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScMain)).BeginInit();
            this.ScMain.Panel1.SuspendLayout();
            this.ScMain.Panel2.SuspendLayout();
            this.ScMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // GbPatchList
            // 
            this.GbPatchList.Controls.Add(this.TbPatchList);
            this.GbPatchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbPatchList.Location = new System.Drawing.Point(0, 0);
            this.GbPatchList.Name = "GbPatchList";
            this.GbPatchList.Size = new System.Drawing.Size(190, 197);
            this.GbPatchList.TabIndex = 1;
            this.GbPatchList.TabStop = false;
            this.GbPatchList.Text = "Список патчей (полное имя VSS)";
            // 
            // TbPatchList
            // 
            this.TbPatchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbPatchList.Location = new System.Drawing.Point(3, 16);
            this.TbPatchList.Multiline = true;
            this.TbPatchList.Name = "TbPatchList";
            this.TbPatchList.Size = new System.Drawing.Size(184, 178);
            this.TbPatchList.TabIndex = 0;
            // 
            // BtTest
            // 
            this.BtTest.Location = new System.Drawing.Point(12, 210);
            this.BtTest.Name = "BtTest";
            this.BtTest.Size = new System.Drawing.Size(120, 34);
            this.BtTest.TabIndex = 2;
            this.BtTest.Text = "Проверить";
            this.BtTest.UseVisualStyleBackColor = true;
            this.BtTest.Click += new System.EventHandler(this.BtTest_Click);
            // 
            // GbErrors
            // 
            this.GbErrors.Controls.Add(this.TbErrors);
            this.GbErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GbErrors.Location = new System.Drawing.Point(0, 0);
            this.GbErrors.Name = "GbErrors";
            this.GbErrors.Size = new System.Drawing.Size(378, 197);
            this.GbErrors.TabIndex = 3;
            this.GbErrors.TabStop = false;
            this.GbErrors.Text = "Ошибки:";
            // 
            // TbErrors
            // 
            this.TbErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbErrors.Location = new System.Drawing.Point(3, 16);
            this.TbErrors.Multiline = true;
            this.TbErrors.Name = "TbErrors";
            this.TbErrors.Size = new System.Drawing.Size(372, 178);
            this.TbErrors.TabIndex = 0;
            // 
            // ScMain
            // 
            this.ScMain.Location = new System.Drawing.Point(15, 7);
            this.ScMain.Name = "ScMain";
            // 
            // ScMain.Panel1
            // 
            this.ScMain.Panel1.Controls.Add(this.GbPatchList);
            // 
            // ScMain.Panel2
            // 
            this.ScMain.Panel2.Controls.Add(this.GbErrors);
            this.ScMain.Size = new System.Drawing.Size(572, 197);
            this.ScMain.SplitterDistance = 190;
            this.ScMain.TabIndex = 4;
            // 
            // BtGetList
            // 
            this.BtGetList.Location = new System.Drawing.Point(138, 210);
            this.BtGetList.Name = "BtGetList";
            this.BtGetList.Size = new System.Drawing.Size(120, 34);
            this.BtGetList.TabIndex = 5;
            this.BtGetList.Text = "Получить список патчей из папки";
            this.BtGetList.UseVisualStyleBackColor = true;
            this.BtGetList.Click += new System.EventHandler(this.BtGetList_Click);
            // 
            // TestPatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 256);
            this.Controls.Add(this.BtGetList);
            this.Controls.Add(this.ScMain);
            this.Controls.Add(this.BtTest);
            this.Name = "TestPatch";
            this.Text = "Проверка патчей";
            this.Resize += new System.EventHandler(this.TestPatch_Resize);
            this.GbPatchList.ResumeLayout(false);
            this.GbPatchList.PerformLayout();
            this.GbErrors.ResumeLayout(false);
            this.GbErrors.PerformLayout();
            this.ScMain.Panel1.ResumeLayout(false);
            this.ScMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScMain)).EndInit();
            this.ScMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox GbPatchList;
        private System.Windows.Forms.Button BtTest;
        private System.Windows.Forms.GroupBox GbErrors;
        private System.Windows.Forms.TextBox TbPatchList;
        private System.Windows.Forms.TextBox TbErrors;
        private System.Windows.Forms.SplitContainer ScMain;
        private System.Windows.Forms.Button BtGetList;
    }
}