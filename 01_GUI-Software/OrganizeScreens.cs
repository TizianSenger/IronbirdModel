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
    public partial class OrganizeScreens : Form
    {
        private InformationCenterMainPage informationCenterMainPage;
        private InformationCenterDataView informationCenterDataView;
        //private InformationCenterVisualisierung InformationCenterVisualisierung;
        
        public OrganizeScreens()
        {
            InitializeComponent();
            informationCenterDataView = new InformationCenterDataView();
            informationCenterMainPage = new InformationCenterMainPage();
        }

        private void OrganizeScreens_Load(object sender, EventArgs e)
        {
            //InformationCenterMainPage = new InformationCenterMainPage();
            //InformationCenterDataView = new InformationCenterDataView();
            //InformationCenterVisualisierung = new InformationCenterVisualisierung();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            informationCenterDataView.DisplayContent();

            try
            {
                var selectedWindow = comboBox1.SelectedItem;
                if (selectedWindow != null)
                {
                    switch (selectedWindow.ToString())
                    {
                        case "Information Center":
                            informationCenterMainPage.moveToScreen(0);
                            Screen screen = Screen.AllScreens[0];
                            informationCenterMainPage.Location = screen.WorkingArea.Location;
                            break;
                        case "Information Center DataView":
                            informationCenterDataView.moveToScreen(0);
                            break;
                        case "Information Center Visualisierung":
                            //InformationCenterVisualisierung.moveToScreen(0)                            
                            break;
                        default:
                            MessageBox.Show("No valid Option had been choosen");
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error choosing screen");
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                var selectedWindow = comboBox1.SelectedItem;
                if (selectedWindow != null)
                {
                    switch (selectedWindow.ToString())
                    {
                        case "Information Center":
                            informationCenterMainPage.moveToScreen(0);
                            break;
                        case "Information Center DataView":
                            informationCenterDataView.moveToScreen(0);
                            break;
                        case "Information Center Visualisierung":
                            //InformationCenterVisualisierung.moveToScreen(0)                            
                            break;
                        default:
                            MessageBox.Show("No valid Option had been choosen");
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error choosing screen");
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                var selectedWindow = comboBox1.SelectedItem;
                if (selectedWindow != null)
                {
                    switch (selectedWindow.ToString())
                    {
                        case "Information Center":
                            informationCenterMainPage.moveToScreen(2);
                            break;
                        case "Information Center DataView":
                            informationCenterDataView.moveToScreen(2);
                            break;
                        case "Information Center Visualisierung":
                            //InformationCenterVisualisierung.moveToScreen(2)                            
                            break;
                        default:
                            MessageBox.Show("No valid Option had been choosen");
                            break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error choosing screen");
            }
        }
    }
}
