using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Instruction : Node 
    {
        private string m_command;
        private readonly bool m_count;
        private List<string> m_parameters;

        public string Command { get => m_command; set => m_command = value; }
        public bool Count { get => m_count; }
        public List<string> Parameters { get => m_parameters; }

        public Instruction()
        {
            m_command = string.Empty;
            m_count = false;
            m_parameters = new List<string>();
        }

        public Instruction(string command) : this(command, true) { }
        public Instruction(string command, bool count) : this(command, new List<string>(), count) { }
        public Instruction(string command, string parameter) : this(command, parameter, true) { }
        public Instruction(string command, string parameter, bool count) : this(command, new[] { parameter }.ToList(), count) { }
        public Instruction(string command, List<string> parameters) : this(command, parameters, true) { }

        public Instruction(string command, List<string> parameters, bool count) : this()
        {
            m_command = command;
            if (parameters != null)
                m_parameters.AddRange(parameters);
            m_count = count;
        }

        public string Format(string function)
        {
            string o = string.Empty;
            try
            {
                string command = Formatter.FormatReserved(m_command);
                /* don't format any parameter of IFs, RULEs or GOTOs
                 * detect RULEs either by command or by assignment operator in expression
                 * IFs and RULEs already have their expression parameter formatted
                 * GOTO requires unformatted marker - ambiguous definition might result in wrong formatting otherwise
                 */
                /*if (!command.Equals("If") && !command.Equals("Rule") && !command.Equals("Goto") && !(m_parameters.Count > 0 && m_parameters[0].Contains("=")))
                {
                    for (int i = 0; i < m_parameters.Count; i++)
                    {
                        m_parameters[i] = Formatter.FormatKeyword(m_parameters[i]);
                    }
                }*/

                switch (command)
                {
                    case "Abs":
                        o = $"{m_parameters[0]} = {Formatter.FormatMath("Abs")}({m_parameters[1]});";
                        break;

                    case "Accel":
                        o = $"{m_parameters[0]}.Accel({m_parameters[1]});";
                        break;

                    case "Add":
                        o = $"{m_parameters[0]} += {m_parameters[1]};";
                        break;

                    case "Add_string":
                        o = $"{m_parameters[0]} += {m_parameters[1]};";
                        break;

                    case "Addt":
                        o = $"{m_parameters[0]} += {m_parameters[1]} * { Formatter.FormatKeyword("Time_corr")};"; //TODO: captialize Time_corr?
                        break;

                    case "And":
                        o = $"{m_parameters[0]} &= {m_parameters[1]};";
                        break;

                    case "Asin":
                        o = $"{m_parameters[0]} = {Formatter.FormatMath("Asin")}({m_parameters[1]});";
                        break;

                    case "Beep":
                        o = $"Diag.Beep();";
                        break;

                    case "Branch":
                        o = $"Run({m_parameters[0]}); yield break;";
                        break;

                    case "Break":
                        o = $"break;";
                        break;

                    case "Call":
                        o = $"Run({m_parameters[0]});";
                        break;

                    case "Do":
                        o = $"Run({m_parameters[0]});";
                        break;

                    case "Drop":
                        o = $"{m_parameters[0]}.Drop();";
                        break;

                    case "Else": //"else" instruction is distinguished from "Else" command via case sensitivity
                        o = $"else";
                        break;

                    case "End":
                        o = $"yield break;";
                        break;

                    case "Exclusive":
                        o = $"My.Exclusive();";
                        break;

                    case "Exec_rules":
                        o = $"Run({m_parameters[0]});";
                        break;

                    case "Exit":
                        if (m_parameters.Count > 0)
                            o = $"Environment.Exit({m_parameters[0]}); yield break;";
                        else
                            o = $"Environment.Exit(); yield break;";
                        break;

                    case "Explode":
                        o = $"{m_parameters[0]}.Explode();";
                        break;

                    case "Fade_pal":
                        o = $"{m_parameters[0]}.Fade({m_parameters[1]});";
                        break;

                    case "Find":
                        o = $"{m_parameters[0]}.Find({m_parameters[1]});";
                        break;

                    case "Freeze":
                        o = $"Environment.Freeze({m_parameters[0]}, {m_parameters[1]});";
                        break;

                    case "Getmidi":
                        o = $"{m_parameters[1]} = Media.Getmidi({m_parameters[0]});";
                        break;

                    case "Goto":
                        o = $"goto {Formatter.FormatGotoLabel(m_parameters[0])};";
                        break;

                    case "If":
                        o = $"if {m_parameters[0]}";
                        break;

                    case "If_above":
                        o = $"if ({m_parameters[0]} > {m_parameters[1]})";
                        if(m_parameters.Count > 2 && !int.TryParse(m_parameters[2], out _)) //third parameter - if not numeric - is goto label
                            o += $" {{ goto {Formatter.FormatGotoLabel(m_parameters[2])}; }}"; 
                        break;

                    case "If_below":
                        o = $"if ({m_parameters[0]} < {m_parameters[1]})";
                        if (m_parameters.Count > 2 && !int.TryParse(m_parameters[2], out _)) //third parameter - if not numeric - is goto label
                            o += $" {{ goto {Formatter.FormatGotoLabel(m_parameters[2])}; }}";
                        break;

                    case "If_equal":
                        //special treatment for actor target comparison
                        if (m_parameters[0].EndsWith(".Target"))
                            o = $"if ({m_parameters[0]}.Equals({Formatter.FormatActorTarget(m_parameters[1])}))";
                        else
                            o = $"if ({m_parameters[0]} == {m_parameters[1]})";
                        if (m_parameters.Count > 2 && !int.TryParse(m_parameters[2], out _)) //third parameter - if not numeric - is goto label
                            o += $" {{ goto {Formatter.FormatGotoLabel(m_parameters[2])}; }}";
                        break;

                    case "If_nequal":
                        //special treatment for actor target comparison
                        if (m_parameters[0].EndsWith(".Target"))
                            o = $"if (!{m_parameters[0]}.Equals({Formatter.FormatActorTarget(m_parameters[1])}))";
                        else
                            o = $"if ({m_parameters[0]} != {m_parameters[1]})";
                        if (m_parameters.Count > 2 && !int.TryParse(m_parameters[2], out _)) //third parameter - if not numeric - is goto label
                            o += $" {{ goto {Formatter.FormatGotoLabel(m_parameters[2])}; }}";
                        break;

                    case "If_max":
                        //Remove .Val from Skill
                        int max_i = m_parameters[0].LastIndexOf('.');
                        string max = (max_i < 0) ? m_parameters[0] : m_parameters[0].Substring(0, max_i);
                        o = $"if ({m_parameters[0]} >= {max}.Max)";
                        if (m_parameters.Count > 2 && !int.TryParse(m_parameters[2], out _)) //third parameter - if not numeric - is goto label
                            o += $" {{ goto {Formatter.FormatGotoLabel(m_parameters[2])}; }}";
                        break;

                    case "If_min":
                        //Remove .Val from Skill
                        int min_i = m_parameters[0].LastIndexOf('.');
                        string min = (min_i < 0) ? m_parameters[0] : m_parameters[0].Substring(0, min_i);
                        o = $"if ({m_parameters[0]} <= {min}.Min)";
                        if (m_parameters.Count > 2 && !int.TryParse(m_parameters[2], out _)) //third parameter - if not numeric - is goto label
                            o += $" {{ goto {Formatter.FormatGotoLabel(m_parameters[2])}; }}";
                        break;

                    case "Inkey":
                        o = $"yield return Scheduler.Inkey({m_parameters[0]}); {m_parameters[0]} = Scheduler.Inkey_string;";
                        break;

                    case "Inport":
                        o = $"{m_parameters[0]} = Environment.Inport({m_parameters[1]});";
                        break;

                    case "Lift":
                        o = $"{m_parameters[0]}.Lift({m_parameters[1]});";
                        break;

                    case "Load":
                        o = $"Environment.Load({Formatter.FormatFile(m_parameters[0])}, {m_parameters[1]}, false);";
                        break;

                    case "Load_info":
                        o = $"Environment.Load({Formatter.FormatFile(m_parameters[0])}, {m_parameters[1]}, true);";
                        break;

                    case "Locate":
                        if (m_parameters.Count > 0)
                            o = $"{m_parameters[0]}.Locate();";
                        else
                            o = $"Player.Locate();";
                        break;

                    case "Map":
                        if (m_parameters.Count > 0)
                            o = $"Environment.Map({Formatter.FormatFile(m_parameters[0])});";
                        else
                            o = $"Environment.Map();";
                        break;

                    case "Midi_com":
                        o = $"Media.Midi_com({m_parameters[0]}, {m_parameters[1]}, {m_parameters[2]});";
                        break;

                    case "Next_my": //TODO: this needs to be updated
                        o = $"Globals.My = Globals.My.Next();";
                        break;

                    case "Next_my_there": //TODO: this needs to be updated
                        o = $"Globals.My = Globals.My.Next_there();";
                        break;

                    case "Next_there": //TODO: this needs to be updated
                        o = $"Globals.There = Globals.There.Next();";
                        break;

                    case "Nop":
                        o = string.Empty;
                        break;

                    case "Level":
                        o = $"Environment.Level({Formatter.FormatFile(m_parameters[0])});";
                        break;

                    case "Outport":
                        o = $"Environment.Outport({m_parameters[0]} , {m_parameters[1]});";
                        break;

                    case "Place":
                        o = $"{m_parameters[0]}.Place();";
                        break;

                    case "Play_cd":
                        o = $"Media.Play_cd({m_parameters[0]}, {m_parameters[1]});";
                        break;

                    case "Play_demo":
                        o = $"Environment.Play_demo({Formatter.FormatFile(m_parameters[0])}, {m_parameters[1]}, true);";
                        break;

                    case "Play_flic":
                        //m_parameters[0] = Formatter.FormatAssetIdRef(m_parameters[0]); //TODO: extra formatting required?
                        o = $"Media.Play_flic({m_parameters[0]});";
                        break;

                    case "Play_flicfile":
                        o = $"Media.Play_flic({m_parameters[0]});";
                        break;

                    case "Play_song":
                        //m_parameters[0] = Formatter.FormatAssetIdRef(m_parameters[0]); //TODO: extra formatting required?
                        o = $"Media.Play_song({m_parameters[0]}, {m_parameters[1]}, true);";
                        break;

                    case "Play_song_once":
                        //m_parameters[0] = Formatter.FormatAssetIdRef(m_parameters[0]); //TODO: extra formatting required?
                        o = $"Media.Play_song({m_parameters[0]}, {m_parameters[1]}, false);";
                        break;

                    case "Play_sound":
                        //m_parameters[0] = Formatter.FormatAssetIdRef(m_parameters[0]); //TODO: extra formatting required?
                        if (m_parameters.Count > 2)
                        {
                            //check for specific properties containing an object, e.g. <object>.Genius - checking for dot may lead to false positives
                            if (Registry.Is("Thing", m_parameters[2]) || Registry.Is("Actor", m_parameters[2]) || Registry.Is("Wall", m_parameters[2]) ||
                                m_parameters[2].StartsWith("Globals.") || m_parameters[2].EndsWith(".Genius") || (string.Compare(m_parameters[2], "My") == 0)
                                )
                            {
                                o = $"{m_parameters[2]}.Play_sound({m_parameters[0]}, {m_parameters[1]});";
                            }
                            else
                            {
                                o = $"Media.Play_sound({m_parameters[0]}, {m_parameters[1]}, {m_parameters[2]});";
                            }
                        }
                        else
                        {
                            o = $"Media.Play_sound({m_parameters[0]}, {m_parameters[1]});";
                        }
                        break;

                    case "Play_soundfile":
                        if (m_parameters.Count > 2)
                        {
                            o = $"Media.Play_sound({m_parameters[0]}, {m_parameters[1]}, {m_parameters[2]});";
                        }
                        else
                        {
                            o = $"Media.Play_sound({m_parameters[0]}, {m_parameters[1]});";
                        }
                        break;

                    case "Print":
                    case "Print_value":
                    case "Print_string":
                        o = $"Diag.Print({m_parameters[0]});";
                        break;

                    case "Printfile":
                        o = $"Diag.Printfile({m_parameters[0]}, {m_parameters[1]});";
                        break;

                    case "Push":
                        o = $"Player.Push({m_parameters[0]});";
                        break;

                    case "Randomize":
                        o = $"{m_parameters[0]}.Randomize({m_parameters[1]});";
                        break;

                    case "Rotate":
                        o = $"{m_parameters[0]}.Rotate({m_parameters[1]});";
                        break;

                    case "Rule":
                        //ridiculous patch: A3 accepts RULE statements without assignment
                        //TODO: find out real behaviour in A3, currently first identifier is treated as assignee
                        //patch is derived from the behaviour of A3 for statements like "+ =" - seems like "=" is optional for WDL parser
                        /*if (!m_parameters[0].Contains("="))
                        {
                            Console.WriteLine("(W) INSTRUCTION patched invalid rule: " + m_parameters[0]);
                            string[] fragments = m_parameters[0].Split(new[] { ' ' });
                            fragments[1] += "="; //first operator is changed to assignment operator
                            m_parameters[0] = string.Join(" ", fragments);
                        }*/
                        o = $"{m_parameters[0]};";
                        break;

                    case "Save":
                        o = $"Environment.Save({Formatter.FormatFile(m_parameters[0])}, {m_parameters[1]}, false);";
                        break;

                    case "Save_demo":
                        o = $"Environment.Save({Formatter.FormatFile(m_parameters[0])}, {m_parameters[1]});";
                        break;

                    case "Save_info":
                        o = $"Environment.Save({Formatter.FormatFile(m_parameters[0])}, {m_parameters[1]}, true);";
                        break;

                    case "Scan":
                        o = $"Environment.Scan({m_parameters[0]});";
                        break;

                    case "Screenshot":
                        o = $"Environment.Screenshot({Formatter.FormatFile(m_parameters[0])}, {m_parameters[1]});";
                        break;

                    case "Set":
                        //Special case "Target" property
                        if (m_parameters[0].EndsWith(".Target"))
                        {
                            o = $"{m_parameters[0]} = {Formatter.FormatActorTarget(m_parameters[1])};";
                        }

                        //special case: function assignments to each_tick and each_sec (not applicable for Set_all)
                        else if (m_parameters[0].StartsWith(Formatter.FormatGlobal("Each_")))
                        {
                            string p = Formatter.FormatIdentifier(m_parameters[1]);
                            //if function assigns itself, use existing instance
                            if (string.Compare(function, p) == 0)
                                o = $"{m_parameters[0]} = this;";
                            else
                                o = $"{m_parameters[0]} = {m_parameters[1]};";
                        }
                        else
                        {
                            o = $"{m_parameters[0]} = {m_parameters[1]};";
                        }
                        break;

                    case "Set_all":
                        //split property from object for iteration through all instances
                        int i = m_parameters[0].IndexOf('.');
                        string target = i < 0 ? m_parameters[0] : m_parameters[0].Substring(0, i);
                        string property = i < 0 ? "" : m_parameters[0].Substring(i);

                        //Special case "Target" property
                        if (property.EndsWith(".Target"))
                        {
                            o = $"foreach (var instance in {target}) instance{property} = {Formatter.FormatActorTarget(m_parameters[1])};";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(property))
                                o = $"foreach (var instance in {target}) instance{property} = {m_parameters[1]};"; //TODO: get rid of extra formatting
                            else
                            {
                                //special case: no property has been given
                                o = $"{target} = {m_parameters[1]};";
                                Console.WriteLine($"(W) INSTRUCTION malformed SET_ALL instruction replaced with: {o}");
                            }
                        }
                        break;

                    case "Set_info":
                        o = $"{m_parameters[0]} = {m_parameters[1]}.ToString();";
                        break;

                    case "Set_skill":
                        o = $"{m_parameters[0]} = Convert.ToDouble({m_parameters[1]});";
                        break;

                    case "Set_string":
                        o = $"{m_parameters[0]} = string.Copy({m_parameters[1]});";
                        break;

                    case "Setmidi":
                        o = $"Media.Setmidi({m_parameters[0]}, {m_parameters[1]});";
                        break;

                    case "Shake":
                        o = $"{m_parameters[0]}.Shake();";
                        break;

                    case "Shift":
                        o = $"{m_parameters[0]}.Shift({m_parameters[1]}, {m_parameters[2]});";
                        break;

                    case "Shoot":
                        if (m_parameters.Count > 0)
                            o = $"{m_parameters[0]}.Shoot();";
                        else
                            o = $"Player.Shoot();";
                        break;

                    case "Skip":
                        o = $"goto {m_parameters[1]};";
                        break;

                    case "Sin":
                        o = $"{m_parameters[0]} = {Formatter.FormatMath("Sin")}({m_parameters[1]});";
                        break;

                    case "Sqrt":
                        o = $"{m_parameters[0]} = {Formatter.FormatMath("Sqrt")}({m_parameters[1]});";
                        break;

                    case "Stop_demo":
                        o = $"Environment.Stop_demo();";
                        break;

                    case "Stop_flic":
                        o = $"Media.Stop_flic();";
                        break;

                    case "Stop_sound":
                        o = $"Media.Stop_sound();";
                        break;

                    case "Sub":
                        o = $"{m_parameters[0]} -= {m_parameters[1]};";
                        break;

                    case "Subt":
                        o = $"{m_parameters[0]} -= {m_parameters[1]} * {Formatter.FormatKeyword("Time_corr")};"; //TODO: captialize Time_corr?
                        break;

                    case "Tilt":
                        o = $"{m_parameters[0]}.Tilt({m_parameters[1]});";
                        break;

                    case "To_string":
                        o = $"{m_parameters[0]} = {m_parameters[1]}.ToString();";
                        break;

                    case "Wait":
                        o = $"yield return Wait({m_parameters[0]});";
                        break;

                    case "Waitt":
                    case "Wait_ticks": //undocumented
                        o = $"yield return Waitt({m_parameters[0]});";
                        break;

                    case "While":
                        //while(1) patch -> C# needs while(true)
                        string x = m_parameters[0].Trim(new[] { '(', ')' });
                        if(int.TryParse(x, out int n) && n > 0)
                            o = $"while (true)";
                        else
                            o = $"while {m_parameters[0]}";
                        break;

                    default:
                        if (m_parameters.Count == 0)
                        {
                            o = m_command;
                        }
                        else
                        {
                            string pars = string.Empty;
                            foreach (string p in m_parameters)
                            {
                                pars += p + " ";
                            }
                            o = /*"//TODO: " +*/ m_command + " " + pars;
                        }
                        break;
                }
            }
            catch//(Exception e)
            {
//                Console.WriteLine("(E) INSTRUCTION: " + e.Message + " ("+e.Source+")");
                Console.WriteLine("(E) INSTRUCTION discard malformed instruction: " + m_command + " " + string.Join(", ", m_parameters));
            }

            return o;
        }

    }
}
