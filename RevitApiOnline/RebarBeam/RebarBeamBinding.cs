using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.RebarBeam
{
    [Transaction(TransactionMode.Manual)]
    public class RebarBeamBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            FamilyInstance beam = doc.GetElement(uiDoc.Selection.GetElementIds().FirstOrDefault()) as FamilyInstance;
            FamilySymbol typeBeam = doc.GetElement(beam.GetTypeId()) as FamilySymbol;
            double b = typeBeam.LookupParameter("b").AsDouble();
            double h = typeBeam.LookupParameter("h").AsDouble();
            Line lineBeam = (beam.Location as LocationCurve).Curve as Line;
            XYZ directionBeam = lineBeam.Direction.Normalize();
            RebarBarType stirrupType = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>()
                .FirstOrDefault(x => x.Name == "13M");
            RebarHookType rebarHookType = new FilteredElementCollector(doc).OfClass(typeof(RebarHookType)).Cast<RebarHookType>()
                .FirstOrDefault(x => x.Name == "Stirrup/Tie - 135 deg");

            XYZ pStartBeam = lineBeam.GetEndPoint(0);
            XYZ normalBeamHorizontal = directionBeam.CrossProduct(XYZ.BasisZ).Normalize();
            XYZ normalBeamVertical = normalBeamHorizontal.CrossProduct(directionBeam).Normalize();
            RebarCoverType rebarCoverType = doc.GetElement(beam.get_Parameter(BuiltInParameter.CLEAR_COVER_OTHER).AsElementId()) as RebarCoverType;
            double rebarCover = rebarCoverType.CoverDistance;
            double bRebar = b - 2 * rebarCover - stirrupType.BarModelDiameter;
            double offsetToTop = rebarCover + stirrupType.BarNominalDiameter / 2;
            double offsetToBot = h - (rebarCover + stirrupType.BarNominalDiameter / 2);

            Transform transfomLeft = Transform.CreateTranslation(normalBeamHorizontal * bRebar / 2);
            Transform transformRight = Transform.CreateTranslation(-normalBeamHorizontal * bRebar / 2);
            Transform transformTop = Transform.CreateTranslation(-normalBeamVertical * offsetToTop);
            Transform transformBot = Transform.CreateTranslation(-normalBeamVertical * offsetToBot);


            Transform moveToStartRebar = Transform.CreateTranslation(directionBeam * 100 / 304.8);
            pStartBeam = moveToStartRebar.OfPoint(pStartBeam);
            XYZ leftTop = pStartBeam;
            leftTop = transfomLeft.OfPoint(leftTop);
            leftTop = transformTop.OfPoint(leftTop);
            XYZ rightTop = pStartBeam;
            rightTop = transformRight.OfPoint(rightTop);
            rightTop = transformTop.OfPoint(rightTop);
            XYZ rightBot = pStartBeam;
            rightBot = transformRight.OfPoint(rightBot);
            rightBot = transformBot.OfPoint(rightBot);
            XYZ leftBot = pStartBeam;
            leftBot = transfomLeft.OfPoint(leftBot);
            leftBot = transformBot.OfPoint(leftBot);

            Line line1 = Line.CreateBound(leftTop, rightTop);
            Line line2 = Line.CreateBound(rightTop, rightBot);
            Line line3 = Line.CreateBound(rightBot, leftBot);
            Line line4 = Line.CreateBound(leftBot, leftTop);
            IList<Curve> listCurve = new List<Curve>() { line1, line2, line3, line4 };

            Rebar rebar = null;
            using (Transaction t = new Transaction(doc, "CreateRebarCurve"))
            {
                t.Start();
#if REVIT2025
                rebar = Rebar.CreateFromCurves(doc, RebarStyle.StirrupTie, stirrupType, rebarHookType, rebarHookType, beam, directionBeam, listCurve,
                                               RebarHookOrientation.Right, RebarHookOrientation.Right, true, true);
#elif REVIT2026
            rebar = Rebar.CreateFromCurves(doc, RebarStyle.StirrupTie, stirrupType, rebarHookType, rebarHookType, beam, directionBeam, listCurve,
                                               RebarHookOrientation.Right, RebarHookOrientation.Right, true, true);
#endif
                t.Commit();
            }
            using (Transaction t2 = new Transaction(doc, "SetLayout"))
            {
                t2.Start();
                RebarShapeDrivenAccessor drivenAccessor = rebar.GetShapeDrivenAccessor();
                drivenAccessor.SetLayoutAsMaximumSpacing(200 / 304.8, lineBeam.Length, true, false, false);
                t2.Commit();
            }

            using (Transaction t2 = new Transaction(doc, "SetLayout"))
            {
                t2.Start();
                rebar.SetUnobscuredInView(doc.ActiveView, true);
                t2.Commit();
            }

            // thep chu
            RebarBarType typeRebarMain = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>()
                .FirstOrDefault(x => x.Name == "22M");
            XYZ sBeam = lineBeam.GetEndPoint(0);
            Transform toStartMain1 = Transform.CreateTranslation(directionBeam * 50 / 304.8);
            Transform toTopMainBeam = Transform.CreateTranslation(-normalBeamVertical * (offsetToTop + stirrupType.BarModelDiameter / 2 + typeRebarMain.BarModelDiameter / 2));
            XYZ sRebarMain = toStartMain1.OfPoint(sBeam);
            sRebarMain = toTopMainBeam.OfPoint(sRebarMain);

            XYZ eBeam = lineBeam.GetEndPoint(1);
            Transform toEndMain1 = Transform.CreateTranslation(-directionBeam * 50 / 304.8);
            XYZ eRebarMain = toEndMain1.OfPoint(eBeam);
            eRebarMain = toTopMainBeam.OfPoint(eRebarMain);

            Transform toHockTop = Transform.CreateTranslation(-normalBeamVertical * 400 / 304.8);
            XYZ hockStart = toHockTop.OfPoint(sRebarMain);
            XYZ hockEnd = toHockTop.OfPoint(eRebarMain);

            Line line1M = Line.CreateBound(hockStart, sRebarMain);
            Line line2M = Line.CreateBound(sRebarMain, eRebarMain);
            Line line3M = Line.CreateBound(eRebarMain, hockEnd);

            IList<Curve> listCurveMain = new List<Curve>() { line1M, line2M, line3M };

            Rebar rebarMain = null;
            using (Transaction t = new Transaction(doc, "CreateRebarCurve"))
            {
                t.Start();
                rebarMain = Rebar.CreateFromCurves(doc, RebarStyle.Standard, typeRebarMain, null, null, beam, normalBeamHorizontal, listCurveMain,
                    RebarHookOrientation.Right, RebarHookOrientation.Right, true, true);
                t.Commit();
            }
            double lengthArrayMain = b - (2 * rebarCover + 2 * stirrupType.BarModelDiameter + typeRebarMain.BarModelDiameter);
            using (Transaction t = new Transaction(doc, "Move Rebar"))
            {
                t.Start();
                XYZ vectorMove = -normalBeamHorizontal * lengthArrayMain / 2;
                ElementTransformUtils.MoveElement(doc, rebarMain.Id, vectorMove);
                t.Commit();
            }
            using (Transaction t2 = new Transaction(doc, "SetLayout"))
            {
                t2.Start();
                RebarShapeDrivenAccessor drivenAccessor = rebarMain.GetShapeDrivenAccessor();
                drivenAccessor.SetLayoutAsFixedNumber(4, lengthArrayMain, true, true, true);
                t2.Commit();
            }





            return Result.Succeeded;
        }
    }
}
