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
using Emgu.CV.CvEnum;
using CameraCalibrator.WinForm.Support;

namespace CameraCalibrator
{
	public partial class CalibrateDialog : Form
	{
		private CalibrateDialogModel m_Model;

		public CalibrateDialog(CalibrateDialogModel model)
		{
			m_Model = model;
			InitializeComponent();
			UpdateFromCameraParameters();
			UpdateFromCalibrationFlags();
		}

		private void UpdateFromCameraParameters()
		{
			m_TextBoxFx.Text = m_Model.IntrinsicCameraParameters.IntrinsicMatrix[0, 0].ToString();
			m_TextBoxFy.Text = m_Model.IntrinsicCameraParameters.IntrinsicMatrix[1, 1].ToString();

			m_TextBoxCx.Text = m_Model.IntrinsicCameraParameters.IntrinsicMatrix[0, 2].ToString();
			m_TextBoxCy.Text = m_Model.IntrinsicCameraParameters.IntrinsicMatrix[1, 2].ToString();

			m_TextBoxK1.Text = m_Model.IntrinsicCameraParameters.DistortionCoeffs[0, 0].ToString();
			m_TextBoxK2.Text = m_Model.IntrinsicCameraParameters.DistortionCoeffs[1, 0].ToString();
			m_TextBoxK3.Text = m_Model.IntrinsicCameraParameters.DistortionCoeffs[4, 0].ToString();

			m_TextBoxP1.Text = m_Model.IntrinsicCameraParameters.DistortionCoeffs[2, 0].ToString();
			m_TextBoxP2.Text = m_Model.IntrinsicCameraParameters.DistortionCoeffs[3, 0].ToString();
		}

		private void UpdateFromCalibrationFlags()
		{
			CALIB_TYPE calibrationFlags = m_Model.CalibrationFlags;
			m_UseIntrinsicGuessCheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_USE_INTRINSIC_GUESS) > 0;
			m_FixPrincipalPointCheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_FIX_PRINCIPAL_POINT) > 0;
			m_FixAspectRatioCheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_FIX_ASPECT_RATIO) > 0;
			m_FixFocalLengthCheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_FIX_FOCAL_LENGTH) > 0;
			m_FixK1CheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_FIX_K1) > 0;
			m_FixK2CheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_FIX_K2) > 0;
			m_FixK3CheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_FIX_K3) > 0;
			m_ZeroTangentialDistortionCheckBox.Checked = (calibrationFlags & CALIB_TYPE.CV_CALIB_ZERO_TANGENT_DIST) > 0;
		}

		private void OnLoadButtonClicked(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text file|*.txt";

			DialogResult result = openFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				m_Model.LoadCameraParameters(openFileDialog.FileName);
				UpdateFromCameraParameters();
			}
		}

		private void OnSaveButtonClicked(object sender, EventArgs e)
		{
			UpdateCameraParametersFromUI();

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "Text file|*.txt";

			DialogResult result = saveFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				m_Model.SaveCameraParameters(saveFileDialog.FileName);
			}
		}

		private void OnClibrationFlagsCheckChanged(object sender, EventArgs e)
		{
			CALIB_TYPE calibrationFlags = CALIB_TYPE.DEFAULT;

			if (m_UseIntrinsicGuessCheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_USE_INTRINSIC_GUESS;
			}
			if (m_FixPrincipalPointCheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_FIX_PRINCIPAL_POINT;
			}
			if (m_FixAspectRatioCheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_FIX_ASPECT_RATIO;
			}
			if (m_FixFocalLengthCheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_FIX_FOCAL_LENGTH;
			}
			if (m_FixK1CheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_FIX_K1;
			}
			if (m_FixK2CheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_FIX_K2;
			}
			if (m_FixK3CheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_FIX_K3;
			}
			if (m_ZeroTangentialDistortionCheckBox.Checked)
			{
				calibrationFlags |= CALIB_TYPE.CV_CALIB_ZERO_TANGENT_DIST;
			}

			m_Model.CalibrationFlags = calibrationFlags;
		}

		private void OnCalibrateButtonClicked(object sender, EventArgs e)
		{
			Cursor currentCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			try
			{
				UpdateCameraParametersFromUI();

				m_Model.Calibrate();
				UpdateFromCameraParameters();
			}
			finally
			{
				// Swap the current cursor back to the original cursor
				Cursor.Current = currentCursor;
			}
		}

		private void OnTestButtonClicked(object sender, EventArgs e)
		{
			UpdateCameraParametersFromUI();
			using (TestFormModel testFormModel = m_Model.CreateTestFormModel())
			{
				TestForm testForm = new TestForm(testFormModel);
				testForm.ShowDialog();
			}
		}

		private void UpdateCameraParametersFromUI()
		{
			m_Model.IntrinsicCameraParameters.IntrinsicMatrix[0, 0] = Double.Parse(m_TextBoxFx.Text);
			m_Model.IntrinsicCameraParameters.IntrinsicMatrix[1, 1] = Double.Parse(m_TextBoxFy.Text);

			m_Model.IntrinsicCameraParameters.IntrinsicMatrix[0, 2] = Double.Parse(m_TextBoxCx.Text);
			m_Model.IntrinsicCameraParameters.IntrinsicMatrix[1, 2] = Double.Parse(m_TextBoxCy.Text);

			m_Model.IntrinsicCameraParameters.DistortionCoeffs[0, 0] = Double.Parse(m_TextBoxK1.Text);
			m_Model.IntrinsicCameraParameters.DistortionCoeffs[1, 0] = Double.Parse(m_TextBoxK2.Text);
			m_Model.IntrinsicCameraParameters.DistortionCoeffs[4, 0] = Double.Parse(m_TextBoxK3.Text);

			m_Model.IntrinsicCameraParameters.DistortionCoeffs[2, 0] = Double.Parse(m_TextBoxP1.Text);
			m_Model.IntrinsicCameraParameters.DistortionCoeffs[3, 0] = Double.Parse(m_TextBoxP2.Text);
		}
	}
}
