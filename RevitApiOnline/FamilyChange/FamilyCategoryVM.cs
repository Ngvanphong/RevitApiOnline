using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class FamilyCategoryVM
    {
        public ElementId FamilyCategoryId { set; get; }

        public string FamilyCategoryName { set; get; }
    }
}
