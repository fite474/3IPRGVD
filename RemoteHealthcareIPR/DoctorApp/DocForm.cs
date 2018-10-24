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

        public void handleResponse(Datagram receivedData)
        {
            switch (receivedData.DataType)
            {
                case DataType.SessionSnapshot:
                    {
                        setDoctorValues(receivedData.Data);
                        break;
                    }
            }
        }

        private void DocForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
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
    }
}
