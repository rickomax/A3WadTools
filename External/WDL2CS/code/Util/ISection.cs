using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    interface ISection
    {
        string Name
        {
            get;
        }

        string Type
        {
            get;
        }
        void Format(StringBuilder sb, bool skipProperties);
        void ToList(PropertyList list);
        bool IsInitialized();
    }
}
