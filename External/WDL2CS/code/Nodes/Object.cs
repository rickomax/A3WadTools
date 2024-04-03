using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Object : Node, ISection
    {
        private static readonly string s_indent = "\t\t";
        private static readonly string s_indentInit = "\t\t\t";
        private static readonly string s_nl = Environment.NewLine;

        private string m_name;
        private string m_type;
        private readonly bool m_isString;
        private readonly bool m_isInitialized;
        private List<Property> m_properties;
        private readonly string m_string;
        private Object m_way; //"inlined" Way object

        public string Name { get => m_name; set => m_name = value; }
        public string Type { get => m_type; }

        public Object(string type, string name, bool isInitialized, bool isString)
        {
            m_type = type;
            m_name = name;
            m_isString = isString;
            m_properties = new List<Property>();
            m_isInitialized = isInitialized;
        }

        public Object() : this(string.Empty, string.Empty) { }
        public Object(string type, string name) : this(type, name, false, false) { }


        public Object(string type, string name, bool isInitialized, List<Property> properties) : this(type, name, isInitialized, false)
        {
            if (properties != null)
            {
                m_properties.AddRange(properties);
            }
        }

        public Object(string type, string name, bool isInitialized, string str) : this(type, name, isInitialized, true)
        {
            m_string = str;
        }


        public bool IsInitialized()
        {
            return m_isInitialized;
        }

        public void Format(StringBuilder sb, bool skipProperties)
        {
            m_type = Formatter.FormatReserved(m_type);
            if (m_isInitialized) //format predefined Skill
                m_name = Formatter.FormatSkill(m_name);
            else
                m_name = Formatter.FormatObjectId(m_name);

            string properties = string.Empty;
            bool isSkill = false;
            //string carries text instead of properties
/*
            if (m_isString)
                properties = m_string;
            else
                properties = ProcessProperties();
*/
            string scope = "public static ";

            switch (m_type)
            {
                case "Synonym":
                    properties = ProcessProperties();
                    sb.Append(s_indent + scope + properties + ";");
                    break;

                case "String":
                    properties = m_string; //string carries text instead of properties
                    sb.Append(s_indent + scope + "string " + m_name);
                    if (!string.IsNullOrEmpty(properties))
                        sb.Append(" = " + properties);
                    sb.Append(";");
                    break;

                case "Skill":
                    isSkill = true;
                    goto default;

                default:
                    if (isSkill || !skipProperties) //Skills always need their properties and thus must not be skipped
                        properties = ProcessProperties(); //must be called early - will find "inlined" way definitions

                    //if "inlined" Way definition was found earlier, prepend Way definition here 
                    if (m_way != null)
                    {
                        Console.WriteLine("NOW FORMATTING " + m_way.Name);
                        m_way.Format(sb, skipProperties);
                        sb.AppendLine();
                    }
                    //Ways never have properties, but need to be instantiated nonetheless
                    //if (!string.IsNullOrEmpty(props) || string.Compare(type, "Way", true) == 0 || string.Compare(type, "Skill", true) == 0)
                    {
                        string indent = s_indent;
                        if (!IsInitialized())
                        {
                            indent = s_indent;
                            sb.Append(indent + scope + m_type + " ");
                        }
                        else
                        {
                            indent = s_indentInit;
                            sb.Append(indent);
                        }

                        sb.Append(m_name + " = new " + m_type);
                        if (isSkill) //Skills are treated like variable instances, thus no name identifier is passed on construction
                            sb.Append("()");
                        else
                            sb.Append("(" + Formatter.FormatToString(m_name) + ")");

                        if (!string.IsNullOrEmpty(properties))
                        {
                            sb.Append(s_nl + indent + "{");
                            sb.Append(properties + s_nl);
                            sb.Append(indent + "}");
                        }
                        sb.Append(";");
                    }
                    break;
            }
        }

        public void ToList(PropertyList list)
        {
            string type = Formatter.FormatReserved(m_type);
            switch (type)
            {
                case "Synonym":
                case "String":
                case "Skill":
                    break; //ignore - these objects are not exported in property list

                default:
                    string name = Formatter.FormatObjectId(m_name);
                    var item = list.AddItem(type, name);
                    //sort properties alphabetically
                    List<Property> properties = m_properties.OrderBy(x => x.Name).ToList(); //TODO: sort AFTER formatting (as previously)
                    foreach (Property property in properties)
                    {
                        list.AddProperty(item, property.Name, property.FormatList(), property.AllowMerge, property.AllowMultiple);
                    }
                    break;
            }
        }

        private string ProcessProperties()
        {
            //sort properties alphabetically
            List<Property> properties = m_properties.OrderBy(x => x.Name).ToList(); //TODO: sort AFTER formatting (as previously)

            ObjectData objectData = new ObjectData();
            StringBuilder sb = new StringBuilder();

            foreach (Property property in properties)
            {
                //add property to active dataset
                AddProperty(property, objectData.Properties);
            }

            //Synonyms need special treatment: convert from WDL object with properties to C# object reference
            if (m_type.Equals("Synonym"))
                ProcessSynonym(objectData);
            else
                ProcessProperties(objectData);

            //copy standard properties to output
            sb.Append(objectData.PropertyStream);

            //handle palette range definitions
            if (objectData.RangeStream.Length > 0)
            {
                sb.Append(s_nl + s_indent + "\tRange = new[,]" + s_nl);
                sb.Append(s_indent + "\t{" + objectData.RangeStream);
                sb.Append(s_nl + s_indent + "\t}");
            }

            //handle UI control definitions
            if (objectData.ControlStream.Length > 0)
            {
                sb.Append(s_nl + s_indent + "\tControls = new UIControl[]" + s_nl);
                sb.Append(s_indent + "\t{ " + objectData.ControlStream);
                sb.Append(s_nl + s_indent + "\t}");
            }

            return sb.ToString();
        }

        private void ProcessSynonym(ObjectData objectData)
        {
            //Current implementation is not compatible with preprocessor directives - most likely not relevant for any A3 game ever created
            Property property;
            //workaround: build synonym definition in property
            //Type declares datatype of Synonym
            property = objectData.Properties.Where(x => x.Name.Equals("Type")).FirstOrDefault();
            if (property != null)
            {
                string synType = Formatter.FormatReserved(property.Values[0]);
                //use C# strings -> convert to "string"
                if (synType.Equals("String"))
                    synType = "string";
                //"Action" keyword is reserved in C# -> use "Function" instead (mandatory)
                if (synType.Equals("Action"))
                    synType = "Function";
                //Scripts don't distinguish between object types and just use BaseObject which just carries all properties - like WDL
                //Trying to keep this more strict results in all kind of type problems in WDL actions
                if (synType.Equals("Wall") || synType.Equals("Thing") || synType.Equals("Actor"))
                    synType = "BaseObject";

                objectData.PropertyStream.Append(synType + " " + m_name);

                //Default declares default assignment of Synonym (optional)
                property = objectData.Properties.Where(x => x.Name.Equals("Default")).FirstOrDefault();
                if (property != null)
                {
                    if (!property.Values[0].Equals("null"))
                    {
                        objectData.PropertyStream.Append(" = " + Formatter.FormatIdentifier(property.Values[0]));
                    }
                }
            }
        }

        private void ProcessProperties(ObjectData objectData)
        {
            //initialized object definitions need different indention
            string indent = s_indent;
            if (IsInitialized())
                indent = s_indentInit;

            foreach (Property property in objectData.Properties)
            {
                switch (property.Name)
                {
                    case "Range":
                        objectData.RangeStream.Append(s_nl);
                        objectData.RangeStream.Append(indent + "\t\t");
                        objectData.RangeStream.Append(property.Format(m_type));
                        objectData.RangeStream.Append(",");
                        break;

                    case "Digits":
                    case "Hbar":
                    case "Vbar":
                    case "Hslider":
                    case "Vslider":
                    case "Picture":
                    case "Window":
                    case "Button":
                        objectData.ControlStream.Append(s_nl);
                        objectData.ControlStream.Append(indent + "\t\t");
                        objectData.ControlStream.Append(property.Format(m_type));
                        objectData.ControlStream.Append(",");
                        break;

                    case "Way":
                        //Ways can be "inlined" in object definitions
                        //if way is not yet defined, create and register it outside of serialized parser stream
                        //during formatting, formatted way object will be printend along with this object
                        if (!Registry.Is("Way", property.Values[0]))
                        {
                            Console.WriteLine("(I) OBJECT add missing Way definition for: " + property.Values[0]);
                            m_way = new Object("Way", property.Values[0]);
                            Registry.Register("WAY", property.Values[0]);
                        }
                        goto default;

                    case "Flags":
                    default:
                        objectData.PropertyStream.Append(s_nl);
                        objectData.PropertyStream.Append(indent + "\t");
                        objectData.PropertyStream.Append(property.Format(m_type));
                        objectData.PropertyStream.Append(",");
                        break;
                }
            }
        }

        private void AddProperty(Property property, List<Property> properties)
        {
            List<string> propertyNames = properties.Select(x => x.Name).ToList();
            if (propertyNames.Contains(property.Name))
            {
                //Eliminate double definitions of properties only where their values can be merged
                if (property.AllowMerge)
                {
                    int i = propertyNames.IndexOf(property.Name);
                    properties[i].Values.AddRange(property.Values);
                }
                else if (property.AllowMultiple)
                {
                    properties.Add(property);
                }
                else
                {
                    //TODO: take first or last definition?
                    Console.WriteLine("(W) OBJECT ignore double definition of property: " + property.Name);
                }
            }
            else
            {
                properties.Add(property);
            }
        }
    }
}
