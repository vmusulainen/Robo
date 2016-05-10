
byte startByte = 0xfe;
boolean readingCommand = false;

byte commandCode = -1;
byte commandData[256];
byte commandLen = 0;

byte maxCommandLength = 254;
int posInCommand = 0;


void processCommand() {
  Serial.println("Received command:" );
  Serial.println(commandCode);
  Serial.write(commandData, sizeof(commandData));
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

  byte bt;
  for (int i = 0; i < byteCount; i++) {
    bt = Serial.read();

    if (readingCommand) {
      switch (posInCommand) {
        case 0:
          //command code
          commandCode = bt;
          Serial.println("Command code:");
          Serial.println(commandCode);
          break;
        case 1:
          //command length
          commandLen = bt;
          Serial.println("Command len:");
          Serial.println(commandLen);
          break;
        default: 
          //data
          commandData[posInCommand - 2] = bt;
        break;
      }

      if (posInCommand >= 1 && posInCommand-1 == commandLen){
        readingCommand = false;
        processCommand();
      }

      posInCommand++;
    }
    else {
      if (bt == startByte) {
        readingCommand = true;
        posInCommand = 0;
        Serial.println("Start byte found");
      }
    }

    delay(50);
    //Serial.print(bt, HEX);
    //Serial.println();
  }

  //digitalWrite(13, HIGH);   // turn the LED on (HIGH is the voltage level)
  //delay(500);              // wait for a second
  //digitalWrite(13, LOW);    // turn the LED off by making the voltage LOW
  //delay(500);              // wait for a second
}
