using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreaetWallByPoint
{
    public class CreateWallDataContext
    {
        public WallTypeVM WallType { set; get; }
        public double Height { set; get; }

    }
}
