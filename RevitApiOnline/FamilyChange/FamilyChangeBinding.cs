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
            ChangeFamilyTypeAppShow.ShowForm();
            GridDataContext dataContext = new GridDataContext();
           
            var categories = doc.Settings.Categories;
            List<FamilyCategoryVM> listFamilyVm= new List<FamilyCategoryVM>();

            ChangeFamilyTypeAppShow.formChangeFamilyType.combob.ItemsSource = listFamilyVm;
            ChangeFamilyTypeAppShow.formChangeFamilyType.dataGrid.ItemsSource = dataContext.ObservableFamilyOriginTarget;
            ChangeFamilyTypeAppShow.formChangeFamilyType.DataContext = dataContext;

            return Result.Succeeded;
        }
    }
}
