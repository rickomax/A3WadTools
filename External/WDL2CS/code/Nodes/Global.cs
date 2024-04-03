using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Global : Node, ISection
    {
        private static readonly string s_indent = "\t\t\t";
        private string m_name;
        private List<string> m_parameters;

        public string Name { get => m_name; set => m_name = value; }
        public string Type { get => "Global"; }
        public List<string> Parameters { get => m_parameters; set => m_parameters = value; }

        public Global()
        {
            m_name = string.Empty;
            m_parameters = new List<string>();
        }

        public Global(string name, string parameter) : this(name, new string[] { parameter }.ToList()) { }
        public Global(string name, List<string> parameters) : this()
        {
            m_name = name;
            if (parameters != null)
                m_parameters.AddRange(parameters);
        }

        public bool IsInitialized()
        {
            return true;
        }

        public void Format(StringBuilder sb, bool skipProperties)
        {
            bool forceMulti = false;
            //identify data type for array definition
            string type = string.Empty;
            if (m_name.Contains("Each_"))
            {
                type = "Function";
                forceMulti = true;
            }
            if (m_name.Contains("Panels"))
            {
                type = "Panel";
                forceMulti = true;
            }
            if (m_name.Contains("Layers"))
            {
                type = "Overlay";
                forceMulti = true;
            }
            if (m_name.Contains("Messages"))
            {
                type = "Text";
                forceMulti = true;
            }

            if (m_parameters.Count > 1 || forceMulti)
            {
                //make sure parameter list is extended to 16
                int count = m_parameters.Count;
                for (int i = count; i < 16; i++)
                {
                    m_parameters.Add(Formatter.FormatNull());
                }

                string parameters = string.Join(", ", m_parameters);
                sb.Append(s_indent + m_name + " = new " + type + "[] {" + parameters + "};");
            }
            else
            {
                string parameter = m_parameters[0].ToString();
                //Patch for video mode definition
                if (m_name.Contains("Video"))
                    parameter = Formatter.FormatVideo(parameter);
                sb.Append(s_indent + m_name + " = " + parameter + ";");
            }
        }

        public void ToList(PropertyList list)
        {
            //not supported by Global
        }

    }
}
