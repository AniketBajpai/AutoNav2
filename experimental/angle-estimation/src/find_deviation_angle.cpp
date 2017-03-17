#include <bits/stdc++.h>
#include <opencv2/opencv.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/highgui/highgui.hpp>
#include "fishcam.h"
#include "undistort.h"
#include "correspondance.h"
#include "verify_two_view_matches.h"
#include "twoview_info.h"
#include "helpers.h"

using namespace std;
using namespace cv;

Point2f center;

void printTransformation(TwoViewInfo& twoview_info) {
    cout << "Rotation matrix:" << endl;
    cout << twoview_info.rotationmat_2(0,0) << " " << twoview_info.rotationmat_2(0,1) << " " << twoview_info.rotationmat_2(0,2) << "\n" <<
             twoview_info.rotationmat_2(1,0) << " " << twoview_info.rotationmat_2(1,1) << " " << twoview_info.rotationmat_2(1,2) << "\n" <<
             twoview_info.rotationmat_2(2,0) << " " << twoview_info.rotationmat_2(2,1) << " " << twoview_info.rotationmat_2(2,2) << "\n";
    cout << "Translation matrix:" << endl;
    cout << twoview_info.translation_2(0) << " " << twoview_info.translation_2(1) << " " << twoview_info.translation_2(2) <<"\n";
}

double findZAngle(TwoViewInfo& twoview_info) {
    // Find vector after applying transform to unit k vector
    double x_trans = twoview_info.rotationmat_2(0,2) + twoview_info.translation_2(0);
    double y_trans = twoview_info.rotationmat_2(1,2) + twoview_info.translation_2(1);
    double z_trans = twoview_info.rotationmat_2(2,2) + twoview_info.translation_2(2);
    double z_angle = atan2((z_trans), (x_trans - center.x));
    return z_angle;
}

// Undistort
const string CALIB_CONFIG_PATH = "../src/calib_results_flycap.txt";

// Input -> image1 image2
int main(int argc, char const *argv[]) {
    /* Config */
    // Undistort
    FishOcam cam_model;        // Camera model parameters for distortion
	cam_model.init(CALIB_CONFIG_PATH);
    const float focal = cam_model.focal;
    // cout << "Focal length is " << cam_model.focal << "\n";
    // cout << "Width is " << cam_model.wout << "\n";
    // cout << "Hout is " << cam_model.hout << "\n";
    // Size S = Size(cam_model.wout, cam_model.hout);

    // Feature detection
    const int maxCorners = 1000;
    const double qualityLevel = 0.005;
    const double minDistance = 20;
    const int blockSize = 3;
    const bool useHarrisDetector = false;
    const double k = 0.04;

    // R,t calculation
    VerifyTwoViewMatchesOptions options;
    options.bundle_adjustment = false;
    options.min_num_inlier_matches = 10;
    options.estimate_twoview_info_options.max_sampson_error_pixels = 2.25;

    cout << "Completed Initialization" << endl;

    // Read input images
    Mat image1 = imread(argv[1], CV_LOAD_IMAGE_COLOR);
    Mat image2 = imread(argv[2], CV_LOAD_IMAGE_COLOR);
    assert(image1.cols == image2.cols);
    assert(image1.rows == image2.rows);
    center = Point2f((float)cam_model.wout/2, (float)cam_model.hout/2);

    // Undistort input images
    Mat image1_un;
    Mat image2_un;
    cam_model.WarpImage(image1, image1_un);
    cam_model.WarpImage(image2, image2_un);

    cout << "Read and undistorted images" << endl;

    // Find feature points in images
    Mat image1_gray, image2_gray;
    cvtColor(image1_un, image1_gray, CV_BGR2GRAY);
    cvtColor(image2_un, image2_gray, CV_BGR2GRAY);
    vector<Point2f> features1, features2;
    goodFeaturesToTrack( image1_gray, features1, maxCorners, qualityLevel, minDistance, Mat(), blockSize, useHarrisDetector, k);
    goodFeaturesToTrack( image2_gray, features2, maxCorners, qualityLevel, minDistance, Mat(), blockSize, useHarrisDetector, k);

    /*
    // Draw feature points and write images
    const int r = 10;
    for( size_t i = 0; i < corners1.size(); i++ ) {
         circle( image1_un, corners1[i], r, Scalar(0, 0, 255), -1, 8, 0 );
     }
     imwrite("frame1.jpg", image1_un);
    for( size_t i = 0; i < corners2.size(); i++ ) {
         circle( image2_un, corners2[i], r, Scalar(0, 0, 255), -1, 8, 0 );
     }
     imwrite("frame2.jpg", image2_un);
     */

    // Find R,t between images
    corr frameCorrespondence;
    // Insert center subtracted points into point vector
    for(auto point: features1) {
        frameCorrespondence.p1.push_back((point - center));
    }
    for(auto point: features2) {
        frameCorrespondence.p2.push_back((point - center));
    }
    TwoViewInfo twoview_info;
    vector<int> inliers;    // indices of matching points in vector
    if (GetEssentialRT(frameCorrespondence, twoview_info, inliers, options, focal)) {
        cout << "No. of inliers: " << inliers.size() << endl;
        printTransformation(twoview_info);
        double angle_turned = findZAngle(twoview_info);
        cout << "Angle turned: " << angle_turned << endl;
    }
    else {
        cout << "Matching failed" << endl;
    }

    // Clear feature point vectors
    features1.clear(); features2.clear();
    inliers.clear();

    return 0;
}
