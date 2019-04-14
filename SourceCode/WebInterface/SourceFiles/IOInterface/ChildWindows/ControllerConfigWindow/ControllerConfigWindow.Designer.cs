namespace WebInterface
{
    partial class ControllerConfigWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControllerConfigWindow));
            this.controllerDGV = new System.Windows.Forms.DataGridView();
            this.controllerRemoveButton = new System.Windows.Forms.Button();
            this.closeWindowsButton = new System.Windows.Forms.Button();
            this.ControllerTypeDGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.controllerDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControllerTypeDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // controllerDGV
            // 
            this.controllerDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controllerDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.controllerDGV.Location = new System.Drawing.Point(112, 24);
            this.controllerDGV.Margin = new System.Windows.Forms.Padding(2);
            this.controllerDGV.Name = "controllerDGV";
            this.controllerDGV.RowHeadersVisible = false;
            this.controllerDGV.RowTemplate.Height = 24;
            this.controllerDGV.Size = new System.Drawing.Size(246, 398);
            this.controllerDGV.TabIndex = 0;
            // 
            // controllerRemoveButton
            // 
            this.controllerRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.controllerRemoveButton.Location = new System.Drawing.Point(370, 24);
            this.controllerRemoveButton.Margin = new System.Windows.Forms.Padding(2);
            this.controllerRemoveButton.Name = "controllerRemoveButton";
            this.controllerRemoveButton.Size = new System.Drawing.Size(56, 28);
            this.controllerRemoveButton.TabIndex = 1;
            this.controllerRemoveButton.Text = "Remove";
            this.controllerRemoveButton.UseVisualStyleBackColor = true;
            this.controllerRemoveButton.Click += new System.EventHandler(this.controllerRemoveButton_Click);
            // 
            // closeWindowsButton
            // 
            this.closeWindowsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeWindowsButton.Location = new System.Drawing.Point(370, 394);
            this.closeWindowsButton.Margin = new System.Windows.Forms.Padding(2);
            this.closeWindowsButton.Name = "closeWindowsButton";
            this.closeWindowsButton.Size = new System.Drawing.Size(56, 28);
            this.closeWindowsButton.TabIndex = 4;
            this.closeWindowsButton.Text = "Close";
            this.closeWindowsButton.UseVisualStyleBackColor = true;
            this.closeWindowsButton.Click += new System.EventHandler(this.closeWindowsButton_Click);
            // 
            // ControllerTypeDGV
            // 
            this.ControllerTypeDGV.AllowUserToAddRows = false;
            this.ControllerTypeDGV.AllowUserToDeleteRows = false;
            this.ControllerTypeDGV.AllowUserToResizeColumns = false;
            this.ControllerTypeDGV.AllowUserToResizeRows = false;
            this.ControllerTypeDGV.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ControllerTypeDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ControllerTypeDGV.Location = new System.Drawing.Point(13, 24);
            this.ControllerTypeDGV.MultiSelect = false;
            this.ControllerTypeDGV.Name = "ControllerTypeDGV";
            this.ControllerTypeDGV.RowHeadersVisible = false;
            this.ControllerTypeDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ControllerTypeDGV.Size = new System.Drawing.Size(94, 398);
            this.ControllerTypeDGV.TabIndex = 5;
            // 
            // ControllerConfigWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 431);
            this.Controls.Add(this.ControllerTypeDGV);
            this.Controls.Add(this.closeWindowsButton);
            this.Controls.Add(this.controllerRemoveButton);
            this.Controls.Add(this.controllerDGV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ControllerConfigWindow";
            this.Text = "Controller Manager";
            ((System.ComponentModel.ISupportInitialize)(this.controllerDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ControllerTypeDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView controllerDGV;
        private System.Windows.Forms.Button controllerRemoveButton;
        private System.Windows.Forms.Button closeWindowsButton;
        private System.Windows.Forms.DataGridView ControllerTypeDGV;
    }
}