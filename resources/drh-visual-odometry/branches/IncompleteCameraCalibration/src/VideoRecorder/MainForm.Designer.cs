namespace VideoRecorder
{
	partial class MainForm
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
			this.m_BottomPanel = new System.Windows.Forms.Panel();
			this.m_StartStopButton = new System.Windows.Forms.Button();
			this.m_ImageBox = new Emgu.CV.UI.ImageBox();
			this.m_Timer = new System.Windows.Forms.Timer(this.components);
			this.m_BottomPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_ImageBox)).BeginInit();
			this.SuspendLayout();
			// 
			// m_BottomPanel
			// 
			this.m_BottomPanel.Controls.Add(this.m_StartStopButton);
			this.m_BottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.m_BottomPanel.Location = new System.Drawing.Point(0, 555);
			this.m_BottomPanel.Name = "m_BottomPanel";
			this.m_BottomPanel.Size = new System.Drawing.Size(835, 46);
			this.m_BottomPanel.TabIndex = 0;
			// 
			// m_StartStopButton
			// 
			this.m_StartStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.m_StartStopButton.Location = new System.Drawing.Point(748, 11);
			this.m_StartStopButton.Name = "m_StartStopButton";
			this.m_StartStopButton.Size = new System.Drawing.Size(75, 23);
			this.m_StartStopButton.TabIndex = 0;
			this.m_StartStopButton.Text = "Start";
			this.m_StartStopButton.UseVisualStyleBackColor = true;
			this.m_StartStopButton.Click += new System.EventHandler(this.m_StartStopButton_Click);
			// 
			// m_ImageBox
			// 
			this.m_ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.m_ImageBox.Location = new System.Drawing.Point(0, 0);
			this.m_ImageBox.Name = "m_ImageBox";
			this.m_ImageBox.Size = new System.Drawing.Size(835, 555);
			this.m_ImageBox.TabIndex = 2;
			this.m_ImageBox.TabStop = false;
			// 
			// m_Timer
			// 
			this.m_Timer.Tick += new System.EventHandler(this.OnTimerTick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(835, 601);
			this.Controls.Add(this.m_ImageBox);
			this.Controls.Add(this.m_BottomPanel);
			this.Name = "MainForm";
			this.Text = "Recorder";
			this.m_BottomPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.m_ImageBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel m_BottomPanel;
		private Emgu.CV.UI.ImageBox m_ImageBox;
		private System.Windows.Forms.Button m_StartStopButton;
		private System.Windows.Forms.Timer m_Timer;
	}
}