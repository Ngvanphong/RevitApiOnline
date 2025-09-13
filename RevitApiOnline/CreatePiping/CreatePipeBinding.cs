using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitApiOnline.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreatePiping
{
    [Transaction(TransactionMode.Manual)]
    public class CreatePipeBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc= uiDoc.Document;
            var ids= uiDoc.Selection.GetElementIds();
            if (ids == null) return Result.Succeeded;
            foreach(ElementId id in ids)
            {
                Element element= doc.GetElement(id);
                List<Solid> listSolid = new List<Solid>();
                GeometryHelper.GeoSolidElement(doc, element,ref listSolid);
                List<Reference> listHorizotalRef = new List<Reference>();
                List<Reference> listVerticalRef = new List<Reference>();

            }
            return Result.Succeeded;
        }
    }
}
