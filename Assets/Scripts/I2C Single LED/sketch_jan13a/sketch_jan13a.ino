#include "Wire.h"

int valueArrayOld[]={1024, 850, 785, 725, 560};

int valueArray[]={1024, 850, 785, 725, 560};

const int sizeOfArray=5;
int detectionThreshold=10;

byte inputArrayOut[9];
byte inputArrayOut_Previous[9];
byte inputArrayOut_Previous_Pre[9];

byte inputArrayOut_Final[9];

const int tailInput=1;
const int eyeInput=3;
const int blockInput=2;
const int repairInput=0;

long lastTime;
long currentTime;
float delayTime=20;

bool turnOff=false;

void setup() {


  Serial.setTimeout(1);

  inputArrayOut[0]='P';
  inputArrayOut[1]='C';
  inputArrayOut[2]='4';
  inputArrayOut[8]='Z';

  inputArrayOut_Final[0]='P';
  inputArrayOut_Final[1]='C';
  inputArrayOut_Final[2]='4';
  inputArrayOut_Final[8]='Z';

  Serial.begin(9600);

  Wire.begin();
  Wire.beginTransmission(0x20);
  Wire.write(0x00); //Control address
  Wire.write(0x00); //Control setting for A
  Wire.write(0x00); //Control setting for B
  Wire.endTransmission();

  Wire.begin();
  Wire.beginTransmission(0x21);
  Wire.write(0x00); //Control address
  Wire.write(0x00); //Control setting for A
  Wire.write(0x00); //Control setting for B
  Wire.endTransmission();
}

byte incomingArray[8];
byte rawData[100];
int index;
int bytesAvailable;

void loop() {

  currentTime=millis();
  while(currentTime<=(lastTime+delayTime))
  {
      currentTime=millis();
  }

    // if(turnOff)
    // {
    //   turnOff=false;
    // }
    // else
    // {
    //   turnOff=true;
    // }
    lastTime=millis();


  bytesAvailable = Serial.available();
  if(bytesAvailable>0){

    for(int k=0; k<100; k++)
    {
      rawData[k]=0;
    }
    // int len = Serial.readBytes(incomingArray, bytesAvailable);
    Serial.readBytes(rawData, bytesAvailable);

    for(int i=0; i<bytesAvailable; i++)
    {
      if((rawData[i]=='L') && (rawData[i+1]=='C'))
      {
        index = i;
        break;
      }
    }
    for(int j=0; j<8; j++)
      {
        incomingArray[j] = rawData[index+j];
      }
  }

    // incomingArray[3]=B11111111;
    // incomingArray[4]=B11111111;
    // incomingArray[5]=B11111111;
    // incomingArray[6]=B11111111;
    // incomingArray[3]=B11111111;
    // incomingArray[4]=B11111111;
    // incomingArray[5]=B00000000;
    // incomingArray[6]=B11111111;

  if(incomingArray[7]=='Z')
  {
    Wire.beginTransmission(0x20);
    Wire.write(0x14); //Port B
    Wire.write(incomingArray[3]);
    Wire.write(incomingArray[4]); 
    Wire.endTransmission();

    Wire.beginTransmission(0x21);
    Wire.write(0x14); //Port B
    Wire.write(incomingArray[5]);
    Wire.write(incomingArray[6]); 
    Wire.endTransmission();
  }
  // else
  // {
  //   Wire.beginTransmission(0x20);
  //   Wire.write(0x14); //Port B
  //   Wire.write(0x00);
  //   Wire.write(0x00); 
  //   Wire.endTransmission(0x00);

  //   Wire.beginTransmission(0x21);
  //   Wire.write(0x14); //Port B
  //   Wire.write(0x00);
  //   Wire.write(0x00); 
  //   Wire.endTransmission();
  // }

  DetectInput();

  // delay(20);
}

void DetectInput()
{
  int tailValue=analogRead(tailInput);
  int eyeValue=analogRead(eyeInput);
  int blockValue=analogRead(blockInput);
  int repairValue=analogRead(repairInput);

  for(int j=3; j<7; j++)
  {
      inputArrayOut_Previous_Pre[j]= inputArrayOut_Previous[j];
      inputArrayOut_Previous[j]= inputArrayOut[j];
  }
  assignInputNumbers(eyeValue, 3);
  assignInputNumbers(blockValue, 4);
  assignInputNumbers(tailValue, 5);
  assignInputNumbers(repairValue, 6);
  inputArrayOut[7]=(digitalRead(13));

  inputArrayOut[8]='Z';

  for(int j=3; j<7; j++)
  {
      if((inputArrayOut_Previous[j] == inputArrayOut[j]) && (inputArrayOut[j]==inputArrayOut_Previous_Pre[j]))
      {
        inputArrayOut_Final[j]=inputArrayOut[j];
      }
      else
      {
        inputArrayOut_Final[j]=inputArrayOut_Previous_Pre[j];
      }
  }

  inputArrayOut_Final[7]=inputArrayOut[7];

  Serial.write(inputArrayOut, 9);
  // for(int i=0; i<7; i++){
  //   Serial.print(inputArrayOut[i]);
  // }
  // Serial.println();
}

void assignInputNumbers(int readValue, int arrayPlace)
{
  int i;
  for (i=0; i<5; i++){
    if(readValue > (valueArray[i] - detectionThreshold) &&
      readValue < (valueArray[i] + detectionThreshold))
    {
      inputArrayOut[arrayPlace]=i;
      break;
    }
  }
  if(i==5)
  {
    inputArrayOut[arrayPlace]=0;
  }
}
