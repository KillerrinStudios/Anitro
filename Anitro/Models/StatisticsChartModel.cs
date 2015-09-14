using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Models
{
    public class StatisticsChartModel : ModelBase
    {
        private string m_name = "";
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        private int m_amount = 0;
        public int Amount
        {
            get { return m_amount; }
            set
            {
                m_amount = value;
                RaisePropertyChanged(nameof(Amount));
            }
        }

        public StatisticsChartModel() { }
        public StatisticsChartModel(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

    }
}
