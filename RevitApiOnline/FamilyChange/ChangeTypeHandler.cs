using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class ChangeTypeHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            var form = ChangeFamilyTypeAppShow.formChangeFamilyType;
            var listFamilyOriginTarget= form.dataGrid.ItemsSource as ObservableCollection<FamilyOriginTarget>;

        }

        public string GetName()
        {
            return "ChangeTypeHandler";
        }
    }
}
