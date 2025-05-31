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

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            // chon truoc
            IEnumerable<ElementId> selectedIds = uiDoc.Selection.GetElementIds();

            //
            //pick point
            XYZ point1 = uiDoc.Selection.PickPoint("Pick a point");
            XYZ point2 = uiDoc.Selection.PickPoint("Pick a point");

            // pick face
            Reference faceRef = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face, "Pick face");
            Element element = doc.GetElement(faceRef);
            Face face = element.GetGeometryObjectFromReference(faceRef) as Face;

            // pick object
            //Reference objectRef = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select Element");

            Reference objectRef = uiDoc.Selection.PickObject(ObjectType.Element, new WallSelectionFilter(), "Pick a wall");
            Element selectedElemenet = doc.GetElement(objectRef);

            IList<Reference> objectRefs = uiDoc.Selection.PickObjects(ObjectType.Element, new WallSelectionFilter(), "Pick Wall");

            List<Wall> listWall = new List<Wall>();
            foreach (Reference refItem in objectRefs)
            {
                Wall wallItem = doc.GetElement(refItem) as Wall;
                if (wallItem != null)
                {
                    listWall.Add(wallItem);
                }
            }

            // pick rectangle
            IList<Element> listWalls= uiDoc.Selection.PickElementsByRectangle(new WallSelectionFilter(), "Pick walls");

            PickedBox pickedBox = uiDoc.Selection.PickBox(PickBoxStyle.Directional, "Pick Box");
            XYZ min = pickedBox.Min;
            XYZ max = pickedBox.Max;






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
