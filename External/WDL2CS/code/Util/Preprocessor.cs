using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WDL2CS
{
    class Preprocess
    {
        private List<string> m_defines = new List<string>();
        private Dictionary<string, string> m_replacements = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private List<Regex> m_regex = new List<Regex>();
        private Stack<bool> m_stack = new Stack<bool>();
        private List<string> m_directives = new List<string>();
        private readonly string m_path;

        enum ProcId : int
        {
            Ifdef = 0,
            Ifndef = 1,
            Ifelse = 2,
            Endif = 3,
            Replace = 4,
            Define = 5,
            Undef = 6,
            Include = 7,
            Comment = 8,
            String = 9,
            Whitespace = 10,
            Terminator = 11,
            Code = 12,
            Identifier = 13,
        }

        public Preprocess() : this(string.Empty) { }
        public Preprocess(string path)
        {
            m_path = path;
            m_regex.Add(new Regex(@"\G((?i)IFDEF[\s,]+([\w\?\-]*)[\s,]*;*)"));
            m_regex.Add(new Regex(@"\G((?i)IFNDEF[\s,]+([\w\?\-]*)[\s,]*;*)"));
            m_regex.Add(new Regex(@"\G((?i)IFELSE[\s,]*;*)"));
            m_regex.Add(new Regex(@"\G((?i)ENDIF[\s,]*;*)"));
            m_regex.Add(new Regex(@"\G((?i)DEFINE[\s,]+([\w\?\-]+)[\s,]+([\w\?\-\.""]+)[\s,]*;*)"));
            m_regex.Add(new Regex(@"\G((?i)DEFINE[\s,]+([\w\?\-]+)[\s,]*;*)"));
            m_regex.Add(new Regex(@"\G((?i)UNDEF)[\s,]+([\w\?\-]+)[\s,]*;*"));
            m_regex.Add(new Regex(@"\G((?i)INCLUDE[\s,]*<[\s]*([\w\?\-\.]+)[\s]*>[\s,]*;*)"));
            //comments
            m_regex.Add(new Regex(@"\G(#.*(\n|$)|//.*[\n|$]|/\*(.|[\r\n])*?\*/)"));
            //strings
            m_regex.Add(new Regex(@"\G(""(\\""|.|[\r\n])*?"")"));
            //whitespaces
            m_regex.Add(new Regex(@"\G(\s+)"));
            //terminators
            m_regex.Add(new Regex(@"\G([;}])"));
            //standard code
            m_regex.Add(new Regex(@"\G([\x21-\x2F\x3A-\x40\x5B-\x5D\x7B-\x7E])"));
            //identifier
            m_regex.Add(new Regex(@"\G([\w\?_]+)"));

            Regex.CacheSize += m_regex.Count;
        }


        public string Parse(ref string stream)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            StringBuilder sb = new StringBuilder();
            Parse(sb, ref stream);

            Statistics();

            Console.WriteLine("(I) PREPROCESSOR finished in " + watch.Elapsed);
            watch.Stop();

            //Console.WriteLine(sb.ToString());
            return sb.ToString();
        }

        private void Parse(StringBuilder sb, ref string stream)
        {
            try
            {

                int pos = 0;
                bool success = false;
                int length = 0;
                int lastTermPos = sb.Length;
                bool ignore = false;
                Match m;

                while (pos < stream.Length)
                {
                    success = false;
                    for (int r = 0; r < m_regex.Count; r++)
                    {
                        m = m_regex[r].Match(stream, pos);
                        if (m.Success)
                        {
                            length = m.Value.Length;
                            success = true;
                            ProcessMatch(sb, r, pos, m, ref stream, ref ignore);
                            if (r == (int)ProcId.Terminator || r < (int)ProcId.Comment)
                                lastTermPos = sb.Length;
                            break;
                        }
                    }
                    if (!success)
                    {
                        Console.WriteLine("(E) PREPROCESSOR parsing failed");
                        break;
                    }
                    pos = pos + length;
                }
                //trim trailing garbage code
                Trim(sb, lastTermPos);
            }
            catch
            {
                Console.WriteLine("(E) PREPROCESSOR parsing failed");
            }

        }

        private void Include(StringBuilder sb, string file)
        {
            try
            {
                Console.WriteLine("(I) PREPROCESSOR processing include: " + file);
                string pathFile = Path.Combine(m_path, file);
                string stream = File.ReadAllText(pathFile, Encoding.ASCII);
                Parse(sb, ref stream);
            }
            catch
            {
                Console.WriteLine("(E) PREPROCESSOR Include file not found: " + file);
            }
        }

        private void ProcessMatch(StringBuilder sb, int index, int pos, Match match, ref string stream, ref bool ignore)
        {
            string define = string.Empty;
            switch ((ProcId)index)
            {
                case ProcId.Ifdef:
                    m_stack.Push(ignore);
                    define = match.Groups[2].Value.ToLower();
                    if (!ignore)
                    {
                        ignore = !m_defines.Contains(define);
                    }
                    m_directives.Add(new string('\t', m_stack.Count-1) + (ignore ? "(IFDEF) " : "IFDEF ") + define);
                    break;

                case ProcId.Ifndef:
                    m_stack.Push(ignore);
                    define = match.Groups[2].Value.ToLower();
                    if (!ignore)
                    {
                        ignore = m_defines.Contains(define);
                    }

                    m_directives.Add(new string('\t', m_stack.Count-1) + (ignore ? "(IFNDEF) " : "IFNDEF ") + define);
                    break;

                case ProcId.Ifelse:
                    if (!m_stack.Peek())
                        ignore = !ignore;

                    m_directives.Add(new string('\t', m_stack.Count-1) + (ignore ? "(IFELSE)" : "IFELSE"));
                    break;

                case ProcId.Endif:
                    if (m_stack.Count > 0)
                        ignore = m_stack.Pop();
                    else
                        Console.WriteLine("(W) PREPROCESSOR missing ENDIF detected");

                    m_directives.Add(new string('\t', m_stack.Count) + (ignore ? "(ENDIF)" : "ENDIF"));
                    break;

                case ProcId.Whitespace: //intended
                case ProcId.Terminator: //intended
                case ProcId.Code:
                    if (!ignore)
                    {
                        sb.Append(stream, pos, match.Length);
                    }
                    break;

                case ProcId.Identifier:
                    if (!ignore)
                    {
                        int sbPos = sb.Length;
                        if (m_replacements.ContainsKey(match.Value))
                        {
                            sb.Append(m_replacements[match.Value]);
                        }
                        else
                        {
                            sb.Append(stream, pos, match.Length);
                        }

                        for (int i = sbPos; i < sb.Length; i++)
                        {
                                sb[i] = char.ToUpper(sb[i]);
                        }
                    }
                    break;

                case ProcId.String:
                    if (!ignore)
                    {
                        sb.Append(stream, pos, match.Length);
                    }
                    break;

                case ProcId.Define:
                    define = match.Groups[2].Value.ToLower();
                    if (!ignore)
                        m_defines.Add(define);

                    m_directives.Add(new string('\t', m_stack.Count) + (ignore ? "(DEFINE) " : "DEFINE ") + define);
                    break;

                case ProcId.Replace:
                    define = match.Groups[2].Value.ToLower();
                    string replace = match.Groups[3].Value.ToLower();
                    if (!ignore)
                    {
                        if (m_replacements.ContainsKey(define))
                        {
                            Console.WriteLine("(W) PREPROCESSOR double definition of replacement: [" + define + ", " + m_replacements[define] + " <--> " + replace + "]");
                            m_replacements[define] = replace;
                        }
                        else
                            m_replacements.Add(define, replace);
                    }

                    m_directives.Add(new string('\t', m_stack.Count) + (ignore ? "(DEFINE) " : "DEFINE ") + define + ", " + replace);
                    break;

                case ProcId.Include:
                    string file = match.Groups[2].Value.ToLower();
                    if (!ignore)
                        Include(sb, file);

                    m_directives.Add(new string('\t', m_stack.Count) + (ignore ? "(INCLUDE) " : "INCLUDE ") + file);
                    break;

                case ProcId.Undef:
                    define = match.Groups[2].Value.ToLower();
                    if (!ignore)
                    {
                        m_defines.Remove(define);
                        m_replacements.Remove(define);
                    }

                    m_directives.Add(new string('\t', m_stack.Count) + (ignore ? "(UNDEF) " : "UNDEF ") + define);
                    break;

                case ProcId.Comment:
                    break;

                default:
                    break;
            }
        }

        private void Trim(StringBuilder sb, int pos)
        {
            //remove any garbage after last instruction (anything followed after last ; or } )
            int length = sb.Length - pos;
            string tail = sb.ToString(pos, length);
            if (!string.IsNullOrWhiteSpace(tail))
                Console.WriteLine("(W) PREPROCESSOR removed invalid trailing code: [" + tail + "]");
            sb.Remove(pos, length);
            sb.AppendLine();
        }
        private void Statistics()
        {
            Console.WriteLine("(I) PREPROCESSOR statistics:");

            if (m_directives.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Preprocessor Directives");
                Console.WriteLine("-----------------------");
                foreach (string line in m_directives)
                {
                    Console.WriteLine(line);
                }
            }

            if (m_replacements.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Replacements");
                Console.WriteLine("------------");
                foreach (var kvp in m_replacements)
                {
                    Console.WriteLine(kvp.Key + ", " + kvp.Value);
                }
            }

            if (m_defines.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Defines");
                Console.WriteLine("-------");
                foreach (var define in m_defines)
                {
                    Console.WriteLine(define);
                }
            }

            Console.WriteLine();
        }

    }
}
