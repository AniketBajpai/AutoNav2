#include <bits/stdc++.h>
#include <opencv2/video/tracking.hpp>
#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include "fishcam.h"
#include "undistort.h"


using namespace std;
using namespace cv;

int main(int argc, char const *argv[]) {
    /* Config */
    // Undistort
    std::string s = "calib_results_flycap.txt";
    FishOcam f;
    f.init(s);
    cout << "Focal length is " << f.focal << "\n";
    cout << "Width is " << f.wout << "\n";
    cout << "Hout is " << f.hout << "\n";
    Size S = Size(f.wout, f.hout);

    // visual odometry parameters
    VisualOdometryStereo::parameters param;

    // calibration parameters
    param.calib.f  = f.focal; // focal length in pixels
    param.calib.cu = 635.96; // principal point (u-coordinate) in pixels
    param.calib.cv = 194.13; // principal point (v-coordinate) in pixels
    param.base     = 0.5707; // baseline in meters

    // init visual odometry
    VisualOdometryStereo viso(param);

    VideoCapture cap;
    int32_t width, height;
    Mat gray, prev_gray, image, frame;
    uint8_t left_img_data, right_img_data;
    Matrix pose = Matrix::eye(4);
    needToInit = true;  // for initialization on 1st go

    /*
    needToInit = false;
    Mat img1 = imread(argv[1]);
    Mat img2 = imread(argv[2]);
    Mat img1_un, img2_un;
    f.WarpImage(img1, img1_un);
    f.WarpImage(img2, img2_un);
    cvtColor(img1_un, prev_gray, COLOR_BGR2GRAY);
    cvtColor(img2_un, gray, COLOR_BGR2GRAY);
    */

    // while(true) {
    //     cap >> frame;
    //     if( frame.empty() )
    //         break;
    //
    //     f.WarpImage(frame, image);  // undistort frame
    //     cvtColor(image, gray, COLOR_BGR2GRAY);
    //     width  = gray.cols;
    //     height = gray.rows;

        if(!needToInit) {
            left_img_data  = prev_gray.data;
            right_img_data  = gray.data;

            // compute visual odometry
            int32_t dims[] = {width,height,width};
            if (viso.process(left_img_data,right_img_data,dims)) {

              // on success, update current pose
              pose = pose * Matrix::inv(viso.getMotion());

              // output some statistics
              double num_matches = viso.getNumberOfMatches();
              double num_inliers = viso.getNumberOfInliers();
              cout << ", Matches: " << num_matches;
              cout << ", Inliers: " << 100.0*num_inliers/num_matches << " %" << ", Current pose: " << endl;
              cout << pose << endl << endl;

            } else {
              cout << " ... failed!" << endl;
            }
        }

    //     cv::swap(prev_gray, gray);
    // }



    }



}
