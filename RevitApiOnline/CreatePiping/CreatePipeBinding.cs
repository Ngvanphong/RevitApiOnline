using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using RevitApiOnline.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RevitApiOnline.CreatePiping
{
    [Transaction(TransactionMode.Manual)]
    public class CreatePipeBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            var ids = uiDoc.Selection.GetElementIds();
            Duct mainDuct = null;
            List<MEPModel> listAirTerminal = new List<MEPModel>();
            foreach (ElementId id in ids)
            {
                Element element = doc.GetElement(id);
                if (element is Duct)
                {
                    mainDuct = (Duct)element;
                }
                else if (element is FamilyInstance familyInstance)
                {
                    MEPModel mepModel = familyInstance.MEPModel;
                    if (mepModel != null)
                    {
                        Parameter parameter = familyInstance.get_Parameter(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM);
                        if (parameter != null)
                        {
                            string classDifnition = parameter.AsString();
                            if (!string.IsNullOrWhiteSpace(classDifnition))
                            {
                                string removeSpace = classDifnition.Replace(" ", "");
                                if (removeSpace == MEPSystemClassification.SupplyAir.ToString())
                                {
                                    listAirTerminal.Add(mepModel);
                                }
                            }
                            
                        }
                    }
                }
            }

            MEPSystem mepSystem = mainDuct.MEPSystem;
            ElementId systemTypeId = mainDuct.MEPSystem.GetTypeId();
            MEPSystemType mepSystemType = doc.GetElement(systemTypeId) as MEPSystemType;
            Location ductLocation = mainDuct.Location;
            LocationCurve ductCurve = ductLocation as LocationCurve;
            Line lineDuct = ductCurve.Curve as Line;
            XYZ ductDirection = lineDuct.Direction.Normalize();

            Connector startConnector = mainDuct.ConnectorManager.Lookup(0);
            Connector endConnector = mainDuct.ConnectorManager.Lookup(1);

            ConnectorSet connectorSet = mainDuct.ConnectorManager.Connectors;
            foreach (Connector connector in connectorSet)
            {

            }
            DuctType mainDuctType = mainDuct.DuctType;
            ElementId levelId= mainDuct.LevelId;
            List<PointDuct> listPointDuctDivide = new List<PointDuct>();
            using(TransactionGroup tg= new TransactionGroup(doc, "GroupTran"))
            {
                tg.Start();
                foreach (MEPModel airTerminal in listAirTerminal)
                {
                    Connector connector = null;
                    foreach (Connector item in airTerminal.ConnectorManager.Connectors)
                    {
                        connector = item;
                        break;
                    }
                    if (connector == null) continue;
                    XYZ originConnector = connector.Origin;
                    XYZ normalConnector = connector.CoordinateSystem.BasisZ.Normalize();
                    XYZ normalMainDuct = null;
                    if (ductDirection.IsAlmostEqualTo(XYZ.BasisX, 0.00001) || ductDirection.IsAlmostEqualTo(-XYZ.BasisX, 0.00001))
                    {
                        normalMainDuct = ductDirection.CrossProduct(XYZ.BasisY).Normalize();
                    }
                    else
                    {
                        normalMainDuct = ductDirection.CrossProduct(XYZ.BasisX).Normalize();
                    }

                    Plane horizontalDuctPlane = Plane.CreateByNormalAndOrigin(normalMainDuct, startConnector.Origin);
                    XYZ intersection1 = XYZCalculation.IntersectionPointPlanebyVector(horizontalDuctPlane, originConnector, normalConnector);
                    Duct verticalDuct = null;
                    using (Transaction t = new Transaction(doc, "CreateVerticalDuct"))
                    {
                        t.Start();
                        verticalDuct = Duct.Create(doc, mainDuctType.Id, levelId, connector, intersection1);
                        t.Commit();
                    }

                    XYZ normalVereticalPlane = ductDirection.CrossProduct(XYZ.BasisZ).Normalize();
                    Plane verticalDuctPlane = Plane.CreateByNormalAndOrigin(normalVereticalPlane, startConnector.Origin);
                    XYZ intersection2 = XYZCalculation.IntersectionPointPlanebyVector(verticalDuctPlane, intersection1, normalVereticalPlane);
                    Duct horizontalDuct = null;
                    using (Transaction t = new Transaction(doc, "CreateVerticalDuct"))
                    {
                        t.Start();
                        horizontalDuct = Duct.Create(doc, mepSystemType.Id, mainDuctType.Id, levelId, intersection1, intersection2);
                        t.Commit();
                    }
                    PointDuct pointDuct = new PointDuct();
                    pointDuct.Point = intersection2;
                    pointDuct.Duct = horizontalDuct;
                    listPointDuctDivide.Add(pointDuct);
                }

                List<Line> listLineResult = new List<Line>();
                List<Line> listLineDivide = new List<Line> { lineDuct };
                while (listLineDivide != null && listLineDivide.Count > 0)
                {
                    Line lineDivide = listLineDivide[0];
                    XYZ start = lineDivide.GetEndPoint(0);
                    XYZ end = lineDivide.GetEndPoint(1);
                    listLineDivide.RemoveAt(0);
                    bool hasDivide = false;
                    foreach (PointDuct intesection in listPointDuctDivide)
                    {
                        XYZ pointCheck = intesection.Point;
                        double d1 = pointCheck.DistanceTo(start);
                        double d2 = pointCheck.DistanceTo(end);

                        if (d1 > 0.001 && d2 > 0.001 && Math.Abs(d1 + d2 - lineDivide.Length) < 0.001)
                        {
                            Line line1 = Line.CreateBound(start, pointCheck);
                            Line line2 = Line.CreateBound(pointCheck, end);
                            listLineDivide.Add(line1);
                            listLineDivide.Add(line2);
                            hasDivide = true;
                            break;
                        }
                    }
                    if (!hasDivide) listLineResult.Add(lineDivide);
                }
                using (Transaction t = new Transaction(doc, "DivideDuct"))
                {
                    t.Start();
                    doc.Delete(mainDuct.Id);
                    t.Commit();
                }
                
                foreach (Line line in listLineResult)
                {
                    using (Transaction t = new Transaction(doc, "DivideDuct"))
                    {
                        t.Start();
                        Duct.Create(doc, mepSystemType.Id, mainDuctType.Id, levelId, line.GetEndPoint(0), line.GetEndPoint(1));
                        t.Commit();
                    }
                }


                tg.Assimilate();
            }
            









            #region comment
            //if (ids == null) return Result.Succeeded;
            //foreach(ElementId id in ids)
            //{
            //    Element element= doc.GetElement(id);
            //    List<Solid> listSolid = new List<Solid>();
            //    List<Line> listLine = new List<Line>();
            //    GeometryHelper.GeoSolidElement(doc, element,ref listSolid, ref listLine);
            //    if (element is FamilyInstance)
            //    {
            //        FamilyInstance instance = element as FamilyInstance;
            //        Transform transform = instance.GetTransform();
            //        XYZ horizontal = transform.BasisX.Normalize();
            //        XYZ vertical = transform.BasisY.Normalize();
            //        ReferenceArray listHorizotalRef = new ReferenceArray();
            //        ReferenceArray listVerticalRef = new ReferenceArray();
            //        foreach (Solid solid in listSolid)
            //        {
            //            foreach (Face face in solid.Faces)
            //            {
            //                PlanarFace planarFace = face as PlanarFace;
            //                if (planarFace != null)
            //                {
            //                    XYZ normalFace = planarFace.FaceNormal.Normalize();
            //                    double dotProductHorizontal = horizontal.DotProduct(normalFace);
            //                    if (Math.Abs(Math.Abs(dotProductHorizontal) - 1) < 0.000001)
            //                    {
            //                        if (face.Reference != null)
            //                        {
            //                            listHorizotalRef.Append(face.Reference);
            //                        }

            //                    }
            //                    double dotProductVertical = vertical.DotProduct(normalFace);
            //                    if (Math.Abs(Math.Abs(dotProductVertical) - 1) < 0.000001)
            //                    {
            //                        if (face.Reference != null)
            //                        {
            //                            listVerticalRef.Append(face.Reference);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        foreach (Line line in listLine)
            //        {
            //            XYZ directionLine = line.Direction.Normalize(); ;
            //            double dotProductHorizontal = horizontal.DotProduct(directionLine);
            //            if (Math.Abs(Math.Abs(dotProductHorizontal) - 1) < 0.000001)
            //            {
            //                if (line.Reference != null)
            //                {
            //                    listHorizotalRef.Append(line.Reference);
            //                }

            //            }
            //            double dotProductVertical = vertical.DotProduct(directionLine);
            //            if (Math.Abs(Math.Abs(dotProductVertical) - 1) < 0.000001)
            //            {
            //                if (line.Reference != null)
            //                {
            //                    listVerticalRef.Append(line.Reference);
            //                }
            //            }
            //        }

            //        // tim grid
            //        GridInfo gridInfo = FindGridAroundFoundation.GetGridAroundFoundation(doc, instance,
            //            horizontal, vertical);
            //        if (gridInfo.VerticalGrid != null)
            //        {
            //            listHorizotalRef.Append(new Reference(gridInfo.VerticalGrid));
            //        }
            //        if (gridInfo.HorizontalGrid != null)
            //        {
            //            listVerticalRef.Append(new Reference(gridInfo.HorizontalGrid));
            //        }

            //        BoundingBoxXYZ boundingBox = element.get_BoundingBox(doc.ActiveView);
            //        XYZ min = boundingBox.Min;
            //        double extend = 1000 / 304.8;

            //        XYZ pointHorizontal = min - vertical * extend;
            //        XYZ pointVertical = min - horizontal * extend;
            //        Line lineHorizon = Line.CreateUnbound(pointHorizontal, horizontal);
            //        Line lineVertical = Line.CreateUnbound(pointVertical, vertical);
            //        DimensionType dimType = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>()
            //            .FirstOrDefault(x => x.Name == "");
            //        Dimension horizontalDim = null;
            //        Dimension verticalDim = null;
            //        using(Transaction t= new Transaction(doc, "CreateDim"))
            //        {
            //            t.Start();
            //            horizontalDim= doc.Create.NewDimension(doc.ActiveView, lineHorizon, listHorizotalRef);
            //            verticalDim= doc.Create.NewDimension(doc.ActiveView, lineVertical, listVerticalRef);
            //            t.Commit();
            //        }
            //        RemoveZeroDimension.RemoveZeroDim(doc, horizontalDim);
            //        RemoveZeroDimension.RemoveZeroDim(doc, verticalDim);


            //    }


            //}
            //double sum = SumAdd(5);
            #endregion
            return Result.Succeeded;
        }
        public double SumAdd(double n)
        {
            if (n == 1) return 1;
            else return n * SumAdd(n - 1);
        }
    }

    public class PointDuct
    {
        public XYZ Point { set; get; }
        public Duct Duct { set; get; }
    }
}
