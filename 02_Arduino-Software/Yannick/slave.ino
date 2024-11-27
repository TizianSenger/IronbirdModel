#include <Servo.h>
#include <Wire.h>
/*-----------------------------------------*
 | Autor:       Yannick Stibbe             |
 | Department:  THGM-TL1                   |
 | Email:       yannick.stibbe@airbus.com  |
 | Date:        11.08.2023                 |
 *-----------------------------------------*/

Servo test[3];
const int rudder = 3;
const int engine = 4;
void setup()
{
  Serial.begin(9600);
  Wire.begin(8);
  Wire.onReceive(receiveEvent);
  //Servo[0] ist der Test Servo 
  test[0].attach(rudder); // Attach servo on pin 3, change pin if necessary
  test[1].attach(engine);
  
  /*
  Möglichekeit weitere Servos hinzuzufügen 
  test[1].attach(Servo_Pin);
  test[2].attach(Servo_Pin);
  */
}

void loop()
{
  delay(1000);
}

void receiveEvent(int numBytes)
{ 
  if (numBytes >= 2){ //Warte auf 2 Signale vom Master (Index, Angle)
  if (Wire.available()) {
    int index = Wire.read();
    Serial.print("Servo: ");
    Serial.print(index);
    Serial.println();
    int pos = Wire.read();
    /*Serial Output */
    Serial.print("Empfangen: ");
    Serial.print(pos);
    Serial.println();
    
    
    test[index].write(pos); // Stelle den Servo mit dem gesendten Indes auf den richtigen Angle
    /*Achtung Index entspricht nicht dem Pin an dem Servo angeschlossen ist */ 


    /*Serial Output */

    int pos2 = test[index].read();
    Serial.print("Position: ");
    Serial.print(pos2);
    Serial.println();
  }
  }
}