using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using AxWMPLib;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Data.SqlTypes;

namespace EurofighterInformationCenter
{

    // mal schauen bin ich mir noch nicht sicher ob das so richtig ist
    public partial class InformationCenterMainPage : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // WS_EX_COMPOSITED
                return cp;
            }
        }


        DataView dataViewInstance;
        Visualisierung visualisierungInstance;
        datahandler datahandlerInstance;
        logger loggerInstance;

        public Dictionary<int, int[]> movementDict;

        public string userName = Environment.UserName;

        public bool screenSaveMode = false;
        public int ScreenSaveTime = 27500;   //3800    27500
        public TimeSpan timeLeftToScreenSave;

        public string selectedControlMode = "Aircraft";

        public Color defaultLabelTriebwerk = Color.Black;
        public Color selectedLabelTriebwerk = Color.White;
        private List<Label> triebwerkLabels = new List<Label>();


        public Color defaultLabelBewaffnung = Color.White;
        public Color selectedLabelBewaffnung = Color.Black;
        private List<Label> bewaffnungLabels = new List<Label>();

        public Color defaultLabelSysteme = Color.White;
        public Color selectedLabelSysteme = Color.Black;
        private List<Label> systemeLabels = new List<Label>();

        public Color defaultButton = Color.White;
        public Color selectedButton = Color.LightBlue;

        SoundPlayer player = new SoundPlayer();

        public Timer demoTimer = new Timer();
        public int timeDemo = 0;
        public int[] currentPositions = null;

        public bool coolDownStatus = false;
        public bool globalSound = false;

        public bool enableMovement = false;
        public int currentmovementTime = 0;

        public bool foggerStatus = false;
        public bool allreadyActivated = false;







        public string globalSlidesMode = "";





        private readonly GlobalInputListener _globalInputListener;

        public InformationCenterMainPage(DataView dataView, Visualisierung visualisierung)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            if (dataView == null)
            {
                throw new ArgumentNullException(nameof(dataView));
            }

            dataViewInstance = dataView;
            visualisierungInstance = visualisierung;
            datahandlerInstance = new datahandler();
            loggerInstance = new logger();


            _globalInputListener = new GlobalInputListener();
            _globalInputListener.InputDetected += OnInputDetected;
            _globalInputListener.Start();
            axWindowsMediaPlayer1.uiMode = "none";

            movementDict = ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DemoData.txt");

            demoTimer.Interval = 100;
            demoTimer.Tick += demoTimer_Tick;

            string mediaPath = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\ScreenSave.mp4";

            axWindowsMediaPlayer1.URL = mediaPath;
            axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
            axWindowsMediaPlayer1.URL = mediaPath;

            axDemoPlayer.PlayStateChange += AxDemoPlayer_PlayStateChange;


        }

        private void AxDemoPlayer_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                if (ueberschrift.Text == "SUZ Video" && (WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsMediaEnded)
                {
                    button1.PerformClick();
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void OnInputDetected()
        {
            try
            {
                timerReset();
                screnSaveToggle(false);
                dataViewInstance.screnSaveToggle(false);
                //MessageBox.Show("fasdfasdfasdfasdfasdf");

            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        public void endDemo()
        {
            try
            {
                axDemoPlayer.Ctlcontrols.stop();
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
        //  In this method active mouse events are tracked and when an event is triggered
        //  the timer is reset until the ScreenSave mode is activated.
        //=======================================================================================
        private void InformationCenterMainPage_Load(object sender, EventArgs e)
        {
            try
            {
                //Verfügbare Methoden die nicht gewünscht waren für den Auslieferungszustand
                button3.Hide(); //Demo Video, wird abgespielt auf Visualisierung und information Center main page einfach einkommentieren und funktioniert
                button7.Hide(); //MateButton

                screenSafe.Parent = this;
                screenSafe.Dock = DockStyle.Fill;
                screenSafe.Hide();
                //axWindowsMediaPlayer1.uiMode = "none";
                //axWindowsMediaPlayer1.Hide();

                button1.Text = datahandlerInstance.ButtonTextRead(1);
                button2.Text = datahandlerInstance.ButtonTextRead(2);
                button3.Text = datahandlerInstance.ButtonTextRead(3);
                button4.Text = datahandlerInstance.ButtonTextRead(4);
                button5.Text = datahandlerInstance.ButtonTextRead(5);
                button6.Text = datahandlerInstance.ButtonTextRead(6);
                button7.Text = datahandlerInstance.ButtonTextRead(7);
                button8.Text = datahandlerInstance.ButtonTextRead(8);
                button9.Text = datahandlerInstance.ButtonTextRead(9);
                button10.Text = datahandlerInstance.ButtonTextRead(10);
                //panelBackground.BackgroundImage = Image.FromFile("../../Files/Background.jpg");
                //panelSysteme.BackgroundImage = Image.FromFile("../../Files/Background.jpg");
                //panelMain.BackgroundImage = Image.FromFile("../../Files/Background.jpg");
                //panelBewaffnung.BackgroundImage = Image.FromFile("../../Files/Background.jpg");
                //panelTriebwerk.BackgroundImage = Image.FromFile("../../Files/Background.jpg");
                //panelJoystick.BackgroundImage = Image.FromFile("../../Files/Background.jpg");
                //panelDemo.BackgroundImage = Image.FromFile("../../Files/Background.jpg");
                panelJoystick.Dock = DockStyle.Fill;
                panelTriebwerkPicture.Dock = DockStyle.Fill;
                panelBewaffnungPicture.Dock = DockStyle.Fill;
                panelSystemePicture.Dock = DockStyle.Fill;
                panelDemo.Dock = DockStyle.Fill;
                panelMain.Dock = DockStyle.Fill;
                panelSuzInfo.Dock = DockStyle.Fill;

                label2Bewaffnung.Visible = false;
                label3Bewaffnung.Visible = false;
                label4Bewaffnung.Visible = false;
                label5Bewaffnung.Visible = false;
                label6Bewaffnung.Visible = false;
                label7Bewaffnung.Visible = false;
                label8Bewaffnung.Visible = false;
                label9Bewaffnung.Visible = false;
                label11Bewaffnung.Visible = false;
                label12Bewaffnung.Visible = false;
                label17Bewaffnung.Visible = false;
                label19Bewaffnung.Visible = false;
                label14Bewaffnung.Visible = false;


                player.SoundLocation = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\SoundEffects\Shot.wav";
                player.Load();



                showAllPanels();
                timeLeftToScreenSave = new TimeSpan(0, 0, ScreenSaveTime);
                countdownTimer.Start();
                this.MouseDown += new MouseEventHandler(this.mouseClickRefresh);

                axDemoPlayer.URL = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\SUZ Video\Video.mp4";
                axDemoPlayer.Ctlcontrols.stop();

                loadSettings();

                button1.PerformClick();

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
        //  This method is triggered when the behavior of the Media Player changes, 
        //  for example, when the video has ended.
        //=======================================================================================
        private async void AxWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                if ((WMPLib.WMPPlayState)e.newState == WMPLib.WMPPlayState.wmppsMediaEnded)
                {
                    await Task.Delay(200);
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        public void playVideo()
        {
            try
            {
                if (axWindowsMediaPlayer1.currentMedia != null)
                {
                    double remainingTime = axWindowsMediaPlayer1.currentMedia.duration - axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                    if (remainingTime <= 2)
                    {
                        axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
                        visualisierungInstance.screnSaveToggle(true);
                    }
                }
                else
                {
                    MessageBox.Show("Es ist kein Video vorhanden");
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        public void screnSaveToggle(bool status)
        {
            try
            {
                if (status == true)
                {
                    screenSafe.Show();
                    axWindowsMediaPlayer1.Show();
                    panelBackground.Hide();
                    panelControlls.Hide();
                    axWindowsMediaPlayer1.Parent = screenSafe;
                    axWindowsMediaPlayer1.Dock = DockStyle.Fill;
                    axWindowsMediaPlayer1.BringToFront();


                    axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    //  ScreenSave Modus beendet
                    axWindowsMediaPlayer1.Hide();
                    panelBackground.Show();
                    panelControlls.Show();
                    screenSafe.Hide();
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
        //  In this method active mouse events are tracked and when an event is triggered
        //  the timer is reset until the ScreenSave mode is activated.
        //=======================================================================================
        private void mouseClickRefresh(object sender, MouseEventArgs e)
        {
            try
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        timerReset();
                        break;
                    case MouseButtons.Right:
                        timerReset();
                        break;
                    default:
                        break;
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
        //  This method is always called when a button is pressed. 
        //  Afterwards this method resets the timer until the ScreenSave mode is activated.
        //=======================================================================================
        private void timerReset()
        {
            try
            {
                timeLeftToScreenSave = new TimeSpan(0, 0, ScreenSaveTime);
                countdownTimer.Start();
                screenSaveMode = false;
                dataViewInstance.screnSaveToggle(false);
                visualisierungInstance.screnSaveToggle(false);
                screnSaveToggle(false);
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
        //  In this method, the timer is counted down every second until the ScreenSave
        //  mode is activated.
        //=======================================================================================
        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                timeLeftToScreenSave = timeLeftToScreenSave.Subtract(TimeSpan.FromSeconds(1));
                if (timeLeftToScreenSave.TotalSeconds <= 0)
                {
                    countdownTimer.Stop();




                    //MessageBox.Show("Timer ende, ScreenSave wird gestartet.");
                    dataViewInstance.screnSaveToggle(true);
                    visualisierungInstance.screnSaveToggle(true);
                    screnSaveToggle(true);
                    endDemo();

                    //System.Threading.Thread mouseTrackerThread = new System.Threading.Thread(movementTracker);
                    //mouseTrackerThread.Start();



                    screenSaveMode = true;
                    movementTracker();
                    timerReset();
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
        //  in this method the position of the mouse is tracked during the ScreenSave mode
        //  and the Screen Save mode is deactivated when the mouse is moved.
        //=======================================================================================
        private void movementTracker()
        {
            int currentPositionOfX = 0;
            int currentPositionOfY = 0;
            try
            {
                while (screenSaveMode == false)
                {
                    currentPositionOfX = 0;
                    currentPositionOfY = 0;
                }
                while (screenSaveMode == true)
                {
                    System.Threading.Thread.Sleep(100);
                    int x = MousePosition.X;
                    int y = MousePosition.Y;

                    // hier wird darauf geprüft ob die Videos zu Ende sind damit diese
                    //wärend dem Screensave Modus in Dauerschleife laufen
                    //visualisierungInstance.playVideo();
                    dataViewInstance.playVideo();
                    playVideo();


                    if (currentPositionOfX == 0 & currentPositionOfY == 0)
                    {
                        currentPositionOfX = x;
                        currentPositionOfY = y;
                    }
                    else
                    {
                        if (x != currentPositionOfX)
                        {
                            dataViewInstance.screnSaveToggle(false);
                            visualisierungInstance.screnSaveToggle(false);
                            screnSaveToggle(false);
                            return;
                        }

                        else if (y != currentPositionOfY)
                        {
                            dataViewInstance.screnSaveToggle(false);
                            visualisierungInstance.screnSaveToggle(false);
                            screnSaveToggle(false);
                            return;
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





        private void showAllPanels()
        {
            try
            {
                lblFade.SendToBack();
                panelJoystick.SendToBack();
                panelDemo.SendToBack();
                panelSystemePicture.SendToBack();
                panelBewaffnungPicture.SendToBack();
                panelTriebwerkPicture.SendToBack();
                panelSuzInfo.SendToBack();
                panelMainSub.SendToBack();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void resetButtonColors()
        {
            try
            {
                button1.ForeColor = defaultButton;
                button2.ForeColor = defaultButton;
                button3.ForeColor = defaultButton;
                button4.ForeColor = defaultButton;
                button5.ForeColor = defaultButton;
                button6.ForeColor = defaultButton;
                button7.ForeColor = defaultButton;
                button8.ForeColor = defaultButton;
                button9.ForeColor = defaultButton;
                button10.ForeColor = defaultButton;
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
        //  Date:       01.09.2023
        //
        //  Buttons für die verschieden Themen, es werden daten aus txt dateien ausgelesen, und
        //  anschließend an das Fesnster DataViw übergeben
        //=======================================================================================
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    diasbleButtons();
                    coolDownTimer.Start();
                    string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\EF2000\EF2000-technicalData.txt");
                    string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\EF2000\EF2000-description.txt");
                    string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\EF2000\EF2000.png");
                    try
                    {
                        dataViewInstance.changeValue(technicalData, description, picturePath);
                        disableJoystick();
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        showAllPanels();
                        resetButtonColors();
                        button1.ForeColor = selectedButton;
                        labelMainPanel.Show();
                        labelMainPanel.Text = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataForMainPage\EF2000.txt");
                        panelMainSub.BackColor = Color.Black;
                        dataViewInstance.SlidesPresentation(false, "");
                        panelMainSub.BackgroundImage = null;
                        panelMain.BringToFront();
                        this.ResumeLayout();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }


                    timerReset();
                    ueberschrift.Text = button1.Text;
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
                if (coolDownStatus == false)
                {
                    diasbleButtons();
                    coolDownTimer.Start();
                    string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Systeme-technicalData.txt");
                    string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Systeme-description.txt");
                    string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Systeme.png");
                    try
                    {
                        dataViewInstance.changeValue(technicalData, description, picturePath);
                        disableJoystick();
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        showAllPanels();
                        fadeTimer.Stop();
                        panelSystemePicture.BringToFront();
                        resetButtonColors();
                        button2.ForeColor = selectedButton;
                        colorSystemeReset();
                        //panelSystemePicture.BackgroundImage = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Systeme-back.png");
                        //panelSystemePicture.BackgroundImageLayout = ImageLayout.Zoom;
                        //panelTriebwerkPicture.BackgroundImage = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Systeme-back.png");
                        //panelTriebwerkPicture.BackgroundImageLayout = ImageLayout.Zoom;
                        dataViewInstance.SlidesPresentation(false, "");
                        //systemeLabels = CreateLabelList("Systeme", 24);
                        //UpdateLabelsFromTextFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Ressources\LabelTextAndPosition\LabelSysteme.txt", systemeLabels);
                        lblFade.BringToFront();
                        this.ResumeLayout();
                        fadeTimer.Start();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button2.Text;
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
                if (coolDownStatus == false)
                {
                    diasbleButtons();
                    coolDownTimer.Start();
                    string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Demo\Demo-technicalData.txt");
                    string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Demo\Demo-description.txt");
                    string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Demo\Demo.png");
                    try
                    {
                        dataViewInstance.changeValue(technicalData, description, picturePath);
                        disableJoystick();
                        demoTimer.Start();
                        selectedControlMode = "Demo";
                        this.SuspendLayout();
                        resetButtonColors();
                        button3.ForeColor = selectedButton;
                        axDemoPlayer.Enabled = false;
                        axDemoPlayer.uiMode = "none";
                        axDemoPlayer.URL = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DemoVideo.mp4";
                        dataViewInstance.SlidesPresentation(false, "");
                        //visualisierungInstance.playDemoVideo(selectedControlMode);
                        //axDemoPlayer.Ctlcontrols.currentPosition = 0;
                        //axDemoPlayer.Ctlcontrols.play();
                        showAllPanels();
                        panelDemo.BringToFront();
                        this.ResumeLayout();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button3.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    diasbleButtons();
                    coolDownTimer.Start();
                    string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Stick\Stick-technicalData.txt");
                    string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Stick\Stick-description.txt");
                    string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Stick\Stick.png");
                    try
                    {
                        dataViewInstance.changeValue(technicalData, description, picturePath);
                        selectedControlMode = "Aircraft";
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        showAllPanels();
                        panelJoystick.BringToFront();
                        resetButtonColors();
                        dataViewInstance.SlidesPresentation(false, "");
                        button4.ForeColor = selectedButton;
                        this.ResumeLayout();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    //MessageBox.Show("You have the control now");

                    timerReset();
                    ueberschrift.Text = button4.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    diasbleButtons();
                    coolDownTimer.Start();
                    string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Bewaffnung-technicalData.txt");
                    string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Bewaffnung-description.txt");
                    string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Bewaffnung.png");
                    try
                    {
                        dataViewInstance.changeValue(technicalData, description, picturePath);
                        disableJoystick();
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        resetButtonColors();
                        button5.ForeColor = selectedButton;
                        showAllPanels();
                        fadeTimer.Stop();
                        colorBewaffnungReset();
                        panelBewaffnungPicture.BringToFront();
                        dataViewInstance.SlidesPresentation(false, "");
                        //bewaffnungLabels = CreateLabelList("Bewaffnung", 23);
                        //UpdateLabelsFromTextFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Ressources\LabelTextAndPosition\LabelBewaffnung.txt", bewaffnungLabels);

                        lblFade.BringToFront();
                        this.ResumeLayout();
                        fadeTimer.Start();


                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button5.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    diasbleButtons();
                    coolDownTimer.Start();
                    string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Triebwerk-technicalData.txt");
                    string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Triebwerk-description.txt");
                    string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Triebwerk.png");
                    try
                    {
                        dataViewInstance.changeValue(technicalData, description, picturePath);
                        disableJoystick();
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        showAllPanels();
                        fadeTimer.Stop();
                        resetButtonColors();
                        button6.ForeColor = selectedButton;
                        colorTrieberkReset();
                        panelTriebwerkPicture.BringToFront();
                        //panelTriebwerkPicture.BackgroundImage = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Ressources\Backgrounds\engine-back.png");
                        //panelTriebwerkPicture.BackgroundImageLayout = ImageLayout.Zoom;
                        dataViewInstance.SlidesPresentation(false, "");
                        //bewaffnungLabels = CreateLabelList("Triebwerk", 7);
                        //UpdateLabelsFromTextFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Ressources\LabelTextAndPosition\LabelTriebwerk.txt", triebwerkLabels);
                        lblFade.BringToFront();
                        this.ResumeLayout();
                        fadeTimer.Start();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button6.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    diasbleButtons();
                    coolDownTimer.Start();
                    string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\MaTE\MaTE-technicalData.txt");
                    string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\MaTE\MaTE-description.txt");
                    string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\MaTE\MaTE.png");
                    try
                    {
                        dataViewInstance.changeValue(technicalData, description, picturePath);
                        disableJoystick();
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        showAllPanels();
                        panelMainSub.BringToFront();
                        resetButtonColors();
                        button7.ForeColor = selectedButton;
                        labelMainPanel.Hide();
                        panelMainSub.BackColor = Color.Transparent;
                        panelMainSub.BackgroundImage = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataForMainPage/matelogo.gif");
                        dataViewInstance.SlidesPresentation(false, "");
                        panelMain.Show();
                        this.ResumeLayout();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button7.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    try
                    {
                        disableJoystick();
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        showAllPanels();
                        resetButtonColors();
                        button8.ForeColor = selectedButton;
                        panelSuzInfo.BringToFront();
                        globalSlidesMode = "SUZ";
                        pictureBoxPdfView.Dock = DockStyle.None;
                        pictureBoxPdfView.Size = new Size(1559, 1000);
                        pictureBoxPdfView.Location = new Point(0, 0);
                        pictureBoxPdfView.Image = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\SUZ Info\Flyer\1.png");
                        dataViewInstance.SlidesPresentation(true, "SUZ");
                        btnFlyerBack.Hide();
                        btnFlyerForward.Show();
                        this.ResumeLayout();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button8.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    try
                    {

                        disableJoystick();
                        this.SuspendLayout();
                        showAllPanels();
                        startVideo();
                        resetButtonColors();
                        button9.ForeColor = selectedButton;
                        this.ResumeLayout();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button9.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (coolDownStatus == false)
                {
                    try
                    {
                        disableJoystick();
                        //endDemo();
                        endVideo();
                        this.SuspendLayout();
                        showAllPanels();
                        resetButtonColors();
                        button10.ForeColor = selectedButton;
                        panelSuzInfo.BringToFront();
                        globalSlidesMode = "Berufsbildung";
                        pictureBoxPdfView.Dock = DockStyle.Fill;
                        pictureBoxPdfView.Image = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Berufsbildung Info\BerufsbildungRight.png");
                        dataViewInstance.SlidesPresentation(true, "Berufsbildung");
                        btnFlyerBack.Hide();
                        btnFlyerForward.Hide();
                        this.ResumeLayout();
                    }
                    catch (Exception ex)
                    {
                        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                        string severity = ex.GetType().Name;
                        loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                    }

                    timerReset();
                    ueberschrift.Text = button10.Text;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }





        public void startVideo()
        {
            try
            {
                panelSuzInfo.SendToBack();
                panelDemo.BringToFront();

                axDemoPlayer.Enabled = false;
                axDemoPlayer.uiMode = "none";

                dataViewInstance.playVideo(true);
                visualisierungInstance.playVideo(true);
                syncStart();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        public void endVideo()
        {
            try
            {
                axDemoPlayer.Ctlcontrols.stop();
                axDemoPlayer.Ctlcontrols.currentPosition = 0;
                dataViewInstance.playVideo(false);
                visualisierungInstance.playVideo(false);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        public void syncStart()
        {
            try
            {
                axDemoPlayer.Ctlcontrols.currentPosition = 0;
                axDemoPlayer.Ctlcontrols.play();
                visualisierungInstance.syncStart();
                dataViewInstance.syncStart();
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
        //  Methode wird von der Config aufgerufen und ist für die entfernung der Joystick
        //  Funktion (Der Button Joystick wird aus dem UI Entfernt)
        //=======================================================================================
        public void statusJoystick(bool joystickVisible)
        {
            try
            {
                if (joystickVisible == false)
                {
                    button4.Hide();
                }
                else
                {
                    button4.Show();
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
        //  Methode zur weitergabe von Informationenen zwischen den Fenstern hier werden die
        //  ausgelesenen Joystick werte von der Config Seite an diese Methode Übergeben diese
        //  Methode gibt dann anschließend zurück wofür diese genutzt werden sollen
        //  Aircraft/Laser/Demo
        //=======================================================================================
        public string controlUnitDisplay(int valueUp, int valueDown, int valueLeft, int valueRight, int valueTwistLeft, int valueTwistRight, int valueThrottleRight, int valueThrottleLeft, int valueAirbrake, bool triggerSafety, bool trigger, bool sound)
        {
            try
            {
                globalSound = sound;
                panelUpFront.Height = (int)(panelUpBack.Height * (valueUp / 100.0));
                panelDownFront.Height = (int)(panelDownBack.Height * (valueDown / 100.0));
                JoystickLeftFill.Width = (int)(JoystickLeftBack.Width * (valueLeft / 100.0));
                JoystickRightFill.Width = (int)(JoystickRightBack.Width * (valueRight / 100.0));
                RuderLeftFill.Width = (int)(RuderLeftBack.Width * (valueTwistLeft / 100.0));
                RuderRightFill.Width = (int)(RuderRightBack.Width * (valueTwistRight / 100.0));
                throttleFillLeft.Height = (int)(throttleBackLeft.Height * (valueThrottleLeft / 100.0));
                throttleFillRight.Height = (int)(throttleBackRight.Height * (valueThrottleRight / 100.0));
                airbrakeFill.Height = (int)(airbrakeBack.Height * (valueAirbrake / 100.0));
                if (selectedControlMode == "Aircraft" || selectedControlMode == "Laser")
                {
                    if (triggerSafety == true && !triggerSafetyDelay.Enabled)
                    {
                        if (panelLaserStatus.BackColor == Color.Red)
                        {
                            panelLaserStatus.BackColor = Color.LightGreen;
                            labelLaserLockStatus.Text = "Ready";
                            selectedControlMode = "Laser";
                        }
                        else
                        {
                            panelLaserStatus.BackColor = Color.Red;
                            labelLaserLockStatus.Text = "Locked";
                            selectedControlMode = "Aircraft";
                        }
                        triggerSafetyDelay.Start();
                    }
                }
                //if (trigger == true && panelLaserStatus.BackColor == Color.LightGreen && !soundDelay.Enabled)
                //{
                //    if (globalSound == true)
                //    {
                //        player.Play();
                //        soundDelay.Start();
                //    }
                //}
                if (globalSound == false)
                {
                    if (axDemoPlayer != null && !axDemoPlayer.IsDisposed)
                    {
                        // Zugriff auf axDemoPlayer-Eigenschaften oder -Methoden.

                        axDemoPlayer.settings.mute = true;
                    }
                }
                else
                {
                    try
                    {
                        axDemoPlayer.settings.mute = false;
                    }
                    catch
                    {
                        //nothing here
                    }
                }



                if (valueThrottleLeft >= 80 && valueThrottleRight >= 80)
                {
                    if (selectedControlMode == "Aircraft" && allreadyActivated == false)
                    {
                        foggerStatus = true;
                        allreadyActivated = true;
                    }
                    if (!blinkTimer.Enabled)
                    {
                        labelAfterburner.Visible = true; // Damit es beim ersten Mal sofort sichtbar ist
                        blinkTimer.Start();
                    }
                }
                else
                {
                    if (selectedControlMode == "Aircraft")
                    {
                        foggerStatus = false;
                        allreadyActivated = false;
                    }
                    blinkTimer.Stop();
                    labelAfterburner.Visible = false; // Label verstecken, wenn Timer gestoppt wird
                }

                return selectedControlMode;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return "";
            }
        }

        public int[] getCurrentPositons()
        {
            try
            {
                return currentPositions;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return null;
            }
        }


        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       01.09.2023
        //
        //  Label für das Triebwerk
        //=======================================================================================
        private void label1Triebwerk_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\HD-Turbine\HD-Turbine-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\HD-Turbine\HD-Turbine-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\HD-Turbine\HD-Turbine.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorTrieberkReset();
                    label1Triebwerk.ForeColor = selectedLabelTriebwerk;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "HD-Turbine";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label2Triebwerk_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\ND-Turbine\ND-Turbine-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\ND-Turbine\ND-Turbine-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\ND-Turbine\ND-Turbine.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorTrieberkReset();
                    label2Triebwerk.ForeColor = selectedLabelTriebwerk;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "ND-Turbine";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label3Triebwerk_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Nachbrenner\Nachbrenner-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Nachbrenner\Nachbrenner-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Nachbrenner\Nachbrenner.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorTrieberkReset();
                    label3Triebwerk.ForeColor = selectedLabelTriebwerk;
                    foggerStatus = true;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Nachbrenner";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label4Triebwerk_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\ND-Verdichter\ND-Verdichter-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\ND-Verdichter\ND-Verdichter-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\ND-Verdichter\ND-Verdichter.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorTrieberkReset();
                    label4Triebwerk.ForeColor = selectedLabelTriebwerk;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "ND-Verdichter";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label5Triebwerk_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\HD-Verdichter\HD-Verdichter-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\HD-Verdichter\HD-Verdichter-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\HD-Verdichter\HD-Verdichter.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorTrieberkReset();
                    label5Triebwerk.ForeColor = selectedLabelTriebwerk;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "HD-Verdichter";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label6Triebwerk_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Brennkammer\Brennkammer-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Brennkammer\Brennkammer-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Brennkammer\Brennkammer.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorTrieberkReset();
                    label6Triebwerk.ForeColor = selectedLabelTriebwerk;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Brennkammer";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label7Triebwerk_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Schubduese\Schubduese-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Schubduese\Schubduese-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Triebwerk\Schubduese\Schubduese.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorTrieberkReset();
                    label7Triebwerk.ForeColor = selectedLabelTriebwerk;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Schubduese";
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
        //  Label für die Bewaffnung
        //=======================================================================================
        private void label1Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\1000lTank\1000 l Tank-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\1000lTank/1000 l Tank-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\1000lTank/1000 l Tank.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label1Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "1000l Tank";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label2Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\1000lbBomb\1000 lb Bomb-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\1000lbBomb\1000 lb Bomb-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\1000lbBomb\1000 lb Bomb.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label2Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "1000lb Bomb";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label3Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\2000lTank\2000 l Tank-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\2000lTank\2000 l Tank-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\2000lTank\2000 l Tank.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label3Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "2000l Tank";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label4Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\2000lbBomb\2000 lb Bomb-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\2000lbBomb\2000 lb Bomb-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\2000lbBomb\2000 lb Bomb.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label4Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "2000lb Bomb";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label5Bewaffnung_Click_1(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\500lbBomb\500 lb Bomb-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\500lbBomb\500 lb Bomb-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\500lbBomb\500 lb Bomb.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label5Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "2000lb Bomb";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label6Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AGM-65 Maverick\AGM-65 Maverick-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AGM-65 Maverick\AGM-65 Maverick-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AGM-65 Maverick\AGM-65 Maverick.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label6Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "AGM-65 Maverick";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label7Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-132 ASRAAM\AIM-132 ASRAAM-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-132 ASRAAM\AIM-132 ASRAAM-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-132 ASRAAM\AIM-132 ASRAAM.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label7Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "AIM-132 ASRAAM";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label8Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\ALARM\ALARM-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\ALARM\ALARM-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\ALARM\ALARM.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label8Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "ALARM";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label9Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\CBLS-200\CBLS-200-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\CBLS-200\CBLS-200-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\CBLS-200\CBLS-200.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label9Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "CBLS-200";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label10Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\IRIS-T\IRIS-T-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\IRIS-T\IRIS-T-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\IRIS-T\IRIS-T.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label10Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "IRIS-T";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label11Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\JSOW\JSOW-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\JSOW\JSOW-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\JSOW\JSOW.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label11Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "JSOW";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label12Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Kongsberg NSM\Kongsberg NSM-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Kongsberg NSM\Kongsberg NSM-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Kongsberg NSM\Kongsberg NSM.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label12Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Kongsberg NSM";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label13Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Laser Design Pod\Laser Design Pod-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Laser Design Pod\Laser Design Pod-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Laser Design Pod\Laser Design Pod.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label13Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Laser Design Pod";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label14Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\MBDA Brimstone\MBDA Brimstone-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\MBDA Brimstone\MBDA Brimstone-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\MBDA Brimstone\MBDA Brimstone.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label14Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "MBDA Brimstone";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label15Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\MBDA Meteor\MBDA Meteor-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\MBDA Meteor\MBDA Meteor-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\MBDA Meteor\MBDA Meteor.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label15Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "MBDA Meteor";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label16Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Paveway ll\Paveway ll-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Paveway ll\Paveway ll-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Paveway ll\Paveway ll.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label16Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Paveway ll";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label17Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Paveway lll\Paveway lll-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Paveway lll\Paveway lll-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Paveway lll\Paveway lll.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label17Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Paveway lll";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label18Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\RECCE Pod\RECCE Pod-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\RECCE Pod\RECCE Pod-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\RECCE Pod\RECCE Pod.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label18Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "RECCE Pod";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label19Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Storm Shadow\Storm Shadow-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Storm Shadow\Storm Shadow-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Storm Shadow\Storm Shadow.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label19Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Storm Shadow";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label20Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Taurus KEPD 350\Taurus KEPD 350-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Taurus KEPD 350\Taurus KEPD 350-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\Taurus KEPD 350\Taurus KEPD 350.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label20Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "Taurus KEPD 350";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label21Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AGM-88 HARM\AGM-88 HARM-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AGM-88 HARM\AGM-88 HARM-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AGM-88 HARM\AGM-88 HARM.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label21Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "AGM-88 HARM";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label22Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-9 Sidewinder\AIM-9 Sidewinder-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-9 Sidewinder\AIM-9 Sidewinder-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-9 Sidewinder\AIM-9 Sidewinder.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label22Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "AIM-9 Sidewinder";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label23Bewaffnung_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-120 AMRAAM\AIM-120 AMRAAM-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-120 AMRAAM\AIM-120 AMRAAM-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Bewaffnung\AIM-120 AMRAAM\AIM-120 AMRAAM.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorBewaffnungReset();
                    label23Bewaffnung.ForeColor = selectedLabelBewaffnung;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                //ueberschrift.Text = "AIM-120 AMRAAM";
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
        //  Label für die Systeme
        //=======================================================================================
        private void label1Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\A+I\A+I-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\A+I\A+I-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\A+I\A+I.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label1Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label2Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ACS\ACS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ACS\ACS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ACS\ACS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label2Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label3Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Avionic Systems\Avionic Systems-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Avionic Systems\Avionic Systems-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Avionic Systems\Avionic Systems.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label3Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label4Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Comms\Comms-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Comms\Comms-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Comms\Comms.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label4Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label5Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\D+C\D+C-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\D+C\D+C-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\D+C\D+C.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label5Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label6Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\DASS\DASS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\DASS\DASS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\DASS\DASS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label6Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label7Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ECS + LSS\ECS + LSS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ECS + LSS\ECS + LSS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ECS + LSS\ECS + LSS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label7Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label8Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Electric\Electric-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Electric\Electric-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Electric\Electric.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label8Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label9Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Engines\Engines-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Engines\Engines-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Engines\Engines.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label9Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label10Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ESS\ESS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ESS\ESS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\ESS\ESS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label10Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label11Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\FCS\FCS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\FCS\FCS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\FCS\FCS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label11Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label12Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Fuel\Fuel-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Fuel\Fuel-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Fuel\Fuel.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label12Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label13Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\General Systems\General Systems-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\General Systems\General Systems-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\General Systems\General Systems.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label13Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label14Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\GLU\GLU-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\GLU\GLU-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\GLU\GLU.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label14Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label15Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\GSS\GSS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\GSS\GSS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\GSS\GSS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label15Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label16Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Hydraulic\Hydraulic-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Hydraulic\Hydraulic-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Hydraulic\Hydraulic.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label16Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label17Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\IMRS\IMRS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\IMRS\IMRS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\IMRS\IMRS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label17Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label18Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Jettison and CES\Jettison and CES-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Jettison and CES\Jettison and CES-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Jettison and CES\Jettison and CES.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label18Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label19Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Landing Gear\Landing Gear-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Landing Gear\Landing Gear-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Landing Gear\Landing Gear.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label19Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label20Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\MSS\MSS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\MSS\MSS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\MSS\MSS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label20Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label21Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Nav\Nav-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Nav\Nav-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Nav\Nav.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label21Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label22Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\SPS\SPS-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\SPS\SPS-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\SPS\SPS.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label22Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }
        private void label23Systeme_Click(object sender, EventArgs e)
        {
            try
            {
                string technicalData = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Structure\Structure-technicalData.txt");
                string description = datahandlerInstance.ReadDataFromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Structure\Structure-description.txt");
                string picturePath = ($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Systeme\Structure\Structure.png");
                try
                {
                    dataViewInstance.changeValue(technicalData, description, picturePath);
                    colorSystemeReset();
                    label23Systeme.ForeColor = selectedLabelSysteme;
                }
                catch (Exception ex)
                {
                    string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                    string severity = ex.GetType().Name;
                    loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                }

                timerReset();
                ueberschrift.Text = "Systeme";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }








        private void btnFlyerBack_Click(object sender, EventArgs e)
        {
            if (globalSlidesMode == "SUZ")
            {
                this.SuspendLayout();
                btnFlyerBack.Hide();
                btnFlyerForward.Show();
                pictureBoxPdfView.Image = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\SUZ Info\Flyer\1.png");
                this.ResumeLayout();
            }
            else if (globalSlidesMode == "Berufsbildung")
            {
                this.SuspendLayout();
                btnFlyerBack.Hide();
                btnFlyerForward.Show();
                pictureBoxPdfView.Image = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Berufsbildung Info\Flyer\1.png");
                this.ResumeLayout();
            }
            else
            {
                // nothing here
            }
        }

        private void btnFlyerForward_Click(object sender, EventArgs e)
        {
            if (globalSlidesMode == "SUZ")
            {
                this.SuspendLayout();
                btnFlyerBack.Show();
                btnFlyerForward.Hide();
                pictureBoxPdfView.Image = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\SUZ Info\Flyer\2.png");
                this.ResumeLayout();
            }
            else if (globalSlidesMode == "Berufsbildung")
            {
                this.SuspendLayout();
                btnFlyerBack.Show();
                btnFlyerForward.Hide();
                pictureBoxPdfView.Image = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Berufsbildung Info\Flyer\2.png");
                this.ResumeLayout();
            }
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       01.09.2023
        //
        //  Setz die Farbe der Triebwerk-Label zurück
        //=======================================================================================
        private void colorTrieberkReset()
        {
            try
            {
                //Triebwerk
                label1Triebwerk.ForeColor = defaultLabelTriebwerk;
                label2Triebwerk.ForeColor = defaultLabelTriebwerk;
                label3Triebwerk.ForeColor = defaultLabelTriebwerk;
                label4Triebwerk.ForeColor = defaultLabelTriebwerk;
                label5Triebwerk.ForeColor = defaultLabelTriebwerk;
                label6Triebwerk.ForeColor = defaultLabelTriebwerk;
                label7Triebwerk.ForeColor = defaultLabelTriebwerk;
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
        //  Setz die Farbe der Bewaffnung-Label zurück
        //=======================================================================================
        private void colorBewaffnungReset()
        {
            try
            {
                //Bewaffnung
                label1Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label2Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label3Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label4Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label5Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label6Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label7Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label8Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label9Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label10Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label11Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label12Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label13Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label14Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label15Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label16Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label17Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label18Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label19Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label20Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label21Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label22Bewaffnung.ForeColor = defaultLabelBewaffnung;
                label23Bewaffnung.ForeColor = defaultLabelBewaffnung;
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
        //  Setz die Farbe der System-Label zurück
        //=======================================================================================
        private void colorSystemeReset()
        {
            try
            {
                //Systeme
                label1Systeme.ForeColor = defaultLabelSysteme;
                label2Systeme.ForeColor = defaultLabelSysteme;
                label3Systeme.ForeColor = defaultLabelSysteme;
                label4Systeme.ForeColor = defaultLabelSysteme;
                label5Systeme.ForeColor = defaultLabelSysteme;
                label6Systeme.ForeColor = defaultLabelSysteme;
                label7Systeme.ForeColor = defaultLabelSysteme;
                label8Systeme.ForeColor = defaultLabelSysteme;
                label9Systeme.ForeColor = defaultLabelSysteme;
                label10Systeme.ForeColor = defaultLabelSysteme;
                label11Systeme.ForeColor = defaultLabelSysteme;
                label12Systeme.ForeColor = defaultLabelSysteme;
                label13Systeme.ForeColor = defaultLabelSysteme;
                label14Systeme.ForeColor = defaultLabelSysteme;
                label15Systeme.ForeColor = defaultLabelSysteme;
                label16Systeme.ForeColor = defaultLabelSysteme;
                label17Systeme.ForeColor = defaultLabelSysteme;
                label18Systeme.ForeColor = defaultLabelSysteme;
                label19Systeme.ForeColor = defaultLabelSysteme;
                label20Systeme.ForeColor = defaultLabelSysteme;
                label21Systeme.ForeColor = defaultLabelSysteme;
                label22Systeme.ForeColor = defaultLabelSysteme;
                label23Systeme.ForeColor = defaultLabelSysteme;

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
        //  Timer lässt das Afterburner Label aufblinken
        //=======================================================================================
        private void blinkTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                labelAfterburner.Visible = !labelAfterburner.Visible;
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
        //  Sorgt dafür das man nicht zufällig den Modus Laser oder Joystick wächselt
        //=======================================================================================
        private void triggerSafetyDelay_Tick(object sender, EventArgs e)
        {
            try
            {
                triggerSafetyDelay.Stop();
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
        //  Timer sorgt dafür das der Sound komplett abgespielt wird und nicht unterbrochen wird
        //=======================================================================================
        private void soundDelay_Tick(object sender, EventArgs e)
        {
            try
            {
                soundDelay.Stop();
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
        //  Wird bei jedem ablauf des Timer getriggert und aktualisiert die derzeitige position
        //  der Servos
        //=======================================================================================
        private void demoTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                timeDemo = timeDemo + demoTimer.Interval;

                // Demo Daten auslesen und dann reinschreiben
                if (movementDict.ContainsKey(timeDemo))
                {
                    currentPositions = movementDict[timeDemo];
                    //MessageBox.Show(string.Join(", ", currentPositions));

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
        //  Wird getriggert bei dem Anklicken des Media Players
        //=======================================================================================
        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {
            try
            {
                timerReset();
                screnSaveToggle(false);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        private void disableJoystick()
        {
            try
            {
                panelLaserStatus.BackColor = Color.Red;
                labelLaserLockStatus.Text = "Locked";
                selectedControlMode = "none";
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        public void diasbleButtons()
        {
            try
            {
                coolDownStatus = true;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        public void enableButtons()
        {
            try
            {
                coolDownStatus = false;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }



        private void coolDownTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                enableButtons();
                coolDownTimer.Stop();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        public bool foggerTransferStatus()
        {
            try
            {
                bool tempStatus = foggerStatus;
                if (tempStatus == true)
                {
                    foggerStatus = false;
                }
                return tempStatus;
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
        //  Date:       01.09.2023
        //
        //  Alles unter diesem Kommentar sind funktionen welche bereits bedacht wurden aber
        //  aufgrund Zeitmangel, oder weil diese noch zu unausgereift waren verworven wurden
        //=======================================================================================

        //Mögliche Funktionnen bereits vorbereitet aber noch nicht implementiert:

        private List<Label> CreateLabelList(string category, int count)
        {
            List<Label> labelList = new List<Label>();

            for (int i = 1; i <= count; i++)
            {
                // Annahme: Deine Labels haben Namen wie "label1Triebwerk", "label2Triebwerk", usw.
                Label label = (Label)this.Controls.Find($"label{i}{category}", true).FirstOrDefault();

                if (label != null)
                {
                    labelList.Add(label);
                }
                else
                {
                    // Hier kannst du eine Fehlerbehandlung hinzufügen, wenn das Label nicht gefunden wurde.
                    MessageBox.Show($"Label label{i}{category} wurde nicht gefunden.");
                }
            }

            return labelList;
        }

        private void UpdateLabelsFromTextFile(string filePath, List<Label> labelsToUpdate)
        {
            MessageBox.Show("Test");
            string[] lines = datahandlerInstance.ReadLabelSettings(filePath);
            MessageBox.Show(lines[0].ToString());
            if (lines != null && lines.Length > 0)
            {
                // Annahme: Die Zeilen in der Textdatei enthalten den Text und die Position.
                for (int i = 0; i < Math.Min(labelsToUpdate.Count, lines.Length - 1); i++)
                {
                    // Beispielzeile in der Textdatei: label1Systeme position: X=543 Y=453 Text="asdf with spaces"
                    string line = lines[i];


                    int xPosition = int.Parse(getBetween(line, "X=", "Y="));


                    int yPosition = int.Parse(getBetween(line, "Y=", "Text="));

                    // Extrahiere den Text
                    string labelText = getBetween(line, "Text=\"", "\"");

                    labelText = labelText.Replace("\\n", Environment.NewLine);


                    labelsToUpdate[i].Text = labelText;


                    // Aktualisiere die Position und den Text des Labels
                    labelsToUpdate[i].Location = new Point(xPosition, yPosition);
                    // Optional: Zeige den aktualisierten Text in einer MessageBox an
                    //MessageBox.Show($"Label {labelsToUpdate[i].Name} aktualisiert. Position: X={xPosition}, Y={yPosition}, Text={labelText}");
                }
            }
        }
        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            try
            {
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    int Start, End;
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return strSource.Substring(Start, End - Start);
                }

                return "";
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void loadSettings()
        {
            try
            {
                panelBewaffnungPicture.BackgroundImage = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Ressources\Backgrounds\weapons-back.png");
                panelSystemePicture.BackgroundImage = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Ressources\Backgrounds\systeme-back.png");
                panelTriebwerkPicture.BackgroundImage = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Ressources\Backgrounds\engine-back.png");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }




        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // Adjust the alpha value to determine the opacity
                lblFade.SendToBack();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        public bool returnMovement()
        {
            try
            {
                return enableMovement;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return false;
            }
        }

        private void ueberschrift_Click(object sender, EventArgs e)
        {
            try
            {
                if (enableMovement == false)
                {
                    enableMovement = true;
                    lblMovementStatus.Visible = true;
                    currentmovementTime = 0;
                    enableMovementTimer.Start();

                }
                else
                {
                    enableMovement = false;
                    lblMovementStatus.Visible = false;
                    enableMovementTimer.Stop();
                    currentmovementTime = 0;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void enableMovementTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                currentmovementTime = currentmovementTime + enableMovementTimer.Interval;

                // Demo Daten auslesen und dann reinschreiben
                if (movementDict.ContainsKey(currentmovementTime))
                {
                    currentPositions = movementDict[currentmovementTime];
                    //MessageBox.Show(string.Join(", ", currentPositions));

                }
                else
                {
                    currentPositions = null;
                }
                if (currentmovementTime >= 1000000)
                {
                    currentmovementTime = 0;
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }


        public void berufsbildungShow(bool status)
        {
            if (status == true)
            {
                button10.Visible = true;
            }
            else
            {
                button10.Visible = false;
            }
        }


        public Dictionary<int, int[]> ReadDataFromFile(string pathOfFile)
        {
            Dictionary<int, int[]> result = new Dictionary<int, int[]>();
            try
            {
                if (File.Exists(pathOfFile))
                {
                    using (StreamReader sr = new StreamReader(pathOfFile, Encoding.UTF8))
                    {
                        // Überspringen der ersten Zeile (Überschriften)
                        sr.ReadLine();

                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            string[] parts = line.Split('\t'); // Annahme, dass die Werte durch Tabs getrennt sind

                            if (parts.Length > 1) // Mindestens ein Schlüssel und ein Wert sollten vorhanden sein
                            {
                                int key = int.Parse(parts[0]);
                                int[] values = new int[parts.Length - 1];

                                for (int i = 1; i < parts.Length; i++)
                                {
                                    values[i - 1] = int.Parse(parts[i]);
                                }

                                result[key] = values;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("File not found");
                }
            }
            catch (Exception ex)
            {
                //nothing here
            }
            //MessageBox.Show(result.ToString());
            return result;
        }
    }
}



