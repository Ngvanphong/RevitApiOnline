using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public static class ChangeFamilyTypeAppShow
    {
        public static ChangeFamilyTypeWpf formChangeFamilyType;
        public static List<FamilyVM> listFamilyVm;
        public static void ShowForm()
        {
            listFamilyVm = null;
            FamilyCategoryHandler familyCategoryHandler= new FamilyCategoryHandler();
            ExternalEvent familyCategoryEvent= ExternalEvent.Create(familyCategoryHandler);

            GetFamilyTypeHandler getTypeHandler = new GetFamilyTypeHandler();
            ExternalEvent getTypeEvent= ExternalEvent.Create(getTypeHandler);

            ChangeTypeHandler changeTypeHandler = new ChangeTypeHandler();
            ExternalEvent changeTypeEvent = ExternalEvent.Create(changeTypeHandler);
            formChangeFamilyType = new ChangeFamilyTypeWpf(familyCategoryEvent, getTypeEvent, changeTypeEvent);
            formChangeFamilyType.Show();
        }
    }
}
