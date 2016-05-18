
byte startByte = 0xfe;
boolean readingCommand = false;

const byte maxCommandLength = 255-5;
byte commandCode = -1;
byte commandData[maxCommandLength];
byte commandLen = 0;
int posInCommand = 0;

const byte COMMAND_STATUS = 0;

void processCommand() {
  //Serial.println("Received command:" );
  //Serial.println(commandCode);
  switch (commandCode) {
      case COMMAND_STATUS:
          byte data[] = { 4,5,6 };
          sendCommand(COMMAND_STATUS, data, 3);
          break;
  }
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
          //Serial.println("Command code:");
          //Serial.println(commandCode);
          break;
        case 1:
          //command length
          commandLen = bt;
          //Serial.println("Command len:");
          //Serial.println(commandLen);
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
        //Serial.println("Start byte found");
      }
    }
  }
}

void setup() {
  // put your setup code here, to run once:\
  pinMode(13, OUTPUT);
  Serial.begin(9600);      //Set Baud Rate
  Serial.println("Run keyboard control");
}

void loop() {
  // put your main code here, to run repeatedly:
  int byteCount = Serial.available();

  readCommand(byteCount);
  delay(5);

  //digitalWrite(13, HIGH);   // turn the LED on (HIGH is the voltage level)
  //delay(500);              // wait for a second
  //digitalWrite(13, LOW);    // turn the LED off by making the voltage LOW
  //delay(500);              // wait for a second
}
