
namespace VPW2ROMID
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
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lblRomFilePath = new System.Windows.Forms.Label();
			this.tbRomFilePath = new System.Windows.Forms.TextBox();
			this.btnBrowseRom = new System.Windows.Forms.Button();
			this.btnVerifyRom = new System.Windows.Forms.Button();
			this.tbOutput = new System.Windows.Forms.TextBox();
			this.gbOutput = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.gbOutput.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Controls.Add(this.lblRomFilePath, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tbRomFilePath, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnBrowseRom, 2, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(600, 33);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// lblRomFilePath
			// 
			this.lblRomFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRomFilePath.AutoSize = true;
			this.lblRomFilePath.Location = new System.Drawing.Point(3, 10);
			this.lblRomFilePath.Name = "lblRomFilePath";
			this.lblRomFilePath.Size = new System.Drawing.Size(114, 13);
			this.lblRomFilePath.TabIndex = 0;
			this.lblRomFilePath.Text = "Path to &ROM File";
			// 
			// tbRomFilePath
			// 
			this.tbRomFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.tbRomFilePath.Location = new System.Drawing.Point(123, 6);
			this.tbRomFilePath.Name = "tbRomFilePath";
			this.tbRomFilePath.Size = new System.Drawing.Size(354, 20);
			this.tbRomFilePath.TabIndex = 1;
			// 
			// btnBrowseRom
			// 
			this.btnBrowseRom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.btnBrowseRom.Location = new System.Drawing.Point(483, 5);
			this.btnBrowseRom.Name = "btnBrowseRom";
			this.btnBrowseRom.Size = new System.Drawing.Size(114, 23);
			this.btnBrowseRom.TabIndex = 2;
			this.btnBrowseRom.Text = "&Browse...";
			this.btnBrowseRom.UseVisualStyleBackColor = true;
			this.btnBrowseRom.Click += new System.EventHandler(this.btnBrowseRom_Click);
			// 
			// btnVerifyRom
			// 
			this.btnVerifyRom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnVerifyRom.Location = new System.Drawing.Point(12, 55);
			this.btnVerifyRom.Name = "btnVerifyRom";
			this.btnVerifyRom.Size = new System.Drawing.Size(600, 34);
			this.btnVerifyRom.TabIndex = 3;
			this.btnVerifyRom.Text = "&Verify ROM";
			this.btnVerifyRom.UseVisualStyleBackColor = true;
			this.btnVerifyRom.Click += new System.EventHandler(this.btnVerifyRom_Click);
			// 
			// tbOutput
			// 
			this.tbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbOutput.Location = new System.Drawing.Point(3, 16);
			this.tbOutput.Multiline = true;
			this.tbOutput.Name = "tbOutput";
			this.tbOutput.ReadOnly = true;
			this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbOutput.Size = new System.Drawing.Size(594, 155);
			this.tbOutput.TabIndex = 4;
			// 
			// gbOutput
			// 
			this.gbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gbOutput.Controls.Add(this.tbOutput);
			this.gbOutput.Location = new System.Drawing.Point(12, 95);
			this.gbOutput.Name = "gbOutput";
			this.gbOutput.Size = new System.Drawing.Size(600, 174);
			this.gbOutput.TabIndex = 4;
			this.gbOutput.TabStop = false;
			this.gbOutput.Text = "&Output";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(624, 281);
			this.Controls.Add(this.gbOutput);
			this.Controls.Add(this.btnVerifyRom);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.Text = "Virtual Pro-Wrestling 2 ROM Identifier";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.gbOutput.ResumeLayout(false);
			this.gbOutput.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lblRomFilePath;
		private System.Windows.Forms.TextBox tbRomFilePath;
		private System.Windows.Forms.Button btnBrowseRom;
		private System.Windows.Forms.Button btnVerifyRom;
		private System.Windows.Forms.TextBox tbOutput;
		private System.Windows.Forms.GroupBox gbOutput;
	}
}

