using System;
using System.Linq;
using System.Windows.Forms;
using SharpDX.DirectInput;

namespace JoystickTester
{
    public partial class JoystickTester : Form
    {
        private DirectInput directInput = new DirectInput();
        private Joystick joystickThrottle;
        private Joystick joystickJoystick;

        public JoystickTester()
        {
            InitializeComponent();
            InitControls();
        }

        private void InitControls()
        {
            button1.Text = "Geräte aktualisieren";
            button1.Click += (sender, e) =>
            {
                comboBox1.Items.Clear();

                // Suche nach Throttle-Geräten
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                    comboBox1.Items.Add(deviceInstance.InstanceName);

                // Suche nach Joystick-Geräten
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                    comboBox1.Items.Add(deviceInstance.InstanceName);
            };

            // ... (Ihre anderen Button-Click-Events)

            button2.Click += (sender, e) =>
            {
                if (comboBox1.SelectedItem == null)
                {
                    MessageBox.Show("Bitte ein Gerät auswählen.");
                    radioButton1.Checked = true;
                    return;
                }

                // Unterscheide zwischen Throttle und Joystick anhand des ausgewählten Geräts
                var selectedDeviceGuid = new Guid(comboBox1.SelectedItem.ToString());

                // Suche nach Throttle-Geräten
                var devicesThrottle = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
                var deviceInstanceThrottle = devicesThrottle.FirstOrDefault(device => device.InstanceGuid == selectedDeviceGuid);

                // Suche nach Joystick-Geräten
                var devicesJoystick = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
                var deviceInstanceJoystick = devicesJoystick.FirstOrDefault(device => device.InstanceGuid == selectedDeviceGuid);

                if (deviceInstanceThrottle != null)
                {
                    joystickThrottle = new Joystick(directInput, selectedDeviceGuid);
                    joystickThrottle.Acquire();
                }
                else if (deviceInstanceJoystick != null)
                {
                    joystickJoystick = new Joystick(directInput, selectedDeviceGuid);
                    joystickJoystick.Acquire();
                }

                // Initialisiere und starte Timer
                Timer timer = new Timer();
                timer.Interval = 100;
                timer.Tick += (s, ev) =>
                {
                    // Aktualisiere Zustand für Throttle
                    if (joystickThrottle != null)
                    {
                        joystickThrottle.Poll();
                        var stateThrottle = joystickThrottle.GetCurrentState();
                        // ... (Aktualisieren Sie die Anzeige für Throttle nach Bedarf)
                    }

                    // Aktualisiere Zustand für Joystick
                    if (joystickJoystick != null)
                    {
                        joystickJoystick.Poll();
                        var stateJoystick = joystickJoystick.GetCurrentState();
                        // ... (Aktualisieren Sie die Anzeige für Joystick nach Bedarf)
                    }
                };
                timer.Start();
            };

            button3.Text = "Stop";
            button3.Click += (sender, e) =>
            {
                if (joystickThrottle != null && joystickJoystick != null)
                {
                    joystickJoystick.Unacquire();
                    joystickThrottle.Unacquire();
                    joystickJoystick = null;
                    joystickThrottle = null;
                }
            };
        }




        private int NormalizeJoystickValue(int value)
        {
            return (value + 32768) / 655;  // Ändert den Wertebereich von -32768...32767 auf 0...1000
        }




        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            // Suche nach Throttle-Geräten
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                comboBox1.Items.Add(deviceInstance.InstanceName);

            // Suche nach Joystick-Geräten
            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
                comboBox1.Items.Add(deviceInstance.InstanceName);
        }




        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Bitte ein Gerät auswählen.");
                radioButton1.Checked = true;
                return;
            }

            // Überprüfen Sie, ob die ausgewählte Zeichenfolge das erwartete Format hat
            string selectedDeviceString = comboBox1.SelectedItem.ToString();
            if (!IsValidGuidFormat(selectedDeviceString))
            {
                MessageBox.Show("Ungültiges Format für die ausgewählte GUID.");
                return;
            }

            // Versuchen Sie dann, die GUID zu parsen
            if (!Guid.TryParse(selectedDeviceString, out var selectedDeviceGuid))
            {
                MessageBox.Show("Ungültiges Format für die ausgewählte GUID.");
                return;
            }


            // Suche nach Throttle-Geräten
            var devicesThrottle = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
            var deviceInstanceThrottle = devicesThrottle.FirstOrDefault(device => device.InstanceGuid == selectedDeviceGuid);

            // Suche nach Joystick-Geräten
            var devicesJoystick = directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices);
            var deviceInstanceJoystick = devicesJoystick.FirstOrDefault(device => device.InstanceGuid == selectedDeviceGuid);

            if (deviceInstanceThrottle != null)
            {
                joystickThrottle = new Joystick(directInput, selectedDeviceGuid);
                joystickThrottle.Acquire();
            }
            else if (deviceInstanceJoystick != null)
            {
                joystickJoystick = new Joystick(directInput, selectedDeviceGuid);
                joystickJoystick.Acquire();
            }

            // Initialisiere und starte Timer
            Timer timer = new Timer();
            timer.Interval = 100;
            timer.Tick += (s, ev) =>
            {
                // Aktualisiere Zustand für Throttle
                if (joystickThrottle != null)
                {
                    joystickThrottle.Poll();
                    var stateThrottle = joystickThrottle.GetCurrentState();
                    // ... (Aktualisieren Sie die Anzeige für Throttle nach Bedarf)
                }

                // Aktualisiere Zustand für Joystick
                if (joystickJoystick != null)
                {
                    joystickJoystick.Poll();
                    var stateJoystick = joystickJoystick.GetCurrentState();
                    // ... (Aktualisieren Sie die Anzeige für Joystick nach Bedarf)
                }
            };
            timer.Start();
        }
        private bool IsValidGuidFormat(string input)
        {
            try
            {
                Guid.Parse(input);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
