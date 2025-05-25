using Autodesk.Revit.DB;
using RevitApiOnline.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RevitApiOnline.Shared.Implements
{
    public class CurveUttilities : ICurveUtilities
    {
        public List<Curve> GetCurvesFromDetailCurve(Document doc,ICollection<ElementId> ids)
        {
            List<Curve> listCurveReuslt = new List<Curve>();
            foreach (ElementId id in ids)
            {
                Element element = doc.GetElement(id);
                bool isDetail = element is DetailCurve;
                if (isDetail)
                {
                    DetailCurve detailCurve = element as DetailCurve;
                    Curve curve = detailCurve.GeometryCurve;
                    listCurveReuslt.Add(curve);
                }
            }
            return listCurveReuslt;
        }
    }
}
