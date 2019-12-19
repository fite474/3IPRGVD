using SharedData.Helpers;
﻿using ClientServerUtil;
using Newtonsoft.Json;
using SharedData.Data;
using SharedData.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DoctorApp
{
    public partial class DocForm : Form
    {
        bool doc = true;
        ServerConnection connection;
        public delegate void ReceiveResponse(Datagram jsonResponse);
        public event ReceiveResponse OnReceiveResponse;



        public DocForm()
        {
            InitializeComponent();
            connection = new ServerConnection(doc);
            connection.OnReceiveResponse += ResponseHandler;
            OnReceiveResponse += handleResponse;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //public static void VerifyDir(string path)
        //{
        //    try
        //    {
        //        DirectoryInfo dir = new DirectoryInfo(path);
        //        if (!dir.Exists)
        //        {
        //            dir.Create();
        //        }
        //    }
        //    catch { }
        //}

        //public static void Logger(dynamic receivedData)
        //{
        //    string lines = JsonConvert.SerializeObject(receivedData);
        //    string path = "C:/IPR_LOG/";
        //    VerifyDir(path);
        //    string fileName = "Astrand_Test_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + "_Logs.txt";
        //    try
        //    {
        //        System.IO.StreamWriter file = new System.IO.StreamWriter(path + fileName, true);
        //        file.WriteLine(DateTime.Now.ToString() + ": " + lines);
        //        file.Close();
        //    }
        //    catch (Exception) { }
        //}

        public void handleResponse(Datagram receivedData)
        {
            switch (receivedData.DataType)
            {
                case DataType.SessionSnapshot:
                    {
                        setDoctorValues(receivedData.Data);
                        break;
                    }
                //case DataType.LogData:
                //    {
                //        Logger(receivedData.Data);
                //        break;
                //    }
            }
        }




        private void DocForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("c:\\");
            Datagram datagram = new Datagram();
            datagram.DataType = DataType.StartSession;
            connection.SendData(datagram);
        }

        private void ResponseHandler(Datagram jsonResponse)
        {
            Task.Run(() => OnReceiveResponse?.Invoke(jsonResponse));
        }

        private void setDoctorValues(dynamic data)
        {
            int xAs = 0;
            int rpm = data.Rpm;
            int heartbeat = data.HeartRate;

            this.ActionInvoke(() =>
            {
                rpmLabel.Text = ("rpm: " + rpm);
                hearthbeatLabel.Text = ("heart rate: " + heartbeat);
                

                chart1.Series["rpm"].Points.AddXY(xAs, rpm);
                chart1.Series["heartbeat"].Points.AddXY(xAs, heartbeat);
            });

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("C:\\Users\\mauri\\Documents\\1AVANS\\Jaar 2 Herkansingen\\P2.1\\RH IPR\\3IPRGVD\\RemoteHealthcareIPR\\ServerApp\\bin\\Debug\\ClientDataLogs");
        }
    }
}
