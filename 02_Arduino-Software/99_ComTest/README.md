# EF Serial Communication Tester

**Author:** Noah Gerstlauer
**Department:** THGM-TL1
**Email:** Noah.Gerstlauer@airbus.com
**Date:** 2023-08-21

## Overview

This is a software designed to test the communication between a Windows PC and a Master Arduino. It provides a simple Windows Forms interface to select the COM port and baud rate. Additionally, it allows users to choose various modes and control individual servos.

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

---

## Usage

1. Run the software.
2. Select the COM port and baud rate from the dropdown menus.
3. Choose the desired mode (Off, Static, Remote) from the mode dropdown.
4. Control individual servo positions using the sliders (only available in Remote mode).
5. Select an LED mode from the LED dropdown to control LED behavior.

### Note

- The presets option is only available in Static mode.
- Presets are loaded from CSV files located in the "Files/ShowPresets" folder.
- The software ensures synchronization by setting the start byte to 255.

---

## Dependencies

- The software relies on the `System` and `System.IO.Ports` libraries for serial communication.
- It uses the `EF_SerialCommunication_Tester` namespace.

---

## UI Elements

- `LEDselector`: Dropdown for selecting LED modes.
- `portSelector`: Dropdown for selecting the COM port.
- `baudSelector`: Dropdown for selecting the baud rate.
- `modeSelector`: Dropdown for selecting the operating mode (Off, Static, Remote).
- `presetSelector`: Dropdown for selecting presets (visible in Static mode).
- `trackBarLC` to `trackBarFI`: Sliders for controlling servo positions (enabled in Remote mode).
- `statusTextBox`: Textbox for displaying status messages.



## Functions

- `SendSerialData(byte[] payload, string port, int baudrate)`: Sends payload data to the Arduino over the selected COM port and baud rate.
- `AppendStatusText(string msg)`: Appends a message to the status textbox.


## Additional Notes

- Ensure that the baud rate in the Arduino code (Serial.begin) matches the baud rate used in this control software

- This code is designed for specific servo and LED configurations. Make sure the hardware setup is correct.




