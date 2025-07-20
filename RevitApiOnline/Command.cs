using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitApiOnline.DataGridLearn;
using RevitApiOnline.ListBox;
using System.Collections.Generic;
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

            var familyCollection = new FilteredElementCollector(doc).OfClass(typeof(Family))
                .Cast<Family>().Where(x => x.FamilyCategoryId.Value == (long)BuiltInCategory.OST_StructuralFraming)
                .ToList();
            DataGridAppShow.ListFamilyBeam = familyCollection;

            List<DataGridItem> listDataGrid = new List<DataGridItem>();

            List<BeamFamilyVM> listBeamFamilyVm = familyCollection
                .Select(x =>new BeamFamilyVM { FamilyId = x.Id, FamilyName = x.Name }).ToList();
            //foreach(Family family in familyCollection)
            //{
            //    BeamFamilyVM beamFamilyVM = new BeamFamilyVM();
            //    beamFamilyVM.FamilyName = family.Name;
            //    beamFamilyVM.FamilyId = family.Id;
            //    listBeamFamilyVm.Add(beamFamilyVM);
            //}

            foreach (Family family in familyCollection)
            {
                DataGridItem dataItem = new DataGridItem();
                dataItem.BeamFamilies = listBeamFamilyVm;
                listDataGrid.Add(dataItem);
            }

            var form = new DataGridWpf(doc);
            form.dataGridFamilyBeam.ItemsSource = listDataGrid;
            bool? resultForm= form.ShowDialog();
            if (resultForm == true)
            {
                List<DataGridItem> listResultDataGrid = form.dataGridFamilyBeam.ItemsSource as List<DataGridItem>;

            }



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