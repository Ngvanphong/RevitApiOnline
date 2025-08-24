using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class FamilyVM
    {
        public ElementId FamilyId { set; get; }

        public string FamilyName { set; get; }

    }
}
