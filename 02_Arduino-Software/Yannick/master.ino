/*Master Code */
/*-----------------------------------------*
 | Autor:       Yannick Stibbe             |
 | Department:  THGM-TL1                   |
 | Email:       yannick.stibbe@airbus.com  |
 | Date:        11.08.2023                 |
 *-----------------------------------------*/

#include <Wire.h>

const int LED_PIN = 13; 

void setup() {
  Wire.begin(); // Start I2C as master
  Serial.begin(9600);
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW); // Schalte die LED aus
}

void loop() {
  int Angle;
  int index;
  while(Serial.available()){
    digitalWrite(LED_PIN,HIGH);
    byte index = Serial.read();
    byte Angle = Serial.read();
    

    
    /* Index des anzusteuernden Servos */
    int Servo_Index = index;

    /* Sende Sequenz */
    Wire.beginTransmission(8); // Start transmission to device with address 8 (slave 1)
    Wire.write(Servo_Index); // Send Servo Index 
    Wire.write(Angle); // Send Angle
    Wire.endTransmission(); // End transmission
    digitalWrite(LED_PIN,LOW);

    
  }
  

  
  
  delay(1000);
}
