/*------------------------------------------------*
| Autor: Noah Gerstlauer                          |
| Department: THGM-TL1                            |
| Email: Noah.Gerstlauer@airbus.com               |
| Date: 2023-08-21                                |
*------------------------------------------------*/


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using EF_SerialCommunication_Tester;


/* Testsoftware um die Kommunikation zwischen WindowsPC und des Masterarduinos zu testen.
   In einem simplen Windows-Forms Fenster ist es möglich den verwendeten COM-Port und
   Baudrate auszuwählen.

   Anschließend können die verschiedenen Modi ausgewählt werden und die einzelnen Servos 
   angesteuert werden.


   Der Datenaustausch wird durch ein 16 Byte Array realisiert:

   Index | Beschreibung | Wertebereich | Erklärung
   ------+--------------+--------------+------------------------------------------------------------
     0   | Startbyte    |  [255]       | Startbyte zur Synchronisation. Hat immer den max. Wert 255
     1   | MODE         |  [0,2]       | Festlegen des Betriebsmodus (Aus, Static, Remote)
     2   | LC           |  [0,254]     | Left Canard
     3   | RC           |  [0,254]     | Right Canard
     4   | LS           |  [0,254]     | Left Slat
     5   | RS           |  [0,254]     | Right Slat
     6   | LO           |  [0,254]     | Left Out Flap
     7   | RO           |  [0,254]     | Right Out Flap
     8   | LI           |  [0,254]     | Left In Flap
     9   | RI           |  [0,254]     | Right In Flap
    10   | LE           |  [0,254]     | Left Engine
    11   | RE           |  [0,254]     | Right Engine
    12   | AB           |  [0,254]     | Airbreak
    13   | FI           |  [0,254]     | Fin
    14   | LED          |  [0,6]       | Festlegen der LED Modi
    15   | Reserve      |  [0]         | Nicht verwendet

*/


namespace EF_SerialCommunication_Tester
{
    public partial class CommunicationTester : Form
    {
        // Konstanten fuer Indexe des Kommunikationspuffers
        public const int MODE = 1;
        public const int LC = 2;
        public const int RC = 3;
        public const int LS = 4;
        public const int RS = 5;
        public const int LO = 6;
        public const int RO = 7;
        public const int LI = 8;
        public const int RI = 9;
        public const int LE = 10;
        public const int RE = 11;
        public const int AB = 12;
        public const int FI = 13;
        public const int LED = 14;

        // Kommunikationspuffer
        private byte[] outBuffer = new byte[16] { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        string selectedComPort = "null";
        string[] csvPresetFiles;
        int selectedBaudrate = 115200;

        // Liest die Servo Positionen aus .csv Dateien aus
        PresetManager pm = new PresetManager();

        // Konstruktor
        public CommunicationTester()
        {
            InitializeComponent();

            // Ports abfragen und zum Dropdown hinzufuegen
            string[] ports = SerialPort.GetPortNames();
            portSelector.Items.AddRange(ports);

            string[] baud_rates = { "9600", "115200" };
            baudSelector.Items.AddRange(baud_rates);
            baudSelector.SelectedItem = "115200";
 		
            // Dropdown Menue fuer die Modusauswahl
            string[] modes = { "OFF", "Static", "Remote" };
            modeSelector.Items.AddRange(modes);
            modeSelector.SelectedIndex = 0;

            presetSelector.Visible = false; // Presets erst auswaehlbar, wenn Static Mode

            // LED Beleuchtungsmodi
            string[] ledModes = { "OFF", "Laserwarner", "Misslewarner", "Radarwarner", "ESM/ECM", "FLIR", "Cockpit?" };
            LEDselector.Items.AddRange(ledModes);

            // Presets aus Ordner laden
            string presetname;
            csvPresetFiles = pm.GetCsvFileNames("../../Files/ShowPresets");
            foreach (string csvFile in csvPresetFiles)
            {   
               presetname = Path.GetFileNameWithoutExtension(csvFile);
               presetSelector.Items.Add(presetname);   
            }
        }



        // Senden der payload ueber den ausgewaehlten port an den Arduino
        private void SendSerialData(byte[] payload, string port, int baudrate)
        {
            if (port != null && baudrate >= 9600)
            {
                try
                {
                    using (SerialPort arduinoCOM = new SerialPort(port, baudrate))
                    {
                        payload[0] = 255; // Sicherstellen, dass Startbyte richtig gesetzt ist.

                        // Ausgabe zu sendende Payload am Fenster
                        string[] valueStrings = payload.Select(value => value.ToString()).ToArray();
                        string outputString = string.Join(";", valueStrings);
                        AppendStatusText("Sending: " + outputString);

                        // Port oeffnen und Payload versenden
                        arduinoCOM.Open();
                        arduinoCOM.Write(payload, 0, payload.Length);
                        while (arduinoCOM.BytesToWrite > 0) ;   // Nichts machen bis komplett gesendet
                        arduinoCOM.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Fehler beim Senden der Daten: " + ex.Message);
                    AppendStatusText(ex.Message);
                }
            } 
            else
            {
                // Kein gueltiger COM-Port und/oder Baudrate angegeben
                Console.WriteLine("Ungültige Sendeparameter! COM: " + port + " Baudrate: " + baudrate);
                AppendStatusText("Ungültige Sendeparameter! COM: " + port + " Baudrate: " + baudrate);
            }
        }



        // Hinzufuegen einer Zeile zu der Status Textbox
        private void AppendStatusText(string msg)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendStatusText), msg);
                return;
            }

            if (!msg.EndsWith(Environment.NewLine))
            {
                msg += Environment.NewLine;
            }

            statusTextBox.AppendText(msg);

            // Automatisches Scrollen zur letzten Zeile
            statusTextBox.ScrollToCaret();
        }



//---------------------------------------------------------------------------------------------------------------------------------------
// UI-Elemente 

        private void LEDselector_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            byte selectedLEDmode = (byte) LEDselector.SelectedIndex;
            outBuffer[LED] = selectedLEDmode;
            SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
        }


        private void portSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedComPort = portSelector.SelectedItem.ToString();
        }



        private void baudSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedBaudrate = int.Parse(baudSelector.SelectedItem.ToString());
        }



        private void modeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // AUS
            if (modeSelector.SelectedIndex == 0)
            {
                outBuffer = new byte[] { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                presetSelector.Visible = false;
                trackBarLC.Enabled = false;
                trackBarRC.Enabled = false;
                trackBarLS.Enabled = false;
                trackBarRS.Enabled = false;
                trackBarLO.Enabled = false;
                trackBarRO.Enabled = false;
                trackBarLI.Enabled = false;
                trackBarRI.Enabled = false;
                trackBarLE.Enabled = false;
                trackBarRE.Enabled = false;
                trackBarAB.Enabled = false;
                trackBarFI.Enabled = false;

            }
            // Static
            else if (modeSelector.SelectedIndex == 1)
            {
                outBuffer = new byte[] { 255, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                presetSelector.Visible = true;
                trackBarLC.Enabled = false;
                trackBarRC.Enabled = false;
                trackBarLS.Enabled = false;
                trackBarRS.Enabled = false;
                trackBarLO.Enabled = false;
                trackBarRO.Enabled = false;
                trackBarLI.Enabled = false;
                trackBarRI.Enabled = false;
                trackBarLE.Enabled = false;
                trackBarRE.Enabled = false;
                trackBarAB.Enabled = false;
                trackBarFI.Enabled = false;
            }
            // Remote
            else if (modeSelector.SelectedIndex == 2)
            {
                outBuffer = new byte[] { 255, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                presetSelector.Visible = false;
                trackBarLC.Enabled = true;
                trackBarRC.Enabled = true;
                trackBarLS.Enabled = true;
                trackBarRS.Enabled = true;
                trackBarLO.Enabled = true;
                trackBarRO.Enabled = true;
                trackBarLI.Enabled = true;
                trackBarRI.Enabled = true;
                trackBarLE.Enabled = true;
                trackBarRE.Enabled = true;
                trackBarAB.Enabled = true;
                trackBarFI.Enabled = true;
            }
            // Kein gueltiger Index
            else
            {
                Console.WriteLine("Fehler bei Modusauswahl!");
                presetSelector.Visible = false;
                trackBarLC.Enabled = false;
                trackBarRC.Enabled = false;
                trackBarLS.Enabled = false;
                trackBarRS.Enabled = false;
                trackBarLO.Enabled = false;
                trackBarRO.Enabled = false;
                trackBarLI.Enabled = false;
                trackBarRI.Enabled = false;
                trackBarLE.Enabled = false;
                trackBarRE.Enabled = false;
                trackBarAB.Enabled = false;
                trackBarFI.Enabled = false;
            }
        }

        
        private void presetSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Auslesen der Servopositionen aus der ausgewaehlten .csv Datei
                outBuffer = pm.LoadValuesFromCsv(csvPresetFiles[presetSelector.SelectedIndex]);
                Console.WriteLine("Loaded: " + Path.GetFileNameWithoutExtension(csvPresetFiles[presetSelector.SelectedIndex]));
                

                for(int i = 0; i < outBuffer.Length; i++)
                {
                    Console.WriteLine(outBuffer[i]);
                }

                string[] valueStrings = outBuffer.Select(value => value.ToString()).ToArray();
                string outputString = string.Join(";", valueStrings);
                AppendStatusText("Loaded: " + outputString);
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der CSV: {ex.Message}");
            }
        }



        private void trackBarLC_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte) sliderValue;
                
                outBuffer[LC] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarRC_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RC] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarLS_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LS] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarRS_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RS] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarLO_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LO] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarRO_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 254) sliderValue = 254;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RO] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarLI_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LI] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarRI_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RI] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarLE_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LE] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarRE_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RE] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarAB_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[AB] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }



        private void trackBarFI_Scroll(object sender, EventArgs e)
        {
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[FI] = sliderByte;
                SendSerialData(outBuffer, selectedComPort, selectedBaudrate);
            }
        }

        
    }
}
