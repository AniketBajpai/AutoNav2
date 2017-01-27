import cv2
import numpy as np
import aruco

camparam = aruco.CameraParameters()
camparam.readFromXMLFile("cam_params_flycap.yml")
marker_size = 0.165

#if __name__ == '__main__':
def findImageArucoParams(imgName):
    # create markerDetector object
    markerdetector = aruco.MarkerDetector()


    # Load an color image in grayscale
    img = cv2.imread(imgName, cv2.IMREAD_COLOR)

    markers = markerdetector.detect(img, camparam)
    return markers
    '''
    print "Markers detected", len(markers)
    for marker in markers:
        marker.draw(img, np.array([255, 0, 0]), 100, True)
        # for i, point in enumerate(marker):
        #     print i, point

        marker.calculateExtrinsics(marker_size, camparam)
        print "Id:", marker.id
        print "Rvec:\n", marker.Rvec
        print "Tvec:\n", marker.Tvec
    '''

    # cv2.namedWindow('detected', cv2.WINDOW_NORMAL)
    # cv2.imshow('detected', img)
    # cv2.waitKey(0)
    # cv2.destroyAllWindows()
