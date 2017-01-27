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
using System.IO;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using CameraCalibrator.WinForm.Support;
using System.Drawing;
using CameraCalibrator.Support;

namespace CameraCalibrator
{
	public class CalibrateDialogModel
	{
		private MainModel m_MainModel;
		public IntrinsicCameraParameters IntrinsicCameraParameters { get; private set; }
		private CALIB_TYPE m_CalibrationFlags;
		private Size m_ImageSize;

		public CalibrateDialogModel(MainModel mainModel)
		{
			m_MainModel = mainModel;
			InitializeCameraParameters();
			m_CalibrationFlags = CALIB_TYPE.CV_CALIB_FIX_ASPECT_RATIO | CALIB_TYPE.CV_CALIB_FIX_K3 | CALIB_TYPE.CV_CALIB_ZERO_TANGENT_DIST;
		}

		private void InitializeCameraParameters()
		{
			this.IntrinsicCameraParameters = new IntrinsicCameraParameters();

			// We initialize the intrinsic matrix such that the two focal lengths have a ratio of 1.0
			this.IntrinsicCameraParameters.IntrinsicMatrix[0, 0] = 1.0;
			this.IntrinsicCameraParameters.IntrinsicMatrix[1, 1] = 1.0;

			// Also we start with the assumption that the center is at the center of the image
			FileInfo[] calibrationImageFiles = GetCalibrationImageFiles();

			Image<Bgr, Byte> calibrationImage = new Image<Bgr, Byte>(calibrationImageFiles[0].FullName);

			m_ImageSize = calibrationImage.Size;
			double cx = calibrationImage.Width / 2.0;
			double cy = calibrationImage.Height / 2.0;

			this.IntrinsicCameraParameters.IntrinsicMatrix[0, 2] = cx;
			this.IntrinsicCameraParameters.IntrinsicMatrix[1, 2] = cy;
		}

		private FileInfo[] GetCalibrationImageFiles()
		{
			DirectoryInfo imagesFolder = new DirectoryInfo(m_MainModel.CaptureSettings.OutputFolderPath);
			return imagesFolder.GetFiles("*.png", SearchOption.TopDirectoryOnly);
		}

		public void Calibrate()
		{
			FileInfo[] calibrationImageFiles = GetCalibrationImageFiles();

			MCvPoint3D32f[][] cornerPointsOfImages = new MCvPoint3D32f[calibrationImageFiles.Length][];
			for (int i = 0; i < calibrationImageFiles.Length; i++)
			{
				// for each captured image the physical coordinates of the corner points of the chess board are the same
				// in the coordinate system of the chess board.
				cornerPointsOfImages[i] = m_MainModel.CaptureSettings.ChessBoard.CornerPoints;
			}

			PointF[][] imageCorners = CollectImageCorners(calibrationImageFiles);

			ExtrinsicCameraParameters[] extrinsicCameraParametersForImages; // will hold the rotation and translation for each captured chess board
			CameraCalibration.CalibrateCamera(
				cornerPointsOfImages,
				imageCorners.ToArray(),
				m_ImageSize,
				this.IntrinsicCameraParameters,
				this.CalibrationFlags,
				out extrinsicCameraParametersForImages);
		}

		private PointF[][] CollectImageCorners(FileInfo[] calibrationImageFiles)
		{
			List<PointF[]> imageCornersList = new List<PointF[]>();
			foreach (FileInfo calibrationImageFile in calibrationImageFiles)
			{
				PointF[] imageCorners = CollectImageCorners(calibrationImageFile);
				imageCornersList.Add(imageCorners);
			}
			return imageCornersList.ToArray();
		}

		private PointF[] CollectImageCorners(FileInfo calibrationImageFile)
		{
			Image<Bgr, Byte> calibrationImage = new Image<Bgr, Byte>(calibrationImageFile.FullName);
			Image<Gray, Byte> grayImage = calibrationImage.Convert<Gray, Byte>();
			
			PointF[] foundCorners;
			bool foundAllCorners = CameraCalibration.FindChessboardCorners(
				grayImage,
				m_MainModel.CaptureSettings.ChessBoard.PatternSize,
				Emgu.CV.CvEnum.CALIB_CB_TYPE.ADAPTIVE_THRESH | Emgu.CV.CvEnum.CALIB_CB_TYPE.FILTER_QUADS,
				out foundCorners);

			PointF[][] foundPointsForChannels = new PointF[][] { foundCorners };
			if (foundAllCorners)
			{
				MCvTermCriteria terminationCriteria;
				terminationCriteria.max_iter = 30;
				terminationCriteria.epsilon = 0.1;
				terminationCriteria.type = Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_EPS | Emgu.CV.CvEnum.TERMCRIT.CV_TERMCRIT_ITER;

				grayImage.FindCornerSubPix(foundPointsForChannels, new Size(11, 11), new Size(-1, -1), terminationCriteria);
			}

			return foundCorners;
		}

		public TestFormModel CreateTestFormModel()
		{
			return new TestFormModel(this.IntrinsicCameraParameters);
		}

		public void SaveCameraParameters(string filePath)
		{
			IntrinsicParametersSupport.Save(this.IntrinsicCameraParameters, filePath);
		}

		public void LoadCameraParameters(string filePath)
		{
			this.IntrinsicCameraParameters = IntrinsicParametersSupport.Load(filePath);
		}

		public CALIB_TYPE CalibrationFlags
		{
			get { return m_CalibrationFlags; }
			set { m_CalibrationFlags = value; }
		}
	}
}
