#include <SoftwareSerial.h>

uint8_t  EnAutCmd[4] = {0x44, 0x02, 0xaa, 0x00};
uint8_t EnTempCmd[4] = {0x22, 0x00, 0x00, 0x00};
uint8_t TempData[4];
unsigned int TempValue = 0;
unsigned int Degree = 0;
bool Reverse = false;
SoftwareSerial sensorPort(2, 3);


void setup()
{
  Serial.begin(9600);
  sensorPort.begin(9600);
  delay(100);
  Serial.println("Init the sensor");

  EnAutCmd[3] = ((EnAutCmd[0] + EnAutCmd[1] + EnAutCmd[2]) & 0xFF);
  SerialCmd1();
}
void loop()
{


  EnTempCmd[1] = Degree;
  EnTempCmd[3] = ((EnTempCmd[0] + EnTempCmd[1] + EnTempCmd[2]) & 0xFF);

  SerialCmd();

  if (Degree == 46) {
    Reverse = true;
  }
  if (Degree == 0) {
    Reverse = false;
  }

  if (Reverse) {
    Degree = Degree - 1;
  }
  else {
    Degree = Degree + 1;
  }

  delay(1);
}


void SerialCmd1()
{
  int i;
  for (i = 0; i < 4; i++) {
    sensorPort.write(EnAutCmd[i]);
  }
}


void SerialCmd()
{
  int i;
  for (i = 0; i < 4; i++) {
    sensorPort.write(EnTempCmd[i]);
  }
  delay(1);
  while (sensorPort.available() > 0)  //if serial receive any data
  {
    for (i = 0; i < 4; i++) {
      TempData[i] = sensorPort.read();
      Serial.print("Readed: ");
      Serial.print(TempData[i], HEX);
      Serial.print(" | ");
    }
    TempValue = TempData[1] * 256;
    TempValue = TempValue + TempData[2];
    Serial.print("distance: ");
    Serial.print(TempValue, DEC);
    Serial.println();
  }
}
