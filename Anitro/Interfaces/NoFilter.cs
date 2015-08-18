using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Interfaces
{
    public class NoFilter<T> : IFilter<T>
    {
        private string m_name = "None";
        public string Name
        {
            get { return m_name; }
            set
            {
                if (m_name == value) return;
                m_name = value;
            }
        }

        private object m_parameter = new object();
        public object Parameter
        {
            get { return m_parameter; }
            set
            {
                if (m_parameter == value) return;
                m_parameter = value;
            }
        }

        public ObservableCollection<T> Filter(ObservableCollection<T> collection)
        {
            return collection;
        }
    }
}
