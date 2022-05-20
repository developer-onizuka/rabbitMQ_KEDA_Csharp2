import sys 
import cv2
import face_recognition
 
inputFile = sys.argv[1]  
outputFile = sys.argv[2]
 
#print(cv2.__version__)

im = cv2.imread(inputFile)

######

#im = cv2.bitwise_not(im)

imRGB = cv2.cvtColor(im,cv2.COLOR_BGR2RGB)
facePosition = face_recognition.face_locations(imRGB,model='cnn')

for (top,right,bottom,left) in facePosition:
    cv2.rectangle(im,(left,top),(right,bottom),(0,0,255),3)

######

cv2.imwrite(outputFile, im)
