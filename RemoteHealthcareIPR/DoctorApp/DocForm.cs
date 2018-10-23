using SharedData.Data;
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
        bool first = false;

        public DocForm()
        {
            InitializeComponent();
            ServerConnection connection = new ServerConnection();
            connection.OnReceiveResponse += handleResponse;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void handleResponse(SessionSnapshot snap)
        {
            //setDataInUI
        }

        private void DocForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
