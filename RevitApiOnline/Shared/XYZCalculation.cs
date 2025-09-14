using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.Shared
{
    public class XYZCalculation
    {
        public static XYZ IntersectionPointPlanebyVector(Plane plane, XYZ p, XYZ vector)
        {
            try
            {
                double lineParameter = (plane.Normal.DotProduct(plane.Origin) - plane.Normal.DotProduct(p)) / plane.Normal.DotProduct(vector.Normalize());
                return p + lineParameter * vector.Normalize();
            }
            catch { }
            return null;
        }
    }
}
