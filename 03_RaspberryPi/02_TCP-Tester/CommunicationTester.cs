/*------------------------------------------------*
| Autor: Noah Gerstlauer                          |
| Department: THGM-TL1                            |
| Email: Noah.Gerstlauer@airbus.com               |
| Date: 2023-09                                   |
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
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;

namespace TCP_Tester
{
    public partial class TCP_Communication_Tester : Form
    {
        // Konstanten fuer Indexe des Kommunikationspuffers
        public const int MODE = 0;
        public const int LC = 1;
        public const int RC = 2;
        public const int LS = 3;
        public const int RS = 4;
        public const int LO = 5;
        public const int RO = 6;
        public const int LI = 7;
        public const int RI = 8;
        public const int LE = 9;
        public const int RE = 10;
        public const int AB = 11;
        public const int FI = 12;
        public const int LED = 13;

        // Kommunikationspuffer
        private byte[] outBuffer = new byte[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        // Variablen fuer die TCP Kommunikation
        private int tcpPort = 0;
        private string serverIP = "null"; // IP des Raspberrys
        TcpClient client; // UI ist Client
        NetworkStream nw_stream;

        // Liest die Servo Positionen aus .csv Dateien aus
        string[] csvPresetFiles;
        PresetManager pm = new PresetManager();

      

    public TCP_Communication_Tester()
        {
            InitializeComponent();
            statusTextBox.Enabled = false;

            // Eventhandler fuer schließen der TCP Verbindung beim Schließen des Fensters
            this.FormClosing += new FormClosingEventHandler(TCP_Communication_Tester_FormClosing);

            portSelector.KeyPress += new KeyPressEventHandler(portSelector_KeyPress);
            ipSelector.KeyPress += new KeyPressEventHandler(ipSelector_KeyPress);

            portSelector.GotFocus += PortSelectorGotFocus;
            ipSelector.GotFocus += IPSelectorGotFocus;


            // Mode Selector konfigurieren
            string[] modes = { "OFF", "Static", "Remote","ServoTest"};
            modeSelector.Items.AddRange(modes);
            modeSelector.Visible = false;

            // Presets aus Ordner laden
            string presetname;
            csvPresetFiles = pm.GetCsvFileNames("../../Files/ShowPresets");
            foreach (string csvFile in csvPresetFiles)
            {
                presetname = Path.GetFileNameWithoutExtension(csvFile);
                presetSelector.Items.Add(presetname);
            }
            presetSelector.Visible = false;

            AppendStatusText("Anwendung erfolgreich gestartet.");
        }


        private void TCP_Communication_Tester_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                client.Close(); // TCP Verbindung schließen
            }
        }

        private void SendData(byte[] payload)
        {
            try
            {
                if (nw_stream != null)
                {
                    nw_stream.Write(payload, 0, payload.Length);
                    //AppendStatusText("Erfolgreich gesendet");
                }
                else
                {
                    AppendStatusText("Nicht verbunden");
                }
            }
            catch (Exception ex)
            {
                AppendStatusText(ex.Message);
                if (client != null)
                {
                    client.Close();
                    client = null;
                    modeSelector.Visible = false;
                    presetSelector.Visible = false;
                    AppendStatusText("Verbindung getrennt.");
                }
            }
        }

        // Hinzufuegen einer Zeile zu der Status Textbox
        private void AppendStatusText(string msg)
        {
            try
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

                string timestamp = DateTime.Now.ToString("HH:mm:ss"); // Aktuelle Uhrzeit hinzufügen
                string messageWithTimestamp = $"{timestamp} -> {msg}"; // Kombinieren Sie Uhrzeit und Nachricht


                statusTextBox.AppendText(messageWithTimestamp);

                // Automatisches Scrollen zur letzten Zeile
                statusTextBox.ScrollToCaret();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Generieren einer Statusmeldung");
            }
        }

        private void ClearStatusText()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ClearStatusText));
                return;
            }

            statusTextBox.Clear();
        }


        private bool IsValidIPv4(string ipAddress)
        {
            // Überprüfen Sie, ob die eingegebene Zeichenfolge eine gültige IPv4-Adresse ist
            if (IPAddress.TryParse(ipAddress, out IPAddress parsedIp))
            {
                if (parsedIp.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return true;
                }
            }
            return false;
        }

        private bool PingServer(string ipAddress)
        {
            try
            {
                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(ipAddress);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ipSelector_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                serverIP = ipSelector.Text;

                if (IsValidIPv4(serverIP))
                {

                    if (PingServer(serverIP))
                    {
                        AppendStatusText("Server ist erreichbar");
                    }
                    else
                    {
                        AppendStatusText("ACHTUNG: Server nicht erreichbar");
                    }
                    portSelector.Focus();
                }
                else
                {
                    AppendStatusText("IP Adresse nicht gültig!");
                }
            }
        }

        private void portSelector_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (int.TryParse(portSelector.Text, out tcpPort))
                {
                    try
                    {
                        if (tcpPort > 1024)
                        {
                            client = new TcpClient(serverIP, tcpPort);
                            nw_stream = client.GetStream();
                            string msg = "Verbindung hergestellt. Port:" + tcpPort + " IP: " + serverIP;
                            modeSelector.Visible = true;
                            AppendStatusText(msg);
                            SendData(outBuffer);
                        }
                        else
                        {
                            AppendStatusText("Bitte einen Port > 1024 wählen");
                        }
                    }
                    catch (Exception ex)
                    {
                        ClearStatusText();
                        AppendStatusText(ex.Message);
                    }
                }
                else
                {
                    AppendStatusText("Kein gültiger Port angegeben");
                }
            }
        }


        private void PortSelectorGotFocus(object sender, EventArgs e)
        {
            if (portSelector.Text == "Port")
            {
                portSelector.Text = ""; // Platzhaltertext löschen
            }
        }

        private void IPSelectorGotFocus(object sender, EventArgs e)
        {
            if (ipSelector.Text == "Server IP")
            {
                ipSelector.Text = ""; // Platzhaltertext löschen
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            ClearStatusText();
        }

        private void trackBarLC_Scroll(object sender, EventArgs e)
        {
            if ( client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LC] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarRC_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }
            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RC] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarLS_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LS] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarRS_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RS] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarLI_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LI] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarRI_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RI] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarLO_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LO] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarRO_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RO] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarAB_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[AB] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarFI_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[FI] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarLE_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[LE] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void trackBarRE_Scroll(object sender, EventArgs e)
        {
            if (client == null) { return; }

            if (sender is TrackBar slider)
            {

                int sliderValue = slider.Value;
                if (sliderValue < 0) sliderValue = 0;
                else if (sliderValue > 255) sliderValue = 255;
                byte sliderByte = (byte)sliderValue;

                outBuffer[RE] = sliderByte;
                SendData(outBuffer);
            }
        }

        private void modeSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (client == null) { return; }

            // AUS
            if (modeSelector.SelectedIndex == 0)
            {
                outBuffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                SendData(outBuffer);

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
                outBuffer = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                SendData(outBuffer);

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
                outBuffer = new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                SendData(outBuffer);

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
            else if (modeSelector.SelectedIndex == 3) 
            {
                outBuffer = new byte[] { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                SendData(outBuffer);

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

        private void disconButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (client != null)
                {
                    client.Close();
                    client = null; 
                    modeSelector.Visible = false;
                    presetSelector.Visible = false;
                    AppendStatusText("Verbindung getrennt.");
                }
            }
            catch (Exception ex)
            {
                AppendStatusText("Fehler beim Trennen der Verbindung: " + ex.Message);
            }

        }

        private void presetSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Auslesen der Servopositionen aus der ausgewaehlten .csv Datei
                outBuffer = pm.LoadValuesFromCsv(csvPresetFiles[presetSelector.SelectedIndex]);
                Console.WriteLine("Loaded: " + Path.GetFileNameWithoutExtension(csvPresetFiles[presetSelector.SelectedIndex]));


                for (int i = 0; i < outBuffer.Length; i++)
                {
                    Console.WriteLine(outBuffer[i]);
                }

                string[] valueStrings = outBuffer.Select(value => value.ToString()).ToArray();
                string outputString = string.Join(";", valueStrings);
                AppendStatusText("Loaded: " + outputString);
                SendData(outBuffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden der CSV: {ex.Message}");
            }
        }

        private void TCP_Communication_Tester_Load(object sender, EventArgs e)
        {

        }
    }
    
}