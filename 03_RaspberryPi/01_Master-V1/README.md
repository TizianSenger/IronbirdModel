# Steuerungssoftware für Servo- und LED-Treibersystem
---

**Author:** Noah Gerstlauer
**Department:** THGM-TL1
**Email:** Noah.Gerstlauer@airbus.com
**Date:** 2023-09

---

## Einführung
Diese Software steuert ein Servo- und LED-Treibersystem über ein das TCP Netzwerkprotokoll. Sie ermöglicht die Fernsteuerung verschiedener Servos und LEDs des Eurofighter-Messemodells.

---

## Voraussetzungen
Bevor Sie diese Software verwenden können, müssen Sie sicherstellen, dass Sie die folgenden Voraussetzungen erfüllen:

- **Adafruit ServoKit Library:** Diese Software verwendet die Adafruit ServoKit-Bibliothek für die Ansteuerung von Servos und LEDs. Stellen Sie sicher, dass Sie diese Bibliothek installiert haben.

- **Hardware:** Sie benötigen den Adafruit PWM Servo Driver für die Servos und gegebenenfalls einen weiteren Treiber für die LEDs. Stellen Sie sicher, dass die Hardware korrekt angeschlossen ist.

- **Python:** Die Software ist in Python geschrieben. Stellen Sie sicher, dass Sie Python auf Ihrem System installiert haben.

---

## Konfiguration
Die Konfiguration der Software erfolgt über die folgenden Konstanten im Code:

- **I2CSERVO:** Die Adresse des Servo-Treibers.

- **PORT_CONST:** Der TCP-Port, über den die Steuerungskommandos empfangen werden.

- Konstanten für die Indexe der empfangenen Daten: Diese Konstanten definieren die Positionen der Steuerungsdaten im empfangenen Datenpaket.

- Pulslängen für die Servos: Diese Variablen speichern die Pulslängen für die einzelnen Servos.

---

## Verwendung
1. Stellen Sie sicher, dass die Hardware korrekt angeschlossen ist und die Adafruit ServoKit-Bibliothek installiert ist.

2. Ändern Sie die Konfigurationswerte nach Bedarf, um Ihre Hardware und Anforderungen anzupassen.

3. Führen Sie die Software aus, um den Server zu starten, der auf eingehende Verbindungen wartet.

4. Verbinden Sie sich mit dem Server über das Netzwerkprotokoll, um die Servos und LEDs fernzusteuern.

---

## Befehle
Die Software unterstützt verschiedene Steuerungsbefehle, die über das Netzwerkprotokoll gesendet werden können. Diese Befehle umfassen:

- **0 (RuheModus):** Setzt alle Servos und LEDs in die Ausgangsposition.

- **1 (LED-Steuerung):** Steuert die LEDs.

- **2 (RemoteModus):** Steuert die Servos.

- **3 (Servotest):** Führt einen Servotest aus.

- **4 (LDP Steuerung):** Steuert die Servos für den Laser-Entfernungsmesser.

---

## Beenden
Die Software kann durch Drücken von `STRG+C` beendet werden. Dadurch werden die Sockets geschlossen und die Software ordnungsgemäß beendet.

---

## Fehlerbehebung
Bei Problemen oder Fehlern in der Software sollten Sie sicherstellen, dass die Hardware korrekt angeschlossen ist und die Adafruit ServoKit-Bibliothek auf dem neuesten Stand ist.
