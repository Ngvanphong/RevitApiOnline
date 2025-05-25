using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.Shared.Interfaces
{
    public interface ICurveUtilities
    {
        List<Curve> GetCurvesFromDetailCurve(Document doc, ICollection<ElementId> ids);

    }
}
