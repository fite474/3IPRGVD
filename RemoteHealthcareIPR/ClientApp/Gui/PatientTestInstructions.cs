using DoctorApp;
using SharedData.Data;
using SharedData.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace PatientApp.Gui
{
    public partial class PatientTestInstructions : Form
    {
        public PatientTestInstructions()
        {
            ServerConnection connection = new ServerConnection();
            connection.OnReceiveResponse += handleResponse;
            InitializeComponent();
        }

        public void setTimeLabel(string time) {
            this.ActionInvoke(() =>
            {
                timeLabel.Text = time;
            });
        }

        public void setInstructionLabel(string instructions)
        {
            this.ActionInvoke(() =>
            {
                instructionLabelRPM.Text = instructions;
            });
        }

        private void PatientTestInstructions_Load(object sender, EventArgs e)
        {

        }

        public void handleResponse(SessionSnapshot snap)
        {
            //recievedata and start if correct
        }
    }
}
