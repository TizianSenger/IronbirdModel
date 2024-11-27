# Servo Control for EF Model - Arduino Code

**Author:** Noah Gerstlauer
**Department:** THGM-TL1
**Email:** Noah.Gerstlauer@airbus.com
**Date:** 2023-08-21

## Overview

This Arduino code is designed for controlling servos in an EF model. It receives positional instructions from a Windows PC via the serial interface and uses these instructions to control the servos using a Servo Driver. Additionally, another Servo Driver is utilized to control LEDs within the model.

## Data Exchange Protocol

The data exchange between the PC and the Arduino is achieved through a 16-byte array with the following structure:

| Index | Description      | Value Range | Explanation                                            |
|-------|------------------|-------------|--------------------------------------------------------|
| 0     | Start Byte       | [255]       | Synchronization start byte (always 255)              |
| 1     | MODE             | [0,2]       | Operating mode selection (Off, Static, Remote)        |
| 2     | LC               | [0,254]     | Left Canard                                           |
| 3     | RC               | [0,254]     | Right Canard                                          |
| 4     | LS               | [0,254]     | Left Slat                                            |
| 5     | RS               | [0,254]     | Right Slat                                           |
| 6     | LO               | [0,254]     | Left Out Flap                                         |
| 7     | RO               | [0,254]     | Right Out Flap                                        |
| 8     | LI               | [0,254]     | Left In Flap                                          |
| 9     | RI               | [0,254]     | Right In Flap                                         |
| 10    | LE               | [0,254]     | Left Engine                                          |
| 11    | RE               | [0,254]     | Right Engine                                         |
| 12    | AB               | [0,254]     | Airbrake                                              |
| 13    | FI               | [0,254]     | Fin                                                   |
| 14    | LED              | [0,6]       | LED modes selection                                   |
| 15    | Reserve          | [0]         | Not used                                              |

## Libraries Used

This code uses the Adafruit PWM Servo Driver library to control the servos and LEDs. Ensure you have this library installed before uploading the code to your Arduino.

## Code Description

- The code initializes two Adafruit_PWMServoDriver objects for controlling servos and LEDs.
- Constants for buffer indices are defined to interpret received data.
- Pulse lengths for various servos are calculated based on received data.
- Servo signals are set using the PWM_SetServoSignals() function.
- LED signals are set using the PWM_SetLEDSignals() function.
- The serialEvent() function is used to read and process incoming data.
- Setup initializes the serial communication, Servo Drivers, and sets servos to their default positions.
- The loop function is left empty as all functionality is handled in serialEvent().

## Usage

1. Ensure that you have the Adafruit PWM Servo Driver library installed.
2. Upload this code to your Arduino.
3. Connect your Arduino to the Windows PC with the control software via the serial interface.
4. Make sure that the control software on the Windows PC sends the correct 16 byte array over the serial port.

## Notes

- Ensure that the baud rate in the Arduino code (Serial.begin) matches the baud rate used in the control software on the Windows PC.
- This code is designed for specific servo and LED configurations. Make sure the hardware setup is correct.

