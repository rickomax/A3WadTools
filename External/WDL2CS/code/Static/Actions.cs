using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Actions
    {
        private static List<string> s_parameters = new List<string>();

        public static Node AddAction(Node name, Node stream)
        {
            string sname = name.ToString();
            Registry.Register("Action", sname);
            Node a = new Action(sname, stream);

            return a;
        }

        public static Node CreateMarker(Node name, Node stream)
        {
            string sname = name.ToString();
            Node inst = new Instruction(Formatter.FormatGotoMarker(sname), false);
            Node s = new Node(inst, stream);

            return s;

        }

        public static Node CreateIfCondition(Node expr, Node stream)
        {
            Node i1 = new Instruction("if", "(" + expr.ToString() + ")");
            Node i2 = new Instruction("{", false);
            Node i3 = new Instruction("}", false);
            Node s = new Node(new[] { i1, i2, stream, i3});

            return s;
        }

        public static Node CreateElseCondition(Node stream)
        {
            Node i1 = new Instruction("else");
            Node i2 = new Instruction("{", false);
            Node i3 = new Instruction("}", false);
            Node s = new Node(new[] { i1, i2, stream, i3 });

            return s;
        }

        public static Node CreateWhileCondition(Node expr, Node stream)
        {
            Node i1 = new Instruction("while", "(" + expr.ToString() + ")");
            Node i2 = new Instruction("{", false);
            Node i3 = new Instruction("}", false);
            Node s = new Node(new[] { i1, i2, stream, i3 });

            return s;
        }

        public static Node CreateExpression(Node expr)
        {
            string sexpr = expr.ToString();
            //ridiculous patch: A3 accepts RULE statements without assignment
            //TODO: find out real behaviour in A3, currently first identifier is treated as assignee
            //patch is derived from the behaviour of A3 for statements like "+ =" - seems like "=" is optional for WDL parser
            if (!sexpr.Contains("="))
            {
                Console.WriteLine("(W) ACTIONS patched invalid rule: " + expr);
                string[] fragments = sexpr.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                fragments[1] += "="; //first operator is changed to assignment operator
                sexpr = string.Join(" ", fragments);
            }
            Instruction inst = new Instruction("Rule", sexpr);

            return inst;
        }

        public static Node CreateExpression(Node assignee, Node op, Node expr)
        {
            string sexpr = assignee.ToString() + op.ToString() + expr.ToString(); 
            Instruction inst = new Instruction("Rule", sexpr);
            return inst;
        }

        public static Node CreateInstruction(Node command)
        {
            string scommand = command.ToString();
            if (Identifier.IsCommand(ref scommand))
            {
                Instruction inst = new Instruction(scommand, s_parameters);

                //Clean up
                s_parameters.Clear();
                return inst;
            }
            else if (s_parameters.Count == 0) //take care of wrongly defined goto marker (ends with ; instead of :)
            {
                Console.WriteLine("(W) ACTIONS crrect malformed goto marker: " + command);
                Instruction inst = new Instruction(Formatter.FormatGotoMarker(scommand), false);
                return inst;
            }
            else
            {
                //Clean up
                s_parameters.Clear();

                Console.WriteLine("(W) ACTIONS ignore invalid command: " + command);
                return null;
            }
        }

        public static Node AddInstructionParameter(Node param)
        {
            s_parameters.Insert(0, param.ToString());
            return null;
        }

    }
}
