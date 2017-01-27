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
using CameraCalibrator.Properties;
using System.IO;
using CameraCalibrator.Support;

namespace CameraCalibrator
{
	public class CaptureSettings : IDisposable
	{
		public ChessBoard ChessBoard { get; private set; }
		private string m_OutputFolderPath;
		private int m_ChessBoardX;
		private int m_ChessBoardY;
		private int m_ImagesCount;
		private TimeSpan m_WaitBetweenCaptures;

		public event EventHandler Changed;

		public CaptureSettings()
			: this(Settings.Default.ChessBoardX, Settings.Default.ChessBoardY, Environment.CurrentDirectory, Settings.Default.ImagesCount, Settings.Default.WaitBetweenCaptures)
		{
		}

		internal CaptureSettings(int chessBoardX, int chessBoardY, string outputFolderPath, int imagesCount, TimeSpan waitBetweenCaptures)
		{
			m_ChessBoardX = chessBoardX;
			m_ChessBoardY = chessBoardY;
			UpdateChessBoard();

			m_OutputFolderPath = outputFolderPath;
			m_ImagesCount = imagesCount;

			this.WaitBetweenCaptures = waitBetweenCaptures;
		}

		public int ChessBoardX
		{
			get { return m_ChessBoardX; }
			set
			{
				if (value != m_ChessBoardX)
				{
					m_ChessBoardX = value;
					UpdateChessBoard();
					RaiseChangedEvent();
				}
			}
		}

		public int ChessBoardY
		{
			get { return m_ChessBoardY; }
			set
			{
				if (value != m_ChessBoardY)
				{
					m_ChessBoardY = value;
					UpdateChessBoard();
					RaiseChangedEvent();
				}
			}
		}

		public int ImagesCount
		{
			get { return m_ImagesCount; }
			set
			{
				if (value != m_ImagesCount)
				{
					m_ImagesCount = value;
					RaiseChangedEvent();
				}
			}
		}

		public string OutputFolderPath
		{
			get { return m_OutputFolderPath; }
			set
			{
				if (value != m_OutputFolderPath)
				{
					m_OutputFolderPath = value;
					RaiseChangedEvent();
				}
			}
		}

		public TimeSpan WaitBetweenCaptures
		{
			get { return m_WaitBetweenCaptures; }
			set
			{
				if (value != m_WaitBetweenCaptures)
				{
					m_WaitBetweenCaptures = value;
					RaiseChangedEvent();
				}
			}
		}

		private void UpdateChessBoard()
		{
			this.ChessBoard = new ChessBoard(m_ChessBoardX, m_ChessBoardY);
		}

		internal string GetFilePath(int imageIndex)
		{
			string fileName = "Capture" + imageIndex.ToString() + ".png";
			return Path.Combine(this.OutputFolderPath, fileName);
		}

		//internal CaptureSettings Clone()
		//{
		//    return new CaptureSettings(m_ChessBoardX, m_ChessBoardY, m_OutputFolderPath, m_ImagesCount);
		//}

		//internal void UpdateFrom(CaptureSettings captureSettings)
		//{
		//    this.ChessBoardX = captureSettings.ChessBoardX;
		//    this.ChessBoardY = captureSettings.ChessBoardY;
		//    this.OutputFolderPath = captureSettings.OutputFolderPath;
		//}

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
			Settings.Default.ChessBoardX = this.ChessBoardX;
			Settings.Default.ChessBoardY = this.ChessBoardY;
			Settings.Default.ImagesCount = this.ImagesCount;
			Settings.Default.WaitBetweenCaptures = this.WaitBetweenCaptures;
		}
	}
}
