from odometry import *
import MobileCommunication as mc
import time

TURN_DURATION = 10.0
DELAY_DURATION = 0.05


def test1():

	dir = -1
	num = 1

	while(num < 2):
		moveForward()
		# Left
		start_time= time.time()
		while(time.time()-start_time < TURN_DURATION):
			print "Turn", dir
			turn(dir)
			time.sleep(DELAY_DURATION)
		dir=-1*dir
		# stopBot()
		start_time= time.time()
		while(time.time()-start_time < TURN_DURATION):
			moveForward()
			print "moving forward"
			time.sleep(DELAY_DURATION)

		# Right
		start_time= time.time()
		while(time.time()-start_time < TURN_DURATION):
			print "Turn", dir
			turn(dir)
			time.sleep(DELAY_DURATION)
		num += 1

	stopBot()

def linear_movement():

	intensity = 0.5
	angleDelta = 5.0
	FORWARD_DURATION = 10.0

	init_bearing = mc.getYaw()
	start_time= time.time()

	forward_state = True
	stop_state = False
	left_state = False	# moving towards left
	right_state = False	# moving towards right
	while(time.time()-start_time < TURN_DURATION):
		curr_bearing = mc.getYaw()
		print "curr bearing is ", curr_bearing
		if forward_state:
			print "in forward"
			moveForward()
			if((curr_bearing - init_bearing) > angleDelta):
				forward_state=False
				right_state = True
			elif((curr_bearing - init_bearing) < -1*angleDelta):
				forward_state=False
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
		# time.sleep(DELAY_DURATION)

	stopBot()


def main():
	linear_movement()


if __name__ == '__main__':

	initOdometry()  # Initialize odometry module
	main()
