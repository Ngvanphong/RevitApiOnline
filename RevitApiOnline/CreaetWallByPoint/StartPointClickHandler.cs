using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreaetWallByPoint
{
    public class StartPointClickHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            UIDocument uiDoc = app.ActiveUIDocument;
            try
            {
                XYZ pointClick = uiDoc.Selection.PickPoint("Pick a point");
                CreateWallAppShow.StartPoint = pointClick;
            }
            catch { }
            
        }

        public string GetName()
        {
            return "StartPointClickHandler1";
        }
    }
}
