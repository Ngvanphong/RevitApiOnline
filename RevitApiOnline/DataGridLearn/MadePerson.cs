using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.DataGridLearn
{
    public class MadePerson
    {
        public MadePerson()
        {
            Persons = new ObservableCollection<MadePerson>();
        }
        public string Name { set; get; }
        public ObservableCollection<MadePerson> Persons { set; get; }
    }
}
