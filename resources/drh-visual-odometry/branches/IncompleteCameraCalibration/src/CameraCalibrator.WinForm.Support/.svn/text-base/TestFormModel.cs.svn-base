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
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CameraCalibrator.WinForm.Support
{
	public class TestFormModel : IDisposable
	{
		private Capture m_Capture;
		private IntrinsicCameraParameters m_IntrinsicCameraParameters;

		public Image<Bgr, Byte> OriginalImage { get; private set; }
		public Image<Gray, Byte> GrayImage { get; private set; }
		public Image<Bgr, Byte> CorrectedImage { get; private set; }

		private static Matrix<float> m_UndistortMapX;
		private static Matrix<float> m_UndistortMapY;

		public TestFormModel(IntrinsicCameraParameters intrinsicCameraParameters)
		{
			m_IntrinsicCameraParameters = intrinsicCameraParameters;
			m_Capture = new Capture();

			Initialize();
		}

		private void Initialize()
		{
			this.OriginalImage = m_Capture.QueryFrame();
			//this.GrayImage = this.OriginalImage.Convert<Gray, Byte>();

			m_IntrinsicCameraParameters.InitUndistortMap(
				this.OriginalImage.Width,
				this.OriginalImage.Height,
				out m_UndistortMapX,
				out m_UndistortMapY);
		}

		public void ProcessFrame()
		{
			if (m_Capture == null)
			{
				return;
			}
			this.OriginalImage = m_Capture.QueryFrame();
			this.GrayImage = this.OriginalImage.Convert<Gray, Byte>();

			Image<Gray, Byte> grayImage = this.OriginalImage.Convert<Gray, Byte>();
			this.CorrectedImage = m_IntrinsicCameraParameters.Undistort<Bgr, Byte>(this.OriginalImage);
		}

		public void Dispose()
		{
			if (m_Capture != null)
			{
				m_Capture.Dispose();
				m_Capture = null;
			}
		}
	}
}
