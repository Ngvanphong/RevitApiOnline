using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using RevitApiOnline.Shared.Interfaces;
using RevitApiOnline.Shared.Implements;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Net.WebSockets;

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;


            // fitler 
            List<Element> typeWall = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Walls).
                                                WhereElementIsElementType().ToElements().ToList();
            List<Element> walls = new FilteredElementCollector(doc, doc.ActiveView.Id).OfCategory(BuiltInCategory.OST_Walls).
                                                WhereElementIsNotElementType().ToElements().ToList();
            //IEnumerable<Element> typeWall2 = new FilteredElementCollector(doc).OfClass(typeof(WallType));
            //IEnumerable<Element> wall2 = new FilteredElementCollector(doc).OfClass(typeof(Wall));


            string typeName = "Generic - 300mm";
            long id = 1659761;
            ElementId newId= new ElementId(id);
            List<Element> wallsType300 = walls.Where(y => y.Name == typeName && y.Id.Value== id).ToList();

            List<Element> wallsType3002 = walls.Where(x =>
            {
                bool isTrueType = x.Name == typeName;
                bool isTrueId = x.Id.Value == id;
                return isTrueType && isTrueId;
            }).ToList();

            Func<Element, bool> fucntionTypeId = (item) =>
            {
                bool isTue = item.Name == typeName && item.Id.Value == id;
                return isTue;
            };
            List<Element> wallsType3003 = walls.Where(fucntionTypeId).ToList();

            List<Element> wallsType3004 = (from el in walls
                                          where el.Name == typeName && el.Id.Value == id
                                          select el).ToList();


            List<Wall> ofTypeWalls = walls.OfType<Wall>().ToList();
             
            Element elemetns;
            var typeOd = typeof(Element);

            List<Element> listOrder = walls.OrderBy(x => x.Name).ThenByDescending(y=>y.Id.Value).ToList();
            List<Element> listOrderDes = walls.OrderByDescending(x => x.Name).ToList();

            Dictionary<long, Element> dictionaryElement = new Dictionary<long, Element>();
            dictionaryElement.Add(1111, walls[0]);
            dictionaryElement.Add(4444, walls[1]);
            dictionaryElement.Add(444555, walls[1]);
            Element wallKey11 = dictionaryElement[1111];
            foreach(KeyValuePair<long, Element> pairItem in dictionaryElement)
            {

            }

            var groupTypeName = walls.GroupBy(x => x.Name).ToList();
            Dictionary<string, List<Element>> resutlClass;
            List<string> listNameWall = walls.Select(x => x.Name).ToList();
            var listNameWall2 = walls.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            bool isAlllTrue = walls.All(x => x.Name == typeName);
            bool isAnyTrue = walls.Any(x => x.Name == typeName);

            List<Element> listContain = walls.Where(x => x.Name.Contains("Generic")).ToList();
            int totalCount = listContain.Count;
            Wall wall = null;
            int index = walls.IndexOf(wall);
            Element elementFind = walls.ElementAt(1); 

            Element findFist = walls.First(x => x.Name == typeName);
            Element findFistDefault = walls.FirstOrDefault(x => x.Name == typeName);

            bool isExisted = walls.Exists(x => x.Name == typeName);

            List<(string, List<Element>)> tuppleList = new List<(string, List<Element>)>();
            List<Element> list1 = new List<Element>();
            list1.Add(walls[0]);
            list1.Add(walls[1]);
            tuppleList.Add(("11111", list1));
            tuppleList.Add(("11111", list1));
            foreach(var item in tuppleList)
            {
                string itemVal1 = item.Item1;
                List<Element> elements2 = item.Item2;
            }

            List<(string TypeName, List<Element> ListTypes)> tuppleList2 = new List<(string, List<Element>)>();
            foreach(var item in tuppleList2)
            {
                string itemVal1 = item.TypeName;
                List<Element> element2 = item.ListTypes;
            }

            List<string> listString = new List<string> { "1", "1", "3" };
            listString.Add("1");
            HashSet<string> hashSet = new HashSet<string> { "1", "1", "3" };
            hashSet.Add("1");

            return Result.Succeeded;
        }
    }

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if(elem !=null && elem is Wall)
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
