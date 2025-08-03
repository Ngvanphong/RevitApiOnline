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
                .Select(x => new BeamFamilyVM { FamilyId = x.Id, FamilyName = x.Name }).ToList();
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
            List<TreeViewItemVm> listTreeViewItem = new List<TreeViewItemVm>();
            foreach (Category category in doc.Settings.Categories)
            {
                TreeViewItemVm treeViewItem = new TreeViewItemVm();
                treeViewItem.Name= category.Name;
                treeViewItem.Id= category.Id;

                var familyCollectionItem = new FilteredElementCollector(doc).OfClass(typeof(Family))
               .Cast<Family>().Where(x => x.FamilyCategoryId == category.Id)
               .ToList();
                foreach(Family family in familyCollectionItem)
                {
                    TreeViewItemVm treeViewItemFamily = new TreeViewItemVm();
                    treeViewItemFamily.Name= family.Name;
                    treeViewItemFamily.Id= family.Id;
                    treeViewItem.Items.Add(treeViewItemFamily);
                    var symbolIds = family.GetFamilySymbolIds();
                    if (symbolIds != null)
                    {
                        foreach(ElementId idSy in family.GetFamilySymbolIds())
                        {
                            FamilySymbol familySym = doc.GetElement(idSy) as FamilySymbol;
                            TreeViewItemVm treeViewItemSymbol = new TreeViewItemVm();
                            treeViewItemSymbol.Name= familySym.Name;
                            treeViewItemSymbol.Id= familySym.Id;
                            treeViewItemFamily.Items.Add(treeViewItemSymbol);
                        }
                    }
                    treeViewItem.Items.Add(treeViewItemFamily);
                }
                if(treeViewItem.Items.Count > 0)
                {
                    listTreeViewItem.Add(treeViewItem);
                }
            }

            var form = new DataGridWpf(doc, listTreeViewItem);
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