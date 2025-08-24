using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class FamilyOriginTarget : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private FamilyVM family;
        public FamilyVM Family
        {
            get { return family; }
            set { family = value; OnPropertyChanged(nameof(Family)); }
        }

        private TypeVM originalType;
        public TypeVM OriginType
        {
            get { return originalType; }
            set { originalType = value; OnPropertyChanged(nameof(OriginType)); }
        }

        private TypeVM targetType;
        public TypeVM TargetType
        {
            get { return targetType; }
            set { targetType = value; OnPropertyChanged(nameof(TargetType)); }
        }


    }
}
