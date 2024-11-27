#include <Servo.h>
#include <stdlib.h>

Servo servo;

const int LED_PIN = 13;
void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(LED_PIN,OUTPUT);
  servo.attach(8);

  

}

void loop() {
  // put your main code here, to run repeatedly:
  while(Serial.available()){
    digitalWrite(LED_PIN,HIGH);
    String input = Serial.readStringUntil('\n');  // Liest die Daten bis zum Zeilenumbruch
    int x = input.toInt();
    servo.write(x);
    
    delay(1000);
    digitalWrite(LED_PIN,LOW);

    
  }

  }


