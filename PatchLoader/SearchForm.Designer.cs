namespace PatchLoader
{
    partial class SearchForm
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
            this.LbFileName = new System.Windows.Forms.Label();
            this.TbFileName = new System.Windows.Forms.TextBox();
            this.TbRoots = new System.Windows.Forms.TextBox();
            this.LbRoots = new System.Windows.Forms.Label();
            this.TbResult = new System.Windows.Forms.TextBox();
            this.LbResult = new System.Windows.Forms.Label();
            this.BtFindFirst = new System.Windows.Forms.Button();
            this.BtFindAll = new System.Windows.Forms.Button();
            this.BtStopSearch = new System.Windows.Forms.Button();
            this.CbDepth = new System.Windows.Forms.CheckBox();
            this.NudDepth = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.NudDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // LbFileName
            // 
            this.LbFileName.AutoSize = true;
            this.LbFileName.Location = new System.Drawing.Point(12, 9);
            this.LbFileName.Name = "LbFileName";
            this.LbFileName.Size = new System.Drawing.Size(92, 13);
            this.LbFileName.TabIndex = 0;
            this.LbFileName.Text = "Название файла";
            // 
            // TbFileName
            // 
            this.TbFileName.Location = new System.Drawing.Point(110, 6);
            this.TbFileName.Name = "TbFileName";
            this.TbFileName.Size = new System.Drawing.Size(136, 20);
            this.TbFileName.TabIndex = 2;
            // 
            // TbRoots
            // 
            this.TbRoots.Location = new System.Drawing.Point(15, 60);
            this.TbRoots.Multiline = true;
            this.TbRoots.Name = "TbRoots";
            this.TbRoots.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TbRoots.Size = new System.Drawing.Size(231, 114);
            this.TbRoots.TabIndex = 4;
            // 
            // LbRoots
            // 
            this.LbRoots.AutoSize = true;
            this.LbRoots.Location = new System.Drawing.Point(12, 35);
            this.LbRoots.Name = "LbRoots";
            this.LbRoots.Size = new System.Drawing.Size(98, 13);
            this.LbRoots.TabIndex = 3;
            this.LbRoots.Text = "Корни для поиска";
            // 
            // TbResult
            // 
            this.TbResult.Location = new System.Drawing.Point(252, 60);
            this.TbResult.Multiline = true;
            this.TbResult.Name = "TbResult";
            this.TbResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TbResult.Size = new System.Drawing.Size(231, 114);
            this.TbResult.TabIndex = 5;
            // 
            // LbResult
            // 
            this.LbResult.AutoSize = true;
            this.LbResult.Location = new System.Drawing.Point(249, 35);
            this.LbResult.Name = "LbResult";
            this.LbResult.Size = new System.Drawing.Size(59, 13);
            this.LbResult.TabIndex = 6;
            this.LbResult.Text = "Результат";
            // 
            // BtFindFirst
            // 
            this.BtFindFirst.Location = new System.Drawing.Point(15, 180);
            this.BtFindFirst.Name = "BtFindFirst";
            this.BtFindFirst.Size = new System.Drawing.Size(89, 23);
            this.BtFindFirst.TabIndex = 7;
            this.BtFindFirst.Text = "Найти первый";
            this.BtFindFirst.UseVisualStyleBackColor = true;
            this.BtFindFirst.Click += new System.EventHandler(this.BtFindFirst_Click);
            // 
            // BtFindAll
            // 
            this.BtFindAll.Location = new System.Drawing.Point(110, 180);
            this.BtFindAll.Name = "BtFindAll";
            this.BtFindAll.Size = new System.Drawing.Size(89, 23);
            this.BtFindAll.TabIndex = 8;
            this.BtFindAll.Text = "Найти все";
            this.BtFindAll.UseVisualStyleBackColor = true;
            this.BtFindAll.Click += new System.EventHandler(this.BtFindAll_Click);
            // 
            // BtStopSearch
            // 
            this.BtStopSearch.Enabled = false;
            this.BtStopSearch.Location = new System.Drawing.Point(205, 180);
            this.BtStopSearch.Name = "BtStopSearch";
            this.BtStopSearch.Size = new System.Drawing.Size(89, 23);
            this.BtStopSearch.TabIndex = 9;
            this.BtStopSearch.Text = "Остановить";
            this.BtStopSearch.UseVisualStyleBackColor = true;
            this.BtStopSearch.Click += new System.EventHandler(this.BtStopSearch_Click);
            // 
            // CbDepth
            // 
            this.CbDepth.AutoSize = true;
            this.CbDepth.Location = new System.Drawing.Point(261, 8);
            this.CbDepth.Name = "CbDepth";
            this.CbDepth.Size = new System.Drawing.Size(106, 17);
            this.CbDepth.TabIndex = 12;
            this.CbDepth.Text = "Глубина поиска";
            this.CbDepth.UseVisualStyleBackColor = true;
            this.CbDepth.CheckedChanged += new System.EventHandler(this.CbDepth_CheckedChanged);
            // 
            // NudDepth
            // 
            this.NudDepth.Enabled = false;
            this.NudDepth.Location = new System.Drawing.Point(373, 7);
            this.NudDepth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.NudDepth.Name = "NudDepth";
            this.NudDepth.Size = new System.Drawing.Size(47, 20);
            this.NudDepth.TabIndex = 13;
            this.NudDepth.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 213);
            this.Controls.Add(this.NudDepth);
            this.Controls.Add(this.CbDepth);
            this.Controls.Add(this.BtStopSearch);
            this.Controls.Add(this.BtFindAll);
            this.Controls.Add(this.BtFindFirst);
            this.Controls.Add(this.LbResult);
            this.Controls.Add(this.TbResult);
            this.Controls.Add(this.TbRoots);
            this.Controls.Add(this.LbRoots);
            this.Controls.Add(this.TbFileName);
            this.Controls.Add(this.LbFileName);
            this.Name = "SearchForm";
            this.Text = "Поиск";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.NudDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbFileName;
        private System.Windows.Forms.TextBox TbFileName;
        private System.Windows.Forms.TextBox TbRoots;
        private System.Windows.Forms.Label LbRoots;
        private System.Windows.Forms.TextBox TbResult;
        private System.Windows.Forms.Label LbResult;
        private System.Windows.Forms.Button BtFindFirst;
        private System.Windows.Forms.Button BtFindAll;
        private System.Windows.Forms.Button BtStopSearch;
        private System.Windows.Forms.CheckBox CbDepth;
        private System.Windows.Forms.NumericUpDown NudDepth;
    }
}