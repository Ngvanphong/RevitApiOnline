using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class FamilyCategoryHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            var form = ChangeFamilyTypeAppShow.formChangeFamilyType;
            var dataContext = form.DataContext as GridDataContext;
            FamilyCategoryVM familyCategoryVMSelected = dataContext.FamilyCategory;

            var familes = new FilteredElementCollector(doc).OfClass(typeof(Family))
                .Cast<Family>().Where(x => x.FamilyCategory.Id == familyCategoryVMSelected.FamilyCategoryId);

            List<FamilyVM> listFamiyVm= new List<FamilyVM>();
            foreach(Family family in familes)
            {
                FamilyVM familyVm = new FamilyVM();
                familyVm.FamilyId= family.Id;
                familyVm.FamilyName= family.Name;
                listFamiyVm.Add(familyVm);
            }
            ChangeFamilyTypeAppShow.listFamilyVm = listFamiyVm;
            
            FamilyOriginTarget item1 = new FamilyOriginTarget();
            item1.Families = listFamiyVm;
            FamilyOriginTarget item2= new FamilyOriginTarget();
            item2.Families = listFamiyVm;
            dataContext.ObservableFamilyOriginTarget.Add(item1);
            dataContext.ObservableFamilyOriginTarget.Add(item2);
        }

        public string GetName()
        {
            return "FamilyCategoryHandler";
        }
    }
}
