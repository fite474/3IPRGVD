using PatientApp.Bike;
using PatientApp;
using PatientApp.Bike;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatientApp.Gui
{
    public partial class PatientTestStart : Form
    {
        private BikeConnection bikeConnection;
        public string Age { get; set; }
        public string Weight { get; set; }
        public string Doctor { get; set; }


        public PatientTestStart(BikeConnection bikeConnection)
        {
            InitializeComponent();
            this.bikeConnection = bikeConnection;
            bikeConnection.FindBike();
            
        }


        private void PatientGui_Load(object sender, EventArgs e)
        {

        }

        private void startTestButton_Click(object sender, EventArgs e)
        {
            bikeConnection.Age = Age;
            bikeConnection.Weight = Weight;
            this.Hide();
            
            new Thread(bikeConnection.RunTestGUI).Start();
            
            
            
        }

        private void ageTextbox_TextChanged(object sender, EventArgs e)
        {
            Age = ageTextbox.Text;
        }

        private void weightTextbox_TextChanged(object sender, EventArgs e)
        {
            Weight = weightTextbox.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Doctor = textBox1.Text;
        }
    }
}
