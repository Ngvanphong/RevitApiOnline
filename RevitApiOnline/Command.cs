using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using RevitApiOnline.Shared.Interfaces;
using RevitApiOnline.Shared.Implements;

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            ICollection<ElementId> ids= uiDoc.Selection.GetElementIds();

            // implement interface;
            ICurveUtilities curveUtilities = new CurveUtilities();
            List<Curve> listCurveWall = curveUtilities.GetCurvesFromDetailCurve(doc, ids);

            View view = doc.ActiveView;
            Level level= view.GenLevel;

            Parameter levelParameter = view.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL);
            string levelName = levelParameter.AsString();
            
            using(Transaction t= new Transaction(doc, "CreateWall"))
            {
                t.Start();
                foreach(Curve curve in listCurveWall)
                {
                   Wall wall= Wall.Create(doc, curve, level.Id, false);
                }
                t.Commit();
            }
            


            
           

            return Result.Succeeded;
        }
    }
}
