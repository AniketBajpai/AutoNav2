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

namespace CameraCalibrator.WinForm.Support
{
	public partial class TestForm : Form
	{
		private TestFormModel m_Model;
		private bool m_StopProcessing;

		public TestForm(TestFormModel model)
		{
			m_Model = model;
			InitializeComponent();
			m_ImagesSplitter.SplitterDistance = m_ImagesSplitter.Width / 2;
			Application.Idle += ProcessFrame;
		}

		private void ProcessFrame(object sender, EventArgs e)
		{
			if (m_StopProcessing)
			{
				return;
			}
			m_Model.ProcessFrame();

			m_RawImageBoxWithHeading.ImageBox.Image = m_Model.OriginalImage;
			m_CorrectedImageBoxWithHeading.ImageBox.Image = m_Model.CorrectedImage;
		}

		private void OnOKButtonClick(object sender, EventArgs e)
		{
			m_StopProcessing = true;
			Application.Idle -= ProcessFrame;
			this.Close();
		}
	}
}
