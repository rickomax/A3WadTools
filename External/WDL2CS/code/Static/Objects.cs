using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Objects
    {
        private static List<string> s_values = new List<string>();
        private static List<Property> s_properties = new List<Property>();

        public static Node AddStringObject(Node type, Node name, Node text)
        {
            string stype = type.ToString();
            string sname = name.ToString();
            Registry.Register(stype, sname);
            Node o = new Object(stype, sname, false, text.ToString());

            return o;
        }

        public static Node AddObject(Node type, Node name)
        {
            string stype = type.ToString();
            string sname = name.ToString();
            bool initialize = false;
            //Exclude predefined skills
            if (!(stype.Equals("skill", StringComparison.OrdinalIgnoreCase) && Identifier.IsSkill(ref sname)))
                Registry.Register(stype, sname);
            else
                initialize = true; //make sure predefined skills are moved to init section 

            Node o = new Object(stype, sname, initialize, s_properties);
            //Clean up
            s_properties.Clear();

            return o;
        }

        public static Node CreateProperty(Node property)
        {
            string sproperty = property.ToString();
            if (Identifier.IsProperty(ref sproperty) || Identifier.IsFlag(ref sproperty))
            {
                Property prop = new Property(sproperty, s_values);
                s_properties.Add(prop);
            }
            else
            {
                Console.WriteLine("(W) OBJECTS discarded invalid property: " + property);
            }
            //Clean up
            s_values.Clear();

            return null;
        }

        public static Node AddPropertyValue(Node value)
        {
            s_values.Insert(0, value.ToString());
            return null;
        }

    }
}
