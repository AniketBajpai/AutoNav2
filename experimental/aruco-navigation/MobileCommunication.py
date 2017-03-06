import socket, traceback
import netifaces as ni
import math
import Queue
import threading
import time

host = ni.ifaddresses('wlan0')[2][0]['addr']
#print ip  # should print "192.168.100.37
# Note that the IP Address and Port in this script and the script on the Mobile Phone should match.
#host = "10.192.29.88"   #IP Address of this Machine
port = 3400

s = None
q = Queue.LifoQueue()

def initsocket():
    global s
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    s.setsockopt(socket.SOL_SOCKET, socket.SO_BROADCAST, 1)
    s.bind((host, port))

    t = threading.Thread(target=storeYaw, args=(q, 0))
    t.daemon = True
    t.start()
    return s

def storeYaw(q, shit):
    global s
    while True:
        try:
            message, address = s.recvfrom(4096)
            orient = float((message.split())[1])
            orient = orient * 180/math.pi
            q.put(orient)
        except (KeyboardInterrupt, SystemExit):
            raise
        except:
            traceback.print_exc()

def getYaw():
    return q.get()

# if __name__ == '__main__':
#     initsocket()
#     while True:
#         time.sleep(0.2)
#         print getYaw()
