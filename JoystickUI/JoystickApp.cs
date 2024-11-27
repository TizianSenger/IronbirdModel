using System;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace JoystickApp
{
    public partial class MainForm : Form
    {
        private DirectInput directInput;
        private Joystick joystick;
        private Timer timer;

        public MainForm()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            InitializeJoystick();
            timer = new Timer();
            timer.Interval = 100; // Aktualisierung alle 100 ms
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void InitializeJoystick()
        {
            directInput = new DirectInput();
            var joystickGuid = Guid.Empty;

            // Finden Sie alle Joysticks im System
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            if (joystickGuid == Guid.Empty)
            {
                MessageBox.Show("Kein Joystick gefunden!");
                return;
            }

            // Instanz des Joysticks erstellen und erwerben
            joystick = new Joystick(directInput, joystickGuid);
            joystick.Acquire();
        }



        private void Timer_Tick(object sender, EventArgs e)
        {
            if (joystick == null)
                return;

            joystick.Poll();
            var state = joystick.GetCurrentState();

            int xValueRaw = state.X;
            int yValueRaw = state.Y;
            int rzValueRaw = state.RotationZ;
            int throttleRaw = state.Z; // Schubregler

            // Werte in Labels anzeigen:
            lblXValue.Text = "X: " + xValueRaw.ToString();
            lblYValue.Text = "Y: " + yValueRaw.ToString();
            lblRZValue.Text = "RZ: " + rzValueRaw.ToString();
            lblThrottleValue.Text = "Throttle: " + (65535 - throttleRaw).ToString(); // Invertierter Wert für das Throttle-Label
            chkButtonState1.Checked = state.Buttons[0];
            chkButtonState2.Checked = state.Buttons[1];



            // Skalierte Werte berechnen:
            int xValue = ScaleJoystickValue(xValueRaw, 65535, 100);
            int yValue = ScaleJoystickValue(yValueRaw, 65535, 100);
            int rzValue = ScaleJoystickValue(rzValueRaw, 65535, 100);
            int throttleValue = 100 - ScaleJoystickValue(throttleRaw, 65535, 100); // Invertierter Schubregler Wert

            // Progress Bars aktualisieren:
            progressBarXLeft.Value = Clamp(xValue, 0, 100);
            progressBarXRight.Value = 100 - Clamp(xValue, 0, 100);
            progressBarYUp.Value = Clamp(yValue, 0, 100);
            progressBarYDown.Value = 100 - Clamp(yValue, 0, 100);
            progressBarRzLeft.Value = Clamp(rzValue, 0, 100);
            progressBarRzRight.Value = 100 - Clamp(rzValue, 0, 100);
            progressBarThrottle.Value = Clamp(throttleValue, 0, 100); // Schubregler ProgressBar
        }


        private void clearControls() 
        {
            lblXValue.Text = "X: " + "0";
            lblYValue.Text = "Y: " + "0";
            lblRZValue.Text = "RZ: " + "0";
            lblThrottleValue.Text = "Throttle: " + "0";
            progressBarXLeft.Value = 0;
            progressBarXRight.Value = 0;
            progressBarYUp.Value = 0;
            progressBarYDown.Value = 0;
            progressBarThrottle.Value = 0;
            progressBarRzLeft.Value = 0;
            progressBarRzRight.Value = 0;
            chkButtonState1.Checked = false;
            chkButtonState2.Checked = false;
        }


        int ScaleJoystickValue(int joystickValue, int joystickMax, int progressBarMax)
        {
            return (joystickValue * progressBarMax) / joystickMax;
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        // Stellen Sie sicher, dass Sie alle Ressourcen beim Beenden des Programms freigeben
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (joystick != null)
            {
                joystick.Unacquire();
                joystick.Dispose();
            }
            directInput?.Dispose();
            base.OnFormClosing(e);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer.Start();
            if (joystick != null)
            {
                joystick.Acquire();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer.Stop();
            if (joystick != null)
            {
                joystick.Unacquire();
                clearControls();
            }
        }
    }
}
