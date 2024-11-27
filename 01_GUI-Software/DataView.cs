using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.DesignerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EurofighterInformationCenter
{
    public partial class DataView : Form
    {

        public string Label1Text { get { return label1.Text; } set { label1.Text = value; } }
        public string Label2Text { get { return label2.Text; } set { label2.Text = value; } }
        public Image PictureBoxImage { get { return pictureBox1.Image; } set { pictureBox1.Image = value; } }


        datahandler datahandlerInstance;

        public bool isMaximized = false;
        public string userName = Environment.UserName;

        private Image[] picturesSUZ;
        public string InfoMode = "";
        private int currentPictureIndex = 0;


        logger loggerInstance;


        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       05.12.2023
        //
        //  Diese Methode wird bei der Initialisierung aufgerufen, in dieser Methode wird das UI
        //  angepasst, und Daten werden geladen
        //=======================================================================================
        public DataView()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            datahandlerInstance = new datahandler();

            picturesSUZ = datahandlerInstance.loadPictures($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\SUZ Info\Slides");
            loggerInstance = new logger();

            axWindowsMediaPlayer1.uiMode = "none";
            string mediaPath = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\ScreenSave.mp4";


            axWindowsMediaPlayer1.URL = mediaPath;
            axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;


            videoPlayer.uiMode = "none";
            videoPlayer.URL = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\SUZ Video\Video.mp4";
            videoPlayer.settings.mute = true;
            videoPlayer.Ctlcontrols.stop();
        }



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       27.07.2023
        //
        //  This method is responsible for changing the texts and images of the DataView app, 
        //  it is called from the MainPage.
        //=======================================================================================
        public void changeValue(string rightBox, string bottomBox, string imagePath)
        {
            try
            {
                Label1Text = rightBox;
                Label2Text = bottomBox;

                if (isMaximized)
                {
                    MinimizePictureBox();
                }
                if (System.IO.File.Exists(imagePath))
                {
                    PictureBoxImage = Image.FromFile(imagePath);
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else
                {
                    MessageBox.Show("No Image could be found at:  " + imagePath);
                }
                return;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
                return;
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



        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THEM-TL1
        //  Date:       27.07.2023
        //
        //  This method is called from the main page when the ScreenSaveTimer 
        //  (timer until the screensaver starts) has expired. This method then activates the screensaver.
        //=======================================================================================
        public void screnSaveToggle(bool status)
        {
            try
            {
                if (status == true)
                {
                    if (isMaximized)
                    {
                        MinimizePictureBox();
                    }
                    //axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
                    this.SuspendLayout();
                    axWindowsMediaPlayer1.Dock = DockStyle.Fill;
                    axWindowsMediaPlayer1.Show();
                    axWindowsMediaPlayer1.BringToFront();
                    this.ResumeLayout();
                    axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
                    axWindowsMediaPlayer1.Ctlcontrols.play();

                }
                else
                {
                    //  ScreenSave Modus beendet
                    axWindowsMediaPlayer1.Hide();
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
        //  Date:       05.12.2023
        //
        //  Diese Methode wird aufgerufen, und startet die Medienwiedergabe
        //=======================================================================================
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





        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       05.12.2023
        //
        //  Diese Methode wird bei dem Start der Anwendung, wenn das Fenster geladen wird
        //  aufgerufen, in dieser Methode wird das UI angepasst.
        //=======================================================================================
        private void DataView_Load(object sender, EventArgs e)
        {
            try
            {
                this.SuspendLayout();
                axWindowsMediaPlayer1.Hide();
                panelSlides.Dock = DockStyle.Fill;
                panelSlides.Hide();
                this.ResumeLayout();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }

        //=======================================================================================
        //  Author:     Tizian Senger           Department:     THGM-TL1
        //  Date:       05.12.2023
        //
        //  Diese Methode passt die PictureBox so an, dass sie den ganzen Bildschirm bedeckt
        //=======================================================================================
        public void MaximizePictureBox()
        {
            try
            {
                if (!isMaximized)
                {
                    this.SuspendLayout();
                    pictureBox1.Dock = DockStyle.Fill;
                    pictureBox1.BringToFront();
                    this.ResumeLayout();
                    isMaximized = true;
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
        //  Date:       05.12.2023
        //
        //  Diese Methode passt die PictureBox so an das sie wieder in das Uhrsprüngliche Layout
        //  zurückkehrt
        //=======================================================================================
        public void MinimizePictureBox()
        {
            try
            {
                if (isMaximized)
                {
                    this.SuspendLayout();
                    pictureBox1.Dock = DockStyle.None;
                    pictureBox1.Size = new Size(868, 495);
                    pictureBox1.Location = new Point(146, 22);
                    pictureBox1.SendToBack();
                    this.ResumeLayout();
                    isMaximized = false;
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
        //  Date:       05.12.2023
        //
        //  Diese Methode wird aufgerufen, wenn auf die PictureBox1 geklickt wird, daraufhin wird
        //  geprüft ob die PictureBox Maximiert ist, ist das der Fall dann wird sie minimiert,
        //  und auch genau so anders herrunm
        //=======================================================================================
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                if (videoPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    // Wenn das Video läuft, die Maximierung der PictureBox vermeiden
                    return;
                }
                this.SuspendLayout();
                if (isMaximized)
                {
                    MinimizePictureBox();
                }
                else
                {
                    MaximizePictureBox();
                }
                this.ResumeLayout();
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
        //  Date:       05.12.2023
        //
        //  Diese Methode wird von der InformationCenterMain Klasse aufgerufen, und ist dafür da,
        //  das UI zu aktualisieren, je nachdem was in der Klasse InformationCenterMain
        //  ausgewählt wurde
        //=======================================================================================
        public void SlidesPresentation(bool status, string kind)
        {
            try
            {
                InfoMode = kind;
                if (status == true)
                {
                    if (InfoMode == "SUZ")
                    {
                        this.SuspendLayout();
                        panelSlides.BringToFront();
                        currentPictureIndex = 0;
                        pictureBoxSUZPresentation.Dock = DockStyle.None;
                        pictureBoxSUZPresentation.Image = picturesSUZ[currentPictureIndex];
                        btnSlidesBack.Visible = false;
                        if (picturesSUZ.Length > 1)
                        {
                            btnSlidesForward.Enabled = true;
                            btnSlidesForward.Visible = true;
                        }
                        panelSlides.Show();
                        this.ResumeLayout();
                    }
                    else if (InfoMode == "Berufsbildung")
                    {
                        this.SuspendLayout();
                        panelSlides.BringToFront();
                        currentPictureIndex = 0;
                        pictureBoxSUZPresentation.Dock= DockStyle.Fill;
                        pictureBoxSUZPresentation.Image = Image.FromFile($@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DataViewFiles\Berufsbildung Info\BerufsbildungLeft.png");
                        btnSlidesBack.Visible = false;
                        btnSlidesForward.Visible = false;
                        btnSlidesBack.Visible = false;
                        panelSlides.Show();
                        this.ResumeLayout();
                    }
                }
                else
                {
                    this.SuspendLayout();
                    panelSlides.Hide();
                    panelSlides.SendToBack();
                    this.ResumeLayout();
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
        //  Date:       05.12.2023
        //
        //  Diese Methode wird aufgerufen, wenn der Button SlidesForward angeklickt wird,
        //  anschließend wird die Folie durch die nächste ausgetauscht
        //=======================================================================================
        private void btnSlidesForward_Click(object sender, EventArgs e)
        {
            try
            {
                if (InfoMode == "SUZ")
                {
                    this.SuspendLayout();
                    currentPictureIndex = currentPictureIndex + 1;
                    pictureBoxSUZPresentation.Image = picturesSUZ[currentPictureIndex];
                    btnSlidesBack.Enabled = true;
                    btnSlidesForward.Enabled = (currentPictureIndex < picturesSUZ.Length - 1);
                    btnSlidesBack.Visible = (currentPictureIndex > 0);
                    btnSlidesForward.Visible = (currentPictureIndex < picturesSUZ.Length - 1);
                    this.ResumeLayout();
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
        //  Date:       05.12.2023
        //
        //  Diese Methode wird aufgerufen, wenn der Button SlidesBack angeklickt wird, anschließend
        //  wird die Folie durch die nächste ausgetauscht
        //=======================================================================================
        private void btnSlidesBack_Click(object sender, EventArgs e)
        {
            try
            {
                if (InfoMode == "SUZ")
                {
                    this.SuspendLayout();
                    currentPictureIndex = currentPictureIndex - 1;
                    pictureBoxSUZPresentation.Image = picturesSUZ[currentPictureIndex];
                    btnSlidesForward.Enabled = true;
                    btnSlidesBack.Enabled = (currentPictureIndex > 0);
                    btnSlidesBack.Visible = (currentPictureIndex > 0);
                    btnSlidesForward.Visible = (currentPictureIndex < picturesSUZ.Length - 1);
                    this.ResumeLayout();
                }
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
                videoPlayer.Ctlcontrols.currentPosition = 0;
                videoPlayer.Ctlcontrols.play();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }




        public void playVideo(bool status)
        {
            try
            {
                if (status == true)
                {
                    this.SuspendLayout();
                    videoPlayer.BringToFront();

                    axWindowsMediaPlayer1.SendToBack();
                    panelSlides.SendToBack();
                    panelSlides.Hide();
                    videoPlayer.Dock = DockStyle.Fill;
                    videoPlayer.Show();
                    videoPlayer.Enabled = false;
                    videoPlayer.uiMode = "none";
                    this.ResumeLayout();
                }
                else
                {
                    this.SuspendLayout();
                    videoPlayer.Ctlcontrols.stop();
                    videoPlayer.Hide();
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                string severity = ex.GetType().Name;
                loggerInstance.writeToLog($"{methodName}: {severity} - {ex.ToString()}");
            }
        }

        private void videoPlayer_Enter(object sender, EventArgs e)
        {

        }
    }
}





