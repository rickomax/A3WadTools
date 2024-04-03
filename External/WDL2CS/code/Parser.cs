using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using WDL2CS;

namespace VCCCompiler
{
    /// <summary>
    /// Zusammenfassung für MyCompiler.
    /// </summary>
    class WDLCompiler
    {
        List<int> tList = new List<int>();
        List<Regex> rList = new List<Regex>();
        public WDLCompiler()
        {
            tList.Add(t_Char59);
            rList.Add(new Regex("\\G(;)"));
            tList.Add(t_Char123);
            rList.Add(new Regex("\\G(\\{)"));
            tList.Add(t_Char125);
            rList.Add(new Regex("\\G(\\})"));
            tList.Add(t_Char58);
            rList.Add(new Regex("\\G(:)"));
            tList.Add(t_Char124Char124);
            rList.Add(new Regex("\\G(\\|\\|)"));
            tList.Add(t_Char38Char38);
            rList.Add(new Regex("\\G(&&)"));
            tList.Add(t_Char124);
            rList.Add(new Regex("\\G(\\|)"));
            tList.Add(t_Char94);
            rList.Add(new Regex("\\G(\\^)"));
            tList.Add(t_Char38);
            rList.Add(new Regex("\\G(&)"));
            tList.Add(t_Char40);
            rList.Add(new Regex("\\G(\\()"));
            tList.Add(t_Char41);
            rList.Add(new Regex("\\G(\\))"));
            tList.Add(t_Char33Char61);
            rList.Add(new Regex("\\G(!=)"));
            tList.Add(t_Char61Char61);
            rList.Add(new Regex("\\G(==)"));
            tList.Add(t_Char60);
            rList.Add(new Regex("\\G(<)"));
            tList.Add(t_Char60Char61);
            rList.Add(new Regex("\\G(<=)"));
            tList.Add(t_Char62);
            rList.Add(new Regex("\\G(>)"));
            tList.Add(t_Char62Char61);
            rList.Add(new Regex("\\G(>=)"));
            tList.Add(t_Char43);
            rList.Add(new Regex("\\G(\\+)"));
            tList.Add(t_Char45);
            rList.Add(new Regex("\\G(\\-)"));
            tList.Add(t_Char37);
            rList.Add(new Regex("\\G(%)"));
            tList.Add(t_Char42);
            rList.Add(new Regex("\\G(\\*)"));
            tList.Add(t_Char47);
            rList.Add(new Regex("\\G(/)"));
            tList.Add(t_Char33);
            rList.Add(new Regex("\\G(!)"));
            tList.Add(t_RULE);
            rList.Add(new Regex("\\G(RULE)"));
            tList.Add(t_Char42Char61);
            rList.Add(new Regex("\\G(\\*[\\s\\t\\x00]*=)"));
            tList.Add(t_Char43Char61);
            rList.Add(new Regex("\\G(\\+[\\s\\t\\x00]*=)"));
            tList.Add(t_Char45Char61);
            rList.Add(new Regex("\\G(\\-[\\s\\t\\x00]*=)"));
            tList.Add(t_Char47Char61);
            rList.Add(new Regex("\\G(/[\\s\\t\\x00]*=)"));
            tList.Add(t_Char61);
            rList.Add(new Regex("\\G(=)"));
            tList.Add(t_ELSE);
            rList.Add(new Regex("\\G(ELSE)"));
            tList.Add(t_IF);
            rList.Add(new Regex("\\G(IF)"));
            tList.Add(t_WHILE);
            rList.Add(new Regex("\\G(WHILE)"));
            tList.Add(t_Char46);
            rList.Add(new Regex("\\G(\\.)"));
            tList.Add(t_NULL);
            rList.Add(new Regex("\\G(NULL)"));
            tList.Add(t_asset);
            rList.Add(new Regex("\\G((?=[A-Z])(MODEL|SOUND|MUSIC|FLIC|BMAP|OVLY|FONT))"));
            tList.Add(t_object);
            rList.Add(new Regex("\\G((?=[A-Z])(OVERLAY|PANEL|PALETTE|REGION|SKILL|STRING|SYNONYM|TEXTURE|TEXT|VIEW|WALL|WAY))"));
            tList.Add(t_function);
            rList.Add(new Regex("\\G((?=[A-Z])(ACTION|RULES))"));
            tList.Add(t_math);
            rList.Add(new Regex("\\G((?=[A-Z])(ACOS|COS|ATAN|TAN|SIGN|INT|EXP|LOG10|LOG2|LOG))"));
            tList.Add(t_list);
            rList.Add(new Regex("\\G(((EACH_TICK|EACH_SEC|PANELS|LAYERS|MESSAGES)\\.(1[0-6]|[1-9])))"));
            tList.Add(t_ambigChar95objectChar95flag);
            rList.Add(new Regex("\\G((?=[A-Z])(THING|ACTOR))"));
            tList.Add(t_ambigChar95mathChar95command);
            rList.Add(new Regex("\\G((?=[A-Z])(SIN|ASIN|SQRT|ABS))"));
            tList.Add(t_ambigChar95mathChar95skillChar95property);
            rList.Add(new Regex("\\G(RANDOM)"));
            tList.Add(t_integer);
            rList.Add(new Regex("\\G([0-9]+)"));
            tList.Add(t_fixed);
            rList.Add(new Regex("\\G(([0-9]*\\.[0-9]+)|([0-9]+\\.[0-9]*))"));
            tList.Add(t_identifier);
            rList.Add(new Regex("\\G([A-Za-z0-9_][A-Za-z0-9_\\?]*(\\.[1-9][0-9]?)?)"));
            tList.Add(t_file);
            rList.Add(new Regex("\\G(<[\\s]?[^<;:\" ]+\\.[^<;:\" ]+[\\s]?>)"));
            tList.Add(t_string);
            rList.Add(new Regex("\\G(\"(\\\\\"|.|[\\r\\n])*?\")"));
            tList.Add(t_ignore);
            rList.Add(new Regex("\\G([\\r\\n\\t\\s\\x00,]|:=|(#.*(\\n|$))|(//.*(\\n|$))|(/\\*(.|[\\r\\n])*?\\*/))"));
            Regex.CacheSize += rList.Count;
        }
        YYARec[] yya;
        YYARec[] yyg;
        YYRRec[] yyr;
        int[] yyd;
        int[] yyal;
        int[] yyah;
        int[] yygl;
        int[] yygh;

        int yyn = 0;
        int yystate = 0;
        int yychar = -1;
        int yynerrs = 0;
        int yyerrflag = 0;
        int yysp = 0;
        int yymaxdepth = 20000;
        int yyflag = 0;
        int yyfnone = 0;
        int[] yys = new int[20000];
        //string[] yyv = new string[20000];
        Node[] yyv = new Node[20000];

        //string yyval = "";
        Node yyval = new Node();

        //StreamWriter Output;
        string output;
        string scriptName = "Script";
        bool showTokens = false;
        bool generatePropertyList = false;
        Dictionary<string, Dictionary<string, Dictionary<string, List<List<string>>>>> propertyList;

        class YYARec
        {
            public int sym;
            public int act;
            public YYARec(int s, int a) { sym = s; act = a; }
        }

        class YYRRec
        {
            public int len;
            public int sym;
            public YYRRec(int l, int s) { sym = s; len = l; }
        }

        ////////////////////////////////////////////////////////////////
        /// Constant values / tokens
        ////////////////////////////////////////////////////////////////
                int t_Char59 = 257;
                int t_Char123 = 258;
                int t_Char125 = 259;
                int t_Char58 = 260;
                int t_Char124Char124 = 261;
                int t_Char38Char38 = 262;
                int t_Char124 = 263;
                int t_Char94 = 264;
                int t_Char38 = 265;
                int t_Char40 = 266;
                int t_Char41 = 267;
                int t_Char33Char61 = 268;
                int t_Char61Char61 = 269;
                int t_Char60 = 270;
                int t_Char60Char61 = 271;
                int t_Char62 = 272;
                int t_Char62Char61 = 273;
                int t_Char43 = 274;
                int t_Char45 = 275;
                int t_Char37 = 276;
                int t_Char42 = 277;
                int t_Char47 = 278;
                int t_Char33 = 279;
                int t_RULE = 280;
                int t_Char42Char61 = 281;
                int t_Char43Char61 = 282;
                int t_Char45Char61 = 283;
                int t_Char47Char61 = 284;
                int t_Char61 = 285;
                int t_ELSE = 286;
                int t_IF = 287;
                int t_WHILE = 288;
                int t_Char46 = 289;
                int t_NULL = 290;
                int t_asset = 291;
                int t_object = 292;
                int t_function = 293;
                int t_math = 294;
                int t_list = 295;
                int t_ambigChar95objectChar95flag = 296;
                int t_ambigChar95mathChar95command = 297;
                int t_ambigChar95mathChar95skillChar95property = 298;
                int t_integer = 299;
                int t_fixed = 300;
                int t_identifier = 301;
                int t_file = 302;
                int t_string = 303;
                int t_ignore = 256;
///////////////////////////////////////////////////////////
/// Global settings: 
///////////////////////////////////////////////////////////

///////////////////////////////////////////////////////////


        public string ScriptName { get => scriptName; set => scriptName = value; }
        public bool ShowTokens { get => showTokens; set => showTokens = value; }
        public bool GeneratePropertyList { get => generatePropertyList; set => generatePropertyList = value; }        
        public Dictionary<string, Dictionary<string, Dictionary<string, List<List<string>>>>> PropertyList { get => propertyList; }

        public int Parse(string inFile, out string outData)
        {
            outData = string.Empty;
            string inputstream = File.ReadAllText(inFile, Encoding.ASCII);
            string path = Path.GetDirectoryName(inFile);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Preprocess p = new Preprocess(path);
            inputstream = p.Parse(ref inputstream);

            ////////////////////////////////////////////////////////////////
            /// Compiler Code:
            ////////////////////////////////////////////////////////////////

            //                        if (!Scanner(inputstream)) return 1;
            if (!ScannerOpt(inputstream)) return 1;
            if (ShowTokens)
            {
                foreach (AToken t in TokenList)
                {
                    Console.WriteLine("TokenID: " + t.token + "  =  " + t.val);
                }
            }
            InitTables();
            if (!yyparse()) return 1;

            Console.WriteLine("(I) PARSER compilation finished in " + watch.Elapsed);
            watch.Stop();
            outData = output;
            return 0;
        }

        public void yyaction(int yyruleno)
        {
            switch (yyruleno)
            {
                ////////////////////////////////////////////////////////////////
                /// YYAction code:
                ////////////////////////////////////////////////////////////////
							case    1 : 
         yyval = yyv[yysp-0];
         output = Script.Format(scriptName, generatePropertyList);
         if (generatePropertyList)
         propertyList = Script.ToList().List;
         
         
       break;
							case    2 : 
         yyval = new Node (yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case    3 : 
         yyval = null; //bogus keyword at EOF - just discard
         
       break;
							case    4 : 
         yyval = null;
         
       break;
							case    5 : 
         yyval = null;
         
       break;
							case    6 : 
         yyval = Sections.AddDummySection(yyv[yysp-0]);
         
       break;
							case    7 : 
         yyval = Sections.AddDummySection(yyv[yysp-0]);
         
       break;
							case    8 : 
         yyval = Sections.AddSection(yyv[yysp-0]);
         
       break;
							case    9 : 
         yyval = Sections.AddSection(yyv[yysp-0]);
         
       break;
							case   10 : 
         yyval = Sections.AddSection(yyv[yysp-0]);
         
       break;
							case   11 : 
         yyval = Sections.AddSection(yyv[yysp-0]);
         
       break;
							case   12 : 
         yyval = Globals.AddGlobal(yyv[yysp-2]);
         
       break;
							case   13 : 
         yyval = Globals.AddGlobal(yyv[yysp-2], yyv[yysp-1]);
         
       break;
							case   14 : 
         yyval = NodeFormatter.FormatKeyword(yyv[yysp-0]); //TODO: review
         
       break;
							case   15 : 
         yyval = Globals.AddParameter(yyv[yysp-1]);
         
       break;
							case   16 : 
         yyval = null;
         
       break;
							case   17 : 
         yyval = yyv[yysp-0];
         
       break;
							case   18 : 
         yyval = yyv[yysp-0];
         
       break;
							case   19 : 
         yyval = yyv[yysp-0];
         
       break;
							case   20 : 
         yyval = Assets.AddAsset(yyv[yysp-4], yyv[yysp-3], yyv[yysp-2]);
         
       break;
							case   21 : 
         yyval = Assets.AddParameter(yyv[yysp-1]);
         
       break;
							case   22 : 
         yyval = null;
         
       break;
							case   23 : 
         yyval = yyv[yysp-0];
         
       break;
							case   24 : 
         yyval = yyv[yysp-0];
         
       break;
							case   25 : 
         yyval = Objects.AddObject(yyv[yysp-4], yyv[yysp-3]);
         
       break;
							case   26 : 
         yyval = Objects.AddStringObject(yyv[yysp-3], yyv[yysp-2], yyv[yysp-1]);
         
       break;
							case   27 : 
         yyval = Objects.AddObject(yyv[yysp-2], yyv[yysp-1]);
         
       break;
							case   28 : 
         yyval = yyv[yysp-0];
         
       break;
							case   29 : 
         yyval = yyv[yysp-0];
         
       break;
							case   30 : 
         yyval = yyv[yysp-0];
         
       break;
							case   31 : 
         yyval = new Node (yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   32 : 
         yyval = null;
         
       break;
							case   33 : 
         yyval = Objects.CreateProperty(yyv[yysp-2]);
         
       break;
							case   34 : 
         yyval = null;
         
       break;
							case   35 : 
         yyval = Objects.AddPropertyValue(yyv[yysp-0]);
         
       break;
							case   36 : 
         yyval = Objects.AddPropertyValue(yyv[yysp-1]);
         
       break;
							case   37 : 
         yyval = yyv[yysp-0];
         
       break;
							case   38 : 
         yyval = yyv[yysp-0];
         
       break;
							case   39 : 
         yyval = yyv[yysp-0];
         
       break;
							case   40 : 
         yyval = yyv[yysp-0];
         
       break;
							case   41 : 
         yyval = Actions.AddAction(yyv[yysp-3], yyv[yysp-1]);
         
       break;
							case   42 : 
         yyval = yyv[yysp-0];
         
       break;
							case   43 : 
         yyval = new Node (yyv[yysp-2], yyv[yysp-0]);
         
       break;
							case   44 : 
         yyval = Actions.CreateMarker(yyv[yysp-2], yyv[yysp-0]);
         
       break;
							case   45 : 
         yyval = new Node (yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   46 : 
         yyval = new Node(yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   47 : 
         yyval = null;
         
       break;
							case   48 : 
         yyval = Actions.CreateInstruction(yyv[yysp-2]);
         
       break;
							case   49 : 
         yyval = yyv[yysp-1];
         
       break;
							case   50 : 
         yyval = Actions.CreateInstruction(yyv[yysp-1]);
         
       break;
							case   51 : 
         yyval = null;
         
       break;
							case   52 : 
         yyval = Actions.AddInstructionParameter(yyv[yysp-0]);
         
       break;
							case   53 : 
         yyval = Actions.AddInstructionParameter(yyv[yysp-1]);
         
       break;
							case   54 : 
         yyval = yyv[yysp-0];
         
       break;
							case   55 : 
         yyval = yyv[yysp-0];
         
       break;
							case   56 : 
         yyval = yyv[yysp-0];
         
       break;
							case   57 : 
         yyval = yyv[yysp-0];
         
       break;
							case   58 : 
         yyval = yyv[yysp-0];
         
       break;
							case   59 : 
         yyval = yyv[yysp-0];
         
       break;
							case   60 : 
         yyval = NodeFormatter.FormatOperator(yyv[yysp-2], " || ", yyv[yysp-0]);
         
       break;
							case   61 : 
         yyval = yyv[yysp-0];
         
       break;
							case   62 : 
         yyval = NodeFormatter.FormatOperator(yyv[yysp-2], " && ", yyv[yysp-0]);
         
       break;
							case   63 : 
         yyval = yyv[yysp-0];
         
       break;
							case   64 : 
         yyval = NodeFormatter.FormatOperator(yyv[yysp-2], " | ", yyv[yysp-0]);
         
       break;
							case   65 : 
         yyval = yyv[yysp-0];
         
       break;
							case   66 : 
         yyval = NodeFormatter.FormatOperator(yyv[yysp-2], " ^ ", yyv[yysp-0]);
         
       break;
							case   67 : 
         yyval = yyv[yysp-0];
         
       break;
							case   68 : 
         yyval = NodeFormatter.FormatOperator(yyv[yysp-2], " & ", yyv[yysp-0]);
         
       break;
							case   69 : 
         yyval = yyv[yysp-0];
         
       break;
							case   70 : 
         yyval = yyv[yysp-0];
         
       break;
							case   71 : 
         yyval = new Node(yyv[yysp-2], yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   72 : 
         yyval = yyv[yysp-0];
         
       break;
							case   73 : 
         yyval = new Node(yyv[yysp-2], yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   74 : 
         yyval = yyv[yysp-0];
         
       break;
							case   75 : 
         yyval = new Node(yyv[yysp-2], yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   76 : 
         yyval = yyv[yysp-0];
         
       break;
							case   77 : 
         yyval = new Node(yyv[yysp-2], yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   78 : 
         yyval = yyv[yysp-0];
         
       break;
							case   79 : 
         yyval = new Node(yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case   80 : 
         yyval = NodeFormatter.FormatMath(yyv[yysp-3], yyv[yysp-1]);
         
       break;
							case   81 : 
         yyval = NodeFormatter.FormatParentheses(yyv[yysp-1]);
         
       break;
							case   82 : 
         //fixes things like "18,4"
         yyval = NodeFormatter.FormatNumberPatch(yyv[yysp-1], yyv[yysp-0]); //this is what supposedly happens in A3
         
       break;
							case   83 : 
         //fixes things like "Skill 6"
         yyval = NodeFormatter.FormatNumberPatch(yyv[yysp-0], yyv[yysp-1]); //this is what supposedly happens in A3
         
       break;
							case   84 : 
         yyval = yyv[yysp-0];
         
       break;
							case   85 : 
         yyval = yyv[yysp-0];
         
       break;
							case   86 : 
         yyval = yyv[yysp-0];
         
       break;
							case   87 : 
         yyval = new Node(" != ");
         
       break;
							case   88 : 
         yyval = new Node(" == ");
         
       break;
							case   89 : 
         yyval = new Node(" < ");
         
       break;
							case   90 : 
         yyval = new Node(" <= ");
         
       break;
							case   91 : 
         yyval = new Node(" > ");
         
       break;
							case   92 : 
         yyval = new Node(" >= ");
         
       break;
							case   93 : 
         yyval = new Node(" + ");
         
       break;
							case   94 : 
         yyval = new Node(" - ");
         
       break;
							case   95 : 
         yyval = new Node(" % ");
         
       break;
							case   96 : 
         yyval = new Node(" * ");
         
       break;
							case   97 : 
         yyval = new Node(" / ");
         
       break;
							case   98 : 
         yyval = new Node("!");
         
       break;
							case   99 : 
         yyval = new Node("+");
         
       break;
							case  100 : 
         yyval = new Node("-");
         
       break;
							case  101 : 
         yyval = Actions.CreateExpression(yyv[yysp-0]);
         
       break;
							case  102 : 
         yyval = Actions.CreateExpression(yyv[yysp-2], yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case  103 : 
         yyval = Actions.CreateExpression(yyv[yysp-2], yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case  104 : 
         yyval = new Node(" *= ");
         
       break;
							case  105 : 
         yyval = new Node(" += ");
         
       break;
							case  106 : 
         yyval = new Node(" -= ");
         
       break;
							case  107 : 
         yyval = new Node(" /= ");
         
       break;
							case  108 : 
         yyval = new Node(" = ");
         
       break;
							case  109 : 
         yyval = yyv[yysp-0];
         
       break;
							case  110 : 
         yyval = yyv[yysp-0];
         
       break;
							case  111 : 
         yyval = yyv[yysp-0];
         
       break;
							case  112 : 
         yyval = Actions.CreateElseCondition(yyv[yysp-1]);
         
       break;
							case  113 : 
         yyval = Actions.CreateIfCondition(yyv[yysp-3], yyv[yysp-1]);
         
       break;
							case  114 : 
         yyval = Actions.CreateWhileCondition(yyv[yysp-3], yyv[yysp-1]);
         
       break;
							case  115 : 
         yyval = NodeFormatter.FormatKeywordProperty(yyv[yysp-2], yyv[yysp-0]);
         
       break;
							case  116 : 
         yyval = NodeFormatter.FormatKeyword(yyv[yysp-0]);
         
       break;
							case  117 : 
         yyval = yyv[yysp-0];
         
       break;
							case  118 : 
         yyval = new Node(yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case  119 : 
         yyval = yyv[yysp-0];
         
       break;
							case  120 : 
         yyval = yyv[yysp-0];
         
       break;
							case  121 : 
         yyval = NodeFormatter.FormatNumber(yyv[yysp-0]);
         
       break;
							case  122 : 
         yyval = NodeFormatter.FormatNumber(yyv[yysp-0]);
         
       break;
							case  123 : 
         yyval = NodeFormatter.FormatNumber(yyv[yysp-0]);
         
       break;
							case  124 : 
         yyval = NodeFormatter.FormatNumber(yyv[yysp-0]);
         
       break;
							case  125 : 
         yyval = new Node(yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case  126 : 
         yyval = yyv[yysp-0];
         
       break;
							case  127 : 
         yyval = new Node(yyv[yysp-1], yyv[yysp-0]);
         
       break;
							case  128 : 
         yyval = yyv[yysp-0];
         
       break;
							case  129 : 
         yyval = yyv[yysp-0];
         
       break;
							case  130 : 
         yyval = NodeFormatter.FormatList(yyv[yysp-0]);
         
       break;
							case  131 : 
         yyval = yyv[yysp-0];
         
       break;
							case  132 : 
         yyval = yyv[yysp-0];
         
       break;
							case  133 : 
         yyval = yyv[yysp-0];
         
       break;
							case  134 : 
         yyval = yyv[yysp-0];
         
       break;
							case  135 : 
         yyval = yyv[yysp-0];
         
       break;
							case  136 : 
         yyval = yyv[yysp-0];
         
       break;
							case  137 : 
         yyval = yyv[yysp-0];
         
       break;
							case  138 : 
         yyval = NodeFormatter.FormatNull();
         
       break;
							case  139 : 
         yyval = yyv[yysp-0];
         
       break;
							case  140 : 
         yyval = yyv[yysp-0];
         
       break;
							case  141 : 
         yyval = yyv[yysp-0];
         
       break;
							case  142 : 
         yyval = yyv[yysp-0];
         
       break;
							case  143 : 
         yyval = yyv[yysp-0];
         
       break;
							case  144 : 
         yyval = new Node(yyv[yysp-2], yyv[yysp-0]);
         
       break;
							case  145 : 
         yyval = new Node(yyv[yysp-2], yyv[yysp-0]);
         
       break;
							case  146 : 
         yyval = yyv[yysp-0];
         
       break;
							case  147 : 
         yyval = yyv[yysp-0];
         
       break;
							case  148 : 
         yyval = yyv[yysp-0];
         
       break;
							case  149 : 
         yyval = yyv[yysp-0];
         
       break;
							case  150 : 
         yyval = yyv[yysp-0];
         
       break;
							case  151 : 
         yyval = yyv[yysp-0]; //TODO: FormatIdentifier?
         
       break;
							case  152 : 
         yyval = NodeFormatter.FormatFile(yyv[yysp-0]);
         
       break;
							case  153 : 
         yyval = NodeFormatter.FormatString(yyv[yysp-0]);
         
       break;
               default: return;
            }
        }

        public void InitTables()
        {
            ////////////////////////////////////////////////////////////////
            /// Init Table code:
            ////////////////////////////////////////////////////////////////

					int yynacts   = 1172;
					int yyngotos  = 524;
					int yynstates = 217;
					int yynrules  = 153;
					yya = new YYARec[yynacts+1];  int yyac = 1;
					yyg = new YYARec[yyngotos+1]; int yygc = 1;
					yyr = new YYRRec[yynrules+1]; int yyrc = 1;

					yya[yyac] = new YYARec(257,18);yyac++; 
					yya[yyac] = new YYARec(258,19);yyac++; 
					yya[yyac] = new YYARec(259,20);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,22);yyac++; 
					yya[yyac] = new YYARec(292,23);yyac++; 
					yya[yyac] = new YYARec(293,24);yyac++; 
					yya[yyac] = new YYARec(294,25);yyac++; 
					yya[yyac] = new YYARec(296,26);yyac++; 
					yya[yyac] = new YYARec(297,27);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(0,-4 );yyac++; 
					yya[yyac] = new YYARec(0,-139 );yyac++; 
					yya[yyac] = new YYARec(289,-139 );yyac++; 
					yya[yyac] = new YYARec(257,-143 );yyac++; 
					yya[yyac] = new YYARec(274,-143 );yyac++; 
					yya[yyac] = new YYARec(275,-143 );yyac++; 
					yya[yyac] = new YYARec(279,-143 );yyac++; 
					yya[yyac] = new YYARec(290,-143 );yyac++; 
					yya[yyac] = new YYARec(291,-143 );yyac++; 
					yya[yyac] = new YYARec(292,-143 );yyac++; 
					yya[yyac] = new YYARec(293,-143 );yyac++; 
					yya[yyac] = new YYARec(294,-143 );yyac++; 
					yya[yyac] = new YYARec(296,-143 );yyac++; 
					yya[yyac] = new YYARec(297,-143 );yyac++; 
					yya[yyac] = new YYARec(298,-143 );yyac++; 
					yya[yyac] = new YYARec(299,-143 );yyac++; 
					yya[yyac] = new YYARec(300,-143 );yyac++; 
					yya[yyac] = new YYARec(301,-143 );yyac++; 
					yya[yyac] = new YYARec(302,-143 );yyac++; 
					yya[yyac] = new YYARec(303,-143 );yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,40);yyac++; 
					yya[yyac] = new YYARec(289,41);yyac++; 
					yya[yyac] = new YYARec(0,-116 );yyac++; 
					yya[yyac] = new YYARec(257,-116 );yyac++; 
					yya[yyac] = new YYARec(258,-116 );yyac++; 
					yya[yyac] = new YYARec(261,-116 );yyac++; 
					yya[yyac] = new YYARec(262,-116 );yyac++; 
					yya[yyac] = new YYARec(263,-116 );yyac++; 
					yya[yyac] = new YYARec(264,-116 );yyac++; 
					yya[yyac] = new YYARec(265,-116 );yyac++; 
					yya[yyac] = new YYARec(267,-116 );yyac++; 
					yya[yyac] = new YYARec(268,-116 );yyac++; 
					yya[yyac] = new YYARec(269,-116 );yyac++; 
					yya[yyac] = new YYARec(270,-116 );yyac++; 
					yya[yyac] = new YYARec(271,-116 );yyac++; 
					yya[yyac] = new YYARec(272,-116 );yyac++; 
					yya[yyac] = new YYARec(273,-116 );yyac++; 
					yya[yyac] = new YYARec(274,-116 );yyac++; 
					yya[yyac] = new YYARec(275,-116 );yyac++; 
					yya[yyac] = new YYARec(276,-116 );yyac++; 
					yya[yyac] = new YYARec(277,-116 );yyac++; 
					yya[yyac] = new YYARec(278,-116 );yyac++; 
					yya[yyac] = new YYARec(279,-116 );yyac++; 
					yya[yyac] = new YYARec(281,-116 );yyac++; 
					yya[yyac] = new YYARec(282,-116 );yyac++; 
					yya[yyac] = new YYARec(283,-116 );yyac++; 
					yya[yyac] = new YYARec(284,-116 );yyac++; 
					yya[yyac] = new YYARec(285,-116 );yyac++; 
					yya[yyac] = new YYARec(290,-116 );yyac++; 
					yya[yyac] = new YYARec(291,-116 );yyac++; 
					yya[yyac] = new YYARec(292,-116 );yyac++; 
					yya[yyac] = new YYARec(293,-116 );yyac++; 
					yya[yyac] = new YYARec(294,-116 );yyac++; 
					yya[yyac] = new YYARec(295,-116 );yyac++; 
					yya[yyac] = new YYARec(296,-116 );yyac++; 
					yya[yyac] = new YYARec(297,-116 );yyac++; 
					yya[yyac] = new YYARec(298,-116 );yyac++; 
					yya[yyac] = new YYARec(299,-116 );yyac++; 
					yya[yyac] = new YYARec(300,-116 );yyac++; 
					yya[yyac] = new YYARec(301,-116 );yyac++; 
					yya[yyac] = new YYARec(302,-116 );yyac++; 
					yya[yyac] = new YYARec(303,-116 );yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,40);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(299,46);yyac++; 
					yya[yyac] = new YYARec(301,40);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(299,59);yyac++; 
					yya[yyac] = new YYARec(300,60);yyac++; 
					yya[yyac] = new YYARec(301,40);yyac++; 
					yya[yyac] = new YYARec(302,61);yyac++; 
					yya[yyac] = new YYARec(303,62);yyac++; 
					yya[yyac] = new YYARec(257,-16 );yyac++; 
					yya[yyac] = new YYARec(257,18);yyac++; 
					yya[yyac] = new YYARec(258,19);yyac++; 
					yya[yyac] = new YYARec(259,20);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,22);yyac++; 
					yya[yyac] = new YYARec(292,23);yyac++; 
					yya[yyac] = new YYARec(293,24);yyac++; 
					yya[yyac] = new YYARec(294,25);yyac++; 
					yya[yyac] = new YYARec(296,26);yyac++; 
					yya[yyac] = new YYARec(297,27);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(0,-4 );yyac++; 
					yya[yyac] = new YYARec(0,0);yyac++; 
					yya[yyac] = new YYARec(290,-24 );yyac++; 
					yya[yyac] = new YYARec(291,-24 );yyac++; 
					yya[yyac] = new YYARec(292,-24 );yyac++; 
					yya[yyac] = new YYARec(293,-24 );yyac++; 
					yya[yyac] = new YYARec(294,-24 );yyac++; 
					yya[yyac] = new YYARec(296,-24 );yyac++; 
					yya[yyac] = new YYARec(297,-24 );yyac++; 
					yya[yyac] = new YYARec(298,-24 );yyac++; 
					yya[yyac] = new YYARec(299,-24 );yyac++; 
					yya[yyac] = new YYARec(301,-24 );yyac++; 
					yya[yyac] = new YYARec(0,-134 );yyac++; 
					yya[yyac] = new YYARec(289,-134 );yyac++; 
					yya[yyac] = new YYARec(290,-28 );yyac++; 
					yya[yyac] = new YYARec(291,-28 );yyac++; 
					yya[yyac] = new YYARec(292,-28 );yyac++; 
					yya[yyac] = new YYARec(293,-28 );yyac++; 
					yya[yyac] = new YYARec(294,-28 );yyac++; 
					yya[yyac] = new YYARec(296,-28 );yyac++; 
					yya[yyac] = new YYARec(297,-28 );yyac++; 
					yya[yyac] = new YYARec(298,-28 );yyac++; 
					yya[yyac] = new YYARec(301,-28 );yyac++; 
					yya[yyac] = new YYARec(0,-137 );yyac++; 
					yya[yyac] = new YYARec(289,-137 );yyac++; 
					yya[yyac] = new YYARec(290,-42 );yyac++; 
					yya[yyac] = new YYARec(291,-42 );yyac++; 
					yya[yyac] = new YYARec(292,-42 );yyac++; 
					yya[yyac] = new YYARec(293,-42 );yyac++; 
					yya[yyac] = new YYARec(294,-42 );yyac++; 
					yya[yyac] = new YYARec(296,-42 );yyac++; 
					yya[yyac] = new YYARec(297,-42 );yyac++; 
					yya[yyac] = new YYARec(298,-42 );yyac++; 
					yya[yyac] = new YYARec(301,-42 );yyac++; 
					yya[yyac] = new YYARec(0,-135 );yyac++; 
					yya[yyac] = new YYARec(289,-135 );yyac++; 
					yya[yyac] = new YYARec(0,-136 );yyac++; 
					yya[yyac] = new YYARec(289,-136 );yyac++; 
					yya[yyac] = new YYARec(257,-142 );yyac++; 
					yya[yyac] = new YYARec(274,-142 );yyac++; 
					yya[yyac] = new YYARec(275,-142 );yyac++; 
					yya[yyac] = new YYARec(279,-142 );yyac++; 
					yya[yyac] = new YYARec(290,-142 );yyac++; 
					yya[yyac] = new YYARec(291,-142 );yyac++; 
					yya[yyac] = new YYARec(292,-142 );yyac++; 
					yya[yyac] = new YYARec(293,-142 );yyac++; 
					yya[yyac] = new YYARec(294,-142 );yyac++; 
					yya[yyac] = new YYARec(296,-142 );yyac++; 
					yya[yyac] = new YYARec(297,-142 );yyac++; 
					yya[yyac] = new YYARec(298,-142 );yyac++; 
					yya[yyac] = new YYARec(299,-142 );yyac++; 
					yya[yyac] = new YYARec(300,-142 );yyac++; 
					yya[yyac] = new YYARec(301,-142 );yyac++; 
					yya[yyac] = new YYARec(302,-142 );yyac++; 
					yya[yyac] = new YYARec(303,-142 );yyac++; 
					yya[yyac] = new YYARec(290,-29 );yyac++; 
					yya[yyac] = new YYARec(291,-29 );yyac++; 
					yya[yyac] = new YYARec(292,-29 );yyac++; 
					yya[yyac] = new YYARec(293,-29 );yyac++; 
					yya[yyac] = new YYARec(294,-29 );yyac++; 
					yya[yyac] = new YYARec(296,-29 );yyac++; 
					yya[yyac] = new YYARec(297,-29 );yyac++; 
					yya[yyac] = new YYARec(298,-29 );yyac++; 
					yya[yyac] = new YYARec(301,-29 );yyac++; 
					yya[yyac] = new YYARec(0,-133 );yyac++; 
					yya[yyac] = new YYARec(289,-133 );yyac++; 
					yya[yyac] = new YYARec(0,-131 );yyac++; 
					yya[yyac] = new YYARec(289,-131 );yyac++; 
					yya[yyac] = new YYARec(257,-141 );yyac++; 
					yya[yyac] = new YYARec(274,-141 );yyac++; 
					yya[yyac] = new YYARec(275,-141 );yyac++; 
					yya[yyac] = new YYARec(279,-141 );yyac++; 
					yya[yyac] = new YYARec(290,-141 );yyac++; 
					yya[yyac] = new YYARec(291,-141 );yyac++; 
					yya[yyac] = new YYARec(292,-141 );yyac++; 
					yya[yyac] = new YYARec(293,-141 );yyac++; 
					yya[yyac] = new YYARec(294,-141 );yyac++; 
					yya[yyac] = new YYARec(296,-141 );yyac++; 
					yya[yyac] = new YYARec(297,-141 );yyac++; 
					yya[yyac] = new YYARec(298,-141 );yyac++; 
					yya[yyac] = new YYARec(299,-141 );yyac++; 
					yya[yyac] = new YYARec(300,-141 );yyac++; 
					yya[yyac] = new YYARec(301,-141 );yyac++; 
					yya[yyac] = new YYARec(302,-141 );yyac++; 
					yya[yyac] = new YYARec(303,-141 );yyac++; 
					yya[yyac] = new YYARec(258,64);yyac++; 
					yya[yyac] = new YYARec(275,65);yyac++; 
					yya[yyac] = new YYARec(257,-129 );yyac++; 
					yya[yyac] = new YYARec(258,-129 );yyac++; 
					yya[yyac] = new YYARec(290,-129 );yyac++; 
					yya[yyac] = new YYARec(291,-129 );yyac++; 
					yya[yyac] = new YYARec(292,-129 );yyac++; 
					yya[yyac] = new YYARec(293,-129 );yyac++; 
					yya[yyac] = new YYARec(294,-129 );yyac++; 
					yya[yyac] = new YYARec(296,-129 );yyac++; 
					yya[yyac] = new YYARec(297,-129 );yyac++; 
					yya[yyac] = new YYARec(298,-129 );yyac++; 
					yya[yyac] = new YYARec(301,-129 );yyac++; 
					yya[yyac] = new YYARec(302,-129 );yyac++; 
					yya[yyac] = new YYARec(303,-129 );yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(257,68);yyac++; 
					yya[yyac] = new YYARec(258,69);yyac++; 
					yya[yyac] = new YYARec(303,62);yyac++; 
					yya[yyac] = new YYARec(302,61);yyac++; 
					yya[yyac] = new YYARec(299,71);yyac++; 
					yya[yyac] = new YYARec(300,72);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,40);yyac++; 
					yya[yyac] = new YYARec(257,-16 );yyac++; 
					yya[yyac] = new YYARec(257,74);yyac++; 
					yya[yyac] = new YYARec(257,75);yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(299,90);yyac++; 
					yya[yyac] = new YYARec(301,91);yyac++; 
					yya[yyac] = new YYARec(257,92);yyac++; 
					yya[yyac] = new YYARec(257,96);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-32 );yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(299,59);yyac++; 
					yya[yyac] = new YYARec(257,-22 );yyac++; 
					yya[yyac] = new YYARec(257,101);yyac++; 
					yya[yyac] = new YYARec(257,111);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(299,59);yyac++; 
					yya[yyac] = new YYARec(300,60);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(302,61);yyac++; 
					yya[yyac] = new YYARec(303,62);yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(260,115);yyac++; 
					yya[yyac] = new YYARec(259,116);yyac++; 
					yya[yyac] = new YYARec(289,41);yyac++; 
					yya[yyac] = new YYARec(281,-116 );yyac++; 
					yya[yyac] = new YYARec(282,-116 );yyac++; 
					yya[yyac] = new YYARec(283,-116 );yyac++; 
					yya[yyac] = new YYARec(284,-116 );yyac++; 
					yya[yyac] = new YYARec(285,-116 );yyac++; 
					yya[yyac] = new YYARec(257,-120 );yyac++; 
					yya[yyac] = new YYARec(274,-120 );yyac++; 
					yya[yyac] = new YYARec(275,-120 );yyac++; 
					yya[yyac] = new YYARec(279,-120 );yyac++; 
					yya[yyac] = new YYARec(290,-120 );yyac++; 
					yya[yyac] = new YYARec(291,-120 );yyac++; 
					yya[yyac] = new YYARec(292,-120 );yyac++; 
					yya[yyac] = new YYARec(293,-120 );yyac++; 
					yya[yyac] = new YYARec(294,-120 );yyac++; 
					yya[yyac] = new YYARec(295,-120 );yyac++; 
					yya[yyac] = new YYARec(296,-120 );yyac++; 
					yya[yyac] = new YYARec(297,-120 );yyac++; 
					yya[yyac] = new YYARec(298,-120 );yyac++; 
					yya[yyac] = new YYARec(299,-120 );yyac++; 
					yya[yyac] = new YYARec(300,-120 );yyac++; 
					yya[yyac] = new YYARec(301,-120 );yyac++; 
					yya[yyac] = new YYARec(302,-120 );yyac++; 
					yya[yyac] = new YYARec(303,-120 );yyac++; 
					yya[yyac] = new YYARec(260,-151 );yyac++; 
					yya[yyac] = new YYARec(281,118);yyac++; 
					yya[yyac] = new YYARec(282,119);yyac++; 
					yya[yyac] = new YYARec(283,120);yyac++; 
					yya[yyac] = new YYARec(284,121);yyac++; 
					yya[yyac] = new YYARec(285,122);yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(258,147);yyac++; 
					yya[yyac] = new YYARec(257,-119 );yyac++; 
					yya[yyac] = new YYARec(274,-119 );yyac++; 
					yya[yyac] = new YYARec(275,-119 );yyac++; 
					yya[yyac] = new YYARec(279,-119 );yyac++; 
					yya[yyac] = new YYARec(290,-119 );yyac++; 
					yya[yyac] = new YYARec(291,-119 );yyac++; 
					yya[yyac] = new YYARec(292,-119 );yyac++; 
					yya[yyac] = new YYARec(293,-119 );yyac++; 
					yya[yyac] = new YYARec(294,-119 );yyac++; 
					yya[yyac] = new YYARec(295,-119 );yyac++; 
					yya[yyac] = new YYARec(296,-119 );yyac++; 
					yya[yyac] = new YYARec(297,-119 );yyac++; 
					yya[yyac] = new YYARec(298,-119 );yyac++; 
					yya[yyac] = new YYARec(299,-119 );yyac++; 
					yya[yyac] = new YYARec(300,-119 );yyac++; 
					yya[yyac] = new YYARec(301,-119 );yyac++; 
					yya[yyac] = new YYARec(302,-119 );yyac++; 
					yya[yyac] = new YYARec(303,-119 );yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(299,59);yyac++; 
					yya[yyac] = new YYARec(300,60);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(302,61);yyac++; 
					yya[yyac] = new YYARec(303,62);yyac++; 
					yya[yyac] = new YYARec(257,96);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-32 );yyac++; 
					yya[yyac] = new YYARec(259,158);yyac++; 
					yya[yyac] = new YYARec(299,71);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(299,59);yyac++; 
					yya[yyac] = new YYARec(257,-22 );yyac++; 
					yya[yyac] = new YYARec(257,160);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(299,71);yyac++; 
					yya[yyac] = new YYARec(300,72);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(299,59);yyac++; 
					yya[yyac] = new YYARec(300,60);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(302,61);yyac++; 
					yya[yyac] = new YYARec(303,62);yyac++; 
					yya[yyac] = new YYARec(257,-52 );yyac++; 
					yya[yyac] = new YYARec(257,163);yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,166);yyac++; 
					yya[yyac] = new YYARec(266,167);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(276,170);yyac++; 
					yya[yyac] = new YYARec(277,171);yyac++; 
					yya[yyac] = new YYARec(278,172);yyac++; 
					yya[yyac] = new YYARec(257,-74 );yyac++; 
					yya[yyac] = new YYARec(258,-74 );yyac++; 
					yya[yyac] = new YYARec(261,-74 );yyac++; 
					yya[yyac] = new YYARec(262,-74 );yyac++; 
					yya[yyac] = new YYARec(263,-74 );yyac++; 
					yya[yyac] = new YYARec(264,-74 );yyac++; 
					yya[yyac] = new YYARec(265,-74 );yyac++; 
					yya[yyac] = new YYARec(267,-74 );yyac++; 
					yya[yyac] = new YYARec(268,-74 );yyac++; 
					yya[yyac] = new YYARec(269,-74 );yyac++; 
					yya[yyac] = new YYARec(270,-74 );yyac++; 
					yya[yyac] = new YYARec(271,-74 );yyac++; 
					yya[yyac] = new YYARec(272,-74 );yyac++; 
					yya[yyac] = new YYARec(273,-74 );yyac++; 
					yya[yyac] = new YYARec(274,-74 );yyac++; 
					yya[yyac] = new YYARec(275,-74 );yyac++; 
					yya[yyac] = new YYARec(274,174);yyac++; 
					yya[yyac] = new YYARec(275,175);yyac++; 
					yya[yyac] = new YYARec(257,-72 );yyac++; 
					yya[yyac] = new YYARec(258,-72 );yyac++; 
					yya[yyac] = new YYARec(261,-72 );yyac++; 
					yya[yyac] = new YYARec(262,-72 );yyac++; 
					yya[yyac] = new YYARec(263,-72 );yyac++; 
					yya[yyac] = new YYARec(264,-72 );yyac++; 
					yya[yyac] = new YYARec(265,-72 );yyac++; 
					yya[yyac] = new YYARec(267,-72 );yyac++; 
					yya[yyac] = new YYARec(268,-72 );yyac++; 
					yya[yyac] = new YYARec(269,-72 );yyac++; 
					yya[yyac] = new YYARec(270,-72 );yyac++; 
					yya[yyac] = new YYARec(271,-72 );yyac++; 
					yya[yyac] = new YYARec(272,-72 );yyac++; 
					yya[yyac] = new YYARec(273,-72 );yyac++; 
					yya[yyac] = new YYARec(270,177);yyac++; 
					yya[yyac] = new YYARec(271,178);yyac++; 
					yya[yyac] = new YYARec(272,179);yyac++; 
					yya[yyac] = new YYARec(273,180);yyac++; 
					yya[yyac] = new YYARec(257,-70 );yyac++; 
					yya[yyac] = new YYARec(258,-70 );yyac++; 
					yya[yyac] = new YYARec(261,-70 );yyac++; 
					yya[yyac] = new YYARec(262,-70 );yyac++; 
					yya[yyac] = new YYARec(263,-70 );yyac++; 
					yya[yyac] = new YYARec(264,-70 );yyac++; 
					yya[yyac] = new YYARec(265,-70 );yyac++; 
					yya[yyac] = new YYARec(267,-70 );yyac++; 
					yya[yyac] = new YYARec(268,-70 );yyac++; 
					yya[yyac] = new YYARec(269,-70 );yyac++; 
					yya[yyac] = new YYARec(268,182);yyac++; 
					yya[yyac] = new YYARec(269,183);yyac++; 
					yya[yyac] = new YYARec(257,-69 );yyac++; 
					yya[yyac] = new YYARec(258,-69 );yyac++; 
					yya[yyac] = new YYARec(261,-69 );yyac++; 
					yya[yyac] = new YYARec(262,-69 );yyac++; 
					yya[yyac] = new YYARec(263,-69 );yyac++; 
					yya[yyac] = new YYARec(264,-69 );yyac++; 
					yya[yyac] = new YYARec(265,-69 );yyac++; 
					yya[yyac] = new YYARec(267,-69 );yyac++; 
					yya[yyac] = new YYARec(265,184);yyac++; 
					yya[yyac] = new YYARec(257,-67 );yyac++; 
					yya[yyac] = new YYARec(258,-67 );yyac++; 
					yya[yyac] = new YYARec(261,-67 );yyac++; 
					yya[yyac] = new YYARec(262,-67 );yyac++; 
					yya[yyac] = new YYARec(263,-67 );yyac++; 
					yya[yyac] = new YYARec(264,-67 );yyac++; 
					yya[yyac] = new YYARec(267,-67 );yyac++; 
					yya[yyac] = new YYARec(264,185);yyac++; 
					yya[yyac] = new YYARec(257,-65 );yyac++; 
					yya[yyac] = new YYARec(258,-65 );yyac++; 
					yya[yyac] = new YYARec(261,-65 );yyac++; 
					yya[yyac] = new YYARec(262,-65 );yyac++; 
					yya[yyac] = new YYARec(263,-65 );yyac++; 
					yya[yyac] = new YYARec(267,-65 );yyac++; 
					yya[yyac] = new YYARec(263,186);yyac++; 
					yya[yyac] = new YYARec(257,-63 );yyac++; 
					yya[yyac] = new YYARec(258,-63 );yyac++; 
					yya[yyac] = new YYARec(261,-63 );yyac++; 
					yya[yyac] = new YYARec(262,-63 );yyac++; 
					yya[yyac] = new YYARec(267,-63 );yyac++; 
					yya[yyac] = new YYARec(262,187);yyac++; 
					yya[yyac] = new YYARec(257,-61 );yyac++; 
					yya[yyac] = new YYARec(258,-61 );yyac++; 
					yya[yyac] = new YYARec(261,-61 );yyac++; 
					yya[yyac] = new YYARec(267,-61 );yyac++; 
					yya[yyac] = new YYARec(261,188);yyac++; 
					yya[yyac] = new YYARec(257,-59 );yyac++; 
					yya[yyac] = new YYARec(258,-59 );yyac++; 
					yya[yyac] = new YYARec(267,-59 );yyac++; 
					yya[yyac] = new YYARec(281,118);yyac++; 
					yya[yyac] = new YYARec(282,119);yyac++; 
					yya[yyac] = new YYARec(283,120);yyac++; 
					yya[yyac] = new YYARec(284,121);yyac++; 
					yya[yyac] = new YYARec(285,122);yyac++; 
					yya[yyac] = new YYARec(299,190);yyac++; 
					yya[yyac] = new YYARec(257,-85 );yyac++; 
					yya[yyac] = new YYARec(261,-85 );yyac++; 
					yya[yyac] = new YYARec(262,-85 );yyac++; 
					yya[yyac] = new YYARec(263,-85 );yyac++; 
					yya[yyac] = new YYARec(264,-85 );yyac++; 
					yya[yyac] = new YYARec(265,-85 );yyac++; 
					yya[yyac] = new YYARec(268,-85 );yyac++; 
					yya[yyac] = new YYARec(269,-85 );yyac++; 
					yya[yyac] = new YYARec(270,-85 );yyac++; 
					yya[yyac] = new YYARec(271,-85 );yyac++; 
					yya[yyac] = new YYARec(272,-85 );yyac++; 
					yya[yyac] = new YYARec(273,-85 );yyac++; 
					yya[yyac] = new YYARec(274,-85 );yyac++; 
					yya[yyac] = new YYARec(275,-85 );yyac++; 
					yya[yyac] = new YYARec(276,-85 );yyac++; 
					yya[yyac] = new YYARec(277,-85 );yyac++; 
					yya[yyac] = new YYARec(278,-85 );yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,-111 );yyac++; 
					yya[yyac] = new YYARec(257,-136 );yyac++; 
					yya[yyac] = new YYARec(258,-136 );yyac++; 
					yya[yyac] = new YYARec(261,-136 );yyac++; 
					yya[yyac] = new YYARec(262,-136 );yyac++; 
					yya[yyac] = new YYARec(263,-136 );yyac++; 
					yya[yyac] = new YYARec(264,-136 );yyac++; 
					yya[yyac] = new YYARec(265,-136 );yyac++; 
					yya[yyac] = new YYARec(267,-136 );yyac++; 
					yya[yyac] = new YYARec(268,-136 );yyac++; 
					yya[yyac] = new YYARec(269,-136 );yyac++; 
					yya[yyac] = new YYARec(270,-136 );yyac++; 
					yya[yyac] = new YYARec(271,-136 );yyac++; 
					yya[yyac] = new YYARec(272,-136 );yyac++; 
					yya[yyac] = new YYARec(273,-136 );yyac++; 
					yya[yyac] = new YYARec(274,-136 );yyac++; 
					yya[yyac] = new YYARec(275,-136 );yyac++; 
					yya[yyac] = new YYARec(276,-136 );yyac++; 
					yya[yyac] = new YYARec(277,-136 );yyac++; 
					yya[yyac] = new YYARec(278,-136 );yyac++; 
					yya[yyac] = new YYARec(281,-136 );yyac++; 
					yya[yyac] = new YYARec(282,-136 );yyac++; 
					yya[yyac] = new YYARec(283,-136 );yyac++; 
					yya[yyac] = new YYARec(284,-136 );yyac++; 
					yya[yyac] = new YYARec(285,-136 );yyac++; 
					yya[yyac] = new YYARec(289,-136 );yyac++; 
					yya[yyac] = new YYARec(299,-136 );yyac++; 
					yya[yyac] = new YYARec(266,-109 );yyac++; 
					yya[yyac] = new YYARec(257,-131 );yyac++; 
					yya[yyac] = new YYARec(258,-131 );yyac++; 
					yya[yyac] = new YYARec(261,-131 );yyac++; 
					yya[yyac] = new YYARec(262,-131 );yyac++; 
					yya[yyac] = new YYARec(263,-131 );yyac++; 
					yya[yyac] = new YYARec(264,-131 );yyac++; 
					yya[yyac] = new YYARec(265,-131 );yyac++; 
					yya[yyac] = new YYARec(267,-131 );yyac++; 
					yya[yyac] = new YYARec(268,-131 );yyac++; 
					yya[yyac] = new YYARec(269,-131 );yyac++; 
					yya[yyac] = new YYARec(270,-131 );yyac++; 
					yya[yyac] = new YYARec(271,-131 );yyac++; 
					yya[yyac] = new YYARec(272,-131 );yyac++; 
					yya[yyac] = new YYARec(273,-131 );yyac++; 
					yya[yyac] = new YYARec(274,-131 );yyac++; 
					yya[yyac] = new YYARec(275,-131 );yyac++; 
					yya[yyac] = new YYARec(276,-131 );yyac++; 
					yya[yyac] = new YYARec(277,-131 );yyac++; 
					yya[yyac] = new YYARec(278,-131 );yyac++; 
					yya[yyac] = new YYARec(281,-131 );yyac++; 
					yya[yyac] = new YYARec(282,-131 );yyac++; 
					yya[yyac] = new YYARec(283,-131 );yyac++; 
					yya[yyac] = new YYARec(284,-131 );yyac++; 
					yya[yyac] = new YYARec(285,-131 );yyac++; 
					yya[yyac] = new YYARec(289,-131 );yyac++; 
					yya[yyac] = new YYARec(299,-131 );yyac++; 
					yya[yyac] = new YYARec(266,-110 );yyac++; 
					yya[yyac] = new YYARec(257,-132 );yyac++; 
					yya[yyac] = new YYARec(258,-132 );yyac++; 
					yya[yyac] = new YYARec(261,-132 );yyac++; 
					yya[yyac] = new YYARec(262,-132 );yyac++; 
					yya[yyac] = new YYARec(263,-132 );yyac++; 
					yya[yyac] = new YYARec(264,-132 );yyac++; 
					yya[yyac] = new YYARec(265,-132 );yyac++; 
					yya[yyac] = new YYARec(267,-132 );yyac++; 
					yya[yyac] = new YYARec(268,-132 );yyac++; 
					yya[yyac] = new YYARec(269,-132 );yyac++; 
					yya[yyac] = new YYARec(270,-132 );yyac++; 
					yya[yyac] = new YYARec(271,-132 );yyac++; 
					yya[yyac] = new YYARec(272,-132 );yyac++; 
					yya[yyac] = new YYARec(273,-132 );yyac++; 
					yya[yyac] = new YYARec(274,-132 );yyac++; 
					yya[yyac] = new YYARec(275,-132 );yyac++; 
					yya[yyac] = new YYARec(276,-132 );yyac++; 
					yya[yyac] = new YYARec(277,-132 );yyac++; 
					yya[yyac] = new YYARec(278,-132 );yyac++; 
					yya[yyac] = new YYARec(281,-132 );yyac++; 
					yya[yyac] = new YYARec(282,-132 );yyac++; 
					yya[yyac] = new YYARec(283,-132 );yyac++; 
					yya[yyac] = new YYARec(284,-132 );yyac++; 
					yya[yyac] = new YYARec(285,-132 );yyac++; 
					yya[yyac] = new YYARec(289,-132 );yyac++; 
					yya[yyac] = new YYARec(299,-132 );yyac++; 
					yya[yyac] = new YYARec(299,192);yyac++; 
					yya[yyac] = new YYARec(257,-124 );yyac++; 
					yya[yyac] = new YYARec(258,-124 );yyac++; 
					yya[yyac] = new YYARec(261,-124 );yyac++; 
					yya[yyac] = new YYARec(262,-124 );yyac++; 
					yya[yyac] = new YYARec(263,-124 );yyac++; 
					yya[yyac] = new YYARec(264,-124 );yyac++; 
					yya[yyac] = new YYARec(265,-124 );yyac++; 
					yya[yyac] = new YYARec(267,-124 );yyac++; 
					yya[yyac] = new YYARec(268,-124 );yyac++; 
					yya[yyac] = new YYARec(269,-124 );yyac++; 
					yya[yyac] = new YYARec(270,-124 );yyac++; 
					yya[yyac] = new YYARec(271,-124 );yyac++; 
					yya[yyac] = new YYARec(272,-124 );yyac++; 
					yya[yyac] = new YYARec(273,-124 );yyac++; 
					yya[yyac] = new YYARec(274,-124 );yyac++; 
					yya[yyac] = new YYARec(275,-124 );yyac++; 
					yya[yyac] = new YYARec(276,-124 );yyac++; 
					yya[yyac] = new YYARec(277,-124 );yyac++; 
					yya[yyac] = new YYARec(278,-124 );yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(258,194);yyac++; 
					yya[yyac] = new YYARec(299,190);yyac++; 
					yya[yyac] = new YYARec(257,-85 );yyac++; 
					yya[yyac] = new YYARec(258,-85 );yyac++; 
					yya[yyac] = new YYARec(261,-85 );yyac++; 
					yya[yyac] = new YYARec(262,-85 );yyac++; 
					yya[yyac] = new YYARec(263,-85 );yyac++; 
					yya[yyac] = new YYARec(264,-85 );yyac++; 
					yya[yyac] = new YYARec(265,-85 );yyac++; 
					yya[yyac] = new YYARec(267,-85 );yyac++; 
					yya[yyac] = new YYARec(268,-85 );yyac++; 
					yya[yyac] = new YYARec(269,-85 );yyac++; 
					yya[yyac] = new YYARec(270,-85 );yyac++; 
					yya[yyac] = new YYARec(271,-85 );yyac++; 
					yya[yyac] = new YYARec(272,-85 );yyac++; 
					yya[yyac] = new YYARec(273,-85 );yyac++; 
					yya[yyac] = new YYARec(274,-85 );yyac++; 
					yya[yyac] = new YYARec(275,-85 );yyac++; 
					yya[yyac] = new YYARec(276,-85 );yyac++; 
					yya[yyac] = new YYARec(277,-85 );yyac++; 
					yya[yyac] = new YYARec(278,-85 );yyac++; 
					yya[yyac] = new YYARec(258,195);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(299,59);yyac++; 
					yya[yyac] = new YYARec(300,60);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(302,61);yyac++; 
					yya[yyac] = new YYARec(303,62);yyac++; 
					yya[yyac] = new YYARec(257,-35 );yyac++; 
					yya[yyac] = new YYARec(257,197);yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(266,141);yyac++; 
					yya[yyac] = new YYARec(274,56);yyac++; 
					yya[yyac] = new YYARec(275,57);yyac++; 
					yya[yyac] = new YYARec(279,58);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,142);yyac++; 
					yya[yyac] = new YYARec(295,112);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,143);yyac++; 
					yya[yyac] = new YYARec(298,144);yyac++; 
					yya[yyac] = new YYARec(299,145);yyac++; 
					yya[yyac] = new YYARec(300,146);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(267,210);yyac++; 
					yya[yyac] = new YYARec(259,211);yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(257,84);yyac++; 
					yya[yyac] = new YYARec(258,85);yyac++; 
					yya[yyac] = new YYARec(280,86);yyac++; 
					yya[yyac] = new YYARec(286,87);yyac++; 
					yya[yyac] = new YYARec(287,88);yyac++; 
					yya[yyac] = new YYARec(288,89);yyac++; 
					yya[yyac] = new YYARec(290,21);yyac++; 
					yya[yyac] = new YYARec(291,34);yyac++; 
					yya[yyac] = new YYARec(292,35);yyac++; 
					yya[yyac] = new YYARec(293,36);yyac++; 
					yya[yyac] = new YYARec(294,37);yyac++; 
					yya[yyac] = new YYARec(296,38);yyac++; 
					yya[yyac] = new YYARec(297,39);yyac++; 
					yya[yyac] = new YYARec(298,28);yyac++; 
					yya[yyac] = new YYARec(301,29);yyac++; 
					yya[yyac] = new YYARec(259,-47 );yyac++; 
					yya[yyac] = new YYARec(267,214);yyac++; 
					yya[yyac] = new YYARec(276,170);yyac++; 
					yya[yyac] = new YYARec(277,171);yyac++; 
					yya[yyac] = new YYARec(278,172);yyac++; 
					yya[yyac] = new YYARec(257,-75 );yyac++; 
					yya[yyac] = new YYARec(258,-75 );yyac++; 
					yya[yyac] = new YYARec(261,-75 );yyac++; 
					yya[yyac] = new YYARec(262,-75 );yyac++; 
					yya[yyac] = new YYARec(263,-75 );yyac++; 
					yya[yyac] = new YYARec(264,-75 );yyac++; 
					yya[yyac] = new YYARec(265,-75 );yyac++; 
					yya[yyac] = new YYARec(267,-75 );yyac++; 
					yya[yyac] = new YYARec(268,-75 );yyac++; 
					yya[yyac] = new YYARec(269,-75 );yyac++; 
					yya[yyac] = new YYARec(270,-75 );yyac++; 
					yya[yyac] = new YYARec(271,-75 );yyac++; 
					yya[yyac] = new YYARec(272,-75 );yyac++; 
					yya[yyac] = new YYARec(273,-75 );yyac++; 
					yya[yyac] = new YYARec(274,-75 );yyac++; 
					yya[yyac] = new YYARec(275,-75 );yyac++; 
					yya[yyac] = new YYARec(274,174);yyac++; 
					yya[yyac] = new YYARec(275,175);yyac++; 
					yya[yyac] = new YYARec(257,-73 );yyac++; 
					yya[yyac] = new YYARec(258,-73 );yyac++; 
					yya[yyac] = new YYARec(261,-73 );yyac++; 
					yya[yyac] = new YYARec(262,-73 );yyac++; 
					yya[yyac] = new YYARec(263,-73 );yyac++; 
					yya[yyac] = new YYARec(264,-73 );yyac++; 
					yya[yyac] = new YYARec(265,-73 );yyac++; 
					yya[yyac] = new YYARec(267,-73 );yyac++; 
					yya[yyac] = new YYARec(268,-73 );yyac++; 
					yya[yyac] = new YYARec(269,-73 );yyac++; 
					yya[yyac] = new YYARec(270,-73 );yyac++; 
					yya[yyac] = new YYARec(271,-73 );yyac++; 
					yya[yyac] = new YYARec(272,-73 );yyac++; 
					yya[yyac] = new YYARec(273,-73 );yyac++; 
					yya[yyac] = new YYARec(270,177);yyac++; 
					yya[yyac] = new YYARec(271,178);yyac++; 
					yya[yyac] = new YYARec(272,179);yyac++; 
					yya[yyac] = new YYARec(273,180);yyac++; 
					yya[yyac] = new YYARec(257,-71 );yyac++; 
					yya[yyac] = new YYARec(258,-71 );yyac++; 
					yya[yyac] = new YYARec(261,-71 );yyac++; 
					yya[yyac] = new YYARec(262,-71 );yyac++; 
					yya[yyac] = new YYARec(263,-71 );yyac++; 
					yya[yyac] = new YYARec(264,-71 );yyac++; 
					yya[yyac] = new YYARec(265,-71 );yyac++; 
					yya[yyac] = new YYARec(267,-71 );yyac++; 
					yya[yyac] = new YYARec(268,-71 );yyac++; 
					yya[yyac] = new YYARec(269,-71 );yyac++; 
					yya[yyac] = new YYARec(268,182);yyac++; 
					yya[yyac] = new YYARec(269,183);yyac++; 
					yya[yyac] = new YYARec(257,-68 );yyac++; 
					yya[yyac] = new YYARec(258,-68 );yyac++; 
					yya[yyac] = new YYARec(261,-68 );yyac++; 
					yya[yyac] = new YYARec(262,-68 );yyac++; 
					yya[yyac] = new YYARec(263,-68 );yyac++; 
					yya[yyac] = new YYARec(264,-68 );yyac++; 
					yya[yyac] = new YYARec(265,-68 );yyac++; 
					yya[yyac] = new YYARec(267,-68 );yyac++; 
					yya[yyac] = new YYARec(265,184);yyac++; 
					yya[yyac] = new YYARec(257,-66 );yyac++; 
					yya[yyac] = new YYARec(258,-66 );yyac++; 
					yya[yyac] = new YYARec(261,-66 );yyac++; 
					yya[yyac] = new YYARec(262,-66 );yyac++; 
					yya[yyac] = new YYARec(263,-66 );yyac++; 
					yya[yyac] = new YYARec(264,-66 );yyac++; 
					yya[yyac] = new YYARec(267,-66 );yyac++; 
					yya[yyac] = new YYARec(264,185);yyac++; 
					yya[yyac] = new YYARec(257,-64 );yyac++; 
					yya[yyac] = new YYARec(258,-64 );yyac++; 
					yya[yyac] = new YYARec(261,-64 );yyac++; 
					yya[yyac] = new YYARec(262,-64 );yyac++; 
					yya[yyac] = new YYARec(263,-64 );yyac++; 
					yya[yyac] = new YYARec(267,-64 );yyac++; 
					yya[yyac] = new YYARec(263,186);yyac++; 
					yya[yyac] = new YYARec(257,-62 );yyac++; 
					yya[yyac] = new YYARec(258,-62 );yyac++; 
					yya[yyac] = new YYARec(261,-62 );yyac++; 
					yya[yyac] = new YYARec(262,-62 );yyac++; 
					yya[yyac] = new YYARec(267,-62 );yyac++; 
					yya[yyac] = new YYARec(262,187);yyac++; 
					yya[yyac] = new YYARec(257,-60 );yyac++; 
					yya[yyac] = new YYARec(258,-60 );yyac++; 
					yya[yyac] = new YYARec(261,-60 );yyac++; 
					yya[yyac] = new YYARec(267,-60 );yyac++; 
					yya[yyac] = new YYARec(259,215);yyac++; 
					yya[yyac] = new YYARec(259,216);yyac++;

					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,2);yygc++; 
					yyg[yygc] = new YYARec(-31,3);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-25,5);yygc++; 
					yyg[yygc] = new YYARec(-23,6);yygc++; 
					yyg[yygc] = new YYARec(-18,7);yygc++; 
					yyg[yygc] = new YYARec(-13,8);yygc++; 
					yyg[yygc] = new YYARec(-10,9);yygc++; 
					yyg[yygc] = new YYARec(-9,10);yygc++; 
					yyg[yygc] = new YYARec(-8,11);yygc++; 
					yyg[yygc] = new YYARec(-7,12);yygc++; 
					yyg[yygc] = new YYARec(-6,13);yygc++; 
					yyg[yygc] = new YYARec(-5,14);yygc++; 
					yyg[yygc] = new YYARec(-4,15);yygc++; 
					yyg[yygc] = new YYARec(-3,16);yygc++; 
					yyg[yygc] = new YYARec(-2,17);yygc++; 
					yyg[yygc] = new YYARec(-65,30);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-32,32);yygc++; 
					yyg[yygc] = new YYARec(-27,33);yygc++; 
					yyg[yygc] = new YYARec(-65,42);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-27,33);yygc++; 
					yyg[yygc] = new YYARec(-14,43);yygc++; 
					yyg[yygc] = new YYARec(-65,44);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-27,33);yygc++; 
					yyg[yygc] = new YYARec(-19,45);yygc++; 
					yyg[yygc] = new YYARec(-65,42);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-62,47);yygc++; 
					yyg[yygc] = new YYARec(-58,48);yygc++; 
					yyg[yygc] = new YYARec(-27,33);yygc++; 
					yyg[yygc] = new YYARec(-22,49);yygc++; 
					yyg[yygc] = new YYARec(-17,50);yygc++; 
					yyg[yygc] = new YYARec(-16,51);yygc++; 
					yyg[yygc] = new YYARec(-15,52);yygc++; 
					yyg[yygc] = new YYARec(-14,53);yygc++; 
					yyg[yygc] = new YYARec(-12,54);yygc++; 
					yyg[yygc] = new YYARec(-11,55);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,2);yygc++; 
					yyg[yygc] = new YYARec(-31,3);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-25,5);yygc++; 
					yyg[yygc] = new YYARec(-23,6);yygc++; 
					yyg[yygc] = new YYARec(-18,7);yygc++; 
					yyg[yygc] = new YYARec(-13,8);yygc++; 
					yyg[yygc] = new YYARec(-10,9);yygc++; 
					yyg[yygc] = new YYARec(-9,10);yygc++; 
					yyg[yygc] = new YYARec(-8,11);yygc++; 
					yyg[yygc] = new YYARec(-7,12);yygc++; 
					yyg[yygc] = new YYARec(-6,13);yygc++; 
					yyg[yygc] = new YYARec(-5,14);yygc++; 
					yyg[yygc] = new YYARec(-4,15);yygc++; 
					yyg[yygc] = new YYARec(-3,63);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-27,66);yygc++; 
					yyg[yygc] = new YYARec(-15,67);yygc++; 
					yyg[yygc] = new YYARec(-17,70);yygc++; 
					yyg[yygc] = new YYARec(-65,42);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-27,33);yygc++; 
					yyg[yygc] = new YYARec(-14,53);yygc++; 
					yyg[yygc] = new YYARec(-11,73);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,81);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-27,93);yygc++; 
					yyg[yygc] = new YYARec(-26,94);yygc++; 
					yyg[yygc] = new YYARec(-24,95);yygc++; 
					yyg[yygc] = new YYARec(-58,97);yygc++; 
					yyg[yygc] = new YYARec(-22,98);yygc++; 
					yyg[yygc] = new YYARec(-21,99);yygc++; 
					yyg[yygc] = new YYARec(-20,100);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-62,47);yygc++; 
					yyg[yygc] = new YYARec(-58,102);yygc++; 
					yyg[yygc] = new YYARec(-41,103);yygc++; 
					yyg[yygc] = new YYARec(-40,104);yygc++; 
					yyg[yygc] = new YYARec(-38,105);yygc++; 
					yyg[yygc] = new YYARec(-30,106);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-22,49);yygc++; 
					yyg[yygc] = new YYARec(-17,107);yygc++; 
					yyg[yygc] = new YYARec(-16,108);yygc++; 
					yyg[yygc] = new YYARec(-15,109);yygc++; 
					yyg[yygc] = new YYARec(-5,110);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,113);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,114);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-61,117);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,123);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,136);yygc++; 
					yyg[yygc] = new YYARec(-43,137);yygc++; 
					yyg[yygc] = new YYARec(-42,138);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,140);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,136);yygc++; 
					yyg[yygc] = new YYARec(-43,137);yygc++; 
					yyg[yygc] = new YYARec(-42,148);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,136);yygc++; 
					yyg[yygc] = new YYARec(-43,137);yygc++; 
					yyg[yygc] = new YYARec(-42,150);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-62,47);yygc++; 
					yyg[yygc] = new YYARec(-58,102);yygc++; 
					yyg[yygc] = new YYARec(-30,151);yygc++; 
					yyg[yygc] = new YYARec(-29,152);yygc++; 
					yyg[yygc] = new YYARec(-28,153);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-22,49);yygc++; 
					yyg[yygc] = new YYARec(-17,154);yygc++; 
					yyg[yygc] = new YYARec(-16,155);yygc++; 
					yyg[yygc] = new YYARec(-15,156);yygc++; 
					yyg[yygc] = new YYARec(-5,110);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-27,93);yygc++; 
					yyg[yygc] = new YYARec(-26,94);yygc++; 
					yyg[yygc] = new YYARec(-24,157);yygc++; 
					yyg[yygc] = new YYARec(-58,97);yygc++; 
					yyg[yygc] = new YYARec(-22,98);yygc++; 
					yyg[yygc] = new YYARec(-21,99);yygc++; 
					yyg[yygc] = new YYARec(-20,159);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,161);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-62,47);yygc++; 
					yyg[yygc] = new YYARec(-58,102);yygc++; 
					yyg[yygc] = new YYARec(-41,103);yygc++; 
					yyg[yygc] = new YYARec(-40,104);yygc++; 
					yyg[yygc] = new YYARec(-38,162);yygc++; 
					yyg[yygc] = new YYARec(-30,106);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-22,49);yygc++; 
					yyg[yygc] = new YYARec(-17,107);yygc++; 
					yyg[yygc] = new YYARec(-16,108);yygc++; 
					yyg[yygc] = new YYARec(-15,109);yygc++; 
					yyg[yygc] = new YYARec(-5,110);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,164);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,136);yygc++; 
					yyg[yygc] = new YYARec(-43,137);yygc++; 
					yyg[yygc] = new YYARec(-42,165);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,168);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-56,169);yygc++; 
					yyg[yygc] = new YYARec(-54,173);yygc++; 
					yyg[yygc] = new YYARec(-52,176);yygc++; 
					yyg[yygc] = new YYARec(-50,181);yygc++; 
					yyg[yygc] = new YYARec(-61,189);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,136);yygc++; 
					yyg[yygc] = new YYARec(-43,137);yygc++; 
					yyg[yygc] = new YYARec(-42,191);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,193);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-62,47);yygc++; 
					yyg[yygc] = new YYARec(-58,102);yygc++; 
					yyg[yygc] = new YYARec(-30,151);yygc++; 
					yyg[yygc] = new YYARec(-29,152);yygc++; 
					yyg[yygc] = new YYARec(-28,196);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-22,49);yygc++; 
					yyg[yygc] = new YYARec(-17,154);yygc++; 
					yyg[yygc] = new YYARec(-16,155);yygc++; 
					yyg[yygc] = new YYARec(-15,156);yygc++; 
					yyg[yygc] = new YYARec(-5,110);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,198);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,136);yygc++; 
					yyg[yygc] = new YYARec(-43,137);yygc++; 
					yyg[yygc] = new YYARec(-42,199);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,200);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,201);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,202);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,203);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,204);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,205);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,206);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,207);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,208);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-60,124);yygc++; 
					yyg[yygc] = new YYARec(-59,125);yygc++; 
					yyg[yygc] = new YYARec(-58,126);yygc++; 
					yyg[yygc] = new YYARec(-57,127);yygc++; 
					yyg[yygc] = new YYARec(-55,128);yygc++; 
					yyg[yygc] = new YYARec(-53,129);yygc++; 
					yyg[yygc] = new YYARec(-51,130);yygc++; 
					yyg[yygc] = new YYARec(-49,131);yygc++; 
					yyg[yygc] = new YYARec(-48,132);yygc++; 
					yyg[yygc] = new YYARec(-47,133);yygc++; 
					yyg[yygc] = new YYARec(-46,134);yygc++; 
					yyg[yygc] = new YYARec(-45,135);yygc++; 
					yyg[yygc] = new YYARec(-44,136);yygc++; 
					yyg[yygc] = new YYARec(-43,137);yygc++; 
					yyg[yygc] = new YYARec(-42,209);yygc++; 
					yyg[yygc] = new YYARec(-41,139);yygc++; 
					yyg[yygc] = new YYARec(-27,4);yygc++; 
					yyg[yygc] = new YYARec(-5,149);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,212);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-64,1);yygc++; 
					yyg[yygc] = new YYARec(-63,31);yygc++; 
					yyg[yygc] = new YYARec(-39,76);yygc++; 
					yyg[yygc] = new YYARec(-37,77);yygc++; 
					yyg[yygc] = new YYARec(-36,78);yygc++; 
					yyg[yygc] = new YYARec(-35,79);yygc++; 
					yyg[yygc] = new YYARec(-34,80);yygc++; 
					yyg[yygc] = new YYARec(-33,213);yygc++; 
					yyg[yygc] = new YYARec(-27,82);yygc++; 
					yyg[yygc] = new YYARec(-5,83);yygc++; 
					yyg[yygc] = new YYARec(-56,169);yygc++; 
					yyg[yygc] = new YYARec(-54,173);yygc++; 
					yyg[yygc] = new YYARec(-52,176);yygc++; 
					yyg[yygc] = new YYARec(-50,181);yygc++;

					yyd = new int[yynstates];
					yyd[0] = 0;  
					yyd[1] = -140;  
					yyd[2] = 0;  
					yyd[3] = 0;  
					yyd[4] = 0;  
					yyd[5] = -30;  
					yyd[6] = 0;  
					yyd[7] = 0;  
					yyd[8] = -14;  
					yyd[9] = 0;  
					yyd[10] = -11;  
					yyd[11] = -10;  
					yyd[12] = -9;  
					yyd[13] = -8;  
					yyd[14] = -3;  
					yyd[15] = 0;  
					yyd[16] = -1;  
					yyd[17] = 0;  
					yyd[18] = -5;  
					yyd[19] = -6;  
					yyd[20] = -7;  
					yyd[21] = -138;  
					yyd[22] = 0;  
					yyd[23] = 0;  
					yyd[24] = 0;  
					yyd[25] = 0;  
					yyd[26] = 0;  
					yyd[27] = 0;  
					yyd[28] = -132;  
					yyd[29] = -129;  
					yyd[30] = -148;  
					yyd[31] = -139;  
					yyd[32] = 0;  
					yyd[33] = -146;  
					yyd[34] = -134;  
					yyd[35] = -137;  
					yyd[36] = -135;  
					yyd[37] = -136;  
					yyd[38] = -133;  
					yyd[39] = -131;  
					yyd[40] = 0;  
					yyd[41] = 0;  
					yyd[42] = -147;  
					yyd[43] = 0;  
					yyd[44] = -150;  
					yyd[45] = 0;  
					yyd[46] = -149;  
					yyd[47] = -121;  
					yyd[48] = 0;  
					yyd[49] = -122;  
					yyd[50] = -19;  
					yyd[51] = -18;  
					yyd[52] = -17;  
					yyd[53] = 0;  
					yyd[54] = 0;  
					yyd[55] = 0;  
					yyd[56] = -99;  
					yyd[57] = -100;  
					yyd[58] = -98;  
					yyd[59] = -126;  
					yyd[60] = -128;  
					yyd[61] = -152;  
					yyd[62] = -153;  
					yyd[63] = -2;  
					yyd[64] = 0;  
					yyd[65] = 0;  
					yyd[66] = -115;  
					yyd[67] = 0;  
					yyd[68] = -27;  
					yyd[69] = 0;  
					yyd[70] = 0;  
					yyd[71] = -125;  
					yyd[72] = -127;  
					yyd[73] = -15;  
					yyd[74] = -13;  
					yyd[75] = -12;  
					yyd[76] = 0;  
					yyd[77] = 0;  
					yyd[78] = 0;  
					yyd[79] = 0;  
					yyd[80] = 0;  
					yyd[81] = 0;  
					yyd[82] = 0;  
					yyd[83] = 0;  
					yyd[84] = -51;  
					yyd[85] = 0;  
					yyd[86] = 0;  
					yyd[87] = 0;  
					yyd[88] = 0;  
					yyd[89] = 0;  
					yyd[90] = -145;  
					yyd[91] = -144;  
					yyd[92] = -26;  
					yyd[93] = 0;  
					yyd[94] = 0;  
					yyd[95] = 0;  
					yyd[96] = -34;  
					yyd[97] = 0;  
					yyd[98] = -23;  
					yyd[99] = 0;  
					yyd[100] = 0;  
					yyd[101] = -49;  
					yyd[102] = 0;  
					yyd[103] = -55;  
					yyd[104] = 0;  
					yyd[105] = 0;  
					yyd[106] = -54;  
					yyd[107] = -58;  
					yyd[108] = -57;  
					yyd[109] = -56;  
					yyd[110] = -117;  
					yyd[111] = -50;  
					yyd[112] = -130;  
					yyd[113] = -46;  
					yyd[114] = -45;  
					yyd[115] = 0;  
					yyd[116] = -41;  
					yyd[117] = 0;  
					yyd[118] = -104;  
					yyd[119] = -105;  
					yyd[120] = -106;  
					yyd[121] = -107;  
					yyd[122] = -108;  
					yyd[123] = 0;  
					yyd[124] = -86;  
					yyd[125] = 0;  
					yyd[126] = 0;  
					yyd[127] = -78;  
					yyd[128] = -76;  
					yyd[129] = 0;  
					yyd[130] = 0;  
					yyd[131] = 0;  
					yyd[132] = 0;  
					yyd[133] = 0;  
					yyd[134] = 0;  
					yyd[135] = 0;  
					yyd[136] = 0;  
					yyd[137] = 0;  
					yyd[138] = -101;  
					yyd[139] = -84;  
					yyd[140] = 0;  
					yyd[141] = 0;  
					yyd[142] = 0;  
					yyd[143] = 0;  
					yyd[144] = 0;  
					yyd[145] = 0;  
					yyd[146] = -123;  
					yyd[147] = 0;  
					yyd[148] = 0;  
					yyd[149] = 0;  
					yyd[150] = 0;  
					yyd[151] = -38;  
					yyd[152] = 0;  
					yyd[153] = 0;  
					yyd[154] = -40;  
					yyd[155] = -39;  
					yyd[156] = -37;  
					yyd[157] = -31;  
					yyd[158] = -25;  
					yyd[159] = -21;  
					yyd[160] = -20;  
					yyd[161] = -118;  
					yyd[162] = -53;  
					yyd[163] = -48;  
					yyd[164] = -44;  
					yyd[165] = -103;  
					yyd[166] = 0;  
					yyd[167] = 0;  
					yyd[168] = -79;  
					yyd[169] = 0;  
					yyd[170] = -95;  
					yyd[171] = -96;  
					yyd[172] = -97;  
					yyd[173] = 0;  
					yyd[174] = -93;  
					yyd[175] = -94;  
					yyd[176] = 0;  
					yyd[177] = -89;  
					yyd[178] = -90;  
					yyd[179] = -91;  
					yyd[180] = -92;  
					yyd[181] = 0;  
					yyd[182] = -87;  
					yyd[183] = -88;  
					yyd[184] = 0;  
					yyd[185] = 0;  
					yyd[186] = 0;  
					yyd[187] = 0;  
					yyd[188] = 0;  
					yyd[189] = 0;  
					yyd[190] = -83;  
					yyd[191] = 0;  
					yyd[192] = -82;  
					yyd[193] = 0;  
					yyd[194] = 0;  
					yyd[195] = 0;  
					yyd[196] = -36;  
					yyd[197] = -33;  
					yyd[198] = -43;  
					yyd[199] = 0;  
					yyd[200] = -77;  
					yyd[201] = 0;  
					yyd[202] = 0;  
					yyd[203] = 0;  
					yyd[204] = 0;  
					yyd[205] = 0;  
					yyd[206] = 0;  
					yyd[207] = 0;  
					yyd[208] = 0;  
					yyd[209] = -102;  
					yyd[210] = -81;  
					yyd[211] = -112;  
					yyd[212] = 0;  
					yyd[213] = 0;  
					yyd[214] = -80;  
					yyd[215] = -113;  
					yyd[216] = -114; 

					yyal = new int[yynstates];
					yyal[0] = 1;  
					yyal[1] = 14;  
					yyal[2] = 14;  
					yyal[3] = 33;  
					yyal[4] = 42;  
					yyal[5] = 83;  
					yyal[6] = 83;  
					yyal[7] = 92;  
					yyal[8] = 102;  
					yyal[9] = 102;  
					yyal[10] = 119;  
					yyal[11] = 119;  
					yyal[12] = 119;  
					yyal[13] = 119;  
					yyal[14] = 119;  
					yyal[15] = 119;  
					yyal[16] = 132;  
					yyal[17] = 132;  
					yyal[18] = 133;  
					yyal[19] = 133;  
					yyal[20] = 133;  
					yyal[21] = 133;  
					yyal[22] = 133;  
					yyal[23] = 145;  
					yyal[24] = 156;  
					yyal[25] = 167;  
					yyal[26] = 186;  
					yyal[27] = 197;  
					yyal[28] = 216;  
					yyal[29] = 216;  
					yyal[30] = 216;  
					yyal[31] = 216;  
					yyal[32] = 216;  
					yyal[33] = 217;  
					yyal[34] = 217;  
					yyal[35] = 217;  
					yyal[36] = 217;  
					yyal[37] = 217;  
					yyal[38] = 217;  
					yyal[39] = 217;  
					yyal[40] = 217;  
					yyal[41] = 231;  
					yyal[42] = 240;  
					yyal[43] = 240;  
					yyal[44] = 243;  
					yyal[45] = 243;  
					yyal[46] = 244;  
					yyal[47] = 244;  
					yyal[48] = 244;  
					yyal[49] = 246;  
					yyal[50] = 246;  
					yyal[51] = 246;  
					yyal[52] = 246;  
					yyal[53] = 246;  
					yyal[54] = 256;  
					yyal[55] = 257;  
					yyal[56] = 258;  
					yyal[57] = 258;  
					yyal[58] = 258;  
					yyal[59] = 258;  
					yyal[60] = 258;  
					yyal[61] = 258;  
					yyal[62] = 258;  
					yyal[63] = 258;  
					yyal[64] = 258;  
					yyal[65] = 274;  
					yyal[66] = 276;  
					yyal[67] = 276;  
					yyal[68] = 277;  
					yyal[69] = 277;  
					yyal[70] = 288;  
					yyal[71] = 293;  
					yyal[72] = 293;  
					yyal[73] = 293;  
					yyal[74] = 293;  
					yyal[75] = 293;  
					yyal[76] = 293;  
					yyal[77] = 294;  
					yyal[78] = 312;  
					yyal[79] = 328;  
					yyal[80] = 344;  
					yyal[81] = 345;  
					yyal[82] = 346;  
					yyal[83] = 371;  
					yyal[84] = 376;  
					yyal[85] = 376;  
					yyal[86] = 392;  
					yyal[87] = 408;  
					yyal[88] = 427;  
					yyal[89] = 443;  
					yyal[90] = 459;  
					yyal[91] = 459;  
					yyal[92] = 459;  
					yyal[93] = 459;  
					yyal[94] = 475;  
					yyal[95] = 486;  
					yyal[96] = 487;  
					yyal[97] = 487;  
					yyal[98] = 488;  
					yyal[99] = 488;  
					yyal[100] = 493;  
					yyal[101] = 494;  
					yyal[102] = 494;  
					yyal[103] = 505;  
					yyal[104] = 505;  
					yyal[105] = 523;  
					yyal[106] = 524;  
					yyal[107] = 524;  
					yyal[108] = 524;  
					yyal[109] = 524;  
					yyal[110] = 524;  
					yyal[111] = 524;  
					yyal[112] = 524;  
					yyal[113] = 524;  
					yyal[114] = 524;  
					yyal[115] = 524;  
					yyal[116] = 540;  
					yyal[117] = 540;  
					yyal[118] = 556;  
					yyal[119] = 556;  
					yyal[120] = 556;  
					yyal[121] = 556;  
					yyal[122] = 556;  
					yyal[123] = 556;  
					yyal[124] = 557;  
					yyal[125] = 557;  
					yyal[126] = 558;  
					yyal[127] = 574;  
					yyal[128] = 574;  
					yyal[129] = 574;  
					yyal[130] = 593;  
					yyal[131] = 609;  
					yyal[132] = 623;  
					yyal[133] = 633;  
					yyal[134] = 641;  
					yyal[135] = 648;  
					yyal[136] = 654;  
					yyal[137] = 659;  
					yyal[138] = 663;  
					yyal[139] = 663;  
					yyal[140] = 663;  
					yyal[141] = 686;  
					yyal[142] = 702;  
					yyal[143] = 729;  
					yyal[144] = 756;  
					yyal[145] = 783;  
					yyal[146] = 803;  
					yyal[147] = 803;  
					yyal[148] = 819;  
					yyal[149] = 820;  
					yyal[150] = 840;  
					yyal[151] = 841;  
					yyal[152] = 841;  
					yyal[153] = 858;  
					yyal[154] = 859;  
					yyal[155] = 859;  
					yyal[156] = 859;  
					yyal[157] = 859;  
					yyal[158] = 859;  
					yyal[159] = 859;  
					yyal[160] = 859;  
					yyal[161] = 859;  
					yyal[162] = 859;  
					yyal[163] = 859;  
					yyal[164] = 859;  
					yyal[165] = 859;  
					yyal[166] = 859;  
					yyal[167] = 875;  
					yyal[168] = 891;  
					yyal[169] = 891;  
					yyal[170] = 907;  
					yyal[171] = 907;  
					yyal[172] = 907;  
					yyal[173] = 907;  
					yyal[174] = 923;  
					yyal[175] = 923;  
					yyal[176] = 923;  
					yyal[177] = 939;  
					yyal[178] = 939;  
					yyal[179] = 939;  
					yyal[180] = 939;  
					yyal[181] = 939;  
					yyal[182] = 955;  
					yyal[183] = 955;  
					yyal[184] = 955;  
					yyal[185] = 971;  
					yyal[186] = 987;  
					yyal[187] = 1003;  
					yyal[188] = 1019;  
					yyal[189] = 1035;  
					yyal[190] = 1051;  
					yyal[191] = 1051;  
					yyal[192] = 1052;  
					yyal[193] = 1052;  
					yyal[194] = 1053;  
					yyal[195] = 1069;  
					yyal[196] = 1085;  
					yyal[197] = 1085;  
					yyal[198] = 1085;  
					yyal[199] = 1085;  
					yyal[200] = 1086;  
					yyal[201] = 1086;  
					yyal[202] = 1105;  
					yyal[203] = 1121;  
					yyal[204] = 1135;  
					yyal[205] = 1145;  
					yyal[206] = 1153;  
					yyal[207] = 1160;  
					yyal[208] = 1166;  
					yyal[209] = 1171;  
					yyal[210] = 1171;  
					yyal[211] = 1171;  
					yyal[212] = 1171;  
					yyal[213] = 1172;  
					yyal[214] = 1173;  
					yyal[215] = 1173;  
					yyal[216] = 1173; 

					yyah = new int[yynstates];
					yyah[0] = 13;  
					yyah[1] = 13;  
					yyah[2] = 32;  
					yyah[3] = 41;  
					yyah[4] = 82;  
					yyah[5] = 82;  
					yyah[6] = 91;  
					yyah[7] = 101;  
					yyah[8] = 101;  
					yyah[9] = 118;  
					yyah[10] = 118;  
					yyah[11] = 118;  
					yyah[12] = 118;  
					yyah[13] = 118;  
					yyah[14] = 118;  
					yyah[15] = 131;  
					yyah[16] = 131;  
					yyah[17] = 132;  
					yyah[18] = 132;  
					yyah[19] = 132;  
					yyah[20] = 132;  
					yyah[21] = 132;  
					yyah[22] = 144;  
					yyah[23] = 155;  
					yyah[24] = 166;  
					yyah[25] = 185;  
					yyah[26] = 196;  
					yyah[27] = 215;  
					yyah[28] = 215;  
					yyah[29] = 215;  
					yyah[30] = 215;  
					yyah[31] = 215;  
					yyah[32] = 216;  
					yyah[33] = 216;  
					yyah[34] = 216;  
					yyah[35] = 216;  
					yyah[36] = 216;  
					yyah[37] = 216;  
					yyah[38] = 216;  
					yyah[39] = 216;  
					yyah[40] = 230;  
					yyah[41] = 239;  
					yyah[42] = 239;  
					yyah[43] = 242;  
					yyah[44] = 242;  
					yyah[45] = 243;  
					yyah[46] = 243;  
					yyah[47] = 243;  
					yyah[48] = 245;  
					yyah[49] = 245;  
					yyah[50] = 245;  
					yyah[51] = 245;  
					yyah[52] = 245;  
					yyah[53] = 255;  
					yyah[54] = 256;  
					yyah[55] = 257;  
					yyah[56] = 257;  
					yyah[57] = 257;  
					yyah[58] = 257;  
					yyah[59] = 257;  
					yyah[60] = 257;  
					yyah[61] = 257;  
					yyah[62] = 257;  
					yyah[63] = 257;  
					yyah[64] = 273;  
					yyah[65] = 275;  
					yyah[66] = 275;  
					yyah[67] = 276;  
					yyah[68] = 276;  
					yyah[69] = 287;  
					yyah[70] = 292;  
					yyah[71] = 292;  
					yyah[72] = 292;  
					yyah[73] = 292;  
					yyah[74] = 292;  
					yyah[75] = 292;  
					yyah[76] = 293;  
					yyah[77] = 311;  
					yyah[78] = 327;  
					yyah[79] = 343;  
					yyah[80] = 344;  
					yyah[81] = 345;  
					yyah[82] = 370;  
					yyah[83] = 375;  
					yyah[84] = 375;  
					yyah[85] = 391;  
					yyah[86] = 407;  
					yyah[87] = 426;  
					yyah[88] = 442;  
					yyah[89] = 458;  
					yyah[90] = 458;  
					yyah[91] = 458;  
					yyah[92] = 458;  
					yyah[93] = 474;  
					yyah[94] = 485;  
					yyah[95] = 486;  
					yyah[96] = 486;  
					yyah[97] = 487;  
					yyah[98] = 487;  
					yyah[99] = 492;  
					yyah[100] = 493;  
					yyah[101] = 493;  
					yyah[102] = 504;  
					yyah[103] = 504;  
					yyah[104] = 522;  
					yyah[105] = 523;  
					yyah[106] = 523;  
					yyah[107] = 523;  
					yyah[108] = 523;  
					yyah[109] = 523;  
					yyah[110] = 523;  
					yyah[111] = 523;  
					yyah[112] = 523;  
					yyah[113] = 523;  
					yyah[114] = 523;  
					yyah[115] = 539;  
					yyah[116] = 539;  
					yyah[117] = 555;  
					yyah[118] = 555;  
					yyah[119] = 555;  
					yyah[120] = 555;  
					yyah[121] = 555;  
					yyah[122] = 555;  
					yyah[123] = 556;  
					yyah[124] = 556;  
					yyah[125] = 557;  
					yyah[126] = 573;  
					yyah[127] = 573;  
					yyah[128] = 573;  
					yyah[129] = 592;  
					yyah[130] = 608;  
					yyah[131] = 622;  
					yyah[132] = 632;  
					yyah[133] = 640;  
					yyah[134] = 647;  
					yyah[135] = 653;  
					yyah[136] = 658;  
					yyah[137] = 662;  
					yyah[138] = 662;  
					yyah[139] = 662;  
					yyah[140] = 685;  
					yyah[141] = 701;  
					yyah[142] = 728;  
					yyah[143] = 755;  
					yyah[144] = 782;  
					yyah[145] = 802;  
					yyah[146] = 802;  
					yyah[147] = 818;  
					yyah[148] = 819;  
					yyah[149] = 839;  
					yyah[150] = 840;  
					yyah[151] = 840;  
					yyah[152] = 857;  
					yyah[153] = 858;  
					yyah[154] = 858;  
					yyah[155] = 858;  
					yyah[156] = 858;  
					yyah[157] = 858;  
					yyah[158] = 858;  
					yyah[159] = 858;  
					yyah[160] = 858;  
					yyah[161] = 858;  
					yyah[162] = 858;  
					yyah[163] = 858;  
					yyah[164] = 858;  
					yyah[165] = 858;  
					yyah[166] = 874;  
					yyah[167] = 890;  
					yyah[168] = 890;  
					yyah[169] = 906;  
					yyah[170] = 906;  
					yyah[171] = 906;  
					yyah[172] = 906;  
					yyah[173] = 922;  
					yyah[174] = 922;  
					yyah[175] = 922;  
					yyah[176] = 938;  
					yyah[177] = 938;  
					yyah[178] = 938;  
					yyah[179] = 938;  
					yyah[180] = 938;  
					yyah[181] = 954;  
					yyah[182] = 954;  
					yyah[183] = 954;  
					yyah[184] = 970;  
					yyah[185] = 986;  
					yyah[186] = 1002;  
					yyah[187] = 1018;  
					yyah[188] = 1034;  
					yyah[189] = 1050;  
					yyah[190] = 1050;  
					yyah[191] = 1051;  
					yyah[192] = 1051;  
					yyah[193] = 1052;  
					yyah[194] = 1068;  
					yyah[195] = 1084;  
					yyah[196] = 1084;  
					yyah[197] = 1084;  
					yyah[198] = 1084;  
					yyah[199] = 1085;  
					yyah[200] = 1085;  
					yyah[201] = 1104;  
					yyah[202] = 1120;  
					yyah[203] = 1134;  
					yyah[204] = 1144;  
					yyah[205] = 1152;  
					yyah[206] = 1159;  
					yyah[207] = 1165;  
					yyah[208] = 1170;  
					yyah[209] = 1170;  
					yyah[210] = 1170;  
					yyah[211] = 1170;  
					yyah[212] = 1171;  
					yyah[213] = 1172;  
					yyah[214] = 1172;  
					yyah[215] = 1172;  
					yyah[216] = 1172; 

					yygl = new int[yynstates];
					yygl[0] = 1;  
					yygl[1] = 18;  
					yygl[2] = 18;  
					yygl[3] = 18;  
					yygl[4] = 23;  
					yygl[5] = 23;  
					yygl[6] = 23;  
					yygl[7] = 28;  
					yygl[8] = 33;  
					yygl[9] = 33;  
					yygl[10] = 46;  
					yygl[11] = 46;  
					yygl[12] = 46;  
					yygl[13] = 46;  
					yygl[14] = 46;  
					yygl[15] = 46;  
					yygl[16] = 62;  
					yygl[17] = 62;  
					yygl[18] = 62;  
					yygl[19] = 62;  
					yygl[20] = 62;  
					yygl[21] = 62;  
					yygl[22] = 62;  
					yygl[23] = 62;  
					yygl[24] = 62;  
					yygl[25] = 62;  
					yygl[26] = 62;  
					yygl[27] = 62;  
					yygl[28] = 62;  
					yygl[29] = 62;  
					yygl[30] = 62;  
					yygl[31] = 62;  
					yygl[32] = 62;  
					yygl[33] = 62;  
					yygl[34] = 62;  
					yygl[35] = 62;  
					yygl[36] = 62;  
					yygl[37] = 62;  
					yygl[38] = 62;  
					yygl[39] = 62;  
					yygl[40] = 62;  
					yygl[41] = 62;  
					yygl[42] = 65;  
					yygl[43] = 65;  
					yygl[44] = 66;  
					yygl[45] = 66;  
					yygl[46] = 67;  
					yygl[47] = 67;  
					yygl[48] = 67;  
					yygl[49] = 67;  
					yygl[50] = 67;  
					yygl[51] = 67;  
					yygl[52] = 67;  
					yygl[53] = 67;  
					yygl[54] = 73;  
					yygl[55] = 73;  
					yygl[56] = 73;  
					yygl[57] = 73;  
					yygl[58] = 73;  
					yygl[59] = 73;  
					yygl[60] = 73;  
					yygl[61] = 73;  
					yygl[62] = 73;  
					yygl[63] = 73;  
					yygl[64] = 73;  
					yygl[65] = 83;  
					yygl[66] = 83;  
					yygl[67] = 83;  
					yygl[68] = 83;  
					yygl[69] = 83;  
					yygl[70] = 88;  
					yygl[71] = 92;  
					yygl[72] = 92;  
					yygl[73] = 92;  
					yygl[74] = 92;  
					yygl[75] = 92;  
					yygl[76] = 92;  
					yygl[77] = 92;  
					yygl[78] = 106;  
					yygl[79] = 116;  
					yygl[80] = 126;  
					yygl[81] = 126;  
					yygl[82] = 126;  
					yygl[83] = 126;  
					yygl[84] = 127;  
					yygl[85] = 127;  
					yygl[86] = 137;  
					yygl[87] = 157;  
					yygl[88] = 157;  
					yygl[89] = 177;  
					yygl[90] = 197;  
					yygl[91] = 197;  
					yygl[92] = 197;  
					yygl[93] = 197;  
					yygl[94] = 210;  
					yygl[95] = 215;  
					yygl[96] = 215;  
					yygl[97] = 215;  
					yygl[98] = 215;  
					yygl[99] = 215;  
					yygl[100] = 219;  
					yygl[101] = 219;  
					yygl[102] = 219;  
					yygl[103] = 223;  
					yygl[104] = 223;  
					yygl[105] = 237;  
					yygl[106] = 237;  
					yygl[107] = 237;  
					yygl[108] = 237;  
					yygl[109] = 237;  
					yygl[110] = 237;  
					yygl[111] = 237;  
					yygl[112] = 237;  
					yygl[113] = 237;  
					yygl[114] = 237;  
					yygl[115] = 237;  
					yygl[116] = 247;  
					yygl[117] = 247;  
					yygl[118] = 267;  
					yygl[119] = 267;  
					yygl[120] = 267;  
					yygl[121] = 267;  
					yygl[122] = 267;  
					yygl[123] = 267;  
					yygl[124] = 267;  
					yygl[125] = 267;  
					yygl[126] = 267;  
					yygl[127] = 277;  
					yygl[128] = 277;  
					yygl[129] = 277;  
					yygl[130] = 278;  
					yygl[131] = 279;  
					yygl[132] = 280;  
					yygl[133] = 281;  
					yygl[134] = 281;  
					yygl[135] = 281;  
					yygl[136] = 281;  
					yygl[137] = 281;  
					yygl[138] = 281;  
					yygl[139] = 281;  
					yygl[140] = 281;  
					yygl[141] = 282;  
					yygl[142] = 302;  
					yygl[143] = 302;  
					yygl[144] = 302;  
					yygl[145] = 302;  
					yygl[146] = 302;  
					yygl[147] = 302;  
					yygl[148] = 312;  
					yygl[149] = 312;  
					yygl[150] = 312;  
					yygl[151] = 312;  
					yygl[152] = 312;  
					yygl[153] = 325;  
					yygl[154] = 325;  
					yygl[155] = 325;  
					yygl[156] = 325;  
					yygl[157] = 325;  
					yygl[158] = 325;  
					yygl[159] = 325;  
					yygl[160] = 325;  
					yygl[161] = 325;  
					yygl[162] = 325;  
					yygl[163] = 325;  
					yygl[164] = 325;  
					yygl[165] = 325;  
					yygl[166] = 325;  
					yygl[167] = 335;  
					yygl[168] = 355;  
					yygl[169] = 355;  
					yygl[170] = 365;  
					yygl[171] = 365;  
					yygl[172] = 365;  
					yygl[173] = 365;  
					yygl[174] = 376;  
					yygl[175] = 376;  
					yygl[176] = 376;  
					yygl[177] = 388;  
					yygl[178] = 388;  
					yygl[179] = 388;  
					yygl[180] = 388;  
					yygl[181] = 388;  
					yygl[182] = 401;  
					yygl[183] = 401;  
					yygl[184] = 401;  
					yygl[185] = 415;  
					yygl[186] = 430;  
					yygl[187] = 446;  
					yygl[188] = 463;  
					yygl[189] = 481;  
					yygl[190] = 501;  
					yygl[191] = 501;  
					yygl[192] = 501;  
					yygl[193] = 501;  
					yygl[194] = 501;  
					yygl[195] = 511;  
					yygl[196] = 521;  
					yygl[197] = 521;  
					yygl[198] = 521;  
					yygl[199] = 521;  
					yygl[200] = 521;  
					yygl[201] = 521;  
					yygl[202] = 522;  
					yygl[203] = 523;  
					yygl[204] = 524;  
					yygl[205] = 525;  
					yygl[206] = 525;  
					yygl[207] = 525;  
					yygl[208] = 525;  
					yygl[209] = 525;  
					yygl[210] = 525;  
					yygl[211] = 525;  
					yygl[212] = 525;  
					yygl[213] = 525;  
					yygl[214] = 525;  
					yygl[215] = 525;  
					yygl[216] = 525; 

					yygh = new int[yynstates];
					yygh[0] = 17;  
					yygh[1] = 17;  
					yygh[2] = 17;  
					yygh[3] = 22;  
					yygh[4] = 22;  
					yygh[5] = 22;  
					yygh[6] = 27;  
					yygh[7] = 32;  
					yygh[8] = 32;  
					yygh[9] = 45;  
					yygh[10] = 45;  
					yygh[11] = 45;  
					yygh[12] = 45;  
					yygh[13] = 45;  
					yygh[14] = 45;  
					yygh[15] = 61;  
					yygh[16] = 61;  
					yygh[17] = 61;  
					yygh[18] = 61;  
					yygh[19] = 61;  
					yygh[20] = 61;  
					yygh[21] = 61;  
					yygh[22] = 61;  
					yygh[23] = 61;  
					yygh[24] = 61;  
					yygh[25] = 61;  
					yygh[26] = 61;  
					yygh[27] = 61;  
					yygh[28] = 61;  
					yygh[29] = 61;  
					yygh[30] = 61;  
					yygh[31] = 61;  
					yygh[32] = 61;  
					yygh[33] = 61;  
					yygh[34] = 61;  
					yygh[35] = 61;  
					yygh[36] = 61;  
					yygh[37] = 61;  
					yygh[38] = 61;  
					yygh[39] = 61;  
					yygh[40] = 61;  
					yygh[41] = 64;  
					yygh[42] = 64;  
					yygh[43] = 65;  
					yygh[44] = 65;  
					yygh[45] = 66;  
					yygh[46] = 66;  
					yygh[47] = 66;  
					yygh[48] = 66;  
					yygh[49] = 66;  
					yygh[50] = 66;  
					yygh[51] = 66;  
					yygh[52] = 66;  
					yygh[53] = 72;  
					yygh[54] = 72;  
					yygh[55] = 72;  
					yygh[56] = 72;  
					yygh[57] = 72;  
					yygh[58] = 72;  
					yygh[59] = 72;  
					yygh[60] = 72;  
					yygh[61] = 72;  
					yygh[62] = 72;  
					yygh[63] = 72;  
					yygh[64] = 82;  
					yygh[65] = 82;  
					yygh[66] = 82;  
					yygh[67] = 82;  
					yygh[68] = 82;  
					yygh[69] = 87;  
					yygh[70] = 91;  
					yygh[71] = 91;  
					yygh[72] = 91;  
					yygh[73] = 91;  
					yygh[74] = 91;  
					yygh[75] = 91;  
					yygh[76] = 91;  
					yygh[77] = 105;  
					yygh[78] = 115;  
					yygh[79] = 125;  
					yygh[80] = 125;  
					yygh[81] = 125;  
					yygh[82] = 125;  
					yygh[83] = 126;  
					yygh[84] = 126;  
					yygh[85] = 136;  
					yygh[86] = 156;  
					yygh[87] = 156;  
					yygh[88] = 176;  
					yygh[89] = 196;  
					yygh[90] = 196;  
					yygh[91] = 196;  
					yygh[92] = 196;  
					yygh[93] = 209;  
					yygh[94] = 214;  
					yygh[95] = 214;  
					yygh[96] = 214;  
					yygh[97] = 214;  
					yygh[98] = 214;  
					yygh[99] = 218;  
					yygh[100] = 218;  
					yygh[101] = 218;  
					yygh[102] = 222;  
					yygh[103] = 222;  
					yygh[104] = 236;  
					yygh[105] = 236;  
					yygh[106] = 236;  
					yygh[107] = 236;  
					yygh[108] = 236;  
					yygh[109] = 236;  
					yygh[110] = 236;  
					yygh[111] = 236;  
					yygh[112] = 236;  
					yygh[113] = 236;  
					yygh[114] = 236;  
					yygh[115] = 246;  
					yygh[116] = 246;  
					yygh[117] = 266;  
					yygh[118] = 266;  
					yygh[119] = 266;  
					yygh[120] = 266;  
					yygh[121] = 266;  
					yygh[122] = 266;  
					yygh[123] = 266;  
					yygh[124] = 266;  
					yygh[125] = 266;  
					yygh[126] = 276;  
					yygh[127] = 276;  
					yygh[128] = 276;  
					yygh[129] = 277;  
					yygh[130] = 278;  
					yygh[131] = 279;  
					yygh[132] = 280;  
					yygh[133] = 280;  
					yygh[134] = 280;  
					yygh[135] = 280;  
					yygh[136] = 280;  
					yygh[137] = 280;  
					yygh[138] = 280;  
					yygh[139] = 280;  
					yygh[140] = 281;  
					yygh[141] = 301;  
					yygh[142] = 301;  
					yygh[143] = 301;  
					yygh[144] = 301;  
					yygh[145] = 301;  
					yygh[146] = 301;  
					yygh[147] = 311;  
					yygh[148] = 311;  
					yygh[149] = 311;  
					yygh[150] = 311;  
					yygh[151] = 311;  
					yygh[152] = 324;  
					yygh[153] = 324;  
					yygh[154] = 324;  
					yygh[155] = 324;  
					yygh[156] = 324;  
					yygh[157] = 324;  
					yygh[158] = 324;  
					yygh[159] = 324;  
					yygh[160] = 324;  
					yygh[161] = 324;  
					yygh[162] = 324;  
					yygh[163] = 324;  
					yygh[164] = 324;  
					yygh[165] = 324;  
					yygh[166] = 334;  
					yygh[167] = 354;  
					yygh[168] = 354;  
					yygh[169] = 364;  
					yygh[170] = 364;  
					yygh[171] = 364;  
					yygh[172] = 364;  
					yygh[173] = 375;  
					yygh[174] = 375;  
					yygh[175] = 375;  
					yygh[176] = 387;  
					yygh[177] = 387;  
					yygh[178] = 387;  
					yygh[179] = 387;  
					yygh[180] = 387;  
					yygh[181] = 400;  
					yygh[182] = 400;  
					yygh[183] = 400;  
					yygh[184] = 414;  
					yygh[185] = 429;  
					yygh[186] = 445;  
					yygh[187] = 462;  
					yygh[188] = 480;  
					yygh[189] = 500;  
					yygh[190] = 500;  
					yygh[191] = 500;  
					yygh[192] = 500;  
					yygh[193] = 500;  
					yygh[194] = 510;  
					yygh[195] = 520;  
					yygh[196] = 520;  
					yygh[197] = 520;  
					yygh[198] = 520;  
					yygh[199] = 520;  
					yygh[200] = 520;  
					yygh[201] = 521;  
					yygh[202] = 522;  
					yygh[203] = 523;  
					yygh[204] = 524;  
					yygh[205] = 524;  
					yygh[206] = 524;  
					yygh[207] = 524;  
					yygh[208] = 524;  
					yygh[209] = 524;  
					yygh[210] = 524;  
					yygh[211] = 524;  
					yygh[212] = 524;  
					yygh[213] = 524;  
					yygh[214] = 524;  
					yygh[215] = 524;  
					yygh[216] = 524; 

					yyr[yyrc] = new YYRRec(1,-2);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-3);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-3);yyrc++; 
					yyr[yyrc] = new YYRRec(0,-3);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-4);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-4);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-4);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-4);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-4);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-4);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-4);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-6);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-6);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-10);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-11);yyrc++; 
					yyr[yyrc] = new YYRRec(0,-11);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-12);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-12);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-12);yyrc++; 
					yyr[yyrc] = new YYRRec(5,-9);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-20);yyrc++; 
					yyr[yyrc] = new YYRRec(0,-20);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-21);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-18);yyrc++; 
					yyr[yyrc] = new YYRRec(5,-7);yyrc++; 
					yyr[yyrc] = new YYRRec(4,-7);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-7);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-25);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-23);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-23);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-24);yyrc++; 
					yyr[yyrc] = new YYRRec(0,-24);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-26);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-26);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-28);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-28);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-29);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-29);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-29);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-29);yyrc++; 
					yyr[yyrc] = new YYRRec(5,-8);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-31);yyrc++; 
					yyr[yyrc] = new YYRRec(4,-33);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-33);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-33);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-33);yyrc++; 
					yyr[yyrc] = new YYRRec(0,-33);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-36);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-36);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-36);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-36);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-38);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-38);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-40);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-40);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-40);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-40);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-40);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-42);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-43);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-43);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-44);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-44);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-45);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-45);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-46);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-46);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-47);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-47);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-48);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-48);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-49);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-49);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-51);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-51);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-53);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-53);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-55);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-55);yyrc++; 
					yyr[yyrc] = new YYRRec(4,-57);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-57);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-57);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-57);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-57);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-57);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-57);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-50);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-50);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-52);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-52);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-52);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-52);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-54);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-54);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-56);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-56);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-56);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-58);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-58);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-58);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-39);yyrc++; 
					yyr[yyrc] = new YYRRec(4,-39);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-39);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-61);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-61);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-61);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-61);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-61);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-59);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-59);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-59);yyrc++; 
					yyr[yyrc] = new YYRRec(4,-35);yyrc++; 
					yyr[yyrc] = new YYRRec(5,-35);yyrc++; 
					yyr[yyrc] = new YYRRec(5,-35);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-5);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-5);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-30);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-30);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-37);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-37);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-16);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-16);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-60);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-60);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-22);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-22);yyrc++; 
					yyr[yyrc] = new YYRRec(2,-62);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-62);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-63);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-41);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-64);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-64);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-64);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-64);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-64);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-64);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-64);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-27);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-27);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-27);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-13);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-13);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-13);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-65);yyrc++; 
					yyr[yyrc] = new YYRRec(3,-65);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-65);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-14);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-32);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-19);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-19);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-34);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-17);yyrc++; 
					yyr[yyrc] = new YYRRec(1,-15);yyrc++;
        }

        public bool yyact(int state, int sym, ref int act)
        {
            int k = yyal[state];
            while (k <= yyah[state] && yya[k].sym != sym) k++;
            if (k > yyah[state]) return false;
            act = yya[k].act;
            return true;
        }
        public bool yygoto(int state, int sym, ref int nstate)
        {
            int k = yygl[state];
            while (k <= yygh[state] && yyg[k].sym != sym) k++;
            if (k > yygh[state]) return false;
            nstate = yyg[k].act;
            return true;
        }

        public void yyerror(string s)
        {
            System.Console.Write(s);
        }

        int yylexpos = -1;
        //string yylval = "";
        Node yylval = new Node();

        public int yylex()
        {
            yylexpos++;
            if (yylexpos >= TokenList.Count)
            {
                //yylval = "";
                yylval = new Node();
                return 0;
            }
            else
            {
                //yylval = ((AToken)TokenList[yylexpos]).val;
                yylval = new Node(((AToken)TokenList[yylexpos]).val);
                return ((AToken)TokenList[yylexpos]).token;
            }
        }

        public bool yyparse()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            parse:

            yysp++;
            if (yysp >= yymaxdepth)
            {
                yyerror("yyparse stack overflow");
                goto abort;
            }

            yys[yysp] = yystate;
            yyv[yysp] = yyval;

            next:

            if (yyd[yystate] == 0 && yychar == -1)
            {
                yychar = yylex();
                if (yychar < 0) yychar = 0;
            }

            yyn = yyd[yystate];
            if (yyn != 0) goto reduce;


            if (! yyact(yystate, yychar, ref yyn)) goto error;
            else if (yyn>0) goto shift;
            else if (yyn<0) goto reduce;
            else            goto accept;

            error:

            if (yyerrflag==0) yyerror("syntax error " + yylval);

            errlab:

            if (yyerrflag==0) yynerrs++;

            if (yyerrflag<=2)
            {
                yyerrflag = 3;
                while (yysp > 0 && !(yyact(yys[yysp], 255, ref yyn) && yyn > 0)) yysp--;

                if (yysp == 0) goto abort;
                yystate = yyn;
                goto parse;
            }
            else
            {
                if (yychar == 0) goto abort;
                yychar = -1; goto next;
            }

            shift:

            yystate = yyn;
            yychar = -1;
            yyval = yylval;
            if (yyerrflag > 0) yyerrflag--;
            goto parse;

            reduce:

            yyflag = yyfnone;
            yyaction(-yyn);
            int l = yyr[-yyn].len;
            for (int z = yysp; z > yysp - l; z--)
                yyv[z] = null;

            yysp -= l;
            //yysp -= yyr[-yyn].len;

            if (yygoto(yys[yysp], yyr[-yyn].sym, ref yyn)) yystate = yyn;

            switch (yyflag)
            {
                case 1: goto accept;
                case 2: goto abort;
                case 3: goto errlab;
            }

            goto parse;

            accept:

            Console.WriteLine("(I) PARSER parsing finished in " + watch.Elapsed);
            watch.Stop();
            return true;

            abort:

            Console.WriteLine("(E) PARSER parsing aborted.");
            watch.Stop();
            return false;
        }
        ////////////////////////////////////////////////////////////////
        /// Scanner - Optimized
        ////////////////////////////////////////////////////////////////

        public bool ScannerOpt(string Input)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (Input.Length == 0) return true;
            TokenList = new ArrayList();
            int pos = 0;
            while (1 == 1)
            {
                AToken lasttoken = FindTokenOpt(Input, pos);
                if (lasttoken.token == 0) break;
                if (lasttoken.token != t_ignore) TokenList.Add(lasttoken);
                pos += lasttoken.val.Length;
                if (Input.Length <= pos)
                {
                    Console.WriteLine("(I) PARSER scanning finished in " + watch.Elapsed);
                    return true;
                }
            }
            System.Console.WriteLine(Input);
            System.Console.WriteLine();
            System.Console.WriteLine("No matching token found near: " + Input.Substring(pos));
            return false;
        }
        public AToken FindTokenOpt(string Rest, int startpos)
        {
            Match m;
            int maxlength = 0;
            int besttoken = 0;
            AToken ret = new AToken();
            try
            {

                for (int idx = 0; idx < tList.Count; idx++)
                {
                    m = rList[idx].Match(Rest, startpos);
                    if (m.Success)
                    {
                        if (m.Value.Length > maxlength)
                        {
                            maxlength = m.Value.Length;
                            besttoken = tList[idx];
                            ret.token = besttoken;
                            if (besttoken != 0)
                                ret.val = m.Value;
                        }
                    }
                }

            }
            catch { }
            return ret;
        }
		////////////////////////////////////////////////////////////////
		/// Scanner
		////////////////////////////////////////////////////////////////

		public class AToken
		{
			public int token;
			public string val;
		}

		ArrayList TokenList = new ArrayList();

		public bool Scanner (string Input)
		{
		        if (Input.Length == 0) return true;
			TokenList = new ArrayList();
			while (1==1)
			{
				AToken lasttoken = FindToken(Input);
				if (lasttoken.token == 0) break;
				if (lasttoken.token != t_ignore) TokenList.Add(lasttoken);
				if (Input.Length > lasttoken.val.Length)
				Input = Input.Substring(lasttoken.val.Length); else return true;
			}
                      System.Console.WriteLine(Input);
			System.Console.WriteLine();
			System.Console.WriteLine("No matching token found!");
			return false;
		}
		public AToken FindToken (string Rest)
		{
			ArrayList Results  = new ArrayList();
			ArrayList ResultsV = new ArrayList();
                      try{

			if (Regex.IsMatch(Rest,"^(;)")){
				Results.Add (t_Char59);
				ResultsV.Add(Regex.Match(Rest,"^(;)").Value);}

			if (Regex.IsMatch(Rest,"^(\\{)")){
				Results.Add (t_Char123);
				ResultsV.Add(Regex.Match(Rest,"^(\\{)").Value);}

			if (Regex.IsMatch(Rest,"^(\\})")){
				Results.Add (t_Char125);
				ResultsV.Add(Regex.Match(Rest,"^(\\})").Value);}

			if (Regex.IsMatch(Rest,"^(:)")){
				Results.Add (t_Char58);
				ResultsV.Add(Regex.Match(Rest,"^(:)").Value);}

			if (Regex.IsMatch(Rest,"^(\\|\\|)")){
				Results.Add (t_Char124Char124);
				ResultsV.Add(Regex.Match(Rest,"^(\\|\\|)").Value);}

			if (Regex.IsMatch(Rest,"^(&&)")){
				Results.Add (t_Char38Char38);
				ResultsV.Add(Regex.Match(Rest,"^(&&)").Value);}

			if (Regex.IsMatch(Rest,"^(\\|)")){
				Results.Add (t_Char124);
				ResultsV.Add(Regex.Match(Rest,"^(\\|)").Value);}

			if (Regex.IsMatch(Rest,"^(\\^)")){
				Results.Add (t_Char94);
				ResultsV.Add(Regex.Match(Rest,"^(\\^)").Value);}

			if (Regex.IsMatch(Rest,"^(&)")){
				Results.Add (t_Char38);
				ResultsV.Add(Regex.Match(Rest,"^(&)").Value);}

			if (Regex.IsMatch(Rest,"^(\\()")){
				Results.Add (t_Char40);
				ResultsV.Add(Regex.Match(Rest,"^(\\()").Value);}

			if (Regex.IsMatch(Rest,"^(\\))")){
				Results.Add (t_Char41);
				ResultsV.Add(Regex.Match(Rest,"^(\\))").Value);}

			if (Regex.IsMatch(Rest,"^(!=)")){
				Results.Add (t_Char33Char61);
				ResultsV.Add(Regex.Match(Rest,"^(!=)").Value);}

			if (Regex.IsMatch(Rest,"^(==)")){
				Results.Add (t_Char61Char61);
				ResultsV.Add(Regex.Match(Rest,"^(==)").Value);}

			if (Regex.IsMatch(Rest,"^(<)")){
				Results.Add (t_Char60);
				ResultsV.Add(Regex.Match(Rest,"^(<)").Value);}

			if (Regex.IsMatch(Rest,"^(<=)")){
				Results.Add (t_Char60Char61);
				ResultsV.Add(Regex.Match(Rest,"^(<=)").Value);}

			if (Regex.IsMatch(Rest,"^(>)")){
				Results.Add (t_Char62);
				ResultsV.Add(Regex.Match(Rest,"^(>)").Value);}

			if (Regex.IsMatch(Rest,"^(>=)")){
				Results.Add (t_Char62Char61);
				ResultsV.Add(Regex.Match(Rest,"^(>=)").Value);}

			if (Regex.IsMatch(Rest,"^(\\+)")){
				Results.Add (t_Char43);
				ResultsV.Add(Regex.Match(Rest,"^(\\+)").Value);}

			if (Regex.IsMatch(Rest,"^(\\-)")){
				Results.Add (t_Char45);
				ResultsV.Add(Regex.Match(Rest,"^(\\-)").Value);}

			if (Regex.IsMatch(Rest,"^(%)")){
				Results.Add (t_Char37);
				ResultsV.Add(Regex.Match(Rest,"^(%)").Value);}

			if (Regex.IsMatch(Rest,"^(\\*)")){
				Results.Add (t_Char42);
				ResultsV.Add(Regex.Match(Rest,"^(\\*)").Value);}

			if (Regex.IsMatch(Rest,"^(/)")){
				Results.Add (t_Char47);
				ResultsV.Add(Regex.Match(Rest,"^(/)").Value);}

			if (Regex.IsMatch(Rest,"^(!)")){
				Results.Add (t_Char33);
				ResultsV.Add(Regex.Match(Rest,"^(!)").Value);}

			if (Regex.IsMatch(Rest,"^(RULE)")){
				Results.Add (t_RULE);
				ResultsV.Add(Regex.Match(Rest,"^(RULE)").Value);}

			if (Regex.IsMatch(Rest,"^(\\*[\\s\\t\\x00]*=)")){
				Results.Add (t_Char42Char61);
				ResultsV.Add(Regex.Match(Rest,"^(\\*[\\s\\t\\x00]*=)").Value);}

			if (Regex.IsMatch(Rest,"^(\\+[\\s\\t\\x00]*=)")){
				Results.Add (t_Char43Char61);
				ResultsV.Add(Regex.Match(Rest,"^(\\+[\\s\\t\\x00]*=)").Value);}

			if (Regex.IsMatch(Rest,"^(\\-[\\s\\t\\x00]*=)")){
				Results.Add (t_Char45Char61);
				ResultsV.Add(Regex.Match(Rest,"^(\\-[\\s\\t\\x00]*=)").Value);}

			if (Regex.IsMatch(Rest,"^(/[\\s\\t\\x00]*=)")){
				Results.Add (t_Char47Char61);
				ResultsV.Add(Regex.Match(Rest,"^(/[\\s\\t\\x00]*=)").Value);}

			if (Regex.IsMatch(Rest,"^(=)")){
				Results.Add (t_Char61);
				ResultsV.Add(Regex.Match(Rest,"^(=)").Value);}

			if (Regex.IsMatch(Rest,"^(ELSE)")){
				Results.Add (t_ELSE);
				ResultsV.Add(Regex.Match(Rest,"^(ELSE)").Value);}

			if (Regex.IsMatch(Rest,"^(IF)")){
				Results.Add (t_IF);
				ResultsV.Add(Regex.Match(Rest,"^(IF)").Value);}

			if (Regex.IsMatch(Rest,"^(WHILE)")){
				Results.Add (t_WHILE);
				ResultsV.Add(Regex.Match(Rest,"^(WHILE)").Value);}

			if (Regex.IsMatch(Rest,"^(\\.)")){
				Results.Add (t_Char46);
				ResultsV.Add(Regex.Match(Rest,"^(\\.)").Value);}

			if (Regex.IsMatch(Rest,"^(NULL)")){
				Results.Add (t_NULL);
				ResultsV.Add(Regex.Match(Rest,"^(NULL)").Value);}

			if (Regex.IsMatch(Rest,"^((?=[A-Z])(MODEL|SOUND|MUSIC|FLIC|BMAP|OVLY|FONT))")){
				Results.Add (t_asset);
				ResultsV.Add(Regex.Match(Rest,"^((?=[A-Z])(MODEL|SOUND|MUSIC|FLIC|BMAP|OVLY|FONT))").Value);}

			if (Regex.IsMatch(Rest,"^((?=[A-Z])(OVERLAY|PANEL|PALETTE|REGION|SKILL|STRING|SYNONYM|TEXTURE|TEXT|VIEW|WALL|WAY))")){
				Results.Add (t_object);
				ResultsV.Add(Regex.Match(Rest,"^((?=[A-Z])(OVERLAY|PANEL|PALETTE|REGION|SKILL|STRING|SYNONYM|TEXTURE|TEXT|VIEW|WALL|WAY))").Value);}

			if (Regex.IsMatch(Rest,"^((?=[A-Z])(ACTION|RULES))")){
				Results.Add (t_function);
				ResultsV.Add(Regex.Match(Rest,"^((?=[A-Z])(ACTION|RULES))").Value);}

			if (Regex.IsMatch(Rest,"^((?=[A-Z])(ACOS|COS|ATAN|TAN|SIGN|INT|EXP|LOG10|LOG2|LOG))")){
				Results.Add (t_math);
				ResultsV.Add(Regex.Match(Rest,"^((?=[A-Z])(ACOS|COS|ATAN|TAN|SIGN|INT|EXP|LOG10|LOG2|LOG))").Value);}

			if (Regex.IsMatch(Rest,"^(((EACH_TICK|EACH_SEC|PANELS|LAYERS|MESSAGES)\\.(1[0-6]|[1-9])))")){
				Results.Add (t_list);
				ResultsV.Add(Regex.Match(Rest,"^(((EACH_TICK|EACH_SEC|PANELS|LAYERS|MESSAGES)\\.(1[0-6]|[1-9])))").Value);}

			if (Regex.IsMatch(Rest,"^((?=[A-Z])(THING|ACTOR))")){
				Results.Add (t_ambigChar95objectChar95flag);
				ResultsV.Add(Regex.Match(Rest,"^((?=[A-Z])(THING|ACTOR))").Value);}

			if (Regex.IsMatch(Rest,"^((?=[A-Z])(SIN|ASIN|SQRT|ABS))")){
				Results.Add (t_ambigChar95mathChar95command);
				ResultsV.Add(Regex.Match(Rest,"^((?=[A-Z])(SIN|ASIN|SQRT|ABS))").Value);}

			if (Regex.IsMatch(Rest,"^(RANDOM)")){
				Results.Add (t_ambigChar95mathChar95skillChar95property);
				ResultsV.Add(Regex.Match(Rest,"^(RANDOM)").Value);}

			if (Regex.IsMatch(Rest,"^([0-9]+)")){
				Results.Add (t_integer);
				ResultsV.Add(Regex.Match(Rest,"^([0-9]+)").Value);}

			if (Regex.IsMatch(Rest,"^(([0-9]*\\.[0-9]+)|([0-9]+\\.[0-9]*))")){
				Results.Add (t_fixed);
				ResultsV.Add(Regex.Match(Rest,"^(([0-9]*\\.[0-9]+)|([0-9]+\\.[0-9]*))").Value);}

			if (Regex.IsMatch(Rest,"^([A-Za-z0-9_][A-Za-z0-9_\\?]*(\\.[1-9][0-9]?)?)")){
				Results.Add (t_identifier);
				ResultsV.Add(Regex.Match(Rest,"^([A-Za-z0-9_][A-Za-z0-9_\\?]*(\\.[1-9][0-9]?)?)").Value);}

			if (Regex.IsMatch(Rest,"^(<[\\s]?[^<;:\" ]+\\.[^<;:\" ]+[\\s]?>)")){
				Results.Add (t_file);
				ResultsV.Add(Regex.Match(Rest,"^(<[\\s]?[^<;:\" ]+\\.[^<;:\" ]+[\\s]?>)").Value);}

			if (Regex.IsMatch(Rest,"^(\"(\\\\\"|.|[\\r\\n])*?\")")){
				Results.Add (t_string);
				ResultsV.Add(Regex.Match(Rest,"^(\"(\\\\\"|.|[\\r\\n])*?\")").Value);}

			if (Regex.IsMatch(Rest,"^([\\r\\n\\t\\s\\x00,]|:=|(#.*(\\n|$))|(//.*(\\n|$))|(/\\*(.|[\\r\\n])*?\\*/))")){
				Results.Add (t_ignore);
				ResultsV.Add(Regex.Match(Rest,"^([\\r\\n\\t\\s\\x00,]|:=|(#.*(\\n|$))|(//.*(\\n|$))|(/\\*(.|[\\r\\n])*?\\*/))").Value);}

			}catch{}
			int maxlength = 0;
			int besttoken = 0;
			AToken ret = new AToken();
		        ret.token = besttoken;
			for (int i = 0; i < Results.Count; i++){
				if (ResultsV[i].ToString().Length > maxlength)
				{
					maxlength = ResultsV[i].ToString().Length;
					besttoken = (int)Results[i];
		         	        ret.token = besttoken;
		                  	if (besttoken != 0)
			                ret.val   = ResultsV[i].ToString();
				}
			}
			return ret;
		}


	}
}
