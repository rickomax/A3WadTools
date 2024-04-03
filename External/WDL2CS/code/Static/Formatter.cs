using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Formatter
    {
        public static string FormatActionClass(string s)
        {
            return "Action__" + FormatObjectId(s);
        }

        //Targets are not detected specifically in the parser due to complexity limit
        //Therefore, hard code the different targets here
        private static string[] s_targets = new[] { "move", "bullet", "drop", "stick", "straight", "follow", "repel", "vertex", "node0", "node1", "hold", "place", "null" };
        public static string FormatActorTarget(string s)
        {
            if (s_targets.Contains(s))
                return "ActorTarget." + FormatReserved(s);
            else return s;
        }

        public static string FormatAssetIdRef(string asset)
        {
            //allow "null" for asset references
            if (asset.Equals("null"))
                return asset;
            else
                return FormatIdentifier(asset);
        }

        public static string FormatFile(string s)
        {
            s = s.ToLower();
            s = s.Replace('<', '"');
            s = s.Replace('>', '"');
            //Workaround for some very old wdl scripts where quotation marks for file names were omitted
            if (!s.StartsWith("@")) //properly detected strings are prefixed with @ - these never need patching
            {
                if (!s.StartsWith("\"")) s = "\"" + s;
                if (!s.EndsWith("\"")) s += "\"";
            }
            return s;
        }

        public static string FormatFlag(string flag)
        {
            //somewhat dirty workaround due to"base" identifier being patched to avoid clash with C# language
            //remove leading underscores and capitalize first char
            while (flag[0] == '_')
                flag = flag.Substring(1);

            string[] fragments = flag.Split('.');
            switch (fragments.Length)
            {
                case 1:
                    return "A3Flags." + FormatReserved(fragments[0]);

                case 2:
                    if (!fragments[1].Equals("Val"))
                        return "A3Flags." + FormatReserved(fragments[1]);
                    else
                        return "A3Flags." + FormatReserved(fragments[0]);

                case 3:
                    return "A3Flags." + FormatReserved(fragments[1]);

                default:
                    return string.Empty;
            }

        }

        public static string FormatGlobal(string s)
        {
            return "Globals." + FormatReserved(s);
        }

        public static string FormatGotoLabel(string marker)
        {
            //make sure any falsly added prefix is discarded (parser cannot distinguish properly for goto)
            string[] fragments = marker.Split('.');
            switch (fragments.Length)
            {
                case 1:
                    return FormatIdentifier(fragments[0]);

                case 2:
                    if (!fragments[1].Equals("Val"))
                        return FormatIdentifier(fragments[1]);
                    else
                        return FormatIdentifier(fragments[0]);

                case 3:
                    return FormatIdentifier(fragments[1]);

                default:
                    return string.Empty;
            }
        }

        public static string FormatGotoMarker(string s)
        {
            return FormatIdentifier(s) + ":";
        }

        //patch all identifiers conflicting with C# language (except for null)
        private static CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");

        public static string FormatIdentifier(string s)
        {
            //remove unknown and non-allowed characters
            s = s.Replace("-", "");
            s = s.Replace("?", "");
            s = s.Replace(".", "");
            s = s.ToLower();
            if (!provider.IsValidIdentifier(s) && !s.Equals("null"))
                s = "__" + s;
            return s;
        }

        public static string FormatList(string list)
        {
            string[] parts = list.Split('.');
            if (parts.Length == 2)
            {
                return "Globals." + FormatReserved(parts[0]) + "[" + (Convert.ToInt32(parts[1]) - 1) + "]";
            }
            else
            {
                return string.Empty;
            }
        }

        public static string FormatKeyword(string keyword)
        {
            //split property, if any
            string[] split = keyword.Split('.');
            string identifier = split[0];
            string formattedProperty = string.Empty;
            if (split.Length > 1)
            {
                formattedProperty = "." + FormatReserved(split[1]);
                //PATCH: Some properties are skills and needed to be accessed through ".Val"
                if (formattedProperty.Contains("Skill"))
                    formattedProperty += ".Val";
            }

            //known identifiers can override predefined keywords
            if (Registry.Identify(out string obj, identifier))
            {
                string formattedIdentifier = FormatIdentifier(identifier);
                if (obj.Equals("Skill") && string.IsNullOrEmpty(formattedProperty)) //skills must be accessed through property, defaults to ".Val"
                    formattedProperty = ".Val";
                formattedIdentifier += formattedProperty;

                return formattedIdentifier;
            }

            //predefined keywords
            if (Identifier.IsSkill(ref identifier))
            {
                if (string.IsNullOrEmpty(formattedProperty)) //skills must be accessed through property, defaults to ".Val"
                    formattedProperty = ".Val";
                return "Skills." + FormatReserved(identifier) + formattedProperty;
            }

            if (Identifier.IsGlobal(ref identifier))
                return "Globals." + FormatReserved(identifier) + formattedProperty;

            if (Identifier.IsEvent(ref identifier))
                return "Events." + FormatReserved(identifier) + formattedProperty;

            if (Identifier.IsSynonym(ref identifier))
                return "Globals." + FormatReserved(identifier) + formattedProperty;

            if (Identifier.IsLocalSynonym(ref identifier))
                return FormatReserved(identifier) + formattedProperty;

            //assume goto label or known identifier (null)
            //unregistered objects will also go here
            return FormatIdentifier(identifier) + formattedProperty;
        }

        public static string FormatMath(string s)
        {
            return "MathV." + FormatReserved(s);
        }

        public static string FormatNull()
        {
            return "null";
        }

        public static string FormatNumber(string s)
        {
            if (float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out float f))
                s = f.ToString(CultureInfo.InvariantCulture);
            else
                s = "0";
            return s;
        }

        public static string FormatObjectId(string id)
        {
            //escape "null" for object/asset identifiers
            if (id.Equals("null", StringComparison.OrdinalIgnoreCase))
                return "__" + id;
            else
                return FormatIdentifier(id);
        }

        public static string FormatReserved(string s)
        {
            //Reserved keywords never have leading underscores - but they might have been applied during formatiing
            int i = 0;
            while (s[i] == '_')
                i++;
            s = s.Substring(i);
            return Ucfirst(s);
        }

        public static string FormatSkill(string s)
        {
            return "Skills." + FormatReserved(s);
        }

        public static string FormatSkillType(string s)
        {
            return "SkillType." + FormatReserved(s);
        }

        public static string FormatString(string s)
        {
            //convert escaping of quotation marks
            s = s.Replace("\\\"", "\"\"");
            return "@" + s;
        }

        public static string FormatToString(string s)
        {
            return "\"" + s + "\"";
        }

        public static string FormatVideo(string s)
        {
            //somewhat dirty workaround due to identifier starting with number being patched to avoid clash with C' language
            //remove leading underscores and capitalize first char
            while (s[0] == '_')
                s = s.Substring(1);

            s = "\"" + s.ToLower() + "\"";
            return s;
        }

        private static string Ucfirst(string s)
        {
            s = s.ToLower();
            char[] a = s.ToCharArray();
            int i = 0;
            //skip leading underscores
            while (i < a.Length && (a[i] == '_' || (a[i] >= 48 && a[i] <= 57)))
                i++;
            if (i < a.Length)
                a[i] = char.ToUpper(a[i]);
            return new string(a);
        }
    }
}
