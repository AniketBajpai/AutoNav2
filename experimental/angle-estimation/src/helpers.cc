#include "helpers.h"

void ShowCorres(std::string dirname, corr compressed) {
  cv::Mat im1 = cv::imread(dirname+"/img_"+std::to_string(compressed.frame_1)+".jpg");
  cv::Mat im2 = cv::imread(dirname+"/img_"+std::to_string(compressed.frame_2)+".jpg");
  cv::Size sz1 = im1.size();
  cv::resize(im1, im1, cv::Size(sz1.width/2, sz1.height/2));
  cv::resize(im2, im2, cv::Size(sz1.width/2, sz1.height/2));
  sz1 = im1.size();
  system(("mkdir " + dirname + "/corr_" + std::to_string(compressed.frame_1)+"_"+std::to_string(compressed.frame_2)).c_str());
  for (int i=0; i<compressed.p1.size()/10;i++) {
    cv::Mat im3(sz1.height, 2*sz1.width, CV_8UC3);
    cv::Mat left(im3, cv::Rect(0,0,sz1.width, sz1.height));
    im1.copyTo(left);
    cv::Mat right(im3, cv::Rect(sz1.width,0,sz1.width, sz1.height));
    im2.copyTo(right);
    for (int j=10*i; j<10*(i+1); j++) {
      cv::circle(im3, cv::Point2f(compressed.p1[j].x/2,compressed.p1[j].y/2),3, cv::Scalar(255,0,0), -1);
      cv::circle(im3, cv::Point2f(compressed.p2[j].x/2+sz1.width,compressed.p2[j].y/2),3, cv::Scalar(255,0,0), -1);
      cv::line(im3, cv::Point2f(compressed.p1[j].x/2,compressed.p1[j].y/2), cv::Point2f(compressed.p2[j].x/2+sz1.width,compressed.p2[j].y/2), cv::Scalar(0,0,255));
    }
    cv::imwrite(dirname+"/corr_"+std::to_string(compressed.frame_1)+"_"+std::to_string(compressed.frame_2)+"/"+std::to_string(i)+".jpg", im3);
  }
}
