using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitApiOnline.Shared.Implements;
using RevitApiOnline.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace RevitApiOnline.RevitLearn
{
    public class RevitLearnApi
    {
        public void Learn(UIDocument uiDoc, Document doc)
        {
            ICollection<ElementId> ids = uiDoc.Selection.GetElementIds();

            // implement interface;
            ICurveUtilities curveUtilities = new CurveUtilities();
            List<Curve> listCurveWall = curveUtilities.GetCurvesFromDetailCurve(doc, ids);

            View view = doc.ActiveView;
            Level level = view.GenLevel;

            Parameter levelParameter = view.get_Parameter(BuiltInParameter.PLAN_VIEW_LEVEL);
            string levelName = levelParameter.AsString();

            using (Transaction t = new Transaction(doc, "CreateWall"))
            {
                t.Start();
                foreach (Curve curve in listCurveWall)
                {
                    Wall wall = Wall.Create(doc, curve, level.Id, false);
                }
                t.Commit();
            }

            // chon truoc
            IEnumerable<ElementId> selectedIds = uiDoc.Selection.GetElementIds();
            ElementId id = null;
            Element element = doc.GetElement(id);// co Id truy cap nguoc nguoc lai doi tuong

            Wall wallSelect = element as Wall; // ep kieu ve dung doi duong
            Floor floor = element as Floor;
            FloorType floorType;
            Ceiling ceiling = element as Ceiling;
            CeilingType ceilingType;

            Duct duct = element as Duct;
            Pipe pipe = element as Pipe;
            Rebar rebar = element as Rebar;
            bool isRebar = element is Rebar;
            Family family; // nhung family minh tao
            FamilyInstance familyInstance; // minh dat family ra ngoai project

            ElementType elementType = null;// type cua doi tuong;
            WallType wallType = elementType as WallType;
            FamilySymbol familySymbol = elementType as FamilySymbol;
            DuctType ductType = elementType as DuctType;
            PipeType pipeType = elementType as PipeType;
            RebarBarType rebarBartype = elementType as RebarBarType;

            Category category = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);

            //parameter
            FamilyInstance beamSelected = null;
            Parameter workPlanParameter = beamSelected.get_Parameter(BuiltInParameter.SKETCH_PLANE_PARAM);
            string workPlan = workPlanParameter.AsString();

            ElementId idTypeBeam = beamSelected.GetTypeId();
            FamilySymbol typeBeam = doc.GetElement(idTypeBeam) as FamilySymbol;
            Parameter onimParameter = typeBeam.get_Parameter(BuiltInParameter.OMNICLASS_CODE);
            string valueOnim = onimParameter.AsString();
            double valueDouble = onimParameter.AsDouble();
            ElementId valueId = onimParameter.AsElementId();
            int valueInt = onimParameter.AsInteger();
            string valueString = onimParameter.AsValueString();

            //custom para
            Parameter faBPara = beamSelected.LookupParameter("FaB1");
            double valueFab = faBPara.AsDouble();

            Parameter paramterB = typeBeam.LookupParameter("b");
            double valueB = paramterB.AsDouble();

            // doi tu don vi code(inch) den den vi minh mong muon
            double valueBMili = UnitUtils.ConvertFromInternalUnits(valueB, UnitTypeId.Meters);
            double hMili = 800; //mm
            //doi tu don vi hien thi sang don vi code
            double hInch = UnitUtils.ConvertToInternalUnits(hMili, UnitTypeId.Millimeters);



            // chon truoc

            //
            //pick point
            XYZ point1 = uiDoc.Selection.PickPoint("Pick a point");
            XYZ point2 = uiDoc.Selection.PickPoint("Pick a point");

            // pick face
            Reference faceRef = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face, "Pick face");
            Element element2 = doc.GetElement(faceRef);
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
            IList<Element> listWalls = uiDoc.Selection.PickElementsByRectangle(new WallSelectionFilter(), "Pick walls");

            PickedBox pickedBox = uiDoc.Selection.PickBox(PickBoxStyle.Directional, "Pick Box");
            XYZ min = pickedBox.Min;
            XYZ max = pickedBox.Max;
        }
    }
}
