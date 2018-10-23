using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorApp
{
    class DoctorProgram
    {
        public static void Main(string[] args)
        {
            new DoctorProgram();
        }
        
        public DoctorProgram()
        {
            DocForm form = new DocForm();
            form.ShowDialog();
        }
    }
}
