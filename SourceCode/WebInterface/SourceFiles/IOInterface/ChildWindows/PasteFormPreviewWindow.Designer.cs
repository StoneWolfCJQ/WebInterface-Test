﻿namespace WebInterface
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
            this.pDGV.Location = new System.Drawing.Point(27, 57);
            this.pDGV.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pDGV.Name = "pDGV";
            this.pDGV.RowHeadersVisible = false;
            this.pDGV.RowTemplate.Height = 24;
            this.pDGV.Size = new System.Drawing.Size(429, 377);
            this.pDGV.TabIndex = 0;
            // 
            // confirmButton
            // 
            this.confirmButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.confirmButton.Location = new System.Drawing.Point(244, 12);
            this.confirmButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.confirmButton.Name = "confirmButton";
            this.confirmButton.Size = new System.Drawing.Size(101, 38);
            this.confirmButton.TabIndex = 1;
            this.confirmButton.Text = "Replace";
            this.confirmButton.UseVisualStyleBackColor = true;
            this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(355, 12);
            this.cancelButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(101, 38);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // appendButton
            // 
            this.appendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.appendButton.Location = new System.Drawing.Point(132, 12);
            this.appendButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.appendButton.Name = "appendButton";
            this.appendButton.Size = new System.Drawing.Size(101, 38);
            this.appendButton.TabIndex = 3;
            this.appendButton.Text = "Append";
            this.appendButton.UseVisualStyleBackColor = true;
            this.appendButton.Click += new System.EventHandler(this.appendButton_Click);
            // 
            // PasteFormPreviewWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 463);
            this.Controls.Add(this.appendButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.confirmButton);
            this.Controls.Add(this.pDGV);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
    }
}