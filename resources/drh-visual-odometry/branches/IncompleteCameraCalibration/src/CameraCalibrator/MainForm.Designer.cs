namespace CameraCalibrator
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
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(20, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "1.";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(52, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(135, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Settings ...";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnSettingsButtonClicked);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(52, 41);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(135, 23);
			this.button2.TabIndex = 3;
			this.button2.Text = "Capture images ...";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.OnCaptureImagesButtonClicked);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 44);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(20, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "2.";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(52, 70);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(135, 23);
			this.button3.TabIndex = 5;
			this.button3.Text = "Calibrate ...";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.OnCalibrateButtonClicked);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 73);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(20, 17);
			this.label3.TabIndex = 4;
			this.label3.Text = "3.";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(326, 108);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Name = "MainForm";
			this.Text = "Camera Calibration";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label3;
	}
}