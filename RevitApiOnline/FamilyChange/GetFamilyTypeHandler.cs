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
        public void Execute(UIApplication app)
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            return "GetFamilyTypeHandler";
        }
    }
}
