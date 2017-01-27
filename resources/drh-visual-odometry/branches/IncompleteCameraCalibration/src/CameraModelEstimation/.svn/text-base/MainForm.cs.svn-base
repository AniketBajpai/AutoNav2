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
using System.Diagnostics;

namespace CameraModelEstimation
{
	public partial class MainForm : Form
	{
		private Size m_PatternSize = new Size(9, 7);
		public Image<Bgr, Byte> CalibrationImage { get; private set; }
		public Image<Gray, Byte> GrayImage { get; private set; }
		PointF[] m_FoundCorners;

		public MainForm()
		{
			InitializeComponent();
			this.CalibrationImage = new Image<Bgr, byte>(@"C:\svnDev\oss\Google\drh-visual-odometry\TestVideos\2010-07-14 07-22-09.455.jpg");
			//this.CalibrationImage = new Image<Bgr, byte>(@"C:\svnDev\oss\Google\drh-visual-odometry\CalibrationFiles\MicrosoftCinemaFocus14\Set1\Capture4.png");
			FindChessBoardCorners();
		}

		private void FindChessBoardCorners()
		{
			this.GrayImage = this.CalibrationImage.Convert<Gray, Byte>();

			bool foundAllCorners = CameraCalibration.FindChessboardCorners(
				this.GrayImage,
				m_PatternSize,
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

				CameraCalibration.DrawChessboardCorners(this.GrayImage, m_PatternSize, m_FoundCorners, foundAllCorners);

				CircleF circle = new CircleF(m_FoundCorners[1], 3.0f);
				this.GrayImage.Draw(circle, new Gray(255), 2);
				PrintCorners();
			}
			m_ImageBoxWithHeading.ImageBox.Image = this.GrayImage;
		}

		private void PrintCorners()
		{
			int index = -1;
			for (int y = 0; y < m_PatternSize.Height; y++)
			{
				Debug.WriteLine("Row " + y);
				for (int x = 0; x < m_PatternSize.Width; x++)
				{
					index++;
					Debug.WriteLine(m_FoundCorners[index]);
				}
			}
		}
	}
}
