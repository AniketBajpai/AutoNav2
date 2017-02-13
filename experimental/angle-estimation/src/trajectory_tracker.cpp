#include <bits/stdc++.h>
#include <opencv2/video/tracking.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include "fishcam.h"
#include "undistort.h"
#include "correspondance.h"
#include "helpers.h"
#include <gflags/gflags.h>

using namespace std;
using namespace cv;

/* Config */
// constants
const string VIDEO_PATH = "../data/testv1.mp4";
const string PROCESSED_DIR = "../processed/";
const int UPDATE_FREQ = 10;
const int min_corners = 10000;

// Undistort
const string CALIB_CONFIG_PATH = "../src/calib_results_flycap.txt";

// Feature detection
const int maxCorners = 10000;
const double qualityLevel = 0.005;
const double minDistance = 10;
const int blockSize = 7;
const double k = 0.04;
const bool useHarrisDetector = false;
TermCriteria termcrit(TermCriteria::COUNT|TermCriteria::EPS,20,0.03);
Size subPixWinSize(10,10), winSize(31,31);

// compression
const int KEYFRAME_MAX = 10;	// Max number of frames in a keyframe
const int CHUNK_MAX = UPDATE_FREQ;		// Max number of keyframes in a chunk
const bool DUMP_CORRES = false; // Check to dump correspondances into directory
const int windows_size = 5;

inline float get_median(vector<float>& v) {
	size_t n = v.size() / 2;
    nth_element(v.begin(), v.begin()+n, v.end());
    return v[n];
}

int main(int argc, char const *argv[]) {
	cout << "Start" << endl;

	// int numoutmatches;
	int framid = 0;
	int prevframid = 0;
	vector<corr> all_corr;    // stores all correspondences between consecutive frames

	vector<Mat> all_images;   // stores all frames - wasteful, so not used
	Mat oldFrame, newFrame, mask, rawFrame, tempFrame;             // variables to hold images
	vector<Point2f> corners, corners_prev, corners_inverse;   // variables to hold set of feature points
	vector<Vec3b> colors; // stores color of all feature points
	vector<uchar> status, status_inverse; // status of optical ow matches
	vector<float> err;                    // errors in optical flow matches
	vector<int> siftids;                  // stores unique id's for feature points
	vector<unordered_map<int, Point2f> > point_map;		// maps id's to points for each frame
	int siftlatest=0;
	int cx = 640, cy = 512;
	int rc = 0;
	FishOcam cam_model;        // Camera model parameters for distortion

	cam_model.init(CALIB_CONFIG_PATH);
	const float focal = cam_model.focal;
	// Size S = Size(cam_model.wout, cam_model.hout);
	cout << "Completed initialization" << endl;

	// Compression
	int batch_num = 0;
	corr compressed;
	vector<Mat> frames;		// saved frames in interval
	vector<vector<pair<int, int> > > trajectories;	// trajectories of all tracked points in interval

	// Open video handle
	VideoCapture cap(VIDEO_PATH);
	if (!cap.isOpened())
		return 1;

	while (true) {
		// read frame
		cap.read(rawFrame);
		if (rawFrame.empty()) { // check for empty frame
			cout << "Empty frame " << rc << endl;
			if (rc>20)  // break after 20 empty frames - feed stopped
				break;
			rc++;
			continue;
		}
		rc = 0;
		cout << "Read frame" << endl;

		// Undistort frame
		Mat undistorted;
		cam_model.WarpImage(rawFrame, undistorted);
		// undistort(rawFrame);
		undistorted.copyTo(rawFrame);    // TODO use undistort mat
		cout << "Undistorted frame" << endl;

		// cvtColor(rawFrame, newFrame, CV_RGBA2GRAY);     // convert to grayscale - TODO check rgb
		cvtColor(rawFrame, newFrame, CV_BGR2GRAY);
		cout << "On frame id: " << framid << " and tracking " << corners.size() << " points" << endl;

		unordered_map<int, Point2f> curr_point_map;

		if (framid == 0) {    // Initial frame
			newFrame.copyTo(oldFrame);
			cx = oldFrame.cols/2;
			cy = oldFrame.rows/2;
			cout << "Cx is: " << cx << "\n";
			cout << "Cy is: " << cy << "\n";
			// cout << "Focal is: " << focal << "\n";
			goodFeaturesToTrack(oldFrame, corners_prev, maxCorners, qualityLevel, minDistance, mask, blockSize, useHarrisDetector, k);
			// TODO check if corner refinement requirement
			// cornerSubPix(oldFrame, corners_prev, subPixWinSize, Size(-1,-1), termcrit);    // refine corners found
			siftlatest = 0;
			siftids = vector<int> ();
			for (uint i=siftlatest; i<siftlatest+corners_prev.size(); i++) {
				siftids.push_back(i);
			}
			for (uint i=0; i<corners_prev.size(); i++) {
				curr_point_map[i] = corners_prev[i];
				colors.push_back(rawFrame.at<Vec3b>(corners_prev[i]));
			}
			siftlatest = siftlatest+corners_prev.size();
			cout << "Latest id: " << siftlatest << endl;
			prevframid = framid;
			// imwrite(PROCESSED_DIR + "img_" + to_string(framid) + ".jpg", rawFrame);
			point_map.push_back(curr_point_map);
			frames.push_back(rawFrame);
			framid++;
			continue;
		}

		if (corners_prev.size()>0) {    // Frames other than initial
			// Calculate optical flow using forward projection
			calcOpticalFlowPyrLK(oldFrame, newFrame, corners_prev, corners, status, err, winSize, 2, termcrit, 0, 0.001);
			// Back project calculated corners
			calcOpticalFlowPyrLK(newFrame, oldFrame, corners, corners_inverse, status_inverse, err, winSize, 2, termcrit, 0, 0.001);

			GetGoodPoints(corners_prev, corners_inverse, status, status_inverse);   // get good points using forward and backward projection

			// Store Good Tracks;
			corr frame_corr(prevframid, framid);
			for (uint i=0; i<corners.size(); i++) {
				if (status[i]) {    // check points with correct status
					// check if found points lie in image range
					if ((corners[i].x > (2*cx -1)) || (corners[i].y > (2*cy -1)) || (corners[i].x <0) || (corners[i].y<0)) {
						status[i] = 0;
						continue;
					}
					// Assert that points found are in image range
					assert(corners[i].x <= 2*cx-1);
					assert(corners[i].y <= 2*cy-1);
					assert(corners[i].x >= 0);
					assert(corners[i].y >= 0);

					// Set parameters of corr struct for computation
					frame_corr.p1.push_back(corners_prev[i]);
					frame_corr.p2.push_back(corners[i]);
					frame_corr.unique_id.push_back(siftids[i]);
					frame_corr.col.push_back(colors[i]);
				}
			}

			CalculateDelta(frame_corr);     // calculates correspondences between features in frames
			all_corr.push_back(frame_corr);

			newFrame.copyTo(oldFrame);
			vector<int> newsiftids;
			corners_prev.clear();
			vector<Vec3b> temp_colors;
			for (uint i=0; i<corners.size(); i++) {
				if (status[i]) {
					temp_colors.push_back(colors[i]);
					corners_prev.push_back(corners[i]);
					newsiftids.push_back(siftids[i]);
					curr_point_map[siftids[i]] = corners[i];
				}
			}
			colors = temp_colors;
			siftids = newsiftids;
			assert(siftids.size() == corners_prev.size());
			assert(siftids.size() == colors.size());
			assert(siftids.size() == curr_point_map.size());
		}

		// Recompute feature points every N frames
		if (((framid+1) % UPDATE_FREQ == 0) || (corners_prev.size() < min_corners)) {
			mask = Mat::ones(oldFrame.size(), CV_8UC1);
			for (uint i=0; i<corners_prev.size(); i++) {
				circle(mask, corners_prev[i], 3, Scalar(0), -1);
			}
			vector<Point2f> newcorners;
			goodFeaturesToTrack(oldFrame, newcorners, maxCorners, qualityLevel, minDistance, mask, blockSize, useHarrisDetector, k);
			corners_prev.insert(corners_prev.end(), newcorners.begin(), newcorners.end());
			for (uint i=0; i<newcorners.size(); i++) {
				colors.push_back(rawFrame.at<Vec3b> (newcorners[i]));
				curr_point_map[i+siftlatest] = newcorners[i];
			}
			// Assign new id's to feature points
			for (uint i=siftlatest; i<siftlatest+newcorners.size(); i++) {
				siftids.push_back(i);
			}
			siftlatest = siftlatest+newcorners.size();
			assert(colors.size() == siftids.size());
			assert(colors.size() == corners_prev.size());
			assert(colors.size() == curr_point_map.size());
		}

		point_map.push_back(curr_point_map);		// map for current frame

		// Draw feature points on image
		// for (uint i=0; i<corners_prev.size(); i++) {
		// 	circle(rawFrame, corners_prev[i], 4, Scalar(0, 0, 255), -1);
		// }

		// imshow("new", rawFrame);
		// if (waitKey(1) == 27)
		//  break
		// imwrite(PROCESSED_DIR + "img_"+to_string(framid)+".jpg", rawFrame);

		// Correspondance compression
		cout << "Compression\n";
		if(framid == 1) {		// Special case for frame 1
			compressed = all_corr[0];
			frames.push_back(rawFrame);
			prevframid = framid;
			framid++;
			continue;
		}
		if(framid % UPDATE_FREQ == 0) {
			compressed = all_corr[batch_num*CHUNK_MAX-1];
		}
		else {
			assert(all_corr.size() == framid);
			compressed = CompressCorr(compressed, all_corr[framid-1]);
		}

		if((framid+1) % UPDATE_FREQ == 0) {			// Processing at end of interval
			assert(frames.size() == UPDATE_FREQ-1);
			assert(point_map.size() == UPDATE_FREQ);

			for(uint j = 0; j < UPDATE_FREQ-1; j++) {
				for(uint point_id: compressed.unique_id) {
					assert(point_map[j].find(point_id) != point_map[j].end());	// Point id in point map


					// Draw tracked points on frames
					circle(frames[j], point_map[j][point_id], 4, Scalar(0, 0, 255), -1);
				}
			}
			// Trajectory calculation
			vector<float> trajectory_x, trajectory_y;
			float track_x[UPDATE_FREQ], track_y[UPDATE_FREQ];
			for(uint point_id: compressed.unique_id) {
				for(uint j = 0; j < UPDATE_FREQ-1; j++) {
					float x_displacement = point_map[j+1][point_id].x - point_map[j][point_id].x;
					float y_displacement = point_map[j+1][point_id].y - point_map[j][point_id].y;
					line(frames[j], point_map[j][point_id], point_map[j+1][point_id], Scalar(0,255,0));
					track_x[j+1] = x_displacement;
					track_y[j+1] = y_displacement;
				}

				// TODO: look for better heuristic
				trajectory_x.push_back(track_x[UPDATE_FREQ-1] - track_x[1]);
				trajectory_y.push_back(track_y[UPDATE_FREQ-1] - track_y[1]);
			}
			assert(trajectory_x.size() == compressed.unique_id.size());
			assert(trajectory_y.size() == compressed.unique_id.size());
			float delta_x = get_median(trajectory_x);
			float delta_y = get_median(trajectory_y);
			cout << "Median: ";
			cout << delta_x << ", " << delta_y << endl;

			int frame_write_id = framid-UPDATE_FREQ+1;
			for(Mat frame: frames){
				imwrite(PROCESSED_DIR + "img_"+to_string(frame_write_id+1)+".jpg", frame);
				frame_write_id++;
			}
			frames.clear();
			point_map.clear();
			batch_num++;
		}
		else {				// Mid interval operations
			frames.push_back(rawFrame);
		}

		prevframid = framid;
		framid++;

	}

	return 0;
}
