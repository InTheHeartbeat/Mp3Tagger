namespace Mp3Tagger
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBarToolStrip = new System.Windows.Forms.ToolStrip();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.compositionsDataGrid = new System.Windows.Forms.DataGridView();
            this.statusBarProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.statusBarProgressLabel = new System.Windows.Forms.ToolStripLabel();
            this.mainToolStrip.SuspendLayout();
            this.statusBarToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.compositionsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.mainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.Size = new System.Drawing.Size(784, 25);
            this.mainToolStrip.TabIndex = 0;
            this.mainToolStrip.Text = "mainToolStrip";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 22);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // statusBarToolStrip
            // 
            this.statusBarToolStrip.BackColor = System.Drawing.Color.LightGray;
            this.statusBarToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusBarToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.statusBarToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarProgressBar,
            this.statusBarProgressLabel});
            this.statusBarToolStrip.Location = new System.Drawing.Point(0, 536);
            this.statusBarToolStrip.Name = "statusBarToolStrip";
            this.statusBarToolStrip.Size = new System.Drawing.Size(784, 25);
            this.statusBarToolStrip.TabIndex = 1;
            this.statusBarToolStrip.Text = "statusBarToolStrip";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 25);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainer2.Size = new System.Drawing.Size(784, 511);
            this.splitContainer2.SplitterDistance = 408;
            this.splitContainer2.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.AllowDrop = true;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.compositionsDataGrid);
            this.splitContainer1.Size = new System.Drawing.Size(784, 408);
            this.splitContainer1.SplitterDistance = 610;
            this.splitContainer1.TabIndex = 0;
            // 
            // compositionsDataGrid
            // 
            this.compositionsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.compositionsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.compositionsDataGrid.Location = new System.Drawing.Point(3, 3);
            this.compositionsDataGrid.Name = "compositionsDataGrid";
            this.compositionsDataGrid.Size = new System.Drawing.Size(604, 402);
            this.compositionsDataGrid.TabIndex = 0;
            // 
            // statusBarProgressBar
            // 
            this.statusBarProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusBarProgressBar.AutoSize = false;
            this.statusBarProgressBar.Margin = new System.Windows.Forms.Padding(1, 2, 10, 1);
            this.statusBarProgressBar.Name = "statusBarProgressBar";
            this.statusBarProgressBar.Size = new System.Drawing.Size(100, 18);
            // 
            // statusBarProgressLabel
            // 
            this.statusBarProgressLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusBarProgressLabel.Margin = new System.Windows.Forms.Padding(0, 1, 3, 2);
            this.statusBarProgressLabel.Name = "statusBarProgressLabel";
            this.statusBarProgressLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer2);
            this.Controls.Add(this.statusBarToolStrip);
            this.Controls.Add(this.mainToolStrip);
            this.Name = "Form1";
            this.Text = "Mp3Tagger";
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            this.statusBarToolStrip.ResumeLayout(false);
            this.statusBarToolStrip.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.compositionsDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStrip statusBarToolStrip;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView compositionsDataGrid;
        private System.Windows.Forms.ToolStripProgressBar statusBarProgressBar;
        private System.Windows.Forms.ToolStripLabel statusBarProgressLabel;
    }
}

