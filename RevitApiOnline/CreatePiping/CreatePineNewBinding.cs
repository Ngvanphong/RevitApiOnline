using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitApiOnline.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreatePiping
{
    [Transaction(TransactionMode.Manual)]
    public class CreatePineNewBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            //Geometry

            IEnumerable<FamilyInstance> collumns = new FilteredElementCollector(doc, doc.ActiveView.Id)
                                                    .OfCategory(BuiltInCategory.OST_StructuralColumns).OfClass(typeof(FamilyInstance)).Cast<FamilyInstance>();

            List<List<Line>> listColumnCurves= new List<List<Line>>();
            Options options = new Options();
            options.IncludeNonVisibleObjects = false;
            options.View = doc.ActiveView;
            options.ComputeReferences = false;
            foreach(FamilyInstance familyInstance in collumns)
            {
                GeometryElement geoElement= familyInstance.get_Geometry(options);
                foreach(GeometryObject geoObj in geoElement)
                {
                    if(geoObj is GeometryInstance geomInstance)
                    {
                        foreach(GeometryObject geoObj2 in geomInstance.GetInstanceGeometry())
                        {
                            if(geoObj2 is Solid solid && solid.Volume> 0.0000001)
                            {
                                foreach(Face face in solid.Faces)
                                {
                                    if(face is PlanarFace plannarFace)
                                    {
                                        XYZ normal = plannarFace.FaceNormal.Normalize();
                                        if(normal.IsAlmostEqualTo(XYZ.BasisZ, 0.0001))
                                        {
                                            var curveloop = plannarFace.GetEdgesAsCurveLoops().FirstOrDefault();
                                            if (curveloop != null)
                                            {
                                                List<Line> listLine= new List<Line>();
                                                foreach(Curve curve in curveloop)
                                                {
                                                    Line line = curve as Line;
                                                    if(line != null)
                                                    {
                                                        listLine.Add(line);
                                                    }
                                                }
                                                listColumnCurves.Add(listLine);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            ColumnCanvasAppShow.listColumnCurves = listColumnCurves;







            return Result.Succeeded;
        }
    }

   
}
