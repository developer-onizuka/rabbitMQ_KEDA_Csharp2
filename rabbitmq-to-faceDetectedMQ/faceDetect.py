import sys 
import cv2
 
inputFile = sys.argv[1]  
outputFile = sys.argv[2]
 
#print(cv2.__version__)

im = cv2.imread(inputFile)

im = cv2.bitwise_not(im)

cv2.imwrite(outputFile, im)
