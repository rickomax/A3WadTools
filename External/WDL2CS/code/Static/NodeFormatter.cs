using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class NodeFormatter
    {
        public static Node FormatFile(Node file)
        {
            return new Node(Formatter.FormatFile(file.ToString()));
        }

        public static Node FormatList(Node list)
        {
            return new Node(Formatter.FormatList(list.ToString()));
        }

        public static Node FormatKeyword(Node keyword)
        {
            return new Node(Formatter.FormatKeyword(keyword.ToString()));
        }

        public static Node FormatKeywordProperty(Node keyword, Node property)
        {
            return new Node(Formatter.FormatKeyword(keyword.ToString() + "." + property.ToString()));
        }

        public static Node FormatMath(Node math, Node parameter)
        {
            return new Node(Formatter.FormatMath(math.ToString()) + "(" + parameter.ToString() + ")");
        }

        public static Node FormatNull()
        {
            return new Node(Formatter.FormatNull());
        }

        public static Node FormatNumber(Node number)
        {
            return new Node(Formatter.FormatNumber(number.ToString()));
        }

        public static Node FormatNumberPatch(Node number, Node superfluous)
        {
            Console.WriteLine("(W) PARSER discarded superfluous token in expression: " + superfluous.ToString());
            return FormatNumber(number);
        }

        public static Node FormatOperator(Node node1, string op, Node node2)
        {
            Node opNode = new Node(op);
            return new Node(node1, opNode, node2);
        }

        public static Node FormatParentheses(Node node)
        {
            return new Node("(" + node.ToString() + ")");
        }

        public static Node FormatString(Node str)
        {
            return new Node(Formatter.FormatString(str.ToString()));
        }
    }
}
