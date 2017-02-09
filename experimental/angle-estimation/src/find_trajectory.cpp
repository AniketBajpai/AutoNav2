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
    std::string s = "../undistort/calib_results_flycap.txt";
    FishOcam f;
    f.init(s);
    cout << "Focal length is " << f.focal << "\n";
    cout << "Width is " << f.wout << "\n";
    cout << "Hout is " << f.hout << "\n";
    Size S = Size(f.wout, f.hout);

    // Feature detection
    const int maxCorners = 1000;
    const double qualityLevel = 0.005;
    const double minDistance = 10;
    const int blockSize = 3;
    const double k = 0.04;
    TermCriteria termcrit(TermCriteria::COUNT|TermCriteria::EPS,20,0.03);
    Size subPixWinSize(10,10), winSize(31,31);

    const int MAX_FEATURES = 1000;

    VideoCapture cap;
    Mat gray, prev_gray, image, frame;
    vector<Point2f> points[2];  // maintains consecutive feature points
    vector<pair<double ,double> > trajectory;
    bool needToInit = true;  // for initialization on 1st go

    //  Test
    needToInit = false;
    Mat img1 = imread(argv[1]);
    Mat img2 = imread(argv[2]);
    Mat img1_un, img2_un;
    f.WarpImage(img1, img1_un);
    f.WarpImage(img2, img2_un);
    cvtColor(img1_un, prev_gray, COLOR_BGR2GRAY);
    cvtColor(img2_un, gray, COLOR_BGR2GRAY);
    goodFeaturesToTrack(gray, points[0], maxCorners, qualityLevel, minDistance, Mat(), blockSize, 0, k);    // find good features
    cornerSubPix(gray, points[0], subPixWinSize, Size(-1,-1), termcrit);    // refine corners found



    // while(true) {
    //     cap >> frame;
    //     if( frame.empty() )
    //         break;
    //
    //     f.WarpImage(frame, image);  // undistort frame
    //     cvtColor(image, gray, COLOR_BGR2GRAY);

        if( needToInit ) {   // Initialize features
            goodFeaturesToTrack(gray, points[1], maxCorners, qualityLevel, minDistance, Mat(), blockSize, 0, k);    // find good features
            cornerSubPix(gray, points[1], subPixWinSize, Size(-1,-1), termcrit);    // refine corners found
            needToInit = false;
        }
        else {
            assert(!points[0].empty());
            vector<uchar> status;
            vector<float> err;
            if(prev_gray.empty())
                gray.copyTo(prev_gray);
            calcOpticalFlowPyrLK(prev_gray, gray, points[0], points[1], status, err, winSize,
                                 3, termcrit, 0, 0.001);
            cout << "Number of points original: " << points[0].size() << endl;
            cout << "Number of points final: " << points[1].size() << endl;
            size_t i, k;
            for( i = k = 0; i < points[1].size(); i++ )
            {
                if( !status[i] )
                    continue;

                points[1][k++] = points[1][i];
                line(img1_un, points[0][i], points[1][i], Scalar(255,0,0), 3);
                // circle(image, points[1][i], 3, Scalar(0,255,0), -1, 8);
                circle(img1_un, points[1][i], 3, Scalar(0,0,255), -1, 8);
            }
            points[1].resize(k);

            // Add extra corners to points so that no. of features are always >= MAX_FEATURES
            if(k < MAX_FEATURES) {
                vector<Point2f> extraPoints;
                goodFeaturesToTrack(gray, extraPoints, MAX_FEATURES - k, qualityLevel, minDistance, Mat(), blockSize, 0, k);
                cornerSubPix(gray, extraPoints, subPixWinSize, Size(-1,-1), termcrit);
                points[1].insert(points[1].end(), extraPoints.begin(), extraPoints.end());
            }
        }

        std::swap(points[1], points[0]);
        cv::swap(prev_gray, gray);
    // }

    points[0].clear();
    points[1].clear();
    imwrite("frame1.jpg", img1_un);

    return 0;
}
