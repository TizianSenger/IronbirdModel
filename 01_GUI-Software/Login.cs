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
    public partial class Login : Form
    {
        public InformationCenterMainPage informationCenterMainPage;
        
        public Login()
        {
            InitializeComponent();
            informationCenterMainPage = new InformationCenterMainPage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string passwort = textBox2.Text;

            if (username == "Admin" & passwort == "Eurofighter")
            {
                informationCenterMainPage.configSettingsAccess();
            }
            else 
            {
                MessageBox.Show("Nutzername oder Passwort ist falsch");
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }
    }
}
