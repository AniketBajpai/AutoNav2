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

namespace CameraCalibrator
{
	public partial class MainForm : Form
	{
		private MainModel m_Model;

		public MainForm(MainModel mainModel)
		{
			InitializeComponent();
			m_Model = mainModel;

			UpdateFromModel();
			m_Model.Changed += new EventHandler(OnModelChanged);
		}

		private void OnModelChanged(object sender, EventArgs e)
		{
			UpdateFromModel();
		}

		private void UpdateFromModel()
		{
		}

		private void OnSettingsButtonClicked(object sender, EventArgs e)
		{
			//CaptureSettings captureSettings = m_Model.CaptureSettings.Clone();

			CaptureSettingsDialog captureSettingsDialog = new CaptureSettingsDialog(m_Model.CaptureSettings);
			DialogResult result = captureSettingsDialog.ShowDialog();
		}

		private void OnCaptureImagesButtonClicked(object sender, EventArgs e)
		{
			CaptureDialogModel captureDialogModel = new CaptureDialogModel(m_Model.CaptureSettings);
			CaptureDialog captureDialog = new CaptureDialog(captureDialogModel);

			captureDialog.ShowDialog();
		}

		private void OnCalibrateButtonClicked(object sender, EventArgs e)
		{
			CalibrateDialogModel calibrateDialogModel = new CalibrateDialogModel(m_Model);
			CalibrateDialog calibrateDialog = new CalibrateDialog(calibrateDialogModel);
			calibrateDialog.ShowDialog();
		}
	}
}
