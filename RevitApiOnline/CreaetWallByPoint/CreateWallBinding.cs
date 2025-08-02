using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreaetWallByPoint
{
    [Transaction(TransactionMode.Manual)]
    public class CreateWallBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc= uiDoc.Document;   
            IEnumerable<WallType> wallTypeCollection= new FilteredElementCollector(doc).OfClass(typeof(WallType)).Cast<WallType>();
            List<WallTypeVM> listWallTypeVm= new List<WallTypeVM>();
            foreach(WallType wallType in wallTypeCollection)
            {
                WallTypeVM wallTypeVM = new WallTypeVM();
                wallTypeVM.TypeName= wallType.Name;
                wallTypeVM.Id= wallType.Id;
                listWallTypeVm.Add(wallTypeVM);
            }

            CreateWallAppShow.ShowForm();
            CreateWallAppShow.formCreateWall.comboboxWallType.ItemsSource= listWallTypeVm;




            return Result.Succeeded;
        }
    }
}
