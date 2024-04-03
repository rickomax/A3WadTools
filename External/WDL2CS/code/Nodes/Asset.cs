using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Asset : Node, ISection
    {
        private static readonly string s_indent = "\t\t";
        private readonly string m_type;
        private string m_name;
        private string m_file;
        private List<string> m_parameters;

        public string Type { get => m_type; }
        public string Name { get => m_name; set => m_name = value; }
        public string File { get => m_file; set => m_file = value; }
        public List<string> Parameters { get => m_parameters; }

        public Asset()
        {
            m_type = string.Empty;
            m_name = string.Empty;
            m_file = string.Empty;
            m_parameters = new List<string>();
        }

        public Asset(string type, string name, string file) : this(type, name, file, null) { }

        public Asset(string type, string name, string file, List<string> parameters) : this()
        {
            m_type = type;
            m_name = name;
            m_file = file;
            if (parameters != null)
                m_parameters.AddRange(parameters);
        }

        public bool IsInitialized()
        {
            return false;
        }

        public void Format(StringBuilder sb, bool skipProperties)
        {
            string scope = "public static ";
            string type = Formatter.FormatReserved(m_type);
            string name = Formatter.FormatObjectId(m_name);
            string nameStr = Formatter.FormatToString(name);

            sb.Append(s_indent + scope + type + " " + name);
            sb.Append(" = new " + type + "(" + nameStr);
            if (!skipProperties)
            {
                string pars = string.Empty;
                if (m_parameters != null && m_parameters.Count > 0)
                {
                    pars = ", " + string.Join(", ", m_parameters);
                }
                sb.Append(", " + m_file + pars);
            }
            sb.Append(");");
        }

        public void ToList(PropertyList list)
        {
            string type = Formatter.FormatReserved(m_type);
            string name = Formatter.FormatObjectId(m_name);
            string file = m_file.Trim('"');
            var item = list.AddItem(type, name);
            list.AddProperty(item, "File", m_file);
            if (m_parameters != null && m_parameters.Count > 0)
                list.AddProperty(item, "Options", m_parameters);
        }
    }
}
