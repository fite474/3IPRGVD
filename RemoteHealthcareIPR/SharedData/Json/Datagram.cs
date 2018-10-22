using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData.Json
{
    public class Datagram
    {
        public DataType DataType { get; set; }
        public dynamic Data { get; set; }
    }
}
