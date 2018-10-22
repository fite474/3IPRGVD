using PatientApp.Bike;
using PatientApp.Gui;
using PatientApp.Bike;
using SharedData.Data;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatientApp
{
    public class PatientProgram
    {
        
        static void Main(string[] args)
        {
            new PatientProgram();
        }

        public PatientProgram()
        {

            BikeConnection bikeConnection = new BikeConnection();



            PatientTestStart patientGui = new PatientTestStart(bikeConnection);
            Application.EnableVisualStyles();
            Application.Run(patientGui);

        }







    }
}
