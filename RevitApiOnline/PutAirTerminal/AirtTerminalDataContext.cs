using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.PutAirTerminal
{
    public class AirtTerminalDataContext : INotifyPropertyChanged
    {
        public FamilyVM FamilySelected { set;get; }

        public FamilyTypeVM TypeSelected { set; get; }  

        public Level LevelSelected { set; get; }    

        public double Offset { set; get; }


        private List<FamilyTypeVM> listFamilyTypeVm;
        public List<FamilyTypeVM> ListFamilyTypeVm
        {
            get { return listFamilyTypeVm; }
            set { listFamilyTypeVm = value; OnPropertyChanged(nameof(ListFamilyTypeVm)); }
        }
 
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
