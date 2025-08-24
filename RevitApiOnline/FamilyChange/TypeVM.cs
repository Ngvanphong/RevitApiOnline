using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class TypeVM
    {
        public ElementId TypeId { get; set; }
        public string TypeName { set; get; }
    }
}
