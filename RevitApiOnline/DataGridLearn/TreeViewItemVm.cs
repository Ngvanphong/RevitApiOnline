using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.DataGridLearn
{
    public class TreeViewItemVm
    {
        public TreeViewItemVm()
        {
            Items= new List<TreeViewItemVm>();  
        }
        public string Name { get; set; }
        public ElementId Id { set; get; }
        public List<TreeViewItemVm> Items { get; set; }
    }
}
