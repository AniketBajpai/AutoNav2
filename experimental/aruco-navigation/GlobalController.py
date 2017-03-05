import sys
import cv2
import numpy as np
import aruco
import pyfly2
import pbcvt
import time
from math import sqrt
from odometry import *
import MobileCommunication as mc

# load camera parameters
CAM_PARAM_FILE = "../cam-calibration/cam_params_flycap.yml"
camparam = aruco.CameraParameters()
camparam.readFromXMLFile(CAM_PARAM_FILE)

# TODO: Update aruco marker size
MARKER_SIZE = 0.17

DISTANCE_THRES = 1.0

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

id_dict = {}


# odometry Global variables
intensity = 0.5
angleDelta = 5.0


def construct_id_dict():
    global id_dict
    ID_CONFIG_FILE = "./id_dict.txt"
    with open(ID_CONFIG_FILE, 'r') as idfile:
        entry_list = idfile.readlines()
        for entry in entry_list:
            entry_values = entry.strip().split()
            entry_id = int(entry_values[0])
            entry_turn = int(entry_values[1])
            id_dict[entry_id] = entry_turn
        print "Id Dict\n", id_dict


def get_distance(T):
    return math.sqrt(T[0]**2 + T[1]**2 + T[2]**2)


def main():
    global camera, camparam
    global id_dict
    global intensity, angleDelta

    straight_mode = correction_mode = leftturn_mode = rightturn_mode = False
    correction_mode = True

    marker_remaining_list = []   # TODO: add marker ids to list
    marker_complete_list = []
    coming_to_straight_first = True

    while True:
        frame = camera.GrabNumPyImage('bgr')

        # Get angle from free space module
        angle = pbcvt.getTurnAngle(frame) * 180 / math.pi

        if straight_mode:
        	init_bearing = mc.getYaw()
            if(coming_to_straight_first):
                forward_state = True
                left_state = right_state = stop_state = False
                coming_to_straight_first= False

            curr_bearing = mc.getYaw()
        	print "curr bearing is ", curr_bearing
        	if forward_state:
        		print "in forward"
        		moveForward()
        		if((curr_bearing - init_bearing) > angleDelta):
        			forward_state = False
        			right_state = True
        		elif((curr_bearing - init_bearing) < -1*angleDelta):
        			forward_state = False
        			left_state = True
        		else:
        			continue
        	elif left_state:
        		print "in left"
        		turn(float(-1*intensity))
        		# rotate(curr_bearing - init_bearing)
        		if(curr_bearing - init_bearing > angleDelta):
        			left_state = False
        			forward_state = True
        	elif right_state:
        		print "in right"
        		turn(float(intensity))
        		# rotate(curr_bearing - init_bearing)
        		if(curr_bearing - init_bearing < -1*angleDelta):
        			right_state = False
        			forward_state = True

            # Check if marker condition is satisfied for mode change
            markers = markerdetector.detect(frame, camparam)
            print "Markers detected: ", len(markers)
            if len(markers) > 0:
                marker_id_list = [m.id for m in markers]
                print "Found markers", id_list

                for marker in marker_remaining_list:
                    if(marker in marker_id_list):
                        marker_current = marker
                        marker_remaining_list.remove(marker)
                        marker_complete_list.append(marker)
                        break

                if(marker_current in marker_complete_list):
                    marker_current.calculateExtrinsics(MARKER_SIZE, camparam)
                    # print "Id:", marker.id
                    print "Rvec:\n", marker.Rvec
                    print "Tvec:\n", marker.Tvec
                    # distance_old = distance
                    distance = get_distance(marker.Tvec)
                    if(distance < DISTANCE_THRES): # or distance > distance_old):
                        if(id_dict[marker_current] == -1):
                            straight_mode = False
                            leftturn_mode = True
                        elif(id_dict[marker_current] == 1):
                            straight_mode = False
                            rightturn_mode = True

        elif correction_mode:
            rotate(angle)
            correction_mode = False
            straight_mode = True
            coming_to_straight_first = True

        elif leftturn_mode:
            # Turn left by 90 deg
            rotate(-90.0)
            leftturn_mode = False
            straight_mode = True
            coming_to_straight_first = True


        elif rightturn_mode:
            # Turn right by 90 deg
            rotate(90.0)
            rightturn_mode = False
            straight_mode = True
            coming_to_straight_first = True


	    time.sleep(.7)
        # show frame
        # cv2.imshow("frame", frame)
        # cv2.waitKey(10)

    camera.StopCapture()


if __name__ == '__main__':
    construct_id_dict()
	initOdometry()  # Initialize odometry module
    main()
