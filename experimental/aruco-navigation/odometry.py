import time
import math
import sys
import os
import numpy as np
import cgbot
from cgbot.commands import cmd
import subprocess
from EncoderReading import *
from cgbot.redisdb import rdb
from cgbot.robot import rbt
from cgbot.detect import motorcontroller
from cgbot.sensors import Orientation
#import control_servo as cs
import MobileCommunication as mc
import socket
import traceback
import netifaces as ni

SLEEP_TIME = 0.05
BOT_SPEED = 7000
WHEEL_RADIUS = 0.08912  # 0.16 #in metres
ENC_TICKS_360 = 1250
TICK_TO_DIST = 2 * math.pi * WHEEL_RADIUS / ENC_TICKS_360
angleDelta = BOT_SPEED * .001
DELTA = 0.0000

initLeft=0
initRight=0
WHEEL_BASE = 0.32

host = ni.ifaddresses('wlan0')[2][0]['addr']
# print ip  # should print "192.168.100.37
# Note that the IP Address and Port in this script and the script on the Mobile Phone should match.
# host = "10.192.46.131"   #IP Address of this Machine
port = 3400

s = None


def initsocket():
    global s
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    s.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
    s.bind((host, port))
    return s


def initOdometry():
    rbt.connect(motorcontroller())
    mc.initsocket()
    global initLeft
    global initRight
    temp = readMotorTicks()
    initLeft = temp[0]
    initRight = temp[1]



# def getYaw():
#     global s
#     try:
#         #message, address = s.recvfrom(4096)
#         stuff = s.recvfrom(4096)
#         #orient = float((message.split())[1])
#         #orient = orient * 180/3.14;
#         return stuff
#         # return orient
#     except (KeyboardInterrupt, SystemExit):
#         raise
#     except:
#         traceback.print_exc()
#

def readMotorTicks():
    temp = rbt.readTick()
    return (temp[0][1], temp[1][1])


def moveForward():
    cmd.forward(speed=BOT_SPEED)


def stopBot():
    cmd.stop()
    cmd.stop()
    cmd.stop()
    time.sleep(1)


def moveDistance(direction, distance):
    """ @param direction: 1 -> forward and -1 -> backward """

    initleft = readMotorTicks()[0]
    initright = readMotorTicks()[1]
    cmd.forward(speed=BOT_SPEED)
    distDelta = DELTA * BOT_SPEED

    while(True):
        temp = readMotorTicks()
        left = temp[0]
        right = temp[1]
        distanceMoved = ((left - initleft) + (right - initright)) * TICK_TO_DIST / 2
        print distanceMoved
        if(distanceMoved > distance - distDelta):
            break

    cmd.stop()
    cmd.stop()
    cmd.stop()

def getXY():
    global initLeft
    global initRight
    temp = readMotorTicks()
    left = abs(temp[0]  - initLeft)
    right = abs(temp[1] - initRight)
    distance = (left+right) * TICK_TO_DIST /2.0
    theta = abs(left - right) * TICK_TO_DIST / WHEEL_BASE
    print initLeft,initRight
    print "distance is",distance
    print "theta is",theta
    X = distance * math.sin(theta)
    Y = distance * math.cos(theta)
    return (X,Y)

def moveTowardsXY(x,y):
    angle = math.atan(x/y) * 180/math.pi
    print "angle is", angle
    rotate(angle)
    newOdometry = getXY()
    newX = newOdometry[0]
    newY = newOdometry[1]
    print newX,newY
    return
    dist = math.sqrt((x-newX)*(x-newX) + (y-newY)*(y-newY))
    print "distance is", dist
    moveDistance(1,dist)

def rotate(angle):
    """
    angle -> signed double value in degrees
    positive for left
    """

    sign = lambda a: (a > 0) - (a < 0)
    direction = -1*sign(angle)
    angleDelta = abs(0.2*angle)
    intensity = 0.5
    DELAY_DURATION = 0.05

    initYaw = mc.getYaw()
    finalYaw = initYaw + angle
    print "Init", initYaw
    print "finalYaw", finalYaw
    cmd.forward(speed=BOT_SPEED)
    turn(float(intensity*direction))
    turn(float(intensity*direction))
    turn(float(intensity*direction))
    turn(float(intensity*direction))
    while(True):
        yaw = mc.getYaw()
        print yaw
        if(abs(finalYaw - yaw) < angleDelta):
            break
        # time.sleep(DELAY_DURATION)


def turn(intensity):
    # cmd.forward(speed=BOT_SPEED)
    cmd.turn(intensity)

# def main():
#     initOdometry()
#         #initleft = readMotorTicks()[0]
#     # while(True):
#     	# print mc.getYaw()
#      #print retval
#      #temp = readMotorTicks()
#      #print (temp[0]- initleft) * TICK_TO_DIST
#     # moveDistance(1,1)
#     # rotate(90)
#     moveTowardsXY(1,1)
#
#
# if __name__ == '__main__':
#     main()
