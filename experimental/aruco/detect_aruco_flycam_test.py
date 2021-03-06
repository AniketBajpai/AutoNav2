import sys
import cv2
import numpy as np
import aruco
import pyfly2
import time
if __name__ == '__main__':
    # load camera parameters
    camparam = aruco.CameraParameters()
    camparam.readFromXMLFile("../cam-calibration/cam_params_flycap.yml")

    # Aruco marker size
    marker_size = 0.17

    # create markerDetector object
    markerdetector = aruco.MarkerDetector()
    # markerdetector.setMinMaxSize(0.01)

    # Initialize camera
    context = pyfly2.Context()
    if context.num_cameras < 1:
        raise ValueError('No cameras found')
    cameraIndex = 0
    camera = context.get_camera(cameraIndex)
    camera.Connect()
    camera.StartCapture()

    id_current = 0
    id_next = 1
    prev_id_list = []

    while True:
        frame = camera.GrabNumPyImage('bgr')

        print "Processing"
        markers = markerdetector.detect(frame, camparam)
        print "Markers detected: ", len(markers)
        if len(markers) > 0:
            # print "Found markers"
            id_list = [m.id for m in markers]
            # print "Ids detected", id_list
            id_max = 0
            for marker in markers:
                if(marker.id > id_max and not(marker.id in prev_id_list)):
                    id_max = marker.id
            id_current = id_max
            id_next = id_max

            for marker in markers:
                if(marker.id == id_current):
                    marker_current = marker
                    # marker.draw(frame, np.array([255, 255, 0]), 10, True)
                    # for i, point in enumerate(marker):
                    #     print "Current"
                    #     print i, point

            marker_current.calculateExtrinsics(marker_size, camparam)
            #print "Id:", marker.id
            #print "Rvec:\n", marker.Rvec
            print "Tvec:\n", marker.Tvec
	    time.sleep(.7)
        # show frame
        # cv2.imshow("frame", frame)
        # cv2.waitKey(10)

    camera.StopCapture()
