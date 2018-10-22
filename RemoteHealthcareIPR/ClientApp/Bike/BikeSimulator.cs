using SharedData.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientApp.Bike
{
    class BikeSimulator : IBike
    {
        SessionSnapshot bikeData = new SessionSnapshot();

        private CommandSet commSet;
        public BikeSimulator()
        {
            commSet = new CommandSet();

        }

        public void Close()
        {

        }

        public SessionSnapshot ReadData()
        {
            string commandString = commSet.St();

            bikeData = BikeDataHelper.AssignValues(commandString, 0);
            return bikeData;
        }

        /*
        public string PutCommand(string command)
        {

            string[] parameters = command.Split(' ');
            int evaluatedParameter = 0;
            switch (parameters[0].ToLower())
            {
                case "cm":
                    return commSet.Cm();

                case "cd":
                    return commSet.Cd();

                case "rs":
                    return commSet.Rs();

                case "ve":
                    return commSet.Ve();

                case "st":
                    return commSet.St();

                case "id":
                    return commSet.Id();
                case "rp":
                    return commSet.Rp();

                case "pd":
                    // there have to be to parameters for this to work #notfancy
                    if (parameters.Length == 2)
                    {
                        evaluatedParameter = int.Parse(parameters[1]);
                        // the range of the input
                        if (evaluatedParameter >= 0 && evaluatedParameter <= 999)
                        {
                            return commSet.Pd(evaluatedParameter);
                        }
                        else
                        {
                            return "ERROR";
                        }
                    }
                    else
                    {
                        return "ERROR";
                    }

                case "pt":
                    if (parameters.Length == 2)
                    {
                        evaluatedParameter = int.Parse(parameters[1]);
                        if (evaluatedParameter > -1 && evaluatedParameter < 9960)
                        {
                            return commSet.Pt(int.Parse(parameters[1]));
                        }
                        else
                        {
                            return "ERROR";
                        }
                    }
                    else
                    {
                        return "ERROR";
                    }

                case "pw":
                    if (parameters.Length == 2)
                    {
                        evaluatedParameter = int.Parse(parameters[1]);
                        //Console.WriteLine(evaluatedParameter);
                        if (evaluatedParameter > 24 && evaluatedParameter < 400)
                        {
                            return commSet.Pd(evaluatedParameter);
                        }
                        else
                        {
                            return "ERROR1";
                        }
                    }
                    else
                    {
                        return "ERROR2";
                    }

                default:
                    return "ERROR";
            }

        }
        */
        public void PutDistance(int distance)
        {
            commSet.Cd();
            commSet.Pd(distance);
        }

        public void PutPower(int power)
        {
            commSet.Cd();
            commSet.Pw(power);

        }

        public void PutTime(int time)
        {
            commSet.Cd();
            int seconds = time % 60;
            int minutes = (time - seconds) / 60;

            bikeData.Time = $"{minutes:D2}:{seconds:D2}";

            commSet.Pt(time);
        }

        public void Change(int hr, int spd, int en)
        {
            commSet.Change(hr, spd, en);
        }




        private class CommandSet
        {
            private double preciseDistance;
            private string heartrate;          //Hz
            private string rpm;                //Rounds per minute
            private string speed;              //km/h
            private string distance;           //100m
            private string power;              //watt
            private string energy;             //KJ
            private string time;               //mm:ss
            private string deliveredPower;     //watt
            private string deviceId = "Hometrainer1";
            private string firmwareId = "200";
            private bool commandBit = false;
            private bool resetBit = false;


            public CommandSet()
            {
                Random random = new Random();

                heartrate = random.Next(60, 100).ToString();
                rpm = random.Next(30, 80).ToString();
                speed = random.Next(150, 324).ToString();
                distance = (random.Next(1, 30) * 1000).ToString();
                preciseDistance = double.Parse(distance);
                power = (25 + random.Next(0, 20) * 5).ToString();
                energy = random.Next(20, 300).ToString();
                time = $"{random.Next(2, 59)}:{random.Next(2, 59).ToString()}";
                deliveredPower = random.Next(40, 150).ToString();
            }

            public string Rs()
            {
                resetBit = !resetBit;
                return "ACK";
            }

            public string Cd()
            {
                commandBit = !commandBit;
                return "ACK";
            }

            public string Cm()
            {
                commandBit = true;
                return "ACK";
            }

            public string St()
            {
                return $"{heartrate}\t{rpm}\t{speed}\t{distance}\t{power}\t{energy}\t{time}\t{deliveredPower}";
            }

            public string Rp()
            {
                return "PROGRAM ::";
            }

            public string Id()
            {
                return deviceId;
            }

            public string Ve()
            {
                return firmwareId;
            }

            public string Pd(int distance)
            {
                if (!commandBit)
                {
                    return "ERROR";
                }
                this.distance = distance.ToString();
                return St();
            }

            public string Pt(int time)
            {
                if (!commandBit)
                {
                    return "ERROR";
                }
                int seconds = time % 60;
                int minutes = (time - seconds) / 60;

                this.time = $"{minutes:D2}:{seconds:D2}";
                return St();
            }

            public string Pw(int power)
            {
                if (!commandBit)
                {
                    return "ERROR";
                }
                this.power = power.ToString();
                return St();
            }

            public void Change(int hr, int spd, int en)
            {
                heartrate = (int.Parse(heartrate) + hr).ToString();
                speed = (int.Parse(speed) + spd).ToString();
                energy = (int.Parse(energy) + en).ToString();
                rpm = (int.Parse(rpm) + spd / 2).ToString();

                preciseDistance = preciseDistance - double.Parse(speed) * 1000 / 36000;
                distance = ((int)Math.Round(preciseDistance)).ToString();
            }


        }
    }
}
