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
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;

namespace CameraCalibrator
{
	public class CaptureDialogModel : IDisposable
	{
		public CaptureSettings CaptureSettings { get; private set; }
		private Capture m_Capture;
		private bool m_FlipHorizontal;
		private bool m_FlipVertical;
		private DateTime m_TimeOfLastCapture = DateTime.MinValue;
		public int CurrentCapturedImagesCount { get; private set; }

		public Image<Bgr, Byte> OriginalImage { get; private set; }
		public Image<Gray, Byte> GrayImage { get; private set; }
		private PointF[] m_FoundCorners;

		public event EventHandler Changed;

		public CaptureDialogModel(CaptureSettings captureSettings)
		{
			this.CaptureSettings = captureSettings;
			InitializeCapture();
		}

		private void InitializeCapture()
		{
			m_Capture = new Capture();
			m_Capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 1280);
			m_Capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 720);
		}

		public void ProcessFrame()
		{
			if (m_Capture == null)
			{
				return;
			}
			this.OriginalImage = m_Capture.QueryFrame();
			this.GrayImage = this.OriginalImage.Convert<Gray, Byte>();

			bool foundAllCorners = CameraCalibration.FindChessboardCorners(
				this.GrayImage,
				this.CaptureSettings.ChessBoard.PatternSize,
				Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS,
				out m_FoundCorners);

			PointF[][] foundPointsForChannels = new PointF[][] { m_FoundCorners };
			if (foundAllCorners)
			{
				MCvTermCriteria terminationCriteria;
				terminationCriteria.max_iter = 30;
				terminationCriteria.epsilon = 0.05;
				terminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

				this.GrayImage.FindCornerSubPix(foundPointsForChannels, new Size(11, 11), new Size(-1, -1), terminationCriteria);

				CameraCalibration.DrawChessboardCorners(this.GrayImage, this.CaptureSettings.ChessBoard.PatternSize, m_FoundCorners, foundAllCorners);
			}

			// we are done with processing. If needed we flip the images for display purposes only.
			Emgu.CV.CvEnum.FLIP flipType = Emgu.CV.CvEnum.FLIP.NONE;
			if (this.FlipHorizontal)
			{
				flipType = Emgu.CV.CvEnum.FLIP.HORIZONTAL;
			}
			if (this.FlipVertical)
			{
				flipType = flipType |= Emgu.CV.CvEnum.FLIP.VERTICAL;
			}

			this.OriginalImage._Flip(flipType);
			this.GrayImage._Flip(flipType);

			if (!foundAllCorners)
			{
				return;
			}

			if (this.CurrentCapturedImagesCount >= this.CaptureSettings.ImagesCount)
			{
				// we got already all required images
				return;
			}

			DateTime utcNow = DateTime.UtcNow;
			if (utcNow.Ticks - m_TimeOfLastCapture.Ticks < this.CaptureSettings.WaitBetweenCaptures.Ticks)
			{
				// We need to wait longer
				return;
			}

			// We capture the image
			m_TimeOfLastCapture = utcNow;
			this.CurrentCapturedImagesCount++;
			this.OriginalImage.Save(this.CaptureSettings.GetFilePath(this.CurrentCapturedImagesCount));
			this.OriginalImage = this.OriginalImage.Not();

			RaiseChangedEvent();
		}

		public bool FlipHorizontal
		{
			get { return m_FlipHorizontal; }
			set
			{
				if (value != m_FlipHorizontal)
				{
					m_FlipHorizontal = value;
					RaiseChangedEvent();
				}
			}
		}

		public bool FlipVertical
		{
			get { return m_FlipVertical; }
			set
			{
				if (value != m_FlipVertical)
				{
					m_FlipVertical = value;
					RaiseChangedEvent();
				}
			}
		}

		private void RaiseChangedEvent()
		{
			EventHandler handler = this.Changed;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		public void Dispose()
		{
			if (m_Capture != null)
			{
				m_Capture.Dispose();
			}
		}
	}
}
