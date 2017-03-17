export PYTHONPATH=/home/aman/AutoNav2/SegNet/caffe-segnet/python:$PYTHONPATH
# python /home/aman/AutoNav2/SegNet/Scripts/compute_bn_statistics.py /home/aman/AutoNav2/SegNet/Models/segnet_train.prototxt /home/aman/AutoNav2/SegNet/Models/Training/segnet_iter_8000.caffemodel /home/aman/AutoNav2/SegNet/Models/Inference/  # compute BN statistics for SegNet

python /home/aman/AutoNav2/SegNet/Scripts/test_segmentation_camvid.py --model /home/aman/AutoNav2/SegNet/Models/segnet_inference.prototxt --weights /home/aman/AutoNav2/SegNet/Models/Inference/test_weights.caffemodel --iter 233
