using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace EurofighterInformationCenter
{
    public partial class Visualisierung : Form
    {
        public string userName = Environment.UserName;
        public string mediaPath = "";
        logger loggerInstance;
        public Visualisierung()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            loggerInstance = new logger();
            axWindowsMediaPlayer1.uiMode = "none";

            mediaPath = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\ScreenSave.mp4";

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
                    if (axWindowsMediaPlayer1.URL.Contains("Demo")) 
                    {
                        axWindowsMediaPlayer1.URL = mediaPath;
                    }
                    else
                    {
                        axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
                        axWindowsMediaPlayer1.Ctlcontrols.play();
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
                    //axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
                    axWindowsMediaPlayer1.Ctlcontrols.currentPosition = 0;
                    axWindowsMediaPlayer1.Ctlcontrols.play();  
                }
                else
                {
                    //  ScreenSave Modus beendet
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
                        axWindowsMediaPlayer1.URL = mediaPath;
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


        //public void playDemoVideo(string modeSelected) 
        //{
        //    try
        //    {
        //        if (modeSelected == "Demo") 
        //        { 
        //            axWindowsMediaPlayer1.URL = $@"C:\Users\{userName}\Desktop\ApplicationFiles\Files\DemoVideo.mp4";
        //            axWindowsMediaPlayer1.stretchToFit = true;
        //            axWindowsMediaPlayer1.settings.mute = true;
        //        }
        //        else
        //        {
        //            axWindowsMediaPlayer1.URL = mediaPath;
        //        }
        //    }
        //    catch (Exception) 
        //    {
        //        MessageBox.Show("Visualisierung:    Bei dem abspielen des Demo Videos ist ein Fehler aufgetreten");
        //    }
        //}


        private void axWindowsMediaPlayer1_Enter(object sender, EventArgs e)
        {

        }




        public void syncStart() 
        {
            videoPlayer.Ctlcontrols.currentPosition = 0;
            videoPlayer.Ctlcontrols.play();
        }




        public void playVideo(bool status)
        {
            if (status == true)
            {
                this.SuspendLayout();
                videoPlayer.BringToFront();

                axWindowsMediaPlayer1.SendToBack();
                videoPlayer.Dock = DockStyle.Fill;
                videoPlayer.Show();
                videoPlayer.Enabled = false;
                videoPlayer.uiMode = "none";
                //this.ResumeLayout();
            }
            else
            {
                this.SuspendLayout();
                videoPlayer.Ctlcontrols.stop();
                videoPlayer.Hide();

            }
        }
    }
}
