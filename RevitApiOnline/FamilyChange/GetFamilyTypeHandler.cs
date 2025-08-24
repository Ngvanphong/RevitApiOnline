using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class GetFamilyTypeHandler : IExternalEventHandler
    {

        public static int IndexRowChanged = -1;
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            var form = ChangeFamilyTypeAppShow.formChangeFamilyType;
            var dataContext = form.DataContext as GridDataContext;
            var rowSelected = form.dataGridChangeType.SelectedItem as FamilyOriginTarget;
            FamilyVM familySelected = rowSelected.Family;
            Family family = doc.GetElement(familySelected.FamilyId) as Family;
            List<TypeVM> listTypeVm = new List<TypeVM>();
            foreach(ElementId typeId in family.GetFamilySymbolIds())
            {
                FamilySymbol symbol= doc.GetElement(typeId) as FamilySymbol;
                TypeVM typeVM = new TypeVM();
                typeVM.TypeName= symbol.Name;
                typeVM.TypeId = symbol.Id;
                listTypeVm.Add(typeVM);
            }
            rowSelected.ListTypeVM= listTypeVm;

        }

        public string GetName()
        {
            return "GetFamilyTypeHandler";
        }
    }
}
