#include <Adafruit_PWMServoDriver.h>


#include <Wire.h>


Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver(0x40);
Adafruit_PWMServoDriver led = Adafruit_PWMServoDriver(0x41);


/*IC2 Bus */ 
const int SLAVE_ADRESSE = 8; // Slave Adresse 
const int NUM_BYTES = 1;     // Anzahl der Nachrichten(Bytes)

// Konstante Indexe für den Puffer
const int MODE = 1;
const int LC = 2;
const int RC = 3;
const int LS = 4;
const int RS = 5;
const int LO = 6;
const int RO = 7;
const int LI = 8;
const int RI = 9;
const int LE = 10;
const int RE = 11;
const int AB = 12;
const int FI = 13;
const int LED = 14;

// Pulslaengen, welche dem PWM Shield gesendet werden
uint16_t pulselenLC = 0;
uint16_t pulselenRC = 0;
uint16_t pulselenLS = 0;
uint16_t pulselenRS = 0;
uint16_t pulselenAB = 0;
uint16_t pulselenFI = 0;
uint16_t pulselenLO = 0;
uint16_t pulselenLI = 0;
uint16_t pulselenRI = 0;
uint16_t pulselenRO = 0;
uint16_t pulselenLE = 0;
uint16_t pulselenRE = 0;

// Variablen fuer den Empfangspuffer
const int bufferSize = 16;
byte receivedData[bufferSize];
bool bufferComplete = false;
int bufferPos = 0;

// Engine LEDs
int LE_LED = 0; // Left Engine LED
int RE_LED = 0; // Right Engine LED


// Ansteuern der Servos uber den Servo Treiber
void PWM_SetServoSignals()
{
  // Sicherstellen, dass die Servos sich nicht über die max. moegliche Position drehen

// TODO: Richtige Werte herausfinden!
  pulselenLC =  map(receivedData[LC], 0, 254, 240, 500);
  pulselenRC =  map(receivedData[RC], 0, 254, 240, 500);
  pulselenLS =  map(receivedData[LS], 0, 254, 240, 500);
  pulselenRS =  map(receivedData[RS], 0, 254, 240, 500);
  pulselenAB =  map(receivedData[AB], 0, 254, 240, 500);
  pulselenFI =  map(receivedData[FI], 0, 254, 240, 500);
  pulselenLO =  map(receivedData[LO], 0, 254, 240, 500);
  pulselenLI =  map(receivedData[LI], 0, 254, 240, 500);
  pulselenRI =  map(receivedData[RI], 0, 254, 240, 500);
  pulselenRO =  map(receivedData[RO], 0, 254, 240, 500);
  pulselenLE =  map(receivedData[LE], 0, 254, 240, 500);
  pulselenRE =  map(receivedData[RE], 0, 254, 240, 500);

  pwm.setPWM(0 , 0, pulselenLC);
  pwm.setPWM(1 , 0, pulselenRC);  
  pwm.setPWM(2 , 0, pulselenLS);
  pwm.setPWM(3 , 0, pulselenRS);
  pwm.setPWM(4 , 0, pulselenAB);
  pwm.setPWM(5 , 0, pulselenFI);
  pwm.setPWM(6 , 0, pulselenLO);
  pwm.setPWM(7 , 0, pulselenLI);
  pwm.setPWM(8 , 0, pulselenRI);
  pwm.setPWM(9 , 0, pulselenRO);
  pwm.setPWM(10, 0, pulselenLE);
  pwm.setPWM(11, 0, pulselenRE);

}


// Servos im Static Modus langsam in die gewuenschte Position fahren
// Muss noch implementiert werden!
void PWM_SetServoSignals_SLOW()
{
  int targetPos[12];
  targetPos[0]  = map(receivedData[LC], 0, 254, 240, 500);
  targetPos[1]  = map(receivedData[RC], 0, 254, 240, 500);
  targetPos[2]  = map(receivedData[LS], 0, 254, 240, 500);
  targetPos[3]  = map(receivedData[RS], 0, 254, 240, 500);
  targetPos[4]  = map(receivedData[AB], 0, 254, 240, 500);
  targetPos[5]  = map(receivedData[FI], 0, 254, 240, 500);
  targetPos[6]  = map(receivedData[LO], 0, 254, 240, 500);
  targetPos[7]  = map(receivedData[LI], 0, 254, 240, 500);
  targetPos[8]  = map(receivedData[RI], 0, 254, 240, 500);
  targetPos[9]  = map(receivedData[RO], 0, 254, 240, 500);
  targetPos[10] = map(receivedData[LE], 0, 254, 240, 500);
  targetPos[11] = map(receivedData[RE], 0, 254, 240, 500);
/*

  TODO:

  int currentPos[12];
  for (int i = 0; i < 12; i++) {
    currentPos[i] = pwm.getPulseLength(i);
  }

  bool moving = true;
  while (moving) {
    moving = false;
    for (int i = 0; i < 12; i++) {
      if (currentPos[i] < targetPos[i]) {
        currentPos[i]++;
        moving = true;
      } else if (currentPos[i] > targetPos[i]) {
        currentPos[i]--;
        moving = true;
      }
      pwm.setPWM(i , 0, currentPos[i]);
    }
    delay(10);
  }
  */
}

// Ansteuern der LEDs mit dem zweiten Servo Treiber mittels PWM
void PWM_SetLEDSignals()
{
  // Triebwerksbeleuchtung
  LE_LED = map(receivedData[LE], 135, 104, 0, 4096);
  led.setPWM(0, 0, (LE_LED + (4096/16)*16) % 4096);

  RE_LED = map(receivedData[RE], 127, 103, 0, 4096);
  led.setPWM(0, 0, (RE_LED + (4096/16)*16) % 4096);

  // Cockpit
  led.setPWM(5, 0, 4080);

// Achtung: Uebernommen von vorheriger Software:
  switch(receivedData[LED])
  {
  case 0://Alles aus
    //   led.setPWM(0 , 0, 0);
    led.setPWM(1 , 0, 0);
    led.setPWM(2 , 0, 0);
    led.setPWM(3 , 0, 0);
    led.setPWM(4 , 0, 0);
    //  led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 0);
    led.setPWM(7 , 0, 0);
    //    led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0); 
    break; 

  case 1://Laserwarner
    //    led.setPWM(0 , 0, 4080);
    led.setPWM(1 , 0, 4080);
    led.setPWM(2 , 0, 0);
    led.setPWM(3 , 0, 0);
    led.setPWM(4 , 0, 0);
    //    led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 0);
    led.setPWM(7 , 0, 0);
    //   led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0); 
    break;

  case 2://Missilewarner
    //    led.setPWM(0 , 0, 0);
    led.setPWM(1 , 0, 0);
    led.setPWM(2 , 0, 4080);
    led.setPWM(3 , 0, 0);
    led.setPWM(4 , 0, 0);
    //   led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 0);
    led.setPWM(7 , 0, 0);
    //    led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0); 
    break;
  case 3://Radarwarner
    //    led.setPWM(0 , 0, 0);
    led.setPWM(1 , 0, 0);
    led.setPWM(2 , 0, 0);
    led.setPWM(3 , 0, 4080);
    led.setPWM(4 , 0, 0);
    //    led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 0);
    led.setPWM(7 , 0, 0);
    //    led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0);   
    break;

  case 4://ESM/ECM
    //    led.setPWM(0 , 0, 0);
    led.setPWM(1 , 0, 0);
    led.setPWM(2 , 0, 0);
    led.setPWM(3 , 0, 0);
    led.setPWM(4 , 0, 4080);
    //    led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 0);
    led.setPWM(7 , 0, 0);
    //    led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0);    
    break;

  case 5://FLIR
    //    led.setPWM(0 , 0, 0);
    led.setPWM(1 , 0, 0);
    led.setPWM(2 , 0, 0);
    led.setPWM(3 , 0, 0);
    led.setPWM(4 , 0, 0);
    //    led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 4080);
    led.setPWM(7 , 0, 0);
    //    led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0);    
    break;

  case 6:
    //    led.setPWM(0 , 0, 0);
    led.setPWM(1 , 0, 0);
    led.setPWM(2 , 0, 0);
    led.setPWM(3 , 0, 0);
    led.setPWM(4 , 0, 0);
    //    led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 0);
    led.setPWM(7 , 0, 4080);
    //    led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0);   
    break;  
  }
}

// Ruheposition bzw. alles aus
void RuheModus()
{
  led.setPWM(1 , 0, 0);
    led.setPWM(2 , 0, 0);
    led.setPWM(3 , 0, 0);
    led.setPWM(4 , 0, 0);
    //  led.setPWM(5 , 0, 0);
    led.setPWM(6 , 0, 0);
    led.setPWM(7 , 0, 0);
    //    led.setPWM(8 , 0, 0); 
    led.setPWM(9 , 0, 0); 
    
    PWM_SetDefaultPose();
}

// Grundstellung des Flugzeugmodells
void PWM_SetDefaultPose()
{
  pwm.setPWM(0, 0, 224);
  pwm.setPWM(1, 0, 0);
  pwm.setPWM(2, 0, 254);
  pwm.setPWM(3, 0, 0);
  pwm.setPWM(4, 0, 226);
  pwm.setPWM(5, 0, 126);
  pwm.setPWM(6, 0, 238);
  pwm.setPWM(7, 0, 168);
  pwm.setPWM(8, 0, 50);
  pwm.setPWM(9, 0, 56);
  pwm.setPWM(10, 0, 120);
  pwm.setPWM(11, 0, 120);
}

// Auswerten der ueber die serielle Schnittstelle empfangenen Daten
void processSerialData(byte data[], int length)
{
  
  digitalWrite(12, HIGH);
  if(bufferComplete)
  {
   
    bufferComplete = false;
    
    if(receivedData[MODE] == 2) // Remote
    {
      digitalWrite(13, HIGH);
      PWM_SetServoSignals();
      PWM_SetLEDSignals();
    }
    else if (receivedData[MODE] == 1) // Show Modus - Langsam Statische Pos einnehmen
    {
      PWM_SetServoSignals_SLOW();
      PWM_SetLEDSignals();
    }
    else
    {
      // Ruheposition & Alles aus
      RuheModus();
    }
  }

}


// Wird durch Interrupt von SeriellerSchnittstelle aufgerufen
// Auslesen der Daten
void serialEvent() {
  while (Serial.available()) {
    byte inByte = (byte)Serial.read();

    // Startbyte ist immer 255. Dadurch wird die Kommunikation synchronisiert. Ansonsten koennte es
    // zu einem Synchronisationsverlust kommen und Daten werden falsch interpretiert
    if (inByte == 255 && bufferPos == 0) {
      receivedData[bufferPos++] = inByte;
    } else if (bufferPos > 0 && bufferPos < bufferSize) {
      receivedData[bufferPos++] = inByte;
    }

    if (bufferPos == bufferSize) {
      bufferComplete = true;

      // Verarbeitung der empfangenen Daten
      processSerialData(receivedData, bufferSize);

      // Zurücksetzen des Pufferstatus
      bufferPos = 0;
      bufferComplete = false;
    }
  }
}


/*----------------------------------------------------------------------------------------------------------------*/


// Initialisieren 
void setup()
{
  /* IC2 Bus */

  Wire.begin();


  Serial.begin(115200); // Muss mit der Baudrate im C# UI uebereinstimmen
  pwm.begin();
  led.begin();

  pwm.setPWMFreq(60); // 60Hz
  led.setPWMFreq(1000); // 1kHz
  delay(10);

  // Alle Servos in Ruhestellung fahren
  PWM_SetDefaultPose();
  
}

void loop()
{
  // Nothing here
  Wire.beginTransmission(SLAVE_ADRESSE); // Starte Kommunikation auf IC2 Bus

  int message = 10; 
  Wire.write(message);
  Wire.endTransmission();

}