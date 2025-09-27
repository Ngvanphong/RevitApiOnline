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
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RevitApiOnline.CreatePiping
{
    [Transaction(TransactionMode.Manual)]
    public class CreatePipeBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;


            #region MEP
            //var ids = uiDoc.Selection.GetElementIds();
            //Duct mainDuct = null;
            //List<MEPModel> listAirTerminal = new List<MEPModel>();
            //foreach (ElementId id in ids)
            //{
            //    Element element = doc.GetElement(id);
            //    if (element is Duct)
            //    {
            //        mainDuct = (Duct)element;
            //    }
            //    else if (element is FamilyInstance familyInstance)
            //    {
            //        MEPModel mepModel = familyInstance.MEPModel;
            //        if (mepModel != null)
            //        {
            //            Parameter parameter = familyInstance.get_Parameter(BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM);
            //            if (parameter != null)
            //            {
            //                string classDifnition = parameter.AsString();
            //                if (!string.IsNullOrWhiteSpace(classDifnition))
            //                {
            //                    string removeSpace = classDifnition.Replace(" ", "");
            //                    if (removeSpace == MEPSystemClassification.SupplyAir.ToString())
            //                    {
            //                        listAirTerminal.Add(mepModel);
            //                    }
            //                }

            //            }
            //        }
            //    }
            //}

            //MEPSystem mepSystem = mainDuct.MEPSystem;
            //ElementId systemTypeId = mainDuct.MEPSystem.GetTypeId();
            //MEPSystemType mepSystemType = doc.GetElement(systemTypeId) as MEPSystemType;
            //Location ductLocation = mainDuct.Location;
            //LocationCurve ductCurve = ductLocation as LocationCurve;
            //Line lineDuct = ductCurve.Curve as Line;
            //XYZ ductDirection = lineDuct.Direction.Normalize();

            //Connector startConnector = mainDuct.ConnectorManager.Lookup(0);
            //Connector endConnector = mainDuct.ConnectorManager.Lookup(1);

            //ConnectorSet connectorSet = mainDuct.ConnectorManager.Connectors;
            //foreach (Connector connector in connectorSet)
            //{

            //}
            //DuctType mainDuctType = mainDuct.DuctType;
            //ElementId levelId= mainDuct.LevelId;
            //List<PointDuct> listPointDuctDivide = new List<PointDuct>();
            //using(TransactionGroup tg= new TransactionGroup(doc, "GroupTran"))
            //{
            //    tg.Start();
            //    List<Duct> listHorizontalDuct = new List<Duct>();
            //    foreach (MEPModel airTerminal in listAirTerminal)
            //    {
            //        Connector connector = null;
            //        foreach (Connector item in airTerminal.ConnectorManager.Connectors)
            //        {
            //            connector = item;
            //            break;
            //        }
            //        if (connector == null) continue;
            //        XYZ originConnector = connector.Origin;
            //        XYZ normalConnector = connector.CoordinateSystem.BasisZ.Normalize();
            //        XYZ normalMainDuct = null;
            //        if (ductDirection.IsAlmostEqualTo(XYZ.BasisX, 0.00001) || ductDirection.IsAlmostEqualTo(-XYZ.BasisX, 0.00001))
            //        {
            //            normalMainDuct = ductDirection.CrossProduct(XYZ.BasisY).Normalize();
            //        }
            //        else
            //        {
            //            normalMainDuct = ductDirection.CrossProduct(XYZ.BasisX).Normalize();
            //        }

            //        Plane horizontalDuctPlane = Plane.CreateByNormalAndOrigin(normalMainDuct, startConnector.Origin);
            //        XYZ intersection1 = XYZCalculation.IntersectionPointPlanebyVector(horizontalDuctPlane, originConnector, normalConnector);
            //        Duct verticalDuct = null;
            //        using (Transaction t = new Transaction(doc, "CreateVerticalDuct"))
            //        {
            //            t.Start();
            //            verticalDuct = Duct.Create(doc, mainDuctType.Id, levelId, connector, intersection1);
            //            t.Commit();
            //        }
            //        SetSizeDuct(doc, verticalDuct, 100/304.8, 100/304.8);

            //        XYZ normalVereticalPlane = ductDirection.CrossProduct(XYZ.BasisZ).Normalize();
            //        Plane verticalDuctPlane = Plane.CreateByNormalAndOrigin(normalVereticalPlane, startConnector.Origin);
            //        XYZ intersection2 = XYZCalculation.IntersectionPointPlanebyVector(verticalDuctPlane, intersection1, normalVereticalPlane);
            //        Duct horizontalDuct = null;
            //        using (Transaction t = new Transaction(doc, "CreateVerticalDuct"))
            //        {
            //            t.Start();
            //            horizontalDuct = Duct.Create(doc, mepSystemType.Id, mainDuctType.Id, levelId, intersection1, intersection2);
            //            t.Commit();
            //        }
            //        SetSizeDuct(doc, horizontalDuct, 100 / 304.8, 100 / 304.8);

            //        CreateEblowDuct(doc, verticalDuct, horizontalDuct);

            //        listHorizontalDuct.Add(horizontalDuct);

            //        PointDuct pointDuct = new PointDuct();
            //        pointDuct.Point = intersection2;
            //        pointDuct.Duct = horizontalDuct;
            //        listPointDuctDivide.Add(pointDuct);
            //    }

            //    List<Line> listLineResult = new List<Line>();
            //    List<Line> listLineDivide = new List<Line> { lineDuct };
            //    while (listLineDivide != null && listLineDivide.Count > 0)
            //    {
            //        Line lineDivide = listLineDivide[0];
            //        XYZ start = lineDivide.GetEndPoint(0);
            //        XYZ end = lineDivide.GetEndPoint(1);
            //        listLineDivide.RemoveAt(0);
            //        bool hasDivide = false;
            //        foreach (PointDuct intesection in listPointDuctDivide)
            //        {
            //            XYZ pointCheck = intesection.Point;
            //            double d1 = pointCheck.DistanceTo(start);
            //            double d2 = pointCheck.DistanceTo(end);

            //            if (d1 > 0.001 && d2 > 0.001 && Math.Abs(d1 + d2 - lineDivide.Length) < 0.001)
            //            {
            //                Line line1 = Line.CreateBound(start, pointCheck);
            //                Line line2 = Line.CreateBound(pointCheck, end);
            //                listLineDivide.Add(line1);
            //                listLineDivide.Add(line2);
            //                hasDivide = true;
            //                break;
            //            }
            //        }
            //        if (!hasDivide) listLineResult.Add(lineDivide);
            //    }

            //    double widthMainDuct = mainDuct.Width;
            //    double heightMainDuct = mainDuct.Height;
            //    using (Transaction t = new Transaction(doc, "DivideDuct"))
            //    {
            //        t.Start();
            //        doc.Delete(mainDuct.Id);
            //        t.Commit();
            //    }

            //    List<Duct> listMainDuctDivide = new List<Duct>();
            //    foreach (Line line in listLineResult)
            //    {
            //        Duct duct = null;
            //        using (Transaction t = new Transaction(doc, "DivideDuct"))
            //        {
            //            t.Start();
            //            duct= Duct.Create(doc, mepSystemType.Id, mainDuctType.Id, levelId, line.GetEndPoint(0), line.GetEndPoint(1));
            //            t.Commit();
            //        }
            //        SetSizeDuct(doc, duct, widthMainDuct, heightMainDuct);
            //        listMainDuctDivide.Add(duct);
            //    }
            //    foreach(Duct horizonDuct in listHorizontalDuct)
            //    {
            //        XYZ teeLocation = horizonDuct.ConnectorManager.Lookup(1).Origin;
            //        Duct main1 = null;
            //        Duct main2 = null;
            //        foreach(Duct ductDivide in listMainDuctDivide)
            //        {
            //            List<XYZ> listPointCheck = new List<XYZ> { ductDivide.ConnectorManager.Lookup(0).Origin, ductDivide.ConnectorManager.Lookup(1).Origin };
            //            bool hasSamePoint = listPointCheck.Exists(item => item.IsAlmostEqualTo(teeLocation, 0.0001));
            //            if (hasSamePoint)
            //            {
            //                if (main1 == null) main1 = ductDivide;
            //                else main2 = ductDivide;
            //            }
            //            if (main1 != null && main2 != null) break;
            //        }
            //        CreateTeeDuct(doc, main1, main2, horizonDuct);
            //    }



            //    tg.Assimilate();
            //}

            #endregion

            #region Floor
            List<Floor> listFoundation = new List<Floor>();
            foreach(ElementId id in uiDoc.Selection.GetElementIds())
            {
                Floor floor = doc.GetElement(id) as Floor;
                if(floor!=null && floor.Category.Id.Value == (long)BuiltInCategory.OST_StructuralFoundation)
                {
                    listFoundation.Add(floor);
                }
            }
            
            using(TransactionGroup tg= new TransactionGroup(doc, "CreateSlab"))
            {
                tg.Start();
                FloorType floorType200 = new FilteredElementCollector(doc).OfClass(typeof(FloorType)).Cast<FloorType>()
                                        .First(x => x.Name == "Conc 200mm");
                foreach(Floor floor in listFoundation)
                {
                    IList<Reference> listBottomRef = HostObjectUtils.GetBottomFaces(floor);
                    List<PlanarFace> listPlannarFaceBottom = new List<PlanarFace>();
                    foreach(Reference refFace in listBottomRef)
                    {
                        PlanarFace plannarFace = doc.GetElement(refFace).GetGeometryObjectFromReference(refFace) as PlanarFace;
                        if (plannarFace != null)
                        {
                            listPlannarFaceBottom.Add(plannarFace);
                        }
                    }

                    foreach (PlanarFace plannarFace in listPlannarFaceBottom)
                    {
                        Floor btlFloor = null;
                        IList<CurveLoop> listCurveloop= plannarFace.GetEdgesAsCurveLoops();
                        List<CurveLoop> listOffsetCurveloop = new List<CurveLoop>();
                        foreach(CurveLoop curveLoop in listCurveloop)
                        {
                            CurveLoop newCurvelop = CurveLoop.CreateViaOffset(curveLoop, 300 / 304.8, -XYZ.BasisZ);
                            listOffsetCurveloop.Add(newCurvelop);
                        }
                        using(Transaction t= new Transaction(doc, "CreateBTL"))
                        {
                            t.Start();
                            FailureHandlingOptions options = t.GetFailureHandlingOptions();
                            options.SetFailuresPreprocessor(new OverrrideOverlayFloor());
                            t.SetFailureHandlingOptions(options);
                            btlFloor = Floor.Create(doc, listOffsetCurveloop, floorType200.Id, floor.LevelId);
                            t.Commit();
                        }
                        using(Transaction t2= new Transaction(doc, "SetOffset"))
                        {
                            t2.Start();
                            double thickness = floor.FloorType.get_Parameter(BuiltInParameter.FLOOR_ATTR_DEFAULT_THICKNESS_PARAM).AsDouble();
                            double offsetMain = floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble();
                            btlFloor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).Set(-thickness+offsetMain);
                            t2.Commit();
                        }


                    }


                }
                tg.Assimilate();
            }



            #endregion





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

        public void SetSizeDuct(Document doc,Duct duct, double width, double height)
        {
            using(Transaction t= new Transaction(doc, "SetSize"))
            {
                t.Start();
                duct.get_Parameter(BuiltInParameter.RBS_CURVE_WIDTH_PARAM).Set(width);
                duct.get_Parameter(BuiltInParameter.RBS_CURVE_HEIGHT_PARAM).Set(height);

                t.Commit();
            }
        }

        public void CreateEblowDuct(Document doc, Duct verticalDuct, Duct horizontalDuct)
        {
            using (Transaction t = new Transaction(doc, "SetSize"))
            {
                t.Start();
                doc.Create.NewElbowFitting(verticalDuct.ConnectorManager.Lookup(1), horizontalDuct.ConnectorManager.Lookup(0));
                t.Commit();
            }
        }

        public void CreateTeeDuct(Document doc, Duct main1, Duct main2, Duct horizontalDuct)
        {
            XYZ teeLocation = horizontalDuct.ConnectorManager.Lookup(1).Origin;
            //Connector teeConnectorOfMain1 = main1.ConnectorManager.Lookup(0).Origin.IsAlmostEqualTo(teeLocation, 0.0001) ? main1.ConnectorManager.Lookup(0): main1.ConnectorManager.Lookup(1);
            Connector teeConnectorOfMain1 = null;
            foreach(Connector connector in main1.ConnectorManager.Connectors)
            {
                if (connector.Origin.IsAlmostEqualTo(teeLocation, 0.00001))
                {
                    teeConnectorOfMain1 = connector;
                    break;
                }
            }

            Connector teeConnectorOfMain2 = main2.ConnectorManager.Lookup(0).Origin.IsAlmostEqualTo(teeLocation, 0.0001) ? 
                                            main2.ConnectorManager.Lookup(0) : main2.ConnectorManager.Lookup(1);


            using (Transaction t = new Transaction(doc, "SetSize"))
            {
                t.Start();
                doc.Create.NewTeeFitting(teeConnectorOfMain1, teeConnectorOfMain2, horizontalDuct.ConnectorManager.Lookup(1));
                t.Commit();
            }
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

    public class OverrrideOverlayFloor : IFailuresPreprocessor
    {
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            foreach(FailureMessageAccessor message in failuresAccessor.GetFailureMessages())
            {
                var serversity = message.GetSeverity();
                if(serversity == FailureSeverity.Warning)
                {
                    failuresAccessor.DeleteWarning(message);
                }
                else if(serversity == FailureSeverity.Error)
                {
                    return FailureProcessingResult.ProceedWithRollBack;
                }
            }
            return FailureProcessingResult.Continue;
        }
    }
}
