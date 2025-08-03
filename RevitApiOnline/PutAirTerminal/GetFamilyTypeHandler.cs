using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.PutAirTerminal
{
    public class GetFamilyTypeHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            Document doc = app.ActiveUIDocument.Document;
            AirtTerminalDataContext dataContext = AirTerminalAppShow.formPutAirTermianl.DataContext as AirtTerminalDataContext;
            FamilyVM selectedFamilyVm = dataContext.FamilySelected;
            Family selectedFamily = doc.GetElement(selectedFamilyVm.Id) as Family;
            List<FamilyTypeVM> listFamilyTypeVm = new List<FamilyTypeVM>();
            foreach(ElementId id in selectedFamily.GetFamilySymbolIds())
            {
                FamilySymbol familySymbol= doc.GetElement(id) as FamilySymbol;
                FamilyTypeVM familyTypeVM = new FamilyTypeVM();
                familyTypeVM.Id = familySymbol.Id;
                familyTypeVM.Name = familySymbol.Name;
                listFamilyTypeVm.Add(familyTypeVM);
            }
            dataContext.ListFamilyTypeVm = listFamilyTypeVm;




        }

        public string GetName()
        {
            return "GetFamilyTypeHandler1";
        }
    }
}
