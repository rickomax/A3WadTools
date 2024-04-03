using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Node
    {
        private readonly string m_data;
        private readonly List<Node> m_children;

        protected List<Node> Children => m_children;

        public Node() : this(string.Empty) {}

        public Node (string data)
        {
            m_data = data;
        }

        public Node(Node child)
        {
            m_children = new List<Node>();
            if (child != null)
                m_children.Add(child);
        }

        public Node(Node child1, Node child2) : this(child1)
        {
            if (child2 != null)
                m_children.Add(child2);
        }

        public Node(Node child1, Node child2, Node child3) : this(child1, child2)
        {
            if (child3 != null)
                m_children.Add(child3);
        }

        public Node (IEnumerable<Node> children)
        {
            m_children = new List<Node>(children.Where(x => x != null));
        }

        private void Concat(StringBuilder sb)
        {
            if (m_children != null)
            {
                foreach (Node n in m_children)
                    n?.Concat(sb);
            }
            else
            {
                sb.Append(m_data);
            }

        }

        public override string ToString()
        {
            if (m_children != null)
            {
                StringBuilder sb = new StringBuilder();
                Concat(sb);
                //Console.WriteLine("Node2string: " + sb);
                return sb.ToString();
            }
            else
            {
                return m_data;
            }
        }

        public bool IsNullOrEmpty()
        {
            if (!string.IsNullOrEmpty(m_data))
                return false;
            if(m_children != null)
            {
                foreach (Node child in m_children)
                {
                    if (!child.IsNullOrEmpty())
                        return false;
                }
            }
            return true;
        }

        public List<Node> GetAll()
        {
            List<Node> list = new List<Node>();
            if (m_children != null)
            {
                foreach (Node child in m_children)
                {
                    list.AddRange(child.GetAll());
                }
            }
            else
            {
                list.Add(this);
            }
            return list;
        }

        //public static implicit operator Node(string s) => new Node(s);
        //public static implicit operator string(Node n) => n.ToString();
    }
}
