#include <SoftwareSerial.h>


//Standard PWM DC control
int E1 = 5;     //M1 Speed Control
int E2 = 6;     //M2 Speed Control
int M1 = 4;    //M1 Direction Control
int M2 = 7;    //M1 Direction Control

///For previous Romeo, please use these pins.
//int E1 = 6;     //M1 Speed Control
//int E2 = 9;     //M2 Speed Control
//int M1 = 7;    //M1 Direction Control
//int M2 = 8;    //M1 Direction Control

byte* command = {};

SoftwareSerial sensorPort(2, 3);

void stop(void)                    //Stop
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
void doMoveBackward (int speed)         //Move backward
{
  analogWrite (E1, speed);
  digitalWrite(M1, LOW);
  analogWrite (E2, speed);
  digitalWrite(M2, LOW);
}
void doTurnLeft (int speed)            //Turn Left
{
  analogWrite (E1, speed);
  digitalWrite(M1, LOW);
  analogWrite (E2, speed);
  digitalWrite(M2, HIGH);
}
void doTurnRight (int speed)            //Turn Right
{
  analogWrite (E1, speed);
  digitalWrite(M1, HIGH);
  analogWrite (E2, speed);
  digitalWrite(M2, LOW);
}

void doSayHello(int data)
{
  Serial.println("Hello from Robo");
}

void doMeasureDistance (int angle)            //Turn Right
{
  byte data[4] = {0x22, 0x00, 0x00, 0x00};
  data[1] = angle;
  data[3] = ((data[0] + data[1] + data[2]) & 0xFF);
  for (int i = 0; i < 4; i++) {
    sensorPort.write(data[i]);
  }

  memset(data, 0, sizeof(data));
  while (sensorPort.available() > 0)  //if serial receive any data
  {
    for (int i = 0; i < 4; i++) {
      data[i] = sensorPort.read();
    }
    Serial.write(data, 4);
  }
}

void setup(void)
{
  for (int i = 4; i <= 7; i++) {
    pinMode(i, OUTPUT);
  }
  Serial.begin(9600);      //Set Baud Rate
  Serial.println("Run keyboard control");
  sensorPort.begin(9600);


}

void executeCommand(int* command)
{
  int data = command[1];
  switch (command[0]) {

    case 0:
      doSayHello(data);
      break;
    case 1:
      doMoveForward(data);
      break;
    case 2:
      doMoveBackward(data);
      break;
    case 3:
      doTurnLeft(data);
      break;
    case 4:
      doTurnRight(data);
      break;

    case 5:
      doMeasureDistance(data);
      break;

    default:
      // if nothing else matches, do the default
      // default is optional
      break;
  }
}

bool validCommand(int* command)
{
  bool  result = sizeof(command) == 3;
  int sum = 0;
  for (int i = 0; i < sizeof(command) - 1; i++)
  {
    sum = sum + command[i];
  }
  result = result && ((sum & 0xFF) == command[sizeof(command) - 1]);
  return result;
}

void loop(void)
{
  analogWrite (E1, 100);     //PWM Speed Control
  digitalWrite(M1, HIGH);
  analogWrite (E2, 100);
  digitalWrite(M2, HIGH);
  /*int byteCount = Serial.available();

  for (int i = 0; i < byteCount; i++) {
    command[i] = Serial.print("Readed: ");
    Serial.write(command[i]);
    Serial.println();
  }


  if (validCommand(command)) {
    Serial.println("Command is valid");
    executeCommand(command);
    command = {};
  }
  else {
    Serial.println("Command is invalid");
    command = {};
  }*/
}




