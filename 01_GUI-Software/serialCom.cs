using System;
using System.IO;
using System.IO.Ports;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace EurofighterInformationCenter
{
    class serialCom
    {
        private byte[] outBuffer = new byte[16] { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       01.09.2023
        //
        //  Diese Methode sendet an den Arduino
        //=======================================================================================
        public string SendSerialData(byte[] payload, string port, int baudrate)
        {
            if (port != null && baudrate >= 9600)
            {
                try
                {
                    using (SerialPort arduinoCOM = new SerialPort(port, baudrate))
                    {
                        payload[0] = 255; // Sicherstellen, dass Startbyte richtig gesetzt ist.

                        
                        // Port oeffnen und Payload versenden
                        arduinoCOM.Open();
                        arduinoCOM.Write(payload, 0, payload.Length);
                        while (arduinoCOM.BytesToWrite > 0) ;   // Nichts machen bis komplett gesendet
                        arduinoCOM.Close();

                        // Ausgabe zu sendende Payload am Fenster
                        string[] valueStrings = payload.Select(value => value.ToString()).ToArray();
                        string outputString = string.Join(";", valueStrings);
                        return ("Sendingx: " + outputString);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Senden der Daten: " + ex.Message);
                    return ("Sendingx: " + ex.Message);
                }
            }
            else
            {
                // Kein gueltiger COM-Port und/oder Baudrate angegeben
                return ("Ungültige Sendeparameter! COM: " + port + " Baudrate: " + baudrate);
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       01.09.2023
        //
        //  In dieser Methode wird der Modus festgelegt, z.B. Servo Tetst
        //=======================================================================================
        public int selectedMode(int indexSelected, string port, int baudrate) 
        { 
            switch (indexSelected) 
            {
                case 0:
                    outBuffer = new byte[] { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    return indexSelected;
                case 1:
                    outBuffer = new byte[] { 255, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    return indexSelected;
                case 2:
                    outBuffer = new byte[] { 255, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    return indexSelected;
                case 3:
                    outBuffer = new byte[] { 255, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    SendSerialData(outBuffer, port, baudrate);
                    return indexSelected;
                default:
                    return indexSelected;
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       01.09.2023
        //
        //  In dieser Methode wird der String aufbereitet, um anschlkießend in der TextBox im
        //  im ConfigSettigs Fenster angezeigt zu werden
        //=======================================================================================
        public string AppendStatusText(string msg) 
        {
            if (!msg.EndsWith(Environment.NewLine))
            {
                msg += Environment.NewLine;
            }
            return msg;
        }    
    }
}
