using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class GridDataContext : INotifyPropertyChanged
    {

        private FamilyCategoryVM familyCategory;
        public FamilyCategoryVM FamilyCategory
        {
            get { return familyCategory; }
            set { familyCategory = value; OnPropertyChanged(nameof(FamilyCategory)); }
        }

        private ObservableCollection<FamilyOriginTarget> observableFamilyOriginTarget;
        public ObservableCollection<FamilyOriginTarget> ObservableFamilyOriginTarget
        {
            get { return observableFamilyOriginTarget; }
            set { observableFamilyOriginTarget = value;OnPropertyChanged(nameof(FamilyOriginTarget)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
