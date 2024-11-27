using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EurofighterInformationCenter
{
    public partial class InformationCenterDataView : Form
    {
        public InformationCenterDataView()
        {
            InitializeComponent();
        }

        private void InformationCenterDataView_Load(object sender, EventArgs e)
        {
            MessageBox.Show("DataCenter Geladen");
            Screen screen = Screen.PrimaryScreen;
            int width = screen.Bounds.Width;
            int height = screen.Bounds.Height;
            //this.Width = width;
            //this.Height = height;
        }


        public void moveToScreen(int screenNumber)
        {
            //try
            //{
                Screen screen = Screen.AllScreens[screenNumber];
                this.Location = screen.WorkingArea.Location;
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Error during moving of window");
            //}
        }


        public void DisplayContent() 
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
