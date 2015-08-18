using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anitro.Interfaces
{
    public interface IFilter<T>
    {
        string Name { get; set; }
        object Parameter { get; set; }
        ObservableCollection<T> Filter(ObservableCollection<T> collection);
    }
}
