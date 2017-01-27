/******************************************************************************

    Visual, monocular odometry for robots using a regular web cam.
    Copyright (C) 2010  Rainer Hessmer, PhD
    
    Based on the paper by Jason Campbell et al. "A Robust Visual Odometry
    and Precipice Detection System Using Consumer-grade Monocular Vision"
    http://www.cs.cmu.edu/~personalrover/PER/ResearchersPapers/CampbellSukthankarNourbakhshPahwa_VisualOdometryCR.pdf

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Globalization;
using VideoRecorder.Properties;

namespace VideoRecorder
{
	public partial class MainForm : Form
	{
		private Capture m_Capture;
		public Image<Bgr, Byte> CurrentImage { get; private set; }
		private bool m_IsRecording;
		private VideoWriter m_VideoWriter;

		public MainForm()
		{
			InitializeComponent();
			m_Capture = new Capture(@"C:\Users\rainerh\Pictures\LifeCam Files\2010-07-11 11-59-20.065.wmv");
			//Application.Idle += OnApplicationIdle;
			m_Timer.Interval = 33;
			m_Timer.Enabled = true;
		}

		private void OnApplicationIdle(object sender, EventArgs e)
		{
			ProcessFrame();
		}

		private void ProcessFrame()
		{
			this.CurrentImage = m_Capture.QueryFrame();
			m_ImageBox.Image = this.CurrentImage;
			if (m_IsRecording)
			{
				m_VideoWriter.WriteFrame<Bgr, Byte>(this.CurrentImage);
			}
		}

		private void m_StartStopButton_Click(object sender, EventArgs e)
		{
			if (m_IsRecording)
			{
				m_IsRecording = false;
				StopRecording();
				m_StartStopButton.Text = "Start";
			}
			else
			{
				m_IsRecording = true;
				StartRecording();
				m_StartStopButton.Text = "Stop";
			}
		}

		private void StartRecording()
		{
			string recordingFileName = CreateRecordingFilePath();
			int width = (int)m_Capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH);
			int height = (int)m_Capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT);

			// NOTE: Requires that the xvid codec is installed. It can be downloaded from http://www.xvid.org/

			m_VideoWriter = new VideoWriter(
				recordingFileName,
				CvInvoke.CV_FOURCC('D', 'I', 'V', 'X'),
				(int)m_Capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS),
				width,
				height,
				true);
		}

		private void StopRecording()
		{
			m_VideoWriter.Dispose();
		}

		private string CreateRecordingFilePath()
		{
			return DateTime.Now.ToString("o", CultureInfo.InvariantCulture) + ".mjpg";
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			if (m_Capture != null)
			{
				m_Capture.Dispose();
			}
			if (m_VideoWriter != null)
			{
				m_VideoWriter.Dispose();
			}

			// If the MainForm is not minimized, save the current Size and 
			// location to the settings file.  Otherwise, the existing values 
			// in the settings file will not change...
			if (this.WindowState != FormWindowState.Minimized)
			{
				Settings.Default.Size = this.Size;
				Settings.Default.Location = this.Location;
			}
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			ProcessFrame();
		}
	}
}
