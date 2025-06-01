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

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            IEnumerable<Reference> listRefWalls= uiDoc.Selection.PickObjects(ObjectType.Element, new WallSelectionFilter(), "Pick walls");
            List<Wall> listWall = new List<Wall>();
            foreach(Reference refItem in listRefWalls)
            {
                Wall wall = doc.GetElement(refItem) as Wall;
                if(wall != null)
                {
                    listWall.Add(wall);
                }
            }

            //using (Transaction t = new Transaction(doc, "WallModify"))
            //{
            //    t.Start();
            //    foreach(Wall item in listWall)
            //    {
            //        Parameter parameter = item.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET);
            //        if (parameter !=null && !parameter.IsReadOnly)
            //        {
            //            double offsetMili = -100;
            //            double offsetInch = UnitUtils.ConvertToInternalUnits(offsetMili, UnitTypeId.Millimeters);
            //            parameter.Set(offsetInch);
            //        }
            //    }
            //    t.Commit();
            //}

            View activeView = doc.ActiveView;
            
            ReferenceArray referenceArray = new ReferenceArray();
            foreach(Wall wall in listWall)
            {
                Reference refExternal = HostObjectUtils.GetSideFaces(wall, ShellLayerType.Exterior).First();
                Reference refInternal = HostObjectUtils.GetSideFaces(wall, ShellLayerType.Interior).First();
                referenceArray.Append(refExternal);
                referenceArray.Append(refInternal);
            }
            Reference refFist = referenceArray.get_Item(0);
            Element element = doc.GetElement(refFist);
            Face face = element.GetGeometryObjectFromReference(refFist) as Face;
            PlanarFace plannarFace = face as PlanarFace;
            XYZ originFace = plannarFace.Origin;
            XYZ normalFace = plannarFace.FaceNormal.Normalize();

            

            //XYZ vectorA = null;
            //XYZ vectorB = null;
            //double dotProduct = vectorA.Normalize().DotProduct(vectorB.Normalize());
            //XYZ vectorCross = vectorA.CrossProduct(vectorB);

            // tao line dat dim
            XYZ pointPutDim = uiDoc.Selection.PickPoint("Pick a point to put dim");

            Line linePutDim = Line.CreateUnbound(pointPutDim, normalFace);
            using(Transaction t= new Transaction(doc, "CreateDim"))
            {
                t.Start();
                doc.Create.NewDimension(activeView, linePutDim, referenceArray);
                t.Commit();
            }



            //doc.Create.NewDimension(activeView,)




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
