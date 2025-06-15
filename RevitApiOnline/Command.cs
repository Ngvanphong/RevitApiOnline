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

            double offset = -1500;
            double offsetInch = UnitUtils.ConvertToInternalUnits(offset, UnitTypeId.Millimeters);


            FilteredElementCollector beamCollection = new FilteredElementCollector(doc, doc.ActiveView.Id)
                                                       .OfCategory(BuiltInCategory.OST_StructuralColumns).WhereElementIsNotElementType();

            List<Element> listBeam1500 = beamCollection.Where(x =>
            {
                Parameter parameter = x.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM);
                double offsetItem = parameter.AsDouble();
                Parameter paraStyle = x.get_Parameter(BuiltInParameter.SLANTED_COLUMN_TYPE_PARAM);
                int style = paraStyle.AsInteger();
                return Math.Abs(offsetItem - offsetInch) < 0.0001 && style == 0;
            }).ToList();

            // filter base offset;
            ElementId paraOffsetId = new ElementId((long)BuiltInParameter.SCHEDULE_BASE_LEVEL_OFFSET_PARAM);
            FilterRule doubleFilter = ParameterFilterRuleFactory.CreateEqualsRule(paraOffsetId, offsetInch, 0.0001);
            ElementParameterFilter baseOffsetFitler = new ElementParameterFilter(doubleFilter);

            ElementId parameterStyleId = new ElementId((long)BuiltInParameter.ALL_MODEL_TYPE_NAME);
            FilterRule fiterRuleForStyle = ParameterFilterRuleFactory.CreateContainsRule(parameterStyleId, "356x368");
            ElementParameterFilter styleFilter = new ElementParameterFilter(fiterRuleForStyle);

            double valueVolumn = 0.1;
            double valueVolumeInch = UnitUtils.ConvertToInternalUnits(valueVolumn, UnitTypeId.CubicMeters);
            ElementId paraVolumeId = new ElementId((long)BuiltInParameter.HOST_VOLUME_COMPUTED);
            FilterRule filterRuleVol = ParameterFilterRuleFactory.CreateGreaterRule(paraVolumeId, valueVolumeInch, 0.00001);
            ElementParameterFilter volumeFilter = new ElementParameterFilter(filterRuleVol);

            List<ElementFilter> listAndFilter = new List<ElementFilter> { baseOffsetFitler, styleFilter };
            LogicalAndFilter logicalAndFilter = new LogicalAndFilter(listAndFilter);


            List<ElementFilter> listOrFitler = new List<ElementFilter> { logicalAndFilter, volumeFilter };

            LogicalOrFilter logicalOrFilter = new LogicalOrFilter(listOrFitler);

            Category columnCategory = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralColumns);
            List<ElementId> listCategoryIds = new List<ElementId> { columnCategory.Id };
            ParameterFilterElement filter = null;
            using (Transaction t = new Transaction(doc, "CreateFilter"))
            {
                t.Start();
                filter = ParameterFilterElement.Create(doc, "Test1", listCategoryIds);
                filter.SetElementFilter(logicalOrFilter);
                doc.ActiveView.SetFilterVisibility(filter.Id, true);
                t.Commit();
            }

            FillPatternElement patternSolid = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement))
                                                .Cast<FillPatternElement>()
                                                .FirstOrDefault(x => x.GetFillPattern().Target == FillPatternTarget.Drafting
                                                                        && x.GetFillPattern().IsSolidFill);

            OverrideGraphicSettings overrideGraphicSettings = new OverrideGraphicSettings();
            overrideGraphicSettings.SetSurfaceForegroundPatternColor(new Autodesk.Revit.DB.Color(255, 0, 0));
            overrideGraphicSettings.SetSurfaceBackgroundPatternColor(new Autodesk.Revit.DB.Color(255, 0, 0));
            overrideGraphicSettings.SetSurfaceForegroundPatternId(patternSolid.Id);
            overrideGraphicSettings.SetSurfaceBackgroundPatternId(patternSolid.Id);
            overrideGraphicSettings.SetHalftone(true);
            using (Transaction t = new Transaction(doc, "SetOverride"))
            {
                t.Start();
                doc.ActiveView.SetFilterOverrides(filter.Id, overrideGraphicSettings);
                t.Commit();
            }



            IEnumerable<Element> listBeamOff = beamCollection.WherePasses(logicalOrFilter).ToElements();


            IEnumerable<ElementId> selectedIds = listBeamOff.Select(x => x.Id);

            //uiDoc.Selection.SetElementIds(selectedIds.ToList());




            return Result.Succeeded;
        }
    }

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem != null && elem is Wall)
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
