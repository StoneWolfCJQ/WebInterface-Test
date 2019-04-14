
namespace WebInterface
{
    partial class LimitTableWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LimitTableWizard));
            this.axisNumberBox = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.generateButton = new System.Windows.Forms.Button();
            this.tableAddButton = new System.Windows.Forms.Button();
            this.copyScriptButton = new System.Windows.Forms.Button();
            this.tab = new System.Windows.Forms.TabControl();
            this.tableTab = new System.Windows.Forms.TabPage();
            this.limitDGV = new System.Windows.Forms.DataGridView();
            this.scriptTab = new System.Windows.Forms.TabPage();
            this.scriptTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTab = new System.Windows.Forms.TabPage();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.getTableButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.axisNumberBox)).BeginInit();
            this.tab.SuspendLayout();
            this.tableTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.limitDGV)).BeginInit();
            this.scriptTab.SuspendLayout();
            this.descriptionTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // axisNumberBox
            // 
            this.axisNumberBox.Location = new System.Drawing.Point(16, 26);
            this.axisNumberBox.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.axisNumberBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.axisNumberBox.Name = "axisNumberBox";
            this.axisNumberBox.Size = new System.Drawing.Size(90, 22);
            this.axisNumberBox.TabIndex = 0;
            this.axisNumberBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of Axises";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(124, 24);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(84, 26);
            this.generateButton.TabIndex = 3;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // tableAddButton
            // 
            this.tableAddButton.Location = new System.Drawing.Point(316, 24);
            this.tableAddButton.Name = "tableAddButton";
            this.tableAddButton.Size = new System.Drawing.Size(97, 26);
            this.tableAddButton.TabIndex = 4;
            this.tableAddButton.Text = "Add to Table";
            this.tableAddButton.UseVisualStyleBackColor = true;
            this.tableAddButton.Click += new System.EventHandler(this.tableAddButton_Click);
            // 
            // copyScriptButton
            // 
            this.copyScriptButton.Location = new System.Drawing.Point(215, 24);
            this.copyScriptButton.Name = "copyScriptButton";
            this.copyScriptButton.Size = new System.Drawing.Size(93, 26);
            this.copyScriptButton.TabIndex = 5;
            this.copyScriptButton.Text = "Copy Script";
            this.copyScriptButton.UseVisualStyleBackColor = true;
            this.copyScriptButton.Click += new System.EventHandler(this.copyScriptButton_Click);
            // 
            // tab
            // 
            this.tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tab.Controls.Add(this.tableTab);
            this.tab.Controls.Add(this.scriptTab);
            this.tab.Controls.Add(this.descriptionTab);
            this.tab.Location = new System.Drawing.Point(16, 56);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(492, 484);
            this.tab.TabIndex = 6;
            // 
            // tableTab
            // 
            this.tableTab.Controls.Add(this.limitDGV);
            this.tableTab.Location = new System.Drawing.Point(4, 25);
            this.tableTab.Name = "tableTab";
            this.tableTab.Padding = new System.Windows.Forms.Padding(3);
            this.tableTab.Size = new System.Drawing.Size(484, 455);
            this.tableTab.TabIndex = 0;
            this.tableTab.Text = "Table";
            this.tableTab.UseVisualStyleBackColor = true;
            // 
            // limitDGV
            // 
            this.limitDGV.AllowUserToAddRows = false;
            this.limitDGV.AllowUserToDeleteRows = false;
            this.limitDGV.AllowUserToResizeRows = false;
            this.limitDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.limitDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.limitDGV.Location = new System.Drawing.Point(7, 7);
            this.limitDGV.Name = "limitDGV";
            this.limitDGV.RowHeadersVisible = false;
            this.limitDGV.RowTemplate.Height = 24;
            this.limitDGV.Size = new System.Drawing.Size(471, 442);
            this.limitDGV.TabIndex = 0;
            // 
            // scriptTab
            // 
            this.scriptTab.Controls.Add(this.scriptTextBox);
            this.scriptTab.Location = new System.Drawing.Point(4, 25);
            this.scriptTab.Name = "scriptTab";
            this.scriptTab.Padding = new System.Windows.Forms.Padding(3);
            this.scriptTab.Size = new System.Drawing.Size(484, 455);
            this.scriptTab.TabIndex = 1;
            this.scriptTab.Text = "Script";
            this.scriptTab.UseVisualStyleBackColor = true;
            // 
            // scriptTextBox
            // 
            this.scriptTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptTextBox.Location = new System.Drawing.Point(7, 7);
            this.scriptTextBox.Multiline = true;
            this.scriptTextBox.Name = "scriptTextBox";
            this.scriptTextBox.ReadOnly = true;
            this.scriptTextBox.Size = new System.Drawing.Size(471, 445);
            this.scriptTextBox.TabIndex = 0;
            // 
            // descriptionTab
            // 
            this.descriptionTab.Controls.Add(this.descriptionTextBox);
            this.descriptionTab.Location = new System.Drawing.Point(4, 25);
            this.descriptionTab.Name = "descriptionTab";
            this.descriptionTab.Padding = new System.Windows.Forms.Padding(3);
            this.descriptionTab.Size = new System.Drawing.Size(484, 455);
            this.descriptionTab.TabIndex = 2;
            this.descriptionTab.Text = "Axis Description";
            this.descriptionTab.UseVisualStyleBackColor = true;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionTextBox.Location = new System.Drawing.Point(7, 7);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(471, 442);
            this.descriptionTextBox.TabIndex = 0;
            // 
            // getTableButton
            // 
            this.getTableButton.Location = new System.Drawing.Point(419, 24);
            this.getTableButton.Name = "getTableButton";
            this.getTableButton.Size = new System.Drawing.Size(89, 26);
            this.getTableButton.TabIndex = 7;
            this.getTableButton.Text = "Get Table";
            this.getTableButton.UseVisualStyleBackColor = true;
            this.getTableButton.Click += new System.EventHandler(this.getTableButton_Click);
            // 
            // LimitTableWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 552);
            this.Controls.Add(this.getTableButton);
            this.Controls.Add(this.tab);
            this.Controls.Add(this.copyScriptButton);
            this.Controls.Add(this.tableAddButton);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axisNumberBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LimitTableWizard";
            this.Text = "Limit Table Creation Wizard";
            ((System.ComponentModel.ISupportInitialize)(this.axisNumberBox)).EndInit();
            this.tab.ResumeLayout(false);
            this.tableTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.limitDGV)).EndInit();
            this.scriptTab.ResumeLayout(false);
            this.scriptTab.PerformLayout();
            this.descriptionTab.ResumeLayout(false);
            this.descriptionTab.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown axisNumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button tableAddButton;
        private System.Windows.Forms.Button copyScriptButton;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tableTab;
        private System.Windows.Forms.DataGridView limitDGV;
        private System.Windows.Forms.TabPage scriptTab;
        private System.Windows.Forms.TextBox scriptTextBox;
        private System.Windows.Forms.TabPage descriptionTab;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Button getTableButton;
    }
}