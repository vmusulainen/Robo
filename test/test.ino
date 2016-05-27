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

const byte COMMAND_STATUS = 0;
const byte COMMAND_ANSWER = 1;
const byte COMMAND_ERROR = 2;
const byte COMMAND_MOVE = 3;

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
  }
}

void processMoveCommand() {
  byte moveCommand = commandData[0];
  byte moveSpeed = commandData[1];
  
  /*byte buf[5];
  buf[4] = moveSpeed;
  buf[1] = 0;
  buf[2] = moveSpeed;
  sendCommand(COMMAND_STATUS, buf, 5);*/
   
  switch (moveCommand) {
      case MOVE_STOP:
          stopMovement();
          break;
      case MOVE_FORWARD:
          doMoveForward(moveSpeed);
          break;
      case MOVE_BACKWARD:
          doMoveBackward(moveSpeed);
          break;
      case MOVE_TURN_LEFT:
          doTurnLeft(moveSpeed);
          break;
      case MOVE_TURN_RIGHT:
          doTurnRight(moveSpeed);
          break;
  }
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

void doTurnLeft(int speed)            //Turn Left
{
  analogWrite (E1, speed);
  digitalWrite(M1, LOW);
  analogWrite (E2, speed);
  digitalWrite(M2, HIGH);
}

void doTurnRight(int speed)            //Turn Right
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
      }
    }
  }
}

void SensorCmd(byte degree)
{
  byte buf[5];
  EnTempCmd[1] = degree;
  EnTempCmd[3] = ((EnTempCmd[0] + EnTempCmd[1] + EnTempCmd[2]) & 0xFF);
  for (int i = 0; i < 4; i++) {
    sensorPort.write(EnTempCmd[i]);
  }
  
  unsigned long timeStamp = millis();
  while (sensorPort.available() < 4) {
    if ((millis() - timeStamp) > 2000 ) {
      byte errorData[] = {0};
      sendCommand(COMMAND_ERROR,  errorData, 1);
      return;
    }
  }
  
  for (int i = 0; i < 4; i++) {
    buf[i] = sensorPort.read();
  }
 
  buf[4] = degree;
  sendCommand(COMMAND_STATUS, buf, 5);
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
