using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedData.Helpers
{
    public class IdHelper
    {

        private static object lockObj = new object();
        public const string IdFile = "IdFile.conf";
        public static int GetIncrementedId()
        {
            lock (lockObj)
            {
                if (!File.Exists(IdFile))
                {

                    File.WriteAllText(IdFile, "0");
                    return 0;
                }
                else
                {
                    int incremented = int.Parse(File.ReadAllText(IdFile)) + 1;
                    File.WriteAllText(IdFile, incremented.ToString());
                    return incremented;
                }
            }
        }


    }
}
