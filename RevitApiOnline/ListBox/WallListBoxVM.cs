using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.ListBox
{
    public class WallListBoxVM 
    {
        public List<WallInfo> WallInfos;
    }

    public class WallInfo : INotifyPropertyChanged
    {
        private bool isCheck;
        public bool IsChecked
        {
            get { return isCheck; }
            set { isCheck = value; OnPropertyChanged(nameof(IsChecked)); }
        }


        public string NameWall { set; get; }
        public ElementId WallId { set; get; }
        public string LevelName { set; get; }

        public ElementId LevelId { set; get; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
