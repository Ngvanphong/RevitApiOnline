using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitApiOnline.ListBox;
using System.Collections.Generic;
using System.Linq;

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var listWall = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls)
                .WhereElementIsNotElementType().OfClass(typeof(Wall))
                .Cast<Wall>().Where(x => x.WallType != null && x.WallType.Kind != WallKind.Curtain)
                .ToList();
            List<WallInfo> listWallInfo= new List<WallInfo>();
            foreach(Wall wall in listWall)
            {
                WallInfo wallInfo = new WallInfo();
                wallInfo.NameWall = wall.Name;
                wallInfo.WallId = wall.Id;
                wallInfo.LevelId = wall.LevelId;
                wallInfo.LevelName = doc.GetElement(wall.LevelId).Name;
                listWallInfo.Add(wallInfo);
            }
            listWallInfo= listWallInfo.OrderBy(x=>x.NameWall).ToList();
            WallListBoxVM dataContext = new WallListBoxVM();
            dataContext.WallInfos = listWallInfo;
            var form = new ListBoxWpf(listWallInfo);
            form.DataContext = dataContext;
            var resultForm= form.ShowDialog();
            if(resultForm == true)
            {
                var selectedItems = (form.DataContext as WallListBoxVM).WallInfos.Where(x=>x.IsChecked);

            }


            return Result.Succeeded;
        }
    }

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem != null && elem is Wall)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}