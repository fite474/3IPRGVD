using ClientApp.Bike;
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

namespace ClientApp.Gui
{
    public partial class PatientTestStart : Form
    {
        private BikeConnection bikeConnection;


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
            new Thread(bikeConnection.RunBikeLoop).Start();
        }
    }
}
