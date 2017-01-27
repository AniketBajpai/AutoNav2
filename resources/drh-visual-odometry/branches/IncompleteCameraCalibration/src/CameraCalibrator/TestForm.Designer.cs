namespace CameraCalibrator
{
	partial class TestForm
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
			this.m_BottomPanel = new System.Windows.Forms.Panel();
			this.m_OKButton = new System.Windows.Forms.Button();
			this.m_ImagesSplitter = new System.Windows.Forms.SplitContainer();
			this.m_RawImageBoxWithHeading = new Vision.WinForm.ImageBoxWithHeading();
			this.m_CorrectedImageBoxWithHeading = new Vision.WinForm.ImageBoxWithHeading();
			this.m_BottomPanel.SuspendLayout();
			this.m_ImagesSplitter.Panel1.SuspendLayout();
			this.m_ImagesSplitter.Panel2.SuspendLayout();
			this.m_ImagesSplitter.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_BottomPanel
			// 
			this.m_BottomPanel.Controls.Add(this.m_OKButton);
			this.m_BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_BottomPanel.Location = new System.Drawing.Point(0, 465);
			this.m_BottomPanel.Name = "m_BottomPanel";
			this.m_BottomPanel.Size = new System.Drawing.Size(849, 45);
			this.m_BottomPanel.TabIndex = 0;
			// 
			// m_OKButton
			// 
			this.m_OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_OKButton.Location = new System.Drawing.Point(762, 10);
			this.m_OKButton.Name = "m_OKButton";
			this.m_OKButton.Size = new System.Drawing.Size(75, 23);
			this.m_OKButton.TabIndex = 0;
			this.m_OKButton.Text = "OK";
			this.m_OKButton.UseVisualStyleBackColor = true;
			this.m_OKButton.Click += new System.EventHandler(this.OnOKButtonClick);
			// 
			// m_ImagesSplitter
			// 
			this.m_ImagesSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ImagesSplitter.Location = new System.Drawing.Point(0, 0);
			this.m_ImagesSplitter.Name = "m_ImagesSplitter";
			// 
			// m_ImagesSplitter.Panel1
			// 
			this.m_ImagesSplitter.Panel1.Controls.Add(this.m_RawImageBoxWithHeading);
			// 
			// m_ImagesSplitter.Panel2
			// 
			this.m_ImagesSplitter.Panel2.Controls.Add(this.m_CorrectedImageBoxWithHeading);
			this.m_ImagesSplitter.Size = new System.Drawing.Size(849, 465);
			this.m_ImagesSplitter.SplitterDistance = 423;
			this.m_ImagesSplitter.TabIndex = 1;
			// 
			// m_RawImageBoxWithHeading
			// 
			this.m_RawImageBoxWithHeading.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_RawImageBoxWithHeading.Heading = "Raw Image";
			this.m_RawImageBoxWithHeading.Location = new System.Drawing.Point(0, 0);
			this.m_RawImageBoxWithHeading.Name = "m_RawImageBoxWithHeading";
			this.m_RawImageBoxWithHeading.Size = new System.Drawing.Size(423, 465);
			this.m_RawImageBoxWithHeading.TabIndex = 2;
			// 
			// m_CorrectedImageBoxWithHeading1
			// 
			this.m_CorrectedImageBoxWithHeading.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_CorrectedImageBoxWithHeading.Heading = "Corrected";
			this.m_CorrectedImageBoxWithHeading.Location = new System.Drawing.Point(0, 0);
			this.m_CorrectedImageBoxWithHeading.Name = "m_CorrectedImageBoxWithHeading1";
			this.m_CorrectedImageBoxWithHeading.Size = new System.Drawing.Size(422, 465);
			this.m_CorrectedImageBoxWithHeading.TabIndex = 3;
			// 
			// TestForm
			// 
			this.AcceptButton = this.m_OKButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(849, 510);
			this.Controls.Add(this.m_ImagesSplitter);
			this.Controls.Add(this.m_BottomPanel);
			this.Name = "TestForm";
			this.Text = "Test Calibration";
			this.m_BottomPanel.ResumeLayout(false);
			this.m_ImagesSplitter.Panel1.ResumeLayout(false);
			this.m_ImagesSplitter.Panel2.ResumeLayout(false);
			this.m_ImagesSplitter.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_BottomPanel;
		private System.Windows.Forms.Button m_OKButton;
		private System.Windows.Forms.SplitContainer m_ImagesSplitter;
		private Vision.WinForm.ImageBoxWithHeading m_RawImageBoxWithHeading;
		private Vision.WinForm.ImageBoxWithHeading m_CorrectedImageBoxWithHeading;
	}
}