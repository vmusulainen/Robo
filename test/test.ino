
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
    Serial.print(bt, HEX);
    Serial.println();
  }

  digitalWrite(13, HIGH);   // turn the LED on (HIGH is the voltage level)
  delay(500);              // wait for a second
  digitalWrite(13, LOW);    // turn the LED off by making the voltage LOW
  delay(500);              // wait for a second
}
