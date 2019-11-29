namespace PatchLoader
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.CreateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateScriptsRepositoryDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateInfaRepositoryDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BtPatchLocation = new System.Windows.Forms.Button();
            this.LbPatchLocation = new System.Windows.Forms.Label();
            this.TbPatchLocation = new System.Windows.Forms.TextBox();
            this.BtPush = new System.Windows.Forms.Button();
            this.LbFilesList = new System.Windows.Forms.Label();
            this.DgvFileList = new System.Windows.Forms.DataGridView();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AddInRepDir = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.AddToPatch = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BtInstallToTest = new System.Windows.Forms.Button();
            this.BtRefreshList = new System.Windows.Forms.Button();
            this.BtCreateFileSc = new System.Windows.Forms.Button();
            this.MainMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFileList)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateToolStripMenuItem,
            this.SettingsToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(685, 24);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // CreateToolStripMenuItem
            // 
            this.CreateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CreateScriptsRepositoryDirToolStripMenuItem,
            this.CreateInfaRepositoryDirToolStripMenuItem});
            this.CreateToolStripMenuItem.Name = "CreateToolStripMenuItem";
            this.CreateToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.CreateToolStripMenuItem.Text = "Создать";
            // 
            // CreateScriptsRepositoryDirToolStripMenuItem
            // 
            this.CreateScriptsRepositoryDirToolStripMenuItem.Name = "CreateScriptsRepositoryDirToolStripMenuItem";
            this.CreateScriptsRepositoryDirToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.CreateScriptsRepositoryDirToolStripMenuItem.Text = "Папку скриптов в репозитории";
            this.CreateScriptsRepositoryDirToolStripMenuItem.Click += new System.EventHandler(this.CreateScriptsRepositoryDirToolStripMenuItem_Click);
            // 
            // CreateInfaRepositoryDirToolStripMenuItem
            // 
            this.CreateInfaRepositoryDirToolStripMenuItem.Name = "CreateInfaRepositoryDirToolStripMenuItem";
            this.CreateInfaRepositoryDirToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.CreateInfaRepositoryDirToolStripMenuItem.Text = "Папку информатики в репозитории";
            this.CreateInfaRepositoryDirToolStripMenuItem.Click += new System.EventHandler(this.CreateInfaRepositoryDirToolStripMenuItem_Click);
            // 
            // SettingsToolStripMenuItem
            // 
            this.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
            this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.SettingsToolStripMenuItem.Text = "Параметры";
            this.SettingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // BtPatchLocation
            // 
            this.BtPatchLocation.Location = new System.Drawing.Point(517, 22);
            this.BtPatchLocation.Name = "BtPatchLocation";
            this.BtPatchLocation.Size = new System.Drawing.Size(75, 23);
            this.BtPatchLocation.TabIndex = 1;
            this.BtPatchLocation.Text = "Открыть";
            this.BtPatchLocation.UseVisualStyleBackColor = true;
            this.BtPatchLocation.Click += new System.EventHandler(this.BtPatchLocation_Click);
            // 
            // LbPatchLocation
            // 
            this.LbPatchLocation.AutoSize = true;
            this.LbPatchLocation.Location = new System.Drawing.Point(12, 27);
            this.LbPatchLocation.Name = "LbPatchLocation";
            this.LbPatchLocation.Size = new System.Drawing.Size(77, 13);
            this.LbPatchLocation.TabIndex = 2;
            this.LbPatchLocation.Text = "Путь до патча";
            // 
            // TbPatchLocation
            // 
            this.TbPatchLocation.Location = new System.Drawing.Point(95, 24);
            this.TbPatchLocation.Name = "TbPatchLocation";
            this.TbPatchLocation.Size = new System.Drawing.Size(416, 20);
            this.TbPatchLocation.TabIndex = 3;
            // 
            // BtPush
            // 
            this.BtPush.Enabled = false;
            this.BtPush.Location = new System.Drawing.Point(279, 307);
            this.BtPush.Name = "BtPush";
            this.BtPush.Size = new System.Drawing.Size(110, 23);
            this.BtPush.TabIndex = 4;
            this.BtPush.Text = "Выложить";
            this.BtPush.UseVisualStyleBackColor = true;
            this.BtPush.Click += new System.EventHandler(this.BtPush_Click);
            // 
            // LbFilesList
            // 
            this.LbFilesList.AutoSize = true;
            this.LbFilesList.Location = new System.Drawing.Point(12, 51);
            this.LbFilesList.Name = "LbFilesList";
            this.LbFilesList.Size = new System.Drawing.Size(47, 13);
            this.LbFilesList.TabIndex = 6;
            this.LbFilesList.Text = "Список ";
            // 
            // DgvFileList
            // 
            this.DgvFileList.AllowUserToAddRows = false;
            this.DgvFileList.AllowUserToDeleteRows = false;
            this.DgvFileList.AllowUserToResizeRows = false;
            this.DgvFileList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DgvFileList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FileName,
            this.AddInRepDir,
            this.AddToPatch});
            this.DgvFileList.Location = new System.Drawing.Point(15, 67);
            this.DgvFileList.Name = "DgvFileList";
            this.DgvFileList.RowHeadersVisible = false;
            this.DgvFileList.Size = new System.Drawing.Size(658, 234);
            this.DgvFileList.TabIndex = 7;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "Файл";
            this.FileName.Name = "FileName";
            this.FileName.ReadOnly = true;
            this.FileName.Width = 400;
            // 
            // AddInRepDir
            // 
            this.AddInRepDir.HeaderText = "Добавить в общую папку";
            this.AddInRepDir.Name = "AddInRepDir";
            this.AddInRepDir.ReadOnly = true;
            // 
            // AddToPatch
            // 
            this.AddToPatch.HeaderText = "Добавить в патч";
            this.AddToPatch.Name = "AddToPatch";
            this.AddToPatch.ReadOnly = true;
            // 
            // BtInstallToTest
            // 
            this.BtInstallToTest.Enabled = false;
            this.BtInstallToTest.Location = new System.Drawing.Point(163, 307);
            this.BtInstallToTest.Name = "BtInstallToTest";
            this.BtInstallToTest.Size = new System.Drawing.Size(110, 23);
            this.BtInstallToTest.TabIndex = 8;
            this.BtInstallToTest.Text = "Накатить на тест";
            this.BtInstallToTest.UseVisualStyleBackColor = true;
            this.BtInstallToTest.Click += new System.EventHandler(this.BtInstallToTest_Click);
            // 
            // BtRefreshList
            // 
            this.BtRefreshList.Location = new System.Drawing.Point(598, 22);
            this.BtRefreshList.Name = "BtRefreshList";
            this.BtRefreshList.Size = new System.Drawing.Size(75, 23);
            this.BtRefreshList.TabIndex = 9;
            this.BtRefreshList.Text = "Обновить";
            this.BtRefreshList.UseVisualStyleBackColor = true;
            this.BtRefreshList.Click += new System.EventHandler(this.BtRefreshList_Click);
            // 
            // BtCreateFileSc
            // 
            this.BtCreateFileSc.Enabled = false;
            this.BtCreateFileSc.Location = new System.Drawing.Point(15, 307);
            this.BtCreateFileSc.Name = "BtCreateFileSc";
            this.BtCreateFileSc.Size = new System.Drawing.Size(142, 23);
            this.BtCreateFileSc.TabIndex = 10;
            this.BtCreateFileSc.Text = "Создать файл сценария";
            this.BtCreateFileSc.UseVisualStyleBackColor = true;
            this.BtCreateFileSc.Click += new System.EventHandler(this.BtCreateFileSc_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 342);
            this.Controls.Add(this.BtCreateFileSc);
            this.Controls.Add(this.BtRefreshList);
            this.Controls.Add(this.BtInstallToTest);
            this.Controls.Add(this.DgvFileList);
            this.Controls.Add(this.LbFilesList);
            this.Controls.Add(this.BtPush);
            this.Controls.Add(this.TbPatchLocation);
            this.Controls.Add(this.LbPatchLocation);
            this.Controls.Add(this.BtPatchLocation);
            this.Controls.Add(this.MainMenuStrip);
            this.Name = "MainForm";
            this.Text = "Управление патчем";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DgvFileList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem SettingsToolStripMenuItem;
        private System.Windows.Forms.Button BtPatchLocation;
        private System.Windows.Forms.Label LbPatchLocation;
        private System.Windows.Forms.TextBox TbPatchLocation;
        private System.Windows.Forms.Button BtPush;
        private System.Windows.Forms.Label LbFilesList;
        private System.Windows.Forms.DataGridView DgvFileList;
        private System.Windows.Forms.ToolStripMenuItem CreateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateScriptsRepositoryDirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateInfaRepositoryDirToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AddInRepDir;
        private System.Windows.Forms.DataGridViewCheckBoxColumn AddToPatch;
        private System.Windows.Forms.Button BtInstallToTest;
        private System.Windows.Forms.Button BtRefreshList;
        private System.Windows.Forms.Button BtCreateFileSc;
    }
}

