#------------------------------------------------*
# Autor: Noah Gerstlauer                          |
# Department: THGM-TL1                            |
# Email: Noah.Gerstlauer@airbus.com               |
# Date: 2023-09                                   |
#------------------------------------------------*/

import sys
import signal
import socket
import argparse
from adafruit_servokit import ServoKit
from time import sleep

# Adresse des Servo Treibers
I2CSERVO = 0x40
I2CLED = 0x41

# TCP Port
PORT_CONST = 4444

# Konstanten fuer die Indexe der empfangenden Daten
MODE = 0
LC = 1
RC = 2
LS = 3
RS = 4
LO = 5
RO = 6
LI = 7
RI = 8
LE = 9
RE = 10
AB = 11
FI = 12
LED = 13
LED_LE = 14 # LED Left Engine (dynamisch) .  ACHTUNG: Wird nicht verwendet, da Triebwerksbeleuchtung über RE
LED_RE = 15 # LED Right Engine (dynamisch) . ACHTUNG: Wird nicht verwendet, da Triebwerksbeleuchtung über RE
LDP_H = 16 # LDP Servo Horizontal
LDP_V = 17 # LDP vertikal
LDP_L = 18 # LDP Laserpointer


# Pulslaengen fuer die Servos
pulselenLC = 0
pulselenRC = 0
pulselenLS = 0
pulselenRS = 0
pulselenAB = 0
pulselenFI = 0
pulselenLO = 0
pulselenLI = 0
pulselenRI = 0
pulselenRO = 0
pulselenLE = 0
pulselenRE = 0
pulselenLED_ENGINE = 0
pulselenLDP_V = 0
pulselenLDP_H = 0
pulselenLDP_L = 0

try:
    # Initialisierung des Netzwerksockets
    HOST = ''
    PORT = PORT_CONST
    server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_socket.bind((HOST, PORT))
    server_socket.listen(5)
    print(f"Server lauscht auf {HOST}:{PORT}")

except Exception as e:
    print(f"Server konnte nicht gestartet werden: {e}")
    exit(1)

try:
    # Initialisierung des ServoKit-Objekts fuer die Servos TODO: Adresse anpassen
    servodriver = ServoKit(channels=16, address=I2CSERVO)

except Exception as e:
    print(f"(SERVO) ServoKit konnte nicht initialisiert werden: {e}")

try:
    #Initialisierung des ServoKit-Objekts fuer die LEDs TODO: Adresse anpassen
    leddriver = ServoKit(channels=16, address=I2CLED)

except Exception as e:
    print(f"(LED) ServoKit konnte nicht initialisiert werden: {e}")


# Servos in die vorgegebene Position fahren
def PWMsetServo_EF():
    try:
        servodriver.servo[0].angle = pulselenLC
        servodriver.servo[1].angle = pulselenRC
        servodriver.servo[2].angle = pulselenLS
        servodriver.servo[3].angle = pulselenRS
        servodriver.servo[4].angle = pulselenAB
        servodriver.servo[5].angle = pulselenFI
        servodriver.servo[6].angle = pulselenLO
        servodriver.servo[7].angle = pulselenLI
        servodriver.servo[8].angle = pulselenRI
        servodriver.servo[9].angle = pulselenRO
        servodriver.servo[10].angle = pulselenLE
        servodriver.servo[11].angle = pulselenRE
        
        servodriver.servo[12].angle = pulselenLED_ENGINE # Triebwerksbeleuchtung (beide an Port 12)
        servodriver.servo[13].angle = pulselenLDP_L # Laserpointer

        servodriver.servo[14].angle = pulselenLDP_H # Horizontal
        servodriver.servo[15].angle = pulselenLDP_V # Vertikal

    except Exception as e:
        print(f"(EF) Fehler beim Ansteuern der Servos: {e}")


def RuheModus():
    try:
        servodriver.servo[0].angle = 0
        servodriver.servo[1].angle = 0
        servodriver.servo[2].angle = 0
        servodriver.servo[3].angle = 0
        servodriver.servo[4].angle = 0
        servodriver.servo[5].angle = 0
        servodriver.servo[6].angle = 0
        servodriver.servo[7].angle = 0
        servodriver.servo[8].angle = 0
        servodriver.servo[9].angle = 0
        servodriver.servo[10].angle = 0
        servodriver.servo[11].angle = 0
        servodriver.servo[12].angle = 0
        servodriver.servo[13].angle = 0
        servodriver.servo[14].angle = 0
        servodriver.servo[15].angle = 0
    except Exception as e:
        print(f"Fehler beim Ansteuern der Servos: {e}")


# Ansteuern der LEDs mithilfe von PWM Signalen, NOTE: 180 ist 100%
def PWMsetLEDs():
    try:  
        # Triebwerksbeleuchtung
        #LE_LED = map(receivedData[LE], 135, 104, 0, 4096)
        #leddriver.servo[0].angle = (LE_LED + (4096 // 16) * 16) % 4096

        #RE_LED = map(receivedData[RE], 127, 103, 0, 4096)
        #leddriver.servo[0].angle = (RE_LED + (4096 // 16) * 16) % 4096

        # Cockpit
        leddriver.servo[5].angle = 180

        # Achtung: Werte uebernommen von vorheriger Software:
        led = receivedData[LED]
        for i in range(0, 10):
            leddriver.servo[i].angle = 0

        if led == 0:  # Alles aus
            pass
        elif led == 1:  # Laserwarner
            leddriver.servo[1].angle = 180
        elif led == 2:  # Missilewarner
            leddriver.servo[2].angle = 180
        elif led == 3:  # Radarwarner
            leddriver.servo[3].angle = 180
        elif led == 4:  # ESM/ECM
            leddriver.servo[4].angle = 180
        elif led == 5:  # FLIR
            leddriver.servo[6].angle = 180
        elif led == 6:
            leddriver.servo[7].angle = 180
    except Exception as e:
        print(f"Fehler beim Ansteuern der LEDs: {e}")


def ServoTest():
    for cylce in range(2):
        for angle in range(0, 170):
            for servoNum in range(0, 16):
                servodriver.servo[servoNum].angle = angle
                sleep(0.001)
        RuheModus()


# Bereinigt Sockets und beendet Programm
def handle_exit(signum, frame):
    print("Beenden...")
    if 'client_socket' in globals() and client_socket is not None:
        client_socket.close()
    if 'server_socket' in globals() and server_socket is not None:
        server_socket.close()
    sys.exit(0)

#-------------------------------------------------------------------------------------------------------------------
# Main Loop

signal.signal(signal.SIGINT, handle_exit) # Signalhandler fuer STRG+C (Software korrekt beenden)



while True:
    try:
        print("Warte auf Verbindung...")
        client_socket, addr = server_socket.accept()
        print(f"Verbindung von {addr} hergestellt")

    except Exception as e:
        print(f"Verbindung konnte nicht hergestellt werden: {e}")
        exit(1)

    while True:
        receivedData = client_socket.recv(32)
        if not receivedData:
            break

        printData = ','.join(str(byte) for byte in receivedData)
        print(f"Empfangene Daten: {printData}")

        # Empfangende Pulslaengen in Variablen speichern
        pulselenLC = receivedData[LC]
        pulselenRC = receivedData[RC]
        pulselenLS = receivedData[LS]
        pulselenRS = receivedData[RS]
        pulselenAB = receivedData[AB]
        pulselenFI = receivedData[FI]
        pulselenLO = receivedData[LO]
        pulselenLI = receivedData[LI]
        pulselenRI = receivedData[RI]
        pulselenRO = receivedData[RO]
        pulselenLE = receivedData[LE]
        pulselenRE = receivedData[RE]
        pulselenLDP_H = receivedData[LDP_H]
        pulselenLDP_V = receivedData[LDP_V]
        pulselenLDP_L = receivedData[LDP_L]
        pulselenLED_ENGINE = receivedData[RE] # Triebwerksbeleuchtung bekommt selbe Pulslaenge wie Engine weil Right=Left



        if receivedData[MODE] == 0: # Aus / RuheModus
            print("RuheModus")
            RuheModus()

        # NOTE: Showmodus entfernt, da nicht mehr benoetigt
        #        elif receivedData[MODE] == 1: # Showmodus, Servos fahren langsam in Position
        #           print("Showmodus")
        #           #PWMsetServo_EF_SLOW()
        #           PWMsetLEDs()

        elif receivedData[MODE] == 1:
            print("LED-Steuerung")
            PWMsetLEDs()
        elif receivedData[MODE] == 2:  # RemoteModus, Servos fahren (schnell) in Position
            print("RemoteModus")
            PWMsetServo_EF()
        elif receivedData[MODE] == 3:   # Servotest
            print("Servotest")
            ServoTest()
        elif receivedData[MODE] > 3 or receivedData[MODE] < 0:
            print("Fehlerhafte Daten empfangen")
            RuheModus()

    client_socket.close()
