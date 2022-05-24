import sys 
import cv2
import requests
import json
#import face_recognition
 
inputFile = sys.argv[1]  
outputFile = sys.argv[2]
 
#print(cv2.__version__)

im = cv2.imread(inputFile)

######

#im = cv2.bitwise_not(im)

#imRGB = cv2.cvtColor(im,cv2.COLOR_BGR2RGB)
#facePosition = face_recognition.face_locations(imRGB,model='cnn')
url = "http://face-recognizer-api-svc:5000/facerecognizer"
files_data = open(inputFile, 'rb').read()
data = {'img': (inputFile, files_data, 'image/jpeg')}
response = requests.post(url, files=data)

#print(format(response.text))

json_dict = json.loads(response.text)
facePositions = json_dict['facePositions']

for (top,right,bottom,left) in facePositions:
    cv2.rectangle(im,(left,top),(right,bottom),(0,0,255),3)

######

cv2.imwrite(outputFile, im)
