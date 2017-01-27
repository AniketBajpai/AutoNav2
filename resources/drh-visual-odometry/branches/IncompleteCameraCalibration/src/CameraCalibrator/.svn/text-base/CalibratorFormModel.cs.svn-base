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
using CameraCalibrator.Support;

namespace CameraCalibrator
{
	public class CalibratorFormModel : IDisposable
	{
		public ChessBoard ChessBoard { get; private set; }
		private Capture m_Capture;
		private bool m_FlipHorizontal;
		private bool m_FlipVertical;
		private CalibratorState m_State = CalibratorState.Initial;

		private bool m_CanStartCapture;
		private bool m_CanAccept;
		private bool m_CanReject;

		public Image<Bgr, Byte> OriginalImage { get; private set; }
		public Image<Gray, Byte> GrayImage { get; private set; }
		//public Image<Bgr, Byte> FoundCornersImage { get; private set; }
		PointF[] m_FoundCorners;

		List<PointF[]> m_AcceptedImageCorners = new List<PointF[]>();
		private IntrinsicCameraParameters m_IntrinsicCameraParameters;

		public event EventHandler Changed;

		public CalibratorFormModel(ChessBoard chessBoard)
		{
			this.ChessBoard = chessBoard;
			this.State = CalibratorState.FreeRunning;
			InitializeCapture();
		}

		private void InitializeCapture()
		{
			m_Capture = new Capture();
		}

		public void ProcessFrame()
		{
			if (this.State == CalibratorState.CornersRecognized | this.State == CalibratorState.Calibrated)
			{
				return;
			}

			this.OriginalImage = m_Capture.QueryFrame();
			this.GrayImage = this.OriginalImage.Convert<Gray, Byte>();

			bool foundAllCorners = CameraCalibration.FindChessboardCorners(
				this.GrayImage,
				this.ChessBoard.PatternSize,
				Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS,
				out m_FoundCorners);

			PointF[][] foundPointsForChannels = new PointF[][] { m_FoundCorners };
			if (foundAllCorners)
			{
				MCvTermCriteria terminationCriteria;
				terminationCriteria.max_iter = 30;
				terminationCriteria.epsilon = 0.1;
				terminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

				this.GrayImage.FindCornerSubPix(foundPointsForChannels, new Size(11, 11), new Size(-1, -1), terminationCriteria);

				CameraCalibration.DrawChessboardCorners(this.GrayImage, this.ChessBoard.PatternSize, m_FoundCorners, foundAllCorners);
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

			if (foundAllCorners && this.State == CalibratorState.WaitingForCornersRecognition)
			{
				this.State = CalibratorState.CornersRecognized;
			}
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

		private CalibratorState State
		{
			get { return m_State; }
			set
			{
				m_State = value;
				switch (m_State)
				{
					case CalibratorState.Initial:
						return;
					case CalibratorState.FreeRunning:
						this.CanStartCapture = true;
						this.CanAccept = false;
						this.CanReject = false;
						return;
					case CalibratorState.WaitingForCornersRecognition:
						this.CanStartCapture = false;
						this.CanAccept = false;
						this.CanReject = false;
						return;
					case CalibratorState.CornersRecognized:
						this.CanStartCapture = false;
						this.CanAccept = true;
						this.CanReject = true;
						return;
					case CalibratorState.Calibrated:
						this.CanStartCapture = false;
						this.CanAccept = false;
						this.CanReject = false;
						return;
					default:
						break;
				}
			}
		}

		public bool CanStartCapture
		{
			get { return m_CanStartCapture; }
			private set
			{
				if (value != m_CanStartCapture)
				{
					m_CanStartCapture = value;
					RaiseChangedEvent();
				}
			}
		}

		public bool CanAccept
		{
			get { return m_CanAccept; }
			private set
			{
				if (value != m_CanAccept)
				{
					m_CanAccept = value;
					RaiseChangedEvent();
				}
			}
		}

		public bool CanReject
		{
			get { return m_CanReject; }
			private set
			{
				if (value != m_CanReject)
				{
					m_CanReject = value;
					RaiseChangedEvent();
				}
			}
		}

		public void StartCapture()
		{
			if (!this.CanStartCapture)
			{
				throw new InvalidOperationException();
			}

			this.State = CalibratorState.WaitingForCornersRecognition;
		}

		public void Accept()
		{
			if (!this.CanAccept)
			{
				throw new InvalidOperationException();
			}

			// do accept
			m_AcceptedImageCorners.Add(m_FoundCorners);
			this.State = CalibratorState.FreeRunning;
		}

		public void Reject()
		{
			if (!this.CanReject)
			{
				throw new InvalidOperationException();
			}

			// do reject
			this.State = CalibratorState.FreeRunning;
		}

		public int AcceptedImagesCount
		{
			get { return m_AcceptedImageCorners.Count; }
		}

		public void Calibrate()
		{
			MCvPoint3D32f[][] cornerPointsOfImages = new MCvPoint3D32f[this.AcceptedImagesCount][];
			for(int i = 0; i < this.AcceptedImagesCount; i++)
			{
				// for each captured image the physical coordinates of the corner points of the chess board are the same
				// in the coordinate system of the chess board.
				cornerPointsOfImages[i] = this.ChessBoard.CornerPoints;
			}

			IntrinsicCameraParameters intrinsicCameraParameters = new IntrinsicCameraParameters();
			// We initialize the intrinsic matrix such that the two focal lengths have a ratio of 1.0
			intrinsicCameraParameters.IntrinsicMatrix[0, 0] = 1.0;
			intrinsicCameraParameters.IntrinsicMatrix[1, 1] = 1.0;

			ExtrinsicCameraParameters[] extrinsicCameraParametersForImages; // will hold the rotation and translation for each captured chess board
			CameraCalibration.CalibrateCamera(
				cornerPointsOfImages,
				m_AcceptedImageCorners.ToArray(),
				this.OriginalImage.Size,
				intrinsicCameraParameters,
				Emgu.CV.CvEnum.CALIB_TYPE.CV_CALIB_FIX_ASPECT_RATIO | Emgu.CV.CvEnum.CALIB_TYPE.CV_CALIB_FIX_K3 | Emgu.CV.CvEnum.CALIB_TYPE.CV_CALIB_ZERO_TANGENT_DIST,
				out extrinsicCameraParametersForImages);

			m_IntrinsicCameraParameters = intrinsicCameraParameters;
			this.State = CalibratorState.Calibrated;
		}

		internal CalibrationParametersFormModel CreateCalibrationParametersFormModel()
		{
			return new CalibrationParametersFormModel(m_IntrinsicCameraParameters, m_Capture);
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
