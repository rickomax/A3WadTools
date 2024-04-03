using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Assets
    {
        private static List<string> s_parameters = new List<string>();
        public static Node AddAsset(Node type, Node name, Node file)
        {
            string stype = type.ToString();
            string sname = name.ToString();
            string sfile = file.ToString();
            Registry.Register(stype, sname);
            Node a = new Asset(stype, sname, sfile, s_parameters);

            //Clean up
            s_parameters.Clear();

            return a;
        }

        public static Node AddParameter(Node value)
        {
            s_parameters.Insert(0, value.ToString());
            return null;
        }

    }
}
