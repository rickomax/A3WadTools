using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WDL2CS
{
    class ObjectData
    {
        private List<Property> m_properties;
        private StringBuilder m_rangeStream;
        private StringBuilder m_controlStream;
        private StringBuilder m_propertyStream;

        public ObjectData()
        {
            m_properties = new List<Property>();
            m_rangeStream = new StringBuilder();
            m_controlStream = new StringBuilder();
            m_propertyStream = new StringBuilder();
        }

        public List<Property> Properties { get => m_properties; }
        public StringBuilder RangeStream { get => m_rangeStream; }
        public StringBuilder ControlStream { get => m_controlStream; }
        public StringBuilder PropertyStream { get => m_propertyStream; }

    }
}
