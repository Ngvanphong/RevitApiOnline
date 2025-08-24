using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    [Transaction(TransactionMode.Manual)]
    public class FamilyChangeBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            GridDataContext dataContext = new GridDataContext();
            dataContext.ObservableFamilyOriginTarget = new System.Collections.ObjectModel.ObservableCollection<FamilyOriginTarget>();

            var categories = doc.Settings.Categories;
            List<FamilyCategoryVM> listFamilyVm = new List<FamilyCategoryVM>();
            foreach (Category cate in categories)
            {
                if (cate.Id.Value == (long)BuiltInCategory.OST_StructuralColumns ||
                    cate.Id.Value == (long)BuiltInCategory.OST_StructuralFraming ||
                    cate.Id.Value == (long)BuiltInCategory.OST_MechanicalEquipment)
                {
                    FamilyCategoryVM familyCategoryVM = new FamilyCategoryVM();
                    familyCategoryVM.FamilyCategoryId = cate.Id;
                    familyCategoryVM.FamilyCategoryName = cate.Name;
                    listFamilyVm.Add(familyCategoryVM);
                }
            }

            ChangeFamilyTypeAppShow.ShowForm();

            ChangeFamilyTypeAppShow.formChangeFamilyType.commboxFamilyCategory.ItemsSource = listFamilyVm;

            //ChangeFamilyTypeAppShow.formChangeFamilyType.dataGrid.ItemsSource = dataContext.ObservableFamilyOriginTarget;
            //ChangeFamilyTypeAppShow.formChangeFamilyType.DataContext = dataContext;

            ChangeFamilyTypeAppShow.formChangeFamilyType.DataContext= dataContext;
            

            return Result.Succeeded;
        }
    }
}
