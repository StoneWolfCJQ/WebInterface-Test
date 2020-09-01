namespace WebInterface
{
    partial class IOInterface
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IOInterface));
            this.toggleListenButton = new System.Windows.Forms.Button();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.portTextBox = new System.Windows.Forms.TextBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.childItemManageControllers = new System.Windows.Forms.MenuItem();
            this.childItemSaveIO2File = new System.Windows.Forms.MenuItem();
            this.childItemExportImport = new System.Windows.Forms.MenuItem();
            this.childItemImportData = new System.Windows.Forms.MenuItem();
            this.childItemExportData = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.childItemPasteData = new System.Windows.Forms.MenuItem();
            this.childItemLimitWizard = new System.Windows.Forms.MenuItem();
            this.IOTab = new System.Windows.Forms.TabControl();
            this.inputPage = new System.Windows.Forms.TabPage();
            this.inputDGV = new System.Windows.Forms.DataGridView();
            this.outputPage = new System.Windows.Forms.TabPage();
            this.outputDGV = new System.Windows.Forms.DataGridView();
            this.limitPage = new System.Windows.Forms.TabPage();
            this.limitDGV = new System.Windows.Forms.DataGridView();
            this.controllerDropList = new System.Windows.Forms.ComboBox();
            this.toggleConnectButton = new System.Windows.Forms.Button();
            this.deleteRowButton = new System.Windows.Forms.Button();
            this.clearTableButton = new System.Windows.Forms.Button();
            this.IOTab.SuspendLayout();
            this.inputPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.inputDGV)).BeginInit();
            this.outputPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outputDGV)).BeginInit();
            this.limitPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // toggleListenButton
            // 
            this.toggleListenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toggleListenButton.Location = new System.Drawing.Point(557, 11);
            this.toggleListenButton.Name = "toggleListenButton";
            this.toggleListenButton.Size = new System.Drawing.Size(80, 22);
            this.toggleListenButton.TabIndex = 0;
            this.toggleListenButton.Text = "Start Listen!";
            this.toggleListenButton.UseVisualStyleBackColor = true;
            this.toggleListenButton.Click += new System.EventHandler(this.toggleListenButton_Click);
            // 
            // IPTextBox
            // 
            this.IPTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.IPTextBox.Location = new System.Drawing.Point(391, 12);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(96, 20);
            this.IPTextBox.TabIndex = 1;
            this.IPTextBox.Text = "127.0.0.1";
            // 
            // portTextBox
            // 
            this.portTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.portTextBox.Location = new System.Drawing.Point(494, 12);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(56, 20);
            this.portTextBox.TabIndex = 2;
            this.portTextBox.Text = "4000";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem4});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.childItemManageControllers,
            this.childItemSaveIO2File,
            this.childItemExportImport});
            this.menuItem1.Text = "File";
            // 
            // childItemManageControllers
            // 
            this.childItemManageControllers.Index = 0;
            this.childItemManageControllers.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.childItemManageControllers.Text = "Manage Controllers";
            this.childItemManageControllers.Click += new System.EventHandler(this.menuItemAddController_Click);
            // 
            // childItemSaveIO2File
            // 
            this.childItemSaveIO2File.Index = 1;
            this.childItemSaveIO2File.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.childItemSaveIO2File.Text = "Save Data";
            this.childItemSaveIO2File.Click += new System.EventHandler(this.childItemSaveIO2File_Click);
            // 
            // childItemExportImport
            // 
            this.childItemExportImport.Index = 2;
            this.childItemExportImport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.childItemImportData,
            this.childItemExportData});
            this.childItemExportImport.Text = "Export/Import";
            // 
            // childItemImportData
            // 
            this.childItemImportData.Index = 0;
            this.childItemImportData.Text = "Import Data";
            this.childItemImportData.Click += new System.EventHandler(this.childItemImportData_Click);
            // 
            // childItemExportData
            // 
            this.childItemExportData.Index = 1;
            this.childItemExportData.Text = "Export Data";
            this.childItemExportData.Click += new System.EventHandler(this.childItemExportData_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 1;
            this.menuItem4.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.childItemPasteData,
            this.childItemLimitWizard});
            this.menuItem4.Text = "Edit";
            // 
            // childItemPasteData
            // 
            this.childItemPasteData.Index = 0;
            this.childItemPasteData.Shortcut = System.Windows.Forms.Shortcut.CtrlD;
            this.childItemPasteData.Text = "Paste Data";
            this.childItemPasteData.Click += new System.EventHandler(this.childItemPasteData_Click);
            // 
            // childItemLimitWizard
            // 
            this.childItemLimitWizard.Index = 1;
            this.childItemLimitWizard.Shortcut = System.Windows.Forms.Shortcut.CtrlL;
            this.childItemLimitWizard.Text = "Limit Table Wizard";
            this.childItemLimitWizard.Click += new System.EventHandler(this.childItemLimitWizard_Click);
            // 
            // IOTab
            // 
            this.IOTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IOTab.Controls.Add(this.inputPage);
            this.IOTab.Controls.Add(this.outputPage);
            this.IOTab.Controls.Add(this.limitPage);
            this.IOTab.Location = new System.Drawing.Point(23, 39);
            this.IOTab.Margin = new System.Windows.Forms.Padding(2);
            this.IOTab.Name = "IOTab";
            this.IOTab.SelectedIndex = 0;
            this.IOTab.Size = new System.Drawing.Size(614, 628);
            this.IOTab.TabIndex = 8;
            // 
            // inputPage
            // 
            this.inputPage.Controls.Add(this.inputDGV);
            this.inputPage.Location = new System.Drawing.Point(4, 22);
            this.inputPage.Margin = new System.Windows.Forms.Padding(2);
            this.inputPage.Name = "inputPage";
            this.inputPage.Padding = new System.Windows.Forms.Padding(2);
            this.inputPage.Size = new System.Drawing.Size(606, 602);
            this.inputPage.TabIndex = 0;
            this.inputPage.Text = "Input";
            this.inputPage.UseVisualStyleBackColor = true;
            // 
            // inputDGV
            // 
            this.inputDGV.AllowUserToAddRows = false;
            this.inputDGV.AllowUserToDeleteRows = false;
            this.inputDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.inputDGV.Location = new System.Drawing.Point(9, 14);
            this.inputDGV.Margin = new System.Windows.Forms.Padding(2);
            this.inputDGV.Name = "inputDGV";
            this.inputDGV.RowTemplate.Height = 24;
            this.inputDGV.Size = new System.Drawing.Size(594, 586);
            this.inputDGV.TabIndex = 0;
            this.inputDGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.inputDGV_CellContentClick);
            // 
            // outputPage
            // 
            this.outputPage.Controls.Add(this.outputDGV);
            this.outputPage.Location = new System.Drawing.Point(4, 22);
            this.outputPage.Margin = new System.Windows.Forms.Padding(2);
            this.outputPage.Name = "outputPage";
            this.outputPage.Padding = new System.Windows.Forms.Padding(2);
            this.outputPage.Size = new System.Drawing.Size(606, 602);
            this.outputPage.TabIndex = 1;
            this.outputPage.Text = "Output";
            this.outputPage.UseVisualStyleBackColor = true;
            // 
            // outputDGV
            // 
            this.outputDGV.AllowUserToAddRows = false;
            this.outputDGV.AllowUserToDeleteRows = false;
            this.outputDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outputDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.outputDGV.Location = new System.Drawing.Point(9, 14);
            this.outputDGV.Margin = new System.Windows.Forms.Padding(2);
            this.outputDGV.Name = "outputDGV";
            this.outputDGV.RowTemplate.Height = 24;
            this.outputDGV.Size = new System.Drawing.Size(594, 584);
            this.outputDGV.TabIndex = 1;
            // 
            // limitPage
            // 
            this.limitPage.Controls.Add(this.limitDGV);
            this.limitPage.Location = new System.Drawing.Point(4, 22);
            this.limitPage.Margin = new System.Windows.Forms.Padding(2);
            this.limitPage.Name = "limitPage";
            this.limitPage.Padding = new System.Windows.Forms.Padding(2);
            this.limitPage.Size = new System.Drawing.Size(606, 602);
            this.limitPage.TabIndex = 2;
            this.limitPage.Text = "Limit";
            this.limitPage.UseVisualStyleBackColor = true;
            // 
            // limitDGV
            // 
            this.limitDGV.AllowUserToAddRows = false;
            this.limitDGV.AllowUserToDeleteRows = false;
            this.limitDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.limitDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.limitDGV.Location = new System.Drawing.Point(9, 14);
            this.limitDGV.Margin = new System.Windows.Forms.Padding(2);
            this.limitDGV.Name = "limitDGV";
            this.limitDGV.RowTemplate.Height = 24;
            this.limitDGV.Size = new System.Drawing.Size(594, 584);
            this.limitDGV.TabIndex = 2;
            // 
            // controllerDropList
            // 
            this.controllerDropList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.controllerDropList.FormattingEnabled = true;
            this.controllerDropList.Location = new System.Drawing.Point(23, 11);
            this.controllerDropList.Margin = new System.Windows.Forms.Padding(2);
            this.controllerDropList.Name = "controllerDropList";
            this.controllerDropList.Size = new System.Drawing.Size(98, 21);
            this.controllerDropList.TabIndex = 9;
            // 
            // toggleConnectButton
            // 
            this.toggleConnectButton.Location = new System.Drawing.Point(128, 10);
            this.toggleConnectButton.Name = "toggleConnectButton";
            this.toggleConnectButton.Size = new System.Drawing.Size(78, 22);
            this.toggleConnectButton.TabIndex = 10;
            this.toggleConnectButton.Text = "Connect!";
            this.toggleConnectButton.UseVisualStyleBackColor = true;
            this.toggleConnectButton.Click += new System.EventHandler(this.toggleConnectButton_Click);
            // 
            // deleteRowButton
            // 
            this.deleteRowButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deleteRowButton.Location = new System.Drawing.Point(637, 59);
            this.deleteRowButton.Margin = new System.Windows.Forms.Padding(2);
            this.deleteRowButton.Name = "deleteRowButton";
            this.deleteRowButton.Size = new System.Drawing.Size(60, 34);
            this.deleteRowButton.TabIndex = 11;
            this.deleteRowButton.Text = "Delete Row";
            this.deleteRowButton.UseVisualStyleBackColor = true;
            this.deleteRowButton.Click += new System.EventHandler(this.deleteRowButton_Click);
            // 
            // clearTableButton
            // 
            this.clearTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearTableButton.Location = new System.Drawing.Point(638, 98);
            this.clearTableButton.Margin = new System.Windows.Forms.Padding(2);
            this.clearTableButton.Name = "clearTableButton";
            this.clearTableButton.Size = new System.Drawing.Size(60, 34);
            this.clearTableButton.TabIndex = 12;
            this.clearTableButton.Text = "Clear Table";
            this.clearTableButton.UseVisualStyleBackColor = true;
            this.clearTableButton.Click += new System.EventHandler(this.clearTableButton_Click);
            // 
            // IOInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(702, 678);
            this.Controls.Add(this.clearTableButton);
            this.Controls.Add(this.deleteRowButton);
            this.Controls.Add(this.toggleConnectButton);
            this.Controls.Add(this.controllerDropList);
            this.Controls.Add(this.IOTab);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.toggleListenButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "IOInterface";
            this.Text = "WebInterface";
            this.Load += new System.EventHandler(this.IOInterface_Load);
            this.IOTab.ResumeLayout(false);
            this.inputPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.inputDGV)).EndInit();
            this.outputPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.outputDGV)).EndInit();
            this.limitPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.limitDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button toggleListenButton;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.TextBox portTextBox;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem childItemManageControllers;
        private System.Windows.Forms.TabControl IOTab;
        private System.Windows.Forms.TabPage inputPage;
        private System.Windows.Forms.TabPage outputPage;
        private System.Windows.Forms.TabPage limitPage;
        private System.Windows.Forms.ComboBox controllerDropList;
        private System.Windows.Forms.DataGridView inputDGV;
        private System.Windows.Forms.DataGridView outputDGV;
        private System.Windows.Forms.DataGridView limitDGV;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem childItemPasteData;
        private System.Windows.Forms.Button toggleConnectButton;
        private System.Windows.Forms.MenuItem childItemSaveIO2File;
        private System.Windows.Forms.Button deleteRowButton;
        private System.Windows.Forms.Button clearTableButton;
        private System.Windows.Forms.MenuItem childItemLimitWizard;
        private System.Windows.Forms.MenuItem childItemExportImport;
        private System.Windows.Forms.MenuItem childItemImportData;
        private System.Windows.Forms.MenuItem childItemExportData;
    }
}

