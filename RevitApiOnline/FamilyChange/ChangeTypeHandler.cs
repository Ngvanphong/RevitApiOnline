using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.FamilyChange
{
    public class ChangeTypeHandler : IExternalEventHandler
    {
        void IExternalEventHandler.Execute(UIApplication app)
        {
            throw new NotImplementedException();
        }

        string IExternalEventHandler.GetName()
        {
            return "ChangeTypeHandler";
        }
    }
}
