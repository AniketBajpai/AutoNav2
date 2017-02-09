#!/usr/bin/env python

'''
Lucas-Kanade tracker
====================

Lucas-Kanade sparse optical flow demo. Uses goodFeaturesToTrack
for track initialization and back-tracking for match verification
between frames.

Usage
-----
lk_track.py frame1 frame2

'''

# Python 2/3 compatibility
# from __future__ import print_function

import numpy as np
import cv2
import argparse

lk_params = dict(winSize=(15, 15),
                 maxLevel=2,
                 criteria=(cv2.TERM_CRITERIA_EPS | cv2.TERM_CRITERIA_COUNT, 10, 0.03))

feature_params = dict(maxCorners=1000,
                      qualityLevel=0.008,
                      minDistance=7,
                      blockSize=7)

tracks = []

parser = argparse.ArgumentParser()
parser.add_argument("frame1", help="first frame")
parser.add_argument("frame2", help="second frame")
args = parser.parse_args()

img0_color = cv2.imread(args.frame1)
img0 = cv2.cvtColor(img0_color, cv2.COLOR_BGR2GRAY)
img1_color = cv2.imread(args.frame2)
img1 = cv2.cvtColor(img1_color, cv2.COLOR_BGR2GRAY)
vis = img1_color.copy()

mask = np.zeros_like(img1)
mask[:] = 255
p = cv2.goodFeaturesToTrack(img1, mask=mask, **feature_params)
if p is not None:
    for x, y in np.float32(p).reshape(-1, 2):
        tracks.append([(x, y)])

p0 = np.float32([tr[-1] for tr in tracks]).reshape(-1, 1, 2)
p1, st1, err = cv2.calcOpticalFlowPyrLK(img0, img1, p0, None, **lk_params)
p0r, st2, err = cv2.calcOpticalFlowPyrLK(img1, img0, p1, None, **lk_params)
d = abs(p0 - p0r).reshape(-1, 2).max(-1)
good = d < 1
new_tracks = []
for tr, (x, y), good_flag in zip(tracks, p1.reshape(-1, 2), good):
    if not good_flag:
        continue
    new_tracks.append(tr)
    cv2.circle(vis, (x, y), 5, (0, 0, 255), -1)
    cv2.line(vis, (x, y), (tr[0][0], tr[0][1]), (255, 255, 0))

print "number of points tracked:", len(new_tracks)

cv2.imwrite("track.jpg", vis)
