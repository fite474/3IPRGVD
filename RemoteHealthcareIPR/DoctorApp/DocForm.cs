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

        public void handleResponse(Datagram snap)
        {
            //setDataInUI
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
    }
}
