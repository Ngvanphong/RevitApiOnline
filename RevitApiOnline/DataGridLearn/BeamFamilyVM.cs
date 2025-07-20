using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.DataGridLearn
{
    public class DataGridItem : INotifyPropertyChanged
    {
        // family
        public List<BeamFamilyVM> BeamFamilies { get; set; }

        private BeamFamilyVM selectedFamily;
        public BeamFamilyVM SelectedFamily 
        {
            get { return selectedFamily; }
            set { selectedFamily = value; OnPropertyChanged(nameof(SelectedFamily)); }
        }

        // family combobox

        private List<FamilySymbolVM> familySymbols;
        public List<FamilySymbolVM> FamilySymbols
        { 
            get { return familySymbols; }
            set { familySymbols = value; OnPropertyChanged(nameof(FamilySymbols)); }
        }


        private FamilySymbolVM selectedSymbol;
        public FamilySymbolVM SelectedSymbol 
        {
            get { return selectedSymbol; }
            set { selectedSymbol = value; OnPropertyChanged( nameof(SelectedSymbol)); } 
        }

        // parameter combobox
        public List<ParameterVm> ParameterVms { set; get; }

        public ParameterVm SelectedParameter { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public  class BeamFamilyVM
    {
        public string FamilyName { set; get; }
        public ElementId FamilyId { set; get; }

    }

    public class FamilySymbolVM
    {
        public string TypeName { set; get; }
        public ElementId TypeId { set; get; }
    }

    public class ParameterVm
    {
        public string ParameterName { set; get; }
        public ElementId ParameterId { set; get; }
    }
}
