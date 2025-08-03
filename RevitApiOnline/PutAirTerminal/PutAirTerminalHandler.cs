using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.PutAirTerminal
{
    public class PutAirTerminalHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            UIDocument uiDoc = app.ActiveUIDocument;
            Document doc = app.ActiveUIDocument.Document;
            AirtTerminalDataContext dataContext = AirTerminalAppShow.formPutAirTermianl.DataContext as AirtTerminalDataContext;
            FamilySymbol symbol = doc.GetElement(dataContext.TypeSelected.Id) as FamilySymbol;
            Level level = dataContext.LevelSelected;
            double offset = UnitUtils.ConvertToInternalUnits(dataContext.Offset, UnitTypeId.Millimeters);
            while (true)
            {
                XYZ point = null;
                try
                {
                    point = uiDoc.Selection.PickPoint("Pick a point");
                    FamilyInstance instance = null;
                    using(Transaction t= new Transaction(doc, "PutTerminal"))
                    {
                        t.Start();
                        if (!symbol.IsActive) symbol.Activate();
                        instance= doc.Create.NewFamilyInstance(point, symbol, level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                        
                        t.Commit();
                    }
                    using(Transaction t2= new Transaction(doc, "SetOffset"))
                    {
                        t2.Start();
                        Parameter offsetPara = instance.get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM);
                        offsetPara.Set(offset);
                        t2.Commit();
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        public string GetName()
        {
            return "PutAirTerminalHandler";
        }
    }
}
