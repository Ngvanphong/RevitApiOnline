using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
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

            //custom para
            Parameter faBPara = beamSelected.LookupParameter("FaB1");
            double valueFab = faBPara.AsDouble();

            Parameter paramterB = typeBeam.LookupParameter("b");
            double valueB = paramterB.AsDouble();

            // doi tu don vi code(inch) den den vi minh mong muon
            double valueBMili = UnitUtils.ConvertFromInternalUnits(valueB, UnitTypeId.Millimeters);
            double hMili = 800; //mm
            //doi tu don vi hien thi sang don vi code
            double hInch = UnitUtils.ConvertToInternalUnits(hMili, UnitTypeId.Millimeters);
        }
    }
}
