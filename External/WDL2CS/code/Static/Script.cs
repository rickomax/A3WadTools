using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class Script
    {
        public static string Format(string className, bool skipProperties)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            StringBuilder sb = new StringBuilder();
            sb.Append(@"
using System.Collections;
using Acknex3.Api;

namespace Acknex3.Script
{
	class " + Formatter.FormatReserved(className) + @"
	{
		public void Initialize()
		{
");
            Sections.Format(sb, true, false); //initialized data
            sb.Append(@"
		}
");
            Sections.Format(sb, false, skipProperties); //static data
            sb.Append(@"
	}
}
");
            Console.WriteLine("(I) SCRIPT formatting finished in " + watch.Elapsed);
            watch.Stop();


            return sb.ToString();
        }

        public static PropertyList ToList()
        {
            PropertyList list = new PropertyList();
            Sections.ToList(list);
            return list;
        }
    }
}
