using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.PutAirTerminal
{
    [Transaction(TransactionMode.Manual)]
    public class PutAirTerminalBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            IEnumerable<Family> familyCollection = new FilteredElementCollector(doc)
                .OfClass(typeof(Family)).Cast<Family>().Where(x => x.FamilyCategoryId.Value
                == (long)BuiltInCategory.OST_DuctTerminal);
            var listFamilyVm = familyCollection.Select(
                x => new FamilyVM { Name = x.Name, Id = x.Id } );

            IEnumerable<Level> levels = new FilteredElementCollector(doc).OfClass(typeof(Level)).
                Cast<Level>();

            AirTerminalAppShow.ShowForm();
            AirTerminalAppShow.formPutAirTermianl.comboboxFamily.ItemsSource = listFamilyVm;
            AirTerminalAppShow.formPutAirTermianl.comboboxLevel.ItemsSource= levels;
            AirtTerminalDataContext dataContext= new AirtTerminalDataContext();
            dataContext.Offset = 0;
            AirTerminalAppShow.formPutAirTermianl.DataContext= dataContext;

            return Result.Succeeded;
        }
    }
}
