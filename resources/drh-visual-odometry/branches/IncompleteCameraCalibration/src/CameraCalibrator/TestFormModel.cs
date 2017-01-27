using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CameraCalibrator
{
	public class TestFormModel
	{
		private Capture m_Capture;
		private IntrinsicCameraParameters m_IntrinsicCameraParameters;

		public Image<Bgr, Byte> OriginalImage { get; private set; }
		public Image<Bgr, Byte> CorrectedImage { get; private set; }

		private Matrix<float> m_UndistortMapX;
		private Matrix<float> m_UndistortMapY;

		internal TestFormModel(IntrinsicCameraParameters intrinsicCameraParameters, Capture capture)
		{
			m_IntrinsicCameraParameters = intrinsicCameraParameters;
			m_Capture = capture;
		}

		private void Initialize()
		{
			this.OriginalImage = m_Capture.QueryFrame();

			m_IntrinsicCameraParameters.InitUndistortMap(
				this.OriginalImage.Width,
				this.OriginalImage.Height,
				out m_UndistortMapX,
				out m_UndistortMapY);
		}

		public void ProcessFrame()
		{
			this.OriginalImage = m_Capture.QueryFrame();
			this.CorrectedImage = m_IntrinsicCameraParameters.Undistort<Bgr, Byte>(this.OriginalImage);
		}
	}
}
