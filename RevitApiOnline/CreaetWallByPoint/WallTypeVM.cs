using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RevitApiOnline.CreaetWallByPoint
{
    public class WallTypeVM
    {
        public string TypeName { set; get; }
        public ElementId Id { set; get; }
    }
}
