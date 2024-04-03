using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Globals
    {
        static List<string> s_eventPars = new List<string>();

        public static Node AddGlobal(Node name)
        {
            string sname = name.ToString();
            Node g = new Global(sname, s_eventPars);

            //Clean up
            s_eventPars.Clear();

            return g;
        }

        public static Node AddGlobal(Node name, Node parameter)
        {
            string sname = name.ToString();
            string sparameter = parameter.ToString();
            Node g = new Node();
            //ignore Bind and Path statements
            switch (sname)
            {
                case "Globals.Bind":
                    Console.WriteLine("(I) GLOBALS ignore BIND definition: " + sparameter);
                    break;

                case "Globals.Path":
                    Console.WriteLine("(I) GLOBALS ignore PATH definition: " + sparameter);
                    break;

                default:
                    g = new Global(sname, sparameter);
                    break;
            }
            return g;
        }

        public static Node AddParameter(Node parameter)
        {
            string sparameter = parameter.ToString();
            sparameter = Formatter.FormatIdentifier(sparameter); //Events always take action references as parameter TODO: should this be here or outside?

            s_eventPars.Insert(0, sparameter);
            return null;
        }
    }
}
