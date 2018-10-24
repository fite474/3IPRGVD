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
            InitializeComponent();
            instrucitonsLabel.Text = "U gaat fietsen met een snelheid van ongeveer 60 omwentelingen per min.\n U krijgt eerst een korte warming-up van 2 min.\n Hierna wordt (eventueel in korte stappen) de beoogde testbelasting ingesteld.\n De test duurt 4 minuten met daarna een cooling down. ";
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

        public void setPhaseLabel(string currentPhase)
        {
            this.ActionInvoke(() =>
            {
                faseLabel.Text = currentPhase;
            });
        }

        public void setHeartbeatLabel(string currentHeartbeat)
        {
            this.ActionInvoke(() =>
            {
                currentHearbeatLabel.Text = currentHeartbeat;
            });
        }

        public void setSteadyStateLabel(bool state)
        {
            this.ActionInvoke(() =>
            {
                if (state)
                { steadyStateLabel.Text = ("steady state: true"); }
                else { steadyStateLabel.Text = ("steady state: false"); }
                
            });
        }

        public void setHeartbeatLabelvisible()
        {
            this.ActionInvoke(() =>
            {
                steadyStateLabel.Visible = true;
            });
        }

        public void setPower(string currentHeartbeat)
        {
            this.ActionInvoke(() =>
            {
                label1.Text = currentHeartbeat;
            });
        }

        private void PatientTestInstructions_Load(object sender, EventArgs e)
        {

        }

        private void faseLabel_Click(object sender, EventArgs e)
        {

        }

        private void currentHearbeatLabel_Click(object sender, EventArgs e)
        {

        }

        private void timeLabel_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
