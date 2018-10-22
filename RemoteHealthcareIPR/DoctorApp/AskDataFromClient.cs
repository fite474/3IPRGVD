using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp
{
    class AskDataFromClient
    {
        Session session;

        public AskDataFromClient()
        {
            session = new Session(DateTime.Now);
            ServerSimulation simulation = new ServerSimulation(session);
        }
    }
}
