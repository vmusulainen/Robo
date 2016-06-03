#include <SoftwareSerial.h>

uint8_t  EnAutCmd[4] = {0x44, 0x02, 0xaa, 0x00};
uint8_t EnTempCmd[4] = {0x22, 0x00, 0x00, 0x00};
uint8_t TempData[4];
unsigned int TempValue = 0;
unsigned int Degree = 0;
bool Reverse = false;
SoftwareSerial sensorPort(2, 3);

int E1 = 5;     //M1 Speed Control
int E2 = 6;     //M2 Speed Control
int M1 = 4;    //M1 Direction Control
int M2 = 7;    //M1 Direction Control

byte startByte = 0xfe;
boolean readingCommand = false;

const byte maxCommandLength = 255-5;
byte commandCode = -1;
byte commandData[maxCommandLength];
byte commandLen = 0;
int posInCommand = 0;

byte scanBuf[5];

const byte COMMAND_STATUS = 0;
const byte COMMAND_ANSWER = 1;
const byte COMMAND_ERROR = 2;
const byte COMMAND_MOVE = 3;
const byte COMMAND_RANGE_SCAN = 4;

const byte MOVE_STOP = 0;
const byte MOVE_FORWARD = 1;
const byte MOVE_BACKWARD = 2;
const byte MOVE_TURN_LEFT = 3;
const byte MOVE_TURN_RIGHT = 4;
        
void processCommand() {
  switch (commandCode) {
      case COMMAND_STATUS:
          SensorCmd(commandData[0]);
          break;
      case COMMAND_MOVE:
          processMoveCommand();
          break;
      case COMMAND_RANGE_SCAN:
          doRangeScan();
          break;
  }
}

void processMoveCommand() {
  byte moveCommand = commandData[0];
  byte moveSpeed = commandData[1];
  byte degree = commandData[2];
  bool stopAfter = (bool)commandData[3];
  /*byte buf[5];
  buf[4] = moveSpeed;
  buf[1] = 0;
  buf[2] = moveSpeed;
  sendCommand(COMMAND_STATUS, buf, 5);*/

  byte answer[2] = {COMMAND_MOVE, 0xFF};
  switch (moveCommand) {
      case MOVE_STOP:
          stopMovement();
          
          sendCommand(COMMAND_ANSWER, answer, 2);
          break;
      case MOVE_FORWARD:
          doMoveForward(moveSpeed);
          sendCommand(COMMAND_ANSWER, answer, 2);
          break;
      case MOVE_BACKWARD:
          doMoveBackward(moveSpeed);
          sendCommand(COMMAND_ANSWER, answer, 2);
          break;
      case MOVE_TURN_LEFT:
          turn(true, degree, moveSpeed, stopAfter);
          break;
      case MOVE_TURN_RIGHT:
          turn(false, degree, moveSpeed, stopAfter);
          break;
  }
}

void turn(bool left, byte degree, byte turnSpeed, bool stopAfter) {
  float fullTurnTime = 16000;  
  int delayTime = (degree / 360.0) * fullTurnTime * (float)turnSpeed/255.0;
  byte turnDirection;
  
  if (left) {
    turnDirection = MOVE_TURN_LEFT;
    doTurnLeft(turnSpeed);
  }
  else {
    turnDirection = MOVE_TURN_RIGHT;
    doTurnRight(turnSpeed);
  }

  delay(delayTime);

  if (stopAfter) {
    stopMovement();
  }

  byte answer[] = {COMMAND_MOVE, 0xFF, turnDirection};
  sendCommand(COMMAND_ANSWER,  answer, 3);
}

void stopMovement(void)                    //Stop
{
  digitalWrite(E1, LOW);
  digitalWrite(E2, LOW);
}

void doMoveForward(int speed)         //Move forward
{
  analogWrite (E1, speed);     //PWM Speed Control
  digitalWrite(M1, HIGH);
  analogWrite (E2, speed);
  digitalWrite(M2, HIGH);
}

void doMoveBackward(int speed)         //Move backward
{
  analogWrite (E1, speed);
  digitalWrite(M1, LOW);
  analogWrite (E2, speed);
  digitalWrite(M2, LOW);
}

void doTurnRight(int speed)            //Turn Left
{
  analogWrite (E1, speed);
  digitalWrite(M1, LOW);
  analogWrite (E2, speed);
  digitalWrite(M2, HIGH);
}

void doTurnLeft(int speed)            //Turn Right
{
  analogWrite (E1, speed);
  digitalWrite(M1, HIGH);
  analogWrite (E2, speed);
  digitalWrite(M2, LOW);
}

byte calcCrc(byte code, byte len, byte* buf) {
  byte crc = startByte;
  crc += code;
  crc += len;
  for(int i = 0; i < len; i++) {
    crc += buf[i];
  }

  return crc;
}

void sendCommand(byte code, byte *data, byte len) {
  byte buf[255];
  buf[0] = startByte;
  buf[1] = code;
  buf[2] = len;
  byte *bufPtr = buf+3;
  memcpy(bufPtr, data, len);
  buf[len+3] = calcCrc(code, len, data);
  
  Serial.write(buf, len + 4);
}

void readCommand(int byteCount) {
  byte bt;
  for (int i = 0; i < byteCount; i++) {
    bt = Serial.read();

    if (readingCommand) {
      switch (posInCommand) {
        case 0:
          //command code
          commandCode = bt;
          break;
        case 1:
          //command length
          commandLen = bt;
          break;
        default: 
          //data
          if (posInCommand < commandLen + 2) {
            commandData[posInCommand - 2] = bt;
          }
        break;
      }

      if (posInCommand == commandLen+2){
        readingCommand = false;
        if (bt == calcCrc(commandCode, commandLen, commandData)) {
          processCommand();
        }
        else {
          Serial.println("Wrong CRC");
        }
      }

      posInCommand++;
    }
    else {
      if (bt == startByte) {
        readingCommand = true;
        posInCommand = 0;
        commandLen = 0;
      }
    }
  }
}

byte* scanSensor(byte degree, bool sendError=false) {
  //byte buf[5];
  EnTempCmd[1] = degree;
  EnTempCmd[3] = ((EnTempCmd[0] + EnTempCmd[1] + EnTempCmd[2]) & 0xFF);
  for (int i = 0; i < 4; i++) {
    sensorPort.write(EnTempCmd[i]);
  }
  
  unsigned long timeStamp = millis();
  while (sensorPort.available() < 4) {
    if ((millis() - timeStamp) > 2000 ) {
      if (sendError) {
        byte errorData[] = {0};
        sendCommand(COMMAND_ERROR,  errorData, 1);
      }
      return NULL;
    }
  }
  
  for (int i = 0; i < 4; i++) {
    scanBuf[i] = sensorPort.read();
  }
 
  scanBuf[4] = degree;
  return scanBuf;
}

void SensorCmd(byte degree)
{
  byte* buf = scanSensor(degree, true);
  if (buf) {
    sendCommand(COMMAND_STATUS, buf, 5);
  }
}

void doRangeScan() {
  byte startDegree = commandData[0];   
  byte endDegree = commandData[1];

  byte result[90];
  result[0] = commandData[0];
  result[1] = commandData[1];
  
  int delayTime;
  byte degree;
  int len = endDegree - startDegree + 1;
  for(int i = 0; i < len; i++) {
    byte* buf = scanSensor(startDegree + i);
    result[i*2 + 2] = buf[1];
    result[i*2 + 3] = buf[2];
    
    if (i == 0) {
      delayTime = 1000;
    }
    else {
      delayTime = 5;
    }
    
    delay(delayTime);
  }
  
  sendCommand(COMMAND_RANGE_SCAN, result, len*2+2);
}

void setup() {
  // put your setup code here, to run once:\
  pinMode(13, OUTPUT);
  Serial.begin(9600);      //Set Baud Rate
  sensorPort.begin(9600);

  EnAutCmd[3] = ((EnAutCmd[0] + EnAutCmd[1] + EnAutCmd[2]) & 0xFF);
  initSensor();
  Serial.println("FRF Initialization completed");
}


void initSensor()
{
  int i;
  for (i = 0; i < 4; i++) {
    sensorPort.write(EnAutCmd[i]);
  }
}

void loop() {
  // put your main code here, to run repeatedly:
  int byteCount = Serial.available();

  readCommand(byteCount);
  //delay(5);

  //digitalWrite(13, HIGH);   // turn the LED on (HIGH is the voltage level)
  //delay(500);              // wait for a second
  //digitalWrite(13, LOW);    // turn the LED off by making the voltage LOW
  //delay(500);              // wait for a second
}
