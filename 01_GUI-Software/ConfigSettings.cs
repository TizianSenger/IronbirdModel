using System;
using System.IO;
using System.IO.Ports;
using System.Media;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.DirectInput;
using EurofighterInformationCenter;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using System.Web.UI.Design;
using AxWMPLib;
using SerialCommunicator.DMX;
using System.Drawing.Text;
using System.Diagnostics.Eventing.Reader;
using System.Data.SqlTypes;

namespace EurofighterInformationCenter
{
    public partial class ConfigSettings : Form
    {
        public InformationCenterMainPage informationCenterMainPage;
        public DataView dataView;
        public Visualisierung visualisierung;
        public Contact contact;


        private DirectInput directInput;
        private Joystick joystick;
        private Joystick throttle;
        private Timer timer;


        datahandler datahandlerInstance = new datahandler();
        logger loggerInstance = new logger();

        EurofighterControl controlInstance = new EurofighterControl();

        public string userName = Environment.UserName;

        public string iPAdress = "";
        public int portNumber = 0;
        public bool servoTestPerformed = false;
        public bool foggerStatus = false;
        public bool sound = false;


        public int maxValue = 65535;
        public int airbrakeVal = 0;
        public int rudderPos = 32640;
        public int throttlePos = 0;
        public bool throttlePosStart = false;
        public bool rudderPosStart = false;

        public bool randMovement = false;


        public string selectedControlMode = "Aircraft";
        public string tempOutMSG = "";

        public bool applicationRunning = false;
        public bool connectedToEurofighter = false;

        public bool foggerAvailable = true;

        // Konstanten fuer Indexe des Kommunikationspuffers
        public const int MODE = 0;
        public const int LC = 1;        //  Left Canard
        public const int RC = 2;        //  Right Canard
        public const int LS = 3;        //  Left Slat
        public const int RS = 4;        //  Right Slat
        public const int LO = 5;        //  Left Outboard Flap
        public const int RO = 6;        //  Right Outboard Flap
        public const int LI = 7;        //  Left Inboard Flap
        public const int RI = 8;        //  Right Inboard Flap
        public const int LE = 9;        //  Left Engine
        public const int RE = 10;       //  Right Engine
        public const int AB = 11;       //  Airbrake
        public const int FI = 12;       //  Finne
        public const int LED = 13;      //  Onboard LED´s
        public const int LED_LE = 14;   //  LED Left Engine (dynamisch)
        public const int LED_RE = 15;   //  LED Right Engine (dynamisch)
        public const int LDP_H = 16;    //  Laser Horizontal Servo
        public const int LDP_V = 17;    //  Laser Vertikal Servo
        public const int LDP_L = 18;    //  Laser 


        // Kommunikationspuffer
        private byte[] outBuffer = new byte[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };


        private byte[] foggerBuffer = new byte[512];
        private DMXCommunicator dmxCommunicator = null;

        public int xValueRasp = 0;
        public int yValueRasp = 0;
        public int rzValueRasp = 0;
        public int airbrakeValueRasp = 0;
        public int throttleValueRasp = 0; // Invertierter Schubregler Wert



        TcpClient client; // UI ist Client
        NetworkStream nw_stream;



        public ConfigSettings()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            var portsList = DMXCommunicator.GetValidSerialPorts();
            foggerPortList.DataSource = new BindingSource(portsList, null);

            InitializeJoystick();
            timer = new Timer();
            timer.Interval = 10; // Aktualisierung alle 10 ms
            timer.Tick += Timer_Tick;
            timer.Start();
            delayToSend.Interval = 20;
            delayToSend.Start();


            visualisierung = new Visualisierung();
            dataView = new DataView();
            contact = new Contact();
            informationCenterMainPage = new InformationCenterMainPage(dataView, visualisierung);


            joystickConfig.Visible = true;


            iPAdress = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataForMainPage\TCPConfigIPAdress.txt");
            portNumber = int.Parse(datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataForMainPage\TCPConfigPortNumber.txt"));

            this.FormClosing += new FormClosingEventHandler(TCP_Communication_Tester_FormClosing);


            // Mode Selector konfigurieren
            string[] modes = { "OFF", "LED", "Remote", "ServoTest" };
            modeSelector.Items.AddRange(modes);
            modeSelector.Visible = false;

        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL11
        //  Date:       27.09.2023
        //
        //  Diese Methode initialisiert den Joystick. Sie versucht, alle verfügbaren Joysticks
        //  im System zu finden. Wenn ein Joystick gefunden wird, wird er erworben und zur 
        //  weiteren Verwendung bereitgestellt. Falls kein Joystick gefunden wird, wird eine 
        //  Benachrichtigung angezeigt.
        //=======================================================================================
        private void InitializeJoystick()
        {
            try
            {
                directInput = new DirectInput();
                var joystickGuid = Guid.Empty;
                var throttleGuid = Guid.Empty;


                // Finden Sie alle Joysticks im System
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                {
                    if (deviceInstance.ProductName.Contains("Joystick")) // Beispielhaft, ersetzen Sie "Joystick" durch den tatsächlichen Namen des Joysticks
                        joystickGuid = deviceInstance.InstanceGuid;
                    else if (deviceInstance.ProductName.Contains("Throttle")) // Beispielhaft, ersetzen Sie "Throttle" durch den tatsächlichen Namen des Gaspedals
                        throttleGuid = deviceInstance.InstanceGuid;
                }

                if (joystickGuid == Guid.Empty)
                {
                    MessageBox.Show("Joystick nicht verbunden! \n Überprüfen Sie die Verbindung Ihrers Joysticks wenn sie die Joystickfunktion nutzen wollen.\nStarten Sie die Anwendung anschließend erneut.");
                    radioButton4.Checked = false;
                    radioButton5.Checked = true;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;

                    return;
                }
                if (throttleGuid == Guid.Empty)
                {
                    MessageBox.Show("Throttle nicht verbunden!" +
                        "\n" +
                        "Die nutzung des Throttles wird empfohlen da es ansonsten zu Einbußen in der Quallität olgender Funktionalitäten kommt" +
                        "\n" + 
                        "\n  - Die möglichkiet den Ton mithilfe des Switches auszuschalten" +
                        "\n  - Die Klappen des Trieberks zu bewegen" +
                        "\n  - Das Rudder zu bewegen" +
                        "\n" +
                        "\n Überprüfen Sie die Verbindung Ihrers Throttles wenn sie die ThrottleFunktioen nutzen wollen.\\nStarten Sie die Anwendung anschließend erneut.\"");
                }

                // Erwerben Sie die Instanzen der Joysticks
                if (joystickGuid != Guid.Empty)
                {
                    joystick = new Joystick(directInput, joystickGuid);
                    joystick.Acquire();
                }
                if (throttleGuid != Guid.Empty)
                {
                    throttle = new Joystick(directInput, throttleGuid);
                    throttle.Acquire();
                }

            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode überwacht die eingaben des Joysticks und Thrttles und aktualisiert das
        //  Userinterface mit den aktuellen werten
        //=======================================================================================
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (joystick == null && throttle == null)
                {
                    selectedControlMode = informationCenterMainPage.controlUnitDisplay(0, 0, 0, 0, 0, 0, 0, 0, 0, false, false, true);

                    if (selectedControlMode == "Demo")
                    {
                        // Demo Daten auslesen und dann reinschreiben
                        try
                        {
                            int[] currentpositions = informationCenterMainPage.getCurrentPositons();
                            if (currentpositions != null)
                            {
                                outBuffer[LC] = (byte)currentpositions[0];
                                outBuffer[RC] = (byte)currentpositions[1];
                                outBuffer[LS] = (byte)currentpositions[2];
                                outBuffer[RS] = (byte)currentpositions[3];
                                outBuffer[AB] = (byte)currentpositions[4];
                                outBuffer[FI] = (byte)currentpositions[5];
                                outBuffer[LO] = (byte)currentpositions[6];
                                outBuffer[LI] = (byte)currentpositions[7];
                                outBuffer[RI] = (byte)currentpositions[8];
                                outBuffer[RO] = (byte)currentpositions[9];
                                outBuffer[LE] = (byte)currentpositions[10];
                                outBuffer[RE] = (byte)currentpositions[11];
                            }
                        }
                        catch (Exception ex)
                        {
                            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                            string severity = ex.GetType().Name;
                            loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                        }
                    }
                    else
                    {
                        //Grundposition: kein Modus ist ausgewählt
                        try
                        {
                            outBuffer[LC] = (byte)90;       //  Left Canard
                            outBuffer[RC] = (byte)90;       //  Right Canard
                            outBuffer[LS] = (byte)90;       //  Left Slat
                            outBuffer[RS] = (byte)90;       //  Right Slat
                            outBuffer[LO] = (byte)90;       //  Left Outboard Flap
                            outBuffer[RO] = (byte)90;       //  Right Outboard Flap
                            outBuffer[LI] = (byte)90;       //  Left Inboard Flap
                            outBuffer[RI] = (byte)90;       //  Right Inboard Flap
                            outBuffer[LE] = (byte)95;       //  Left Engine
                            outBuffer[RE] = (byte)95;       //  Right Engine
                            outBuffer[FI] = (byte)90;       //  Finne/Ruder
                            outBuffer[LDP_H] = (byte)90;    //  Laser Horizontal
                            outBuffer[LDP_V] = (byte)90;    //  Laser Vertikal
                            outBuffer[LED_LE] = (byte)90;   //  LED Left Engine
                            outBuffer[LED_RE] = (byte)90;   //  LED Right Engine

                            outBuffer[AB] = (byte)0;        //  Air Break
                            outBuffer[LED] = (byte)0;       //  LED`s
                            outBuffer[LDP_L] = (byte)0;     //  LDP Laser
                        }
                        catch (Exception ex)
                        {
                            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                            string severity = ex.GetType().Name;
                            loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                        }
                    }

                }
                else if (joystick != null && throttle == null)
                {
                    singleControll();
                }
                else
                {
                    joystick.Poll();
                    throttle.Poll();


                    var stateJoystick = joystick.GetCurrentState();
                    var stateThrottle = throttle.GetCurrentState();

                    int xValueRaw = stateJoystick.X;    //stateJoystick.X;
                    int yValueRaw = stateJoystick.Y;
                    int rzValueRaw = stateJoystick.TorqueY;
                    int throttleRaw = stateThrottle.RotationZ; // Schubregler

                    var rudderLeft = stateThrottle.Buttons[8];
                    var rudderRight = stateThrottle.Buttons[9];
                    var rudderReset = stateThrottle.Buttons[14];
                    var airbrakeTrigger = stateJoystick.Buttons[3];
                    sound = stateThrottle.Buttons[19];
                    // Werte in Labels anzeigen:
                    lblXValue.Text = "X: " + xValueRaw.ToString();
                    lblYValue.Text = "Y: " + yValueRaw.ToString();
                    lblRZValue.Text = "RZ: " + rudderPos.ToString();//rudderPos
                    lblThrottleValue.Text = "Throttle: " + (maxValue - throttleRaw).ToString(); // Invertierter Wert für das Throttle-Label
                    lblAirbrakeValue.Text = "Airbrake: " + (maxValue - airbrakeVal).ToString();
                    chkButtonState1.Checked = stateJoystick.Buttons[0];     //  Trigger
                    chkButtonState2.Checked = stateJoystick.Buttons[1];     //  Safety
                    chkButtonState3.Checked = stateJoystick.Buttons[3];     //  Airbrake
                    chkButtonState4.Checked = stateThrottle.Buttons[14];    //  Rudder Position Reset
                    chkSoundLever.Checked = stateThrottle.Buttons[19];



                    if (airbrakeTrigger == false && airbrakeVal < maxValue)
                    {
                        airbrakeVal = airbrakeVal + 771;
                    }
                    else if (airbrakeTrigger == true && airbrakeVal > 0)
                    {
                        airbrakeVal = airbrakeVal - 771;
                    }



                    if (rudderReset == true)
                    {
                        rudderPosStart = true;
                    }

                    if (rudderRight == true && rudderPos < maxValue)
                    {
                        rudderPos = rudderPos + 255;
                        rudderPosStart = false;
                    }
                    else if (rudderLeft == true && rudderPos > 0)
                    {
                        rudderPos = rudderPos - 255;
                        rudderPosStart = false;
                    }
                    else
                    {
                        if (rudderPos < 32640 && rudderPosStart == true)
                        {
                            rudderPos = rudderPos + 255;
                        }
                        else if (rudderPos > 32640 && rudderPosStart == true)
                        {
                            rudderPos = rudderPos - 255;
                        }
                        else if (rudderPos == 32640 && rudderPosStart == true)
                        {
                            rudderPos = 32640;
                            rudderPosStart = false;

                        }
                        else
                        {
                            // nothing here
                        }
                    }

                    // Skalierte Werte berechnen:
                    int xValue = ScaleJoystickValue(xValueRaw, maxValue, 200);
                    int yValue = ScaleJoystickValue(yValueRaw, maxValue, 200);
                    int rzValue = ScaleJoystickValue(rudderPos, maxValue, 200); //rudderPos


                    int xValueRight = 100 - xValue;
                    int xValueLeft = xValue - 100;

                    int yValueUp = 100 - yValue;
                    int yValueDown = yValue - 100;

                    int rzValueRight = 100 - rzValue;
                    int rzValueLeft = rzValue - 100;

                    int airbrakeValue = 100 - ScaleJoystickValue(airbrakeVal, maxValue, 100);
                    int throttleValue = 100 - ScaleJoystickValue(throttleRaw, maxValue, 100); // Invertierter Schubregler Wert


                    // Progress Bars aktualisieren:
                    progressBarXLeft.Value = Clamp(xValueLeft, 0, 100);
                    progressBarXRight.Value = Clamp(xValueRight, 0, 100);
                    progressBarYDown.Value = Clamp(yValueDown, 0, 100);
                    progressBarYUp.Value = Clamp(yValueUp, 0, 100);
                    progressBarRzLeft.Value = Clamp(rzValueLeft, 0, 100);
                    progressBarRzRight.Value = Clamp(rzValueRight, 0, 100);


                    progressBarAirbrake.Value = Clamp(airbrakeValue, 0, 100);
                    progressBarThrottle.Value = Clamp(throttleValue, 0, 100); // Schubregler ProgressBar

                    selectedControlMode = informationCenterMainPage.controlUnitDisplay(progressBarYUp.Value, progressBarYDown.Value, progressBarXRight.Value, progressBarXLeft.Value, progressBarRzRight.Value, progressBarRzLeft.Value, progressBarThrottle.Value, progressBarThrottle.Value, progressBarAirbrake.Value, stateJoystick.Buttons[1], stateJoystick.Buttons[0], sound);



                    if (applicationRunning == true && connectedToEurofighter == true)
                    {
                        // Abfragen ob sich Joystick bewegt hat damit nicht ununterbrochen gesendet wird. zur entlastung des Raspberrys
                        //if (xValueRasp != ScaleJoystickValue(xValueRaw, 65535, 180) || yValueRasp != ScaleJoystickValue(yValueRaw, 65535, 180) || rzValueRasp != ScaleJoystickValue(rzValueRaw, 65535, 180) || throttleValueRasp != ScaleJoystickValue(throttleRaw, 65535, 180)) 
                        //{
                        // Werte neu überschreiben
                        xValueRasp = ScaleJoystickValue(xValueRaw, maxValue, 180);
                        yValueRasp = ScaleJoystickValue(yValueRaw, maxValue, 180);
                        rzValueRasp = ScaleJoystickValue(rudderPos, maxValue, 180);
                        airbrakeValueRasp = ScaleJoystickValue(airbrakeVal, maxValue, 180);
                        throttleValueRasp = ScaleJoystickValue(throttleRaw, maxValue, 180);

                        randMovement = informationCenterMainPage.returnMovement();

                        var (aileronRightIN, aileronLeftIN, aileronRightOUT, aileronLeftOUT, rudder, engineThrottle, canardRight, canardLeft) = controlInstance.InterpretJoystickValues(xValueRasp, yValueRasp, throttleValueRasp, rzValueRasp);

                        if (selectedControlMode == "Aircraft")
                        {
                            outBuffer[LE] = (byte)engineThrottle;       //  Throttle
                            outBuffer[RE] = (byte)engineThrottle;       //  Throttle
                            outBuffer[FI] = (byte)rudder;               //  Rudder/Finne
                            outBuffer[LC] = (byte)canardLeft;           //  Left Canard
                            outBuffer[RC] = (byte)canardRight;          //  Right Canard
                            outBuffer[LO] = (byte)aileronLeftOUT;       //  Left Outboard Flap
                            outBuffer[RO] = (byte)aileronRightOUT;      //  Right Outboard Flap

                            //werte der Outboadr flaps aufgrund fehlender Informationen
                            outBuffer[LI] = (byte)aileronLeftIN;        //  Left Inboard Flap
                            outBuffer[RI] = (byte)aileronRightIN;       //  Right Inboard Flap

                            outBuffer[AB] = (byte)airbrakeValueRasp;    //  Airbrake
                            outBuffer[LDP_H] = (byte)90;                //  Laser Horizontal
                            outBuffer[LDP_V] = (byte)90;                //  Laser Vertikal
                            outBuffer[LDP_L] = (byte)0;                 //  Laser laser
                        }
                        else if (selectedControlMode == "Laser")
                        {
                            int laserValue = 0;
                            if (stateJoystick.Buttons[0])
                            {
                                laserValue = 100;
                            }
                            else
                            {
                                laserValue = 0;
                            }
                            outBuffer[LDP_H] = (byte)yValueRasp;
                            outBuffer[LDP_V] = (byte)xValueRasp;
                            outBuffer[LDP_L] = (byte)laserValue;
                            outBuffer[RE] = (byte)95;    //   Right Engine
                            outBuffer[LE] = (byte)95;    //   Left Engine
                            outBuffer[FI] = (byte)90;   //   Rudder/Finne
                            outBuffer[LC] = (byte)90;   //   Left Canard
                            outBuffer[RC] = (byte)90;   //   Right Canard
                            outBuffer[LO] = (byte)90;   //   Left Outboard Flap
                            outBuffer[RO] = (byte)90;   //   Right Outboard Flap
                        }
                        else if (selectedControlMode == "Demo")
                        {
                            // Demo Daten auslesen und dann reinschreiben
                            try
                            {

                                int[] currentpositions = informationCenterMainPage.getCurrentPositons();
                                if (currentpositions != null)
                                {
                                    outBuffer[LC] = (byte)currentpositions[0];
                                    outBuffer[RC] = (byte)currentpositions[1];
                                    outBuffer[LS] = (byte)currentpositions[2];
                                    outBuffer[RS] = (byte)currentpositions[3];
                                    outBuffer[AB] = (byte)currentpositions[4];
                                    outBuffer[FI] = (byte)currentpositions[5];
                                    outBuffer[LO] = (byte)currentpositions[6];
                                    outBuffer[LI] = (byte)currentpositions[7];
                                    outBuffer[RI] = (byte)currentpositions[8];
                                    outBuffer[RO] = (byte)currentpositions[9];
                                    outBuffer[LE] = (byte)currentpositions[10];
                                    outBuffer[RE] = (byte)currentpositions[11];
                                }
                            }
                            catch (Exception ex)
                            {
                                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                                string severity = ex.GetType().Name;
                                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                            }
                        }
                        else
                        {
                            //Grundposition: kein Modus ist ausgewählt
                            restingPosition();
                        }
                        //SendData(outBuffer);
                        //}
                    }

                }
                if (dmxCommunicator != null && dmxCommunicator.IsActive)
                {

                    if (informationCenterMainPage.foggerTransferStatus() == true && foggerAvailable == true)
                    {
                        fogOutput.Start();
                        foggerAvailable = false;
                        for (int i = 0; i < 10; i++) // Hier könnte die Anzahl der Aufrufe angepasst werden
                        {
                            SendValueToChannel(1, 200);
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }




        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode überwacht die eingaben des Joysticks und Thrttles und aktualisiert das
        //  Userinterface mit den aktuellen werten
        //=======================================================================================
        public void singleControll()
        {
            try {
                {
                    joystick.Poll();

                    var stateJoystick = joystick.GetCurrentState();


                    int xValueRaw = stateJoystick.X;    //stateJoystick.X;
                    int yValueRaw = stateJoystick.Y;
                    int rzValueRaw = stateJoystick.TorqueY;


                    var rudderLeft = stateJoystick.Buttons[13];
                    var rudderRight = stateJoystick.Buttons[11];
                    var rudderReset = stateJoystick.Buttons[4];

                    var throttleUp = stateJoystick.Buttons[6];
                    var throttleDown = stateJoystick.Buttons[8];
                    var throttleReset = stateJoystick.Buttons[2];

                    var airbrakeTrigger = stateJoystick.Buttons[3];



                    // Werte in Labels anzeigen:
                    lblXValue.Text = "X: " + xValueRaw.ToString();
                    lblYValue.Text = "Y: " + yValueRaw.ToString();
                    lblRZValue.Text = "RZ: " + rudderPos.ToString();//rudderPos
                    lblThrottleValue.Text = "Throttle: not plugged in";
                    lblAirbrakeValue.Text = "Airbrake: " + (maxValue - airbrakeVal).ToString();
                    chkButtonState1.Checked = stateJoystick.Buttons[0];     //  Trigger
                    chkButtonState2.Checked = stateJoystick.Buttons[1];     //  Safety
                    chkButtonState3.Checked = stateJoystick.Buttons[3];     //  Airbrake
                    chkButtonState4.Checked = false;
                    chkSoundLever.Checked = false;


                    if (airbrakeTrigger == false && airbrakeVal < maxValue)
                    {
                        airbrakeVal = airbrakeVal + 771;
                    }
                    else if (airbrakeTrigger == true && airbrakeVal > 0)
                    {
                        airbrakeVal = airbrakeVal - 771;
                    }




                    if (throttleReset == true)
                    {
                        throttlePosStart = true;
                    }
                    if (throttleUp == true && throttlePos < maxValue)
                    {
                        throttlePos = throttlePos + 255;
                        throttlePosStart = false;
                    }
                    else if (throttleDown == true && throttlePos > 0)
                    {
                        throttlePos = throttlePos - 255;
                        throttlePosStart = false;
                    }
                    else
                    {
                        if (throttlePosStart == true && throttlePos != 0)
                        {
                            throttlePos = throttlePos - 255;
                        }
                        else if (throttlePos == 0 && rudderPosStart == true)
                        {
                            throttlePosStart = false;
                        }

                    }



                    if (rudderReset == true)
                    {
                        rudderPosStart = true;
                    }

                    if (rudderRight == true && rudderPos < maxValue)
                    {
                        rudderPos = rudderPos + 255;
                        rudderPosStart = false;
                    }
                    else if (rudderLeft == true && rudderPos > 0)
                    {
                        rudderPos = rudderPos - 255;
                        rudderPosStart = false;
                    }
                    else
                    {
                        if (rudderPos < 32640 && rudderPosStart == true)
                        {
                            rudderPos = rudderPos + 255;
                        }
                        else if (rudderPos > 32640 && rudderPosStart == true)
                        {
                            rudderPos = rudderPos - 255;
                        }
                        else if (rudderPos == 32640 && rudderPosStart == true)
                        {
                            rudderPos = 32640;
                            rudderPosStart = false;

                        }
                        else
                        {
                            // nothing here
                        }
                    }


                    // Skalierte Werte berechnen:
                    int xValue = ScaleJoystickValue(xValueRaw, maxValue, 200);
                    int yValue = ScaleJoystickValue(yValueRaw, maxValue, 200);
                    int rzValue = ScaleJoystickValue(rudderPos, maxValue, 200); //rudderPos


                    int xValueRight = 100 - xValue;
                    int xValueLeft = xValue - 100;

                    int yValueUp = 100 - yValue;
                    int yValueDown = yValue - 100;

                    int rzValueRight = 100 - rzValue;
                    int rzValueLeft = rzValue - 100;

                    int airbrakeValue = 100 - ScaleJoystickValue(airbrakeVal, maxValue, 100);
                    int throttleValue = ScaleJoystickValue(throttlePos, maxValue, 100); // Invertierter Schubregler Wert

                    // Progress Bars aktualisieren:
                    progressBarXLeft.Value = Clamp(xValueLeft, 0, 100);
                    progressBarXRight.Value = Clamp(xValueRight, 0, 100);
                    progressBarYDown.Value = Clamp(yValueDown, 0, 100);
                    progressBarYUp.Value = Clamp(yValueUp, 0, 100);
                    progressBarRzLeft.Value = Clamp(rzValueLeft, 0, 100);
                    progressBarRzRight.Value = Clamp(rzValueRight, 0, 100);


                    progressBarAirbrake.Value = Clamp(airbrakeValue, 0, 100);
                    progressBarThrottle.Value = Clamp(throttleValue, 0, 100); // Schubregler ProgressBar

                    selectedControlMode = informationCenterMainPage.controlUnitDisplay(progressBarYUp.Value, progressBarYDown.Value, progressBarXRight.Value, progressBarXLeft.Value, progressBarRzRight.Value, progressBarRzLeft.Value, progressBarThrottle.Value, progressBarThrottle.Value, progressBarAirbrake.Value, stateJoystick.Buttons[1], stateJoystick.Buttons[0], true);



                    if (applicationRunning == true && connectedToEurofighter == true)
                    {
                        // Abfragen ob sich Joystick bewegt hat damit nicht ununterbrochen gesendet wird. zur entlastung des Raspberrys
                        //if (xValueRasp != ScaleJoystickValue(xValueRaw, 65535, 180) || yValueRasp != ScaleJoystickValue(yValueRaw, 65535, 180) || rzValueRasp != ScaleJoystickValue(rzValueRaw, 65535, 180) || throttleValueRasp != ScaleJoystickValue(throttleRaw, 65535, 180)) 
                        //{
                        // Werte neu überschreiben
                        xValueRasp = ScaleJoystickValue(xValueRaw, maxValue, 180);
                        yValueRasp = ScaleJoystickValue(yValueRaw, maxValue, 180);
                        rzValueRasp = ScaleJoystickValue(rudderPos, maxValue, 180);
                        airbrakeValueRasp = ScaleJoystickValue(airbrakeVal, maxValue, 180);
                        throttleValueRasp = ScaleJoystickValue(throttlePos, maxValue, 180);

                        randMovement = informationCenterMainPage.returnMovement();

                        var (aileronRightIN, aileronLeftIN, aileronRightOUT, aileronLeftOUT, rudder, engineThrottle, canardRight, canardLeft) = controlInstance.InterpretJoystickValues(xValueRasp, yValueRasp, throttleValueRasp, rzValueRasp);

                        if (selectedControlMode == "Aircraft")
                        {
                            outBuffer[LE] = (byte)engineThrottle;       //  Throttle
                            outBuffer[RE] = (byte)engineThrottle;       //  Throttle
                            outBuffer[FI] = (byte)rudder;               //  Rudder/Finne
                            outBuffer[LC] = (byte)canardLeft;           //  Left Canard
                            outBuffer[RC] = (byte)canardRight;          //  Right Canard
                            outBuffer[LO] = (byte)aileronLeftOUT;       //  Left Outboard Flap
                            outBuffer[RO] = (byte)aileronRightOUT;      //  Right Outboard Flap

                            //werte der Outboadr flaps aufgrund fehlender Informationen
                            outBuffer[LI] = (byte)aileronLeftIN;        //  Left Inboard Flap
                            outBuffer[RI] = (byte)aileronRightIN;       //  Right Inboard Flap

                            outBuffer[AB] = (byte)airbrakeValueRasp;    //  Airbrake
                            outBuffer[LDP_H] = (byte)90;                //  Laser Horizontal
                            outBuffer[LDP_V] = (byte)90;                //  Laser Vertikal
                            outBuffer[LDP_L] = (byte)0;                 //  Laser laser
                        }
                        else if (selectedControlMode == "Laser")
                        {
                            int laserValue = 0;
                            if (stateJoystick.Buttons[0])
                            {
                                laserValue = 100;
                            }
                            else
                            {
                                laserValue = 0;
                            }
                            outBuffer[LDP_H] = (byte)yValueRasp;
                            outBuffer[LDP_V] = (byte)xValueRasp;
                            outBuffer[LDP_L] = (byte)laserValue;
                            outBuffer[RE] = (byte)95;    //   Right Engine
                            outBuffer[LE] = (byte)95;    //   Left Engine
                            outBuffer[FI] = (byte)90;   //   Rudder/Finne
                            outBuffer[LC] = (byte)90;   //   Left Canard
                            outBuffer[RC] = (byte)90;   //   Right Canard
                            outBuffer[LO] = (byte)90;   //   Left Outboard Flap
                            outBuffer[RO] = (byte)90;   //   Right Outboard Flap
                        }
                        else if (selectedControlMode == "Demo")
                        {
                            // Demo Daten auslesen und dann reinschreiben
                            try
                            {

                                int[] currentpositions = informationCenterMainPage.getCurrentPositons();
                                if (currentpositions != null)
                                {
                                    outBuffer[LC] = (byte)currentpositions[0];
                                    outBuffer[RC] = (byte)currentpositions[1];
                                    outBuffer[LS] = (byte)currentpositions[2];
                                    outBuffer[RS] = (byte)currentpositions[3];
                                    outBuffer[AB] = (byte)currentpositions[4];
                                    outBuffer[FI] = (byte)currentpositions[5];
                                    outBuffer[LO] = (byte)currentpositions[6];
                                    outBuffer[LI] = (byte)currentpositions[7];
                                    outBuffer[RI] = (byte)currentpositions[8];
                                    outBuffer[RO] = (byte)currentpositions[9];
                                    outBuffer[LE] = (byte)currentpositions[10];
                                    outBuffer[RE] = (byte)currentpositions[11];
                                }
                            }
                            catch (Exception ex)
                            {
                                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                                string severity = ex.GetType().Name;
                                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                            }
                        }
                        else
                        {
                            //Grundposition: kein Modus ist ausgewählt
                            restingPosition();
                        }
                        //SendData(outBuffer);
                        //}
                    }

                }
                if (dmxCommunicator != null && dmxCommunicator.IsActive)
                {

                    if (informationCenterMainPage.foggerTransferStatus() == true && foggerAvailable == true)
                    {
                        fogOutput.Start();
                        foggerAvailable = false;
                        for (int i = 0; i < 10; i++) // Hier könnte die Anzahl der Aufrufe angepasst werden
                        {
                            SendValueToChannel(1, 200);
                        }
                    }

                }
            }
            catch (Exception ex) 
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        public void restingPosition()
        {
            try
            {
                if (randMovement == true)
                {
                    try
                    {

                        int[] currentpositions = informationCenterMainPage.getCurrentPositons();
                        if (currentpositions != null)
                        {
                            outBuffer[LC] = (byte)currentpositions[0];
                            outBuffer[RC] = (byte)currentpositions[1];
                            outBuffer[LS] = (byte)currentpositions[2];
                            outBuffer[RS] = (byte)currentpositions[3];
                            outBuffer[AB] = (byte)currentpositions[4];
                            outBuffer[FI] = (byte)currentpositions[5];
                            outBuffer[LO] = (byte)(180 - currentpositions[6]);
                            outBuffer[LI] = (byte)currentpositions[7];
                            outBuffer[RI] = (byte)currentpositions[8];
                            outBuffer[RO] = (byte)(180-currentpositions[9]);
                            outBuffer[LE] = (byte)currentpositions[10];
                            outBuffer[RE] = (byte)currentpositions[11];
                        }
                        else 
                        {
                            outBuffer[LC] = (byte)90;       //  Left Canard
                            outBuffer[RC] = (byte)90;       //  Right Canard
                            outBuffer[LS] = (byte)90;       //  Left Slat
                            outBuffer[RS] = (byte)90;       //  Right Slat
                            outBuffer[LO] = (byte)90;       //  Left Outboard Flap
                            outBuffer[RO] = (byte)90;       //  Right Outboard Flap
                            outBuffer[LI] = (byte)90;       //  Left Inboard Flap
                            outBuffer[RI] = (byte)90;       //  Right Inboard Flap
                            outBuffer[LE] = (byte)95;       //  Left Engine
                            outBuffer[RE] = (byte)95;       //  Right Engine
                            outBuffer[FI] = (byte)90;       //  Finne/Ruder
                            outBuffer[LDP_H] = (byte)90;    //  Laser Horizontal
                            outBuffer[LDP_V] = (byte)90;    //  Laser Vertikal
                            outBuffer[LED_LE] = (byte)90;   //  LED Left Engine
                            outBuffer[LED_RE] = (byte)90;   //  LED Right Engine

                            outBuffer[AB] = (byte)0;        //  Air Break
                            outBuffer[LED] = (byte)0;       //  LED`s
                            outBuffer[LDP_L] = (byte)0;     //  LDP Laser
                        }
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }
                }
                else
                {
                    outBuffer[LC] = (byte)90;       //  Left Canard
                    outBuffer[RC] = (byte)90;       //  Right Canard
                    outBuffer[LS] = (byte)90;       //  Left Slat
                    outBuffer[RS] = (byte)90;       //  Right Slat
                    outBuffer[LO] = (byte)90;       //  Left Outboard Flap
                    outBuffer[RO] = (byte)90;       //  Right Outboard Flap
                    outBuffer[LI] = (byte)90;       //  Left Inboard Flap
                    outBuffer[RI] = (byte)90;       //  Right Inboard Flap
                    outBuffer[LE] = (byte)95;       //  Left Engine
                    outBuffer[RE] = (byte)95;       //  Right Engine
                    outBuffer[FI] = (byte)90;       //  Finne/Ruder
                    outBuffer[LDP_H] = (byte)90;    //  Laser Horizontal
                    outBuffer[LDP_V] = (byte)90;    //  Laser Vertikal
                    outBuffer[LED_LE] = (byte)90;   //  LED Left Engine
                    outBuffer[LED_RE] = (byte)90;   //  LED Right Engine

                    outBuffer[AB] = (byte)0;        //  Air Break
                    outBuffer[LED] = (byte)0;       //  LED`s
                    outBuffer[LDP_L] = (byte)0;     //  LDP Laser
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode setzt alle Werte zum Joystick im Userinterface auf 0
        //=======================================================================================
        private void clearControls()
        {
            try
            {
                lblXValue.Text = "X: " + "0";
                lblYValue.Text = "Y: " + "0";
                lblRZValue.Text = "RZ: " + "0";
                lblThrottleValue.Text = "Throttle: " + "0";
                progressBarXLeft.Value = 0;
                progressBarXRight.Value = 0;
                progressBarYDown.Value = 0;
                progressBarYUp.Value = 0;
                progressBarThrottle.Value = 0;
                progressBarRzLeft.Value = 0;
                progressBarRzRight.Value = 0;
                chkButtonState1.Checked = false;
                chkButtonState2.Checked = false;
                chkButtonState3.Checked = false;
                chkButtonState4.Checked = false;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Methode zur skalierung der Joystick Werte um diese zu 
        //=======================================================================================
        int ScaleJoystickValue(int joystickValue, int joystickMax, int progressBarMax)
        {
            try
            {
                return (joystickValue * progressBarMax) / joystickMax;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return 0;
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode begrenzt einen gegebenen Wert zwischen einem Mindest- und 
        //  Höchstwert. Wenn der Wert unter dem Mindestwert liegt, wird der Mindestwert 
        //  zurückgegeben. Wenn der Wert über dem Höchstwert liegt, wird der Höchstwert 
        //  zurückgegeben. Ansonsten wird der gegebene Wert selbst zurückgegeben.
        //=======================================================================================
        private int Clamp(int value, int min, int max)
        {
            try
            {
                if (value < min) return min;
                if (value > max) return max;
                return value;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return 0;
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode überwacht die eingaben des Joysticks und aktualisiert das Userinterface
        //  mit den aktuellen werten
        //=======================================================================================
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                taskbar.Show();
                if (dmxCommunicator != null && dmxCommunicator.IsActive)
                {
                    dmxCommunicator.Stop();
                    dmxCommunicator = null;
                }
                connectedToEurofighter = false;
                if (joystick != null)
                {
                    joystick.Unacquire();
                    joystick.Dispose();
                }
                directInput?.Dispose();
                base.OnFormClosing(e);
                if (client != null)
                {
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       27.07.2023
        //
        //  Methode zum deaktivieren der trackBars damit diese nicht bewegt werden können
        //=======================================================================================
        private void trackBarDisabled()
        {
            try
            {
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
                trackBarLDPH.Enabled = false;
                trackBarLDPV.Enabled = false;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       27.07.2023
        //
        //  Methode zum aktivieren der trackBars damit diese nicht bewegt werden können
        //=======================================================================================
        private void trackBarEnabled()
        {
            try
            {
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
                trackBarLDPH.Enabled = true;
                trackBarLDPV.Enabled = true;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Noah Gerstlauer           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Methode wird aufgerufen, wenn das Programm geschlossen ist, und beendet die TCP 
        //  Verbindung damit der Port nicht belegt ist wenn erneut eine Verbindung aufgebaut wird
        //=======================================================================================
        private void TCP_Communication_Tester_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (client != null)
                {
                    client.Close(); // TCP Verbindung schließen
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Noah Gerstlauer           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode sendet Daten über den `nw_stream`. Wenn der Stream vorhanden ist,
        //  werden die bereitgestellten Daten gesendet. Wenn nicht, wird eine Meldung 
        //  angezeigt, dass keine Verbindung besteht. Im Falle eines Fehlers beim Senden der
        //  Daten wird die Fehlermeldung protokolliert und die Verbindung geschlossen.
        private void SendData(byte[] payload)
        {
            try
            {
                if (nw_stream != null)
                {
                    nw_stream.Write(payload, 0, payload.Length);

                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < payload.Length; i++)
                    {
                        sb.Append(payload[i].ToString());
                        if (i != payload.Length - 1) // Wenn es nicht das letzte Element ist
                        {
                            sb.Append(", "); // Trennzeichen zwischen den Werten
                        }
                    }

                    string payloadAsString = sb.ToString();
                    AppendStatusText("Erfolgreich gesendet: " + payloadAsString);
                }
                else
                {
                    AppendStatusText("Nicht verbunden");
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                AppendStatusText(ex.Message);
                if (client != null)
                {
                    client.Close();
                    client = null;
                    modeSelector.Visible = false;
                    AppendStatusText("Verbindung getrennt.");
                }
            }
        }



        //=======================================================================================
        //  Author:     Noah Gerstlauer           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode fügt dem `statusTextBox`-Steuerelement eine Statusnachricht mit 
        //  einem Zeitstempel hinzu. Wenn die Methode von einem anderen Thread als dem UI-Thread 
        //  aufgerufen wird, wird sie mithilfe der `Invoke`-Methode erneut aufgerufen, um 
        //  Thread-Sicherheit zu gewährleisten. Jede hinzugefügte Nachricht wird automatisch 
        //  in eine neue Zeile gesetzt, und das `statusTextBox`-Steuerelement scrollt, um die 
        //  neueste Nachricht anzuzeigen. Im Fehlerfall wird eine Fehlermeldung angezeigt.
        //=======================================================================================
        private void AppendStatusText(string msg)
        {
            try
            {
                if (applicationRunning == false)
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

                    if (tempOutMSG != msg)
                    {
                        statusTextBox.AppendText(messageWithTimestamp);
                        tempOutMSG = msg;
                    }

                    // Automatisches Scrollen zur letzten Zeile
                    statusTextBox.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Loschte den Text aus der Textbox"statusTextBox"
        //=======================================================================================
        private void ClearStatusText()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(ClearStatusText));
                    return;
                }
                statusTextBox.Clear();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode überprüft, ob eine gegebene Zeichenfolge eine gültige IPv4-Adresse 
        //  darstellt. Sie gibt `true` zurück, wenn die Adresse gültig ist und zum IPv4-Format 
        //  gehört, ansonsten gibt sie `false` zurück.
        //=======================================================================================
        private bool IsValidIPv4(string ipAddress)
        {
            try
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
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return false;
            }
        }






        private bool PingServer(string ipAddress)
        {
            try
            {
                Ping pingSender = new Ping();
                PingReply reply = pingSender.Send(ipAddress);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return false;
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.07.2023
        //
        //  Wird ausgeführt wenn das Fenster geladen wird, lädt dann auch die anderen fenster
        //  und verbirgt die Serielle Config und die Joystick Config
        //=======================================================================================
        private void ConfigSettings_Load(object sender, EventArgs e)
        {
            try
            {
                //  UI Fenster:
                informationCenterMainPage.Show();
                dataView.Show();
                visualisierung.Show();
                trackBarDisabled();
                disableFoggerButtons();
                disconButton.Enabled = false;
                connectToModel.Enabled = true;
                btnStop.Enabled = true;
                btnStart.Enabled = false;
                button5.Visible = false;
                label15.Text = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataForMainPage\Ansprechpartner.txt");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       27.07.2023
        //
        //  Wird ausgeführt wenn der Config abschließen Button gedrückt wird, wenn das passiert,
        //  dann wird das Fenster geschlossen
        //=======================================================================================
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                taskbar.Hide();


                applicationRunning = true;

                if (applicationRunning == true)
                {
                    outBuffer = new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    SendData(outBuffer);
                    trackBarDisabled();
                    statusTextBox.Clear();
                }
                if (cbBerufsildungShow.Checked)
                {
                    informationCenterMainPage.berufsbildungShow(true);
                }
                else 
                {
                    informationCenterMainPage.berufsbildungShow(false);
                }

                if (radioButton5.Checked)
                {
                    informationCenterMainPage.statusJoystick(false);
                }
                else
                {
                    informationCenterMainPage.statusJoystick(true);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       27.07.2023
        //
        //  Wird ausgeführt wenn der Button "Contact/Help" gedrückt wird, daraufhin, öffnet sich
        //  das Contact Fenster und die Kontaktdaten werden angezeigt
        //=======================================================================================
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                contact.Show();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       27.07.2023
        //
        //  Buttons um die verschiedenen Fenster auf andere Bildschirme zu verschieben, der 
        //  Bildschirm auf den Verschoben wird, wird zuvor durch radio Buttons festgelegt
        //=======================================================================================
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    Screen screen = Screen.AllScreens[0]; informationCenterMainPage.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
                else if (radioButton2.Checked)
                {
                    Screen screen = Screen.AllScreens[0]; dataView.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
                else if (radioButton3.Checked)
                {
                    Screen screen = Screen.AllScreens[0]; visualisierung.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    Screen screen = Screen.AllScreens[1]; informationCenterMainPage.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
                else if (radioButton2.Checked)
                {
                    Screen screen = Screen.AllScreens[1]; dataView.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
                else if (radioButton3.Checked)
                {
                    Screen screen = Screen.AllScreens[1]; visualisierung.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    Screen screen = Screen.AllScreens[2]; informationCenterMainPage.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
                else if (radioButton2.Checked)
                {
                    Screen screen = Screen.AllScreens[2]; dataView.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
                else if (radioButton3.Checked)
                {
                    Screen screen = Screen.AllScreens[2]; visualisierung.Location = new Point(screen.Bounds.Left, screen.Bounds.Top);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.07.2023
        //
        //  Trackbars, zum manuellem Bewegen der Servos, funktioniert nur dann wenn der Modus
        //  Remote ausgewählt wurde
        //=======================================================================================
        private void trackBarLC_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {
                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[LC] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarRC_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }
                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[RC] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarLS_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[LS] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarRS_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[RS] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarLO_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[LO] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarRO_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[RO] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarLI_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[LI] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarRI_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[RI] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarLE_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[LE] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarRE_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[RE] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarAB_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 180) sliderValue = 180;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[AB] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarFI_Scroll(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarLDPV_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 255) sliderValue = 255;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[LDP_V] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void trackBarLDPH_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                if (sender is TrackBar slider)
                {

                    int sliderValue = slider.Value;
                    if (sliderValue < 0) sliderValue = 0;
                    else if (sliderValue > 255) sliderValue = 255;
                    byte sliderByte = (byte)sliderValue;

                    outBuffer[LDP_H] = sliderByte;

                    outBuffer[LDP_V] = sliderByte;
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       01.09.2023
        //
        //  Methode Scaliert werte auf einen gewünschten Wertebereich wird verwendet um die
        //  Joystickwerte auf den vom Raspberry benötigten Werteberich zu skallieren
        //=======================================================================================
        public int ScaleValue(int input, int inputMin, int inputMax, int outputMin, int outputMax)
        {
            try
            {
                return (input - inputMin) * (outputMax - outputMin) / (inputMax - inputMin) + outputMin;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return 0;
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Verbindet Den Joystick und startet den Joystick Tracker
        //=======================================================================================
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                timer.Start();
                delayToSend.Start();
                if (joystick != null)
                {
                    joystick.Acquire();
                }
                btnStop.Enabled = true;
                btnStart.Enabled = false;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Schliesst die Verbindung mit dem Joystick und beendet den Joystick Tracker
        //=======================================================================================
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                timer.Stop();
                delayToSend.Stop();
                if (joystick != null)
                {
                    joystick.Unacquire();
                    clearControls();
                }
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Noah Gerstlauer           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  beendet die Verbindung mit dem Raspberry Pi wird benötigt, damit der Port nicht
        //  belegt ist auf dem Raspberry Py
        //=======================================================================================
        private void disconButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (client != null)
                {
                    trackBarDisabled();
                    client.Close();
                    client = null;
                    modeSelector.Visible = false;
                    AppendStatusText("Verbindung getrennt.");
                    connectedToEurofighter = false;
                    disconButton.Enabled = false;
                    connectToModel.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Noah Gerstlauer           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  löscht den Inhalt der TextBox in der der Verlauf der Kommunikation angezeit wird
        //=======================================================================================
        private void clearButton_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatusText();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Noah Gerstlauer           Department:     THGM-TL1
        //  Date:       27.09.2023
        //
        //  Diese Methode wird aufgerufen, wenn auf den Button zum Verbinden mit dem Modell geklickt wird.
        //  Es wird versucht, eine Verbindung zu einem bestimmten IP-Adress- und Port-Nummer-Endpunkt 
        //  herzustellen. Bei einer erfolgreichen Verbindung werden einige UI-Elemente aktualisiert und Daten gesendet.
        //  Bei einem Fehler wird eine Fehlermeldung angezeigt und der Status-Text aktualisiert.
        //=======================================================================================
        private void connectToModel_Click(object sender, EventArgs e)
        {
            try
            {
                if (portNumber > 1024)
                {
                    client = new TcpClient();
                    client.Connect(iPAdress, portNumber);
                    client.ReceiveTimeout = 5000; // Setze den Timeout-Wert auf 5 Sekunden (kann an deine Anforderungen angepasst werden)
                    nw_stream = client.GetStream();
                    string msg = "Verbindung hergestellt. Port: " + portNumber + " IP: " + iPAdress;
                    modeSelector.Visible = true;
                    connectedToEurofighter = true;
                    AppendStatusText(msg);
                    SendData(outBuffer);
                    disconButton.Enabled = true;
                    connectToModel.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Bitte einen Port > 1024 wählen");
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                ClearStatusText();
                AppendStatusText(ex.Message);
                //MessageBox.Show("Fehler beim Aufbau der Verbindung.");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       01.09.2023
        //
        //  Dieser Button Beendet die Anwendung, Hier wird ei Joystick verbindung geschlossen
        //  und die Taskbar wieder eingeblendet
        //=======================================================================================
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                delayToSend.Stop();
                if (joystick != null)
                {
                    joystick.Dispose();
                    joystick = null;
                    directInput.Dispose();
                    directInput = null;
                    connectedToEurofighter = false;
                }
                taskbar.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       27.07.2023
        //
        //  je nach ausgewähltem modus werden die Regler aktiviert oder deaktivert
        //=======================================================================================
        private void modeSelector_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (client == null) { return; }

                // AUS
                if (modeSelector.SelectedIndex == 0)
                {
                    outBuffer = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    SendData(outBuffer);
                    trackBarDisabled();
                    servoTestPerformed = false;
                }
                // LED
                else if (modeSelector.SelectedIndex == 1)
                {
                    outBuffer = new byte[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    SendData(outBuffer);
                    trackBarDisabled();
                    servoTestPerformed = false;
                }
                // Remote
                else if (modeSelector.SelectedIndex == 2)
                {
                    outBuffer = new byte[] { 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    SendData(outBuffer);
                    trackBarEnabled();
                    servoTestPerformed = false;
                }
                // ServoTest
                else if (modeSelector.SelectedIndex == 3 && servoTestPerformed == false)
                {
                    outBuffer = new byte[] { 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    SendData(outBuffer);
                    trackBarDisabled();
                    servoTestPerformed = true;
                }
                else if (modeSelector.SelectedIndex == 3 && servoTestPerformed == true)
                {
                    //nothing here
                }
                // Kein gueltiger Index
                else
                {
                    Console.WriteLine("Fehler bei Modusauswahl!");
                    trackBarDisabled();
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void delayToSend_Tick(object sender, EventArgs e)
        {
            try
            {
                if (servoTestPerformed == false)
                {
                    SendData(outBuffer);
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        private void disableFoggerButtons()
        {
            try
            {
                btnFoggerStart.Enabled = false;
                btnFoggerStop.Enabled = false;
                btnDisconnectFog.Enabled = false;
                btnConnectFog.Enabled = true;
                foggerPortList.Enabled = true;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void enableFoggerButtons()
        {
            try
            {
                btnFoggerStart.Enabled = true;
                btnFoggerStop.Enabled = true;
                btnDisconnectFog.Enabled = true;
                btnConnectFog.Enabled = false;
                foggerPortList.Enabled = false;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        private void btnFoggerStart_Click(object sender, EventArgs e)
        {
            try
            {
                foggerStatus = true;
                SendValueToChannel(1, 200);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void btnFoggerStop_Click(object sender, EventArgs e)
        {
            try
            {
                foggerStatus = false;
                SendValueToChannel(1, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        private void SendValueToChannel(int channel, int value)
        {
            try
            {
                foggerBuffer[channel - 1] = (byte)value;
                if (dmxCommunicator != null)
                    dmxCommunicator.SetByte(channel - 1, (byte)value);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }




        private void btnConnectFog_Click(object sender, EventArgs e)
        {
            try
            {
                if (foggerPortList.Items.Count == 0)
                    return;

                dmxCommunicator = new DMXCommunicator(foggerPortList.SelectedValue.ToString());
                dmxCommunicator.SetBytes(foggerBuffer);
                dmxCommunicator.Start();
                domainUpDownCooldown.Enabled = false;
                domainUpDownDuration.Enabled = false;
                enableFoggerButtons();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }




        private void btnDisconnectFog_Click(object sender, EventArgs e)
        {
            try
            {
                if (dmxCommunicator != null && dmxCommunicator.IsActive)
                    dmxCommunicator.Stop();
                domainUpDownCooldown.Enabled = true;
                domainUpDownDuration.Enabled = true;
                disableFoggerButtons();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void foggerDelay_Tick(object sender, EventArgs e)
        {
            foggerDelay.Stop();
            foggerAvailable = true;
        }

        private void fogOutput_Tick(object sender, EventArgs e)
        {
            fogOutput.Stop();
            SendValueToChannel(1, 0);
            foggerDelay.Start();
        }

        private void domainUpDownCooldown_SelectedItemChanged(object sender, EventArgs e)
        {
            int.TryParse(domainUpDownCooldown.SelectedItem.ToString(), out int selectedNumber);
            foggerDelay.Interval = selectedNumber * 1000; // Konvertierung in Millisekunden
        }

        private void domainUpDownDuration_SelectedItemChanged(object sender, EventArgs e)
        {
            int.TryParse(domainUpDownDuration.SelectedItem.ToString(), out int selectedNumber);
            fogOutput.Interval = selectedNumber * 1000; // Konvertierung in Millisekunden
        }
    }
}