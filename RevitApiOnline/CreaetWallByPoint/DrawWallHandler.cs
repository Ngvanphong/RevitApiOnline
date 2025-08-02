using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreaetWallByPoint
{
    public class DrawWallHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            CreateWallDataContext dataContextForm = CreateWallAppShow.formCreateWall.DataContext as CreateWallDataContext;
            WallTypeVM wallTypeVm = dataContextForm.WallType;
            double height = dataContextForm.Height;

            XYZ startPoint = CreateWallAppShow.StartPoint;
            XYZ endPoint = CreateWallAppShow.EndPoint;
            Line line= Line.CreateBound(startPoint, endPoint);
            double heightFeet= UnitUtils.ConvertToInternalUnits(height, UnitTypeId.Millimeters);
            using(Transaction t= new Transaction(doc, "CreateWall"))
            {
                t.Start();
                Wall.Create(doc, line, wallTypeVm.Id, doc.ActiveView.GenLevel.Id, heightFeet, 0, false, false);
                t.Commit();
            }

        }

        public string GetName()
        {
            return "DrawWallHandler1";
        }
    }
}
