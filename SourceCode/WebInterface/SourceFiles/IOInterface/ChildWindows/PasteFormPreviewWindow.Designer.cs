namespace WebInterface
{
    partial class PasteFormPreviewWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasteFormPreviewWindow));
            this.pDGV = new System.Windows.Forms.DataGridView();
            this.confirmButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.appendButton = new System.Windows.Forms.Button();
            this.getCheckButton = new System.Windows.Forms.Button();
            this.clearCheckButton = new System.Windows.Forms.Button();
            this.AddRowButton = new System.Windows.Forms.Button();
            this.DeleteRowButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // pDGV
            // 
            this.pDGV.AllowUserToAddRows = false;
            this.pDGV.AllowUserToOrderColumns = true;
            this.pDGV.AllowUserToResizeRows = false;
            this.pDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pDGV.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.pDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pDGV.Location = new System.Drawing.Point(20, 46);
            this.pDGV.Margin = new System.Windows.Forms.Padding(2);
            this.pDGV.Name = "pDGV";
            this.pDGV.RowHeadersVisible = false;
            this.pDGV.RowTemplate.Height = 24;
            this.pDGV.Size = new System.Drawing.Size(330, 327);
            this.pDGV.TabIndex = 0;
            // 
            // confirmButton
            // 
            this.confirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmButton.Location = new System.Drawing.Point(191, 10);
            this.confirmButton.Margin = new System.Windows.Forms.Padding(2);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(76, 31);
            this.confirmButton.TabIndex = 1;
            this.confirmButton.Text = "Replace";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(274, 10);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(76, 31);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // appendButton
            // 
            this.appendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.appendButton.Location = new System.Drawing.Point(107, 10);
            this.appendButton.Margin = new System.Windows.Forms.Padding(2);
            this.appendButton.Name = "appendButton";
            this.appendButton.Size = new System.Drawing.Size(76, 31);
            this.appendButton.TabIndex = 3;
            this.appendButton.Text = "Append";
            this.appendButton.UseVisualStyleBackColor = true;
            this.appendButton.Click += new System.EventHandler(this.appendButton_Click);
            // 
            // getCheckButton
            // 
            this.getCheckButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.getCheckButton.Location = new System.Drawing.Point(191, 376);
            this.getCheckButton.Margin = new System.Windows.Forms.Padding(2);
            this.getCheckButton.Name = "getCheckButton";
            this.getCheckButton.Size = new System.Drawing.Size(76, 31);
            this.getCheckButton.TabIndex = 4;
            this.getCheckButton.Text = "GetCheck";
            this.getCheckButton.UseVisualStyleBackColor = true;
            this.getCheckButton.Click += new System.EventHandler(this.getCheckButton_Click);
            // 
            // clearCheckButton
            // 
            this.clearCheckButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.clearCheckButton.Location = new System.Drawing.Point(274, 376);
            this.clearCheckButton.Margin = new System.Windows.Forms.Padding(2);
            this.clearCheckButton.Name = "clearCheckButton";
            this.clearCheckButton.Size = new System.Drawing.Size(76, 31);
            this.clearCheckButton.TabIndex = 5;
            this.clearCheckButton.Text = "ClearCheck";
            this.clearCheckButton.UseVisualStyleBackColor = true;
            this.clearCheckButton.Click += new System.EventHandler(this.clearCheckButton_Click);
            // 
            // AddRowButton
            // 
            this.AddRowButton.Location = new System.Drawing.Point(20, 20);
            this.AddRowButton.Margin = new System.Windows.Forms.Padding(2);
            this.AddRowButton.Name = "AddRowButton";
            this.AddRowButton.Size = new System.Drawing.Size(20, 20);
            this.AddRowButton.TabIndex = 6;
            this.AddRowButton.Text = "+";
            this.AddRowButton.UseVisualStyleBackColor = true;
            this.AddRowButton.Click += new System.EventHandler(this.AddRowButton_Click);
            // 
            // DeleteRowButton
            // 
            this.DeleteRowButton.Location = new System.Drawing.Point(44, 20);
            this.DeleteRowButton.Margin = new System.Windows.Forms.Padding(2);
            this.DeleteRowButton.Name = "DeleteRowButton";
            this.DeleteRowButton.Size = new System.Drawing.Size(20, 20);
            this.DeleteRowButton.TabIndex = 7;
            this.DeleteRowButton.Text = "-";
            this.DeleteRowButton.UseVisualStyleBackColor = true;
            this.DeleteRowButton.Click += new System.EventHandler(this.DeleteRowButton_Click);
            // 
            // PasteFormPreviewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 414);
            this.Controls.Add(this.DeleteRowButton);
            this.Controls.Add(this.AddRowButton);
            this.Controls.Add(this.clearCheckButton);
            this.Controls.Add(this.getCheckButton);
            this.Controls.Add(this.appendButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.pDGV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PasteFormPreviewWindow";
            this.Text = "Table Preview";
            ((System.ComponentModel.ISupportInitialize)(this.pDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView pDGV;
        private System.Windows.Forms.Button confirmButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button appendButton;
        private System.Windows.Forms.Button getCheckButton;
        private System.Windows.Forms.Button clearCheckButton;
        private System.Windows.Forms.Button AddRowButton;
        private System.Windows.Forms.Button DeleteRowButton;
    }
}