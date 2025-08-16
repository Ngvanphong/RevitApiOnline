using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class FamilyCategoryHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            var form = ChangeFamilyTypeAppShow.formChangeFamilyType;
            var dataContext = form.DataContext as GridDataContext;
            FamilyCategoryVM familyCategoryVMSelected = dataContext.FamilyCategory;
            /// get famiy of selected famiy categroy
            /// ///
            /// 
            List<FamilyVM> listFamiyVm= new List<FamilyVM>();
            dataContext.Families = listFamiyVm;
        }

        public string GetName()
        {
            return "FamilyCategoryHandler";
        }
    }
}
