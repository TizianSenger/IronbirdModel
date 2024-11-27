# TCP Communication Tester for Eurofighter Model

**Author:** Noah Gerstlauer
**Department:** THGM-TL1
**Email:** Noah.Gerstlauer@airbus.com
**Date:** 2023-09

## Overview

The TCP Communication Tester is a Windows Forms application designed to facilitate communication with the Eurofighter model via the TCP protocol. This user interface (UI) serves as a TCP client, allowing you to interact with the Eurofighter model remotely. To utilize this software effectively, you must have a TCP server running on a Raspberry Pi or a similar device. The server should be in the same IP range as your computer.

## Getting Started

1. **Prerequisites**: 
   - Ensure that you have a TCP server set up on a Raspberry Pi or similar device.
   - Make sure the server is running and accessible from your computer.

2. **Launching the Application**:
   - Run the TCP Communication Tester application on your Windows machine.

3. **Configuring the Connection**:
   - In the UI, enter the IP address of the server in the "Server IP" field.
   - Enter the desired TCP port number in the "TCP Port" field.

4. **Establishing the Connection**:
   - Press the "Enter" key after entering the server IP and TCP port information.
   - The application will attempt to establish a TCP connection with the server.

5. **Remote Control**:
   - Once the connection is established, you can use the software to control the servos on the Eurofighter model.

6. **Logging and Feedback**:
   - The application provides a status textbox that displays messages and logs.
   - You can use the "Clear" button to clear the status textbox.

## Notes

- This software is intended for testing and development purposes only. It does not represent the final user interface for communication with the Eurofighter model.
- The Eurofighter TCP Communication Tester is designed as a TCP client. You should have a corresponding TCP server set up to receive and process commands from this application.
- Ensure that your server is running and correctly configured with the necessary hardware to control the servos on the Eurofighter model.
