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
    public class CreatePipeBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc= uiDoc.Document;
            var ids= uiDoc.Selection.GetElementIds();
            if (ids == null) return Result.Succeeded;
            foreach(ElementId id in ids)
            {
                Element element= doc.GetElement(id);
                List<Solid> listSolid = new List<Solid>();
                List<Line> listLine = new List<Line>();
                GeometryHelper.GeoSolidElement(doc, element,ref listSolid, ref listLine);
                if(element is FamilyInstance)
                {
                    FamilyInstance instance = element as FamilyInstance;
                    Transform transform = instance.GetTransform();
                    XYZ horizontal = transform.BasisX.Normalize();
                    XYZ vertical = transform.BasisY.Normalize();
                    ReferenceArray listHorizotalRef = new ReferenceArray();
                    ReferenceArray listVerticalRef = new ReferenceArray();
                    foreach (Solid solid in listSolid)
                    {
                        foreach(Face face in solid.Faces)
                        {
                            PlanarFace planarFace = face as PlanarFace;
                            if (planarFace != null)
                            {
                                XYZ normalFace = planarFace.FaceNormal.Normalize();
                                double dotProductHorizontal = horizontal.DotProduct(normalFace);
                                if(Math.Abs(Math.Abs(dotProductHorizontal) - 1) < 0.000001)
                                {
                                    if (face.Reference != null)
                                    {
                                        listHorizotalRef.Append(face.Reference);
                                    }
                                    
                                }
                                double dotProductVertical = vertical.DotProduct(normalFace);
                                if (Math.Abs(Math.Abs(dotProductVertical) - 1) < 0.000001)
                                {
                                    if (face.Reference != null)
                                    {
                                        listVerticalRef.Append(face.Reference);
                                    }
                                }
                            }
                        }
                    }
                    foreach(Line line in listLine)
                    {
                        XYZ directionLine = line.Direction.Normalize(); ;
                        double dotProductHorizontal = horizontal.DotProduct(directionLine);
                        if (Math.Abs(Math.Abs(dotProductHorizontal) - 1) < 0.000001)
                        {
                            if (line.Reference != null)
                            {
                                listHorizotalRef.Append(line.Reference);
                            }
                            
                        }
                        double dotProductVertical = vertical.DotProduct(directionLine);
                        if (Math.Abs(Math.Abs(dotProductVertical) - 1) < 0.000001)
                        {
                            if(line.Reference != null)
                            {
                                listVerticalRef.Append(line.Reference);
                            }
                        }
                    }
                    BoundingBoxXYZ boundingBox = element.get_BoundingBox(doc.ActiveView);
                    XYZ min = boundingBox.Min;
                    double extend = 1000 / 304.8;

                    XYZ pointHorizontal = min - vertical * extend;
                    XYZ pointVertical = min - horizontal * extend;
                    Line lineHorizon = Line.CreateUnbound(pointHorizontal, horizontal);
                    Line lineVertical = Line.CreateUnbound(pointVertical, vertical);
                    DimensionType dimType = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>()
                        .FirstOrDefault(x => x.Name == "");
                    using(Transaction t= new Transaction(doc, "CreateDim"))
                    {
                        t.Start();
                        doc.Create.NewDimension(doc.ActiveView, lineHorizon, listHorizotalRef);
                        doc.Create.NewDimension(doc.ActiveView, lineVertical, listVerticalRef);
                        t.Commit();
                    }
                    
                }
                

            }
            return Result.Succeeded;
        }
    }
}
