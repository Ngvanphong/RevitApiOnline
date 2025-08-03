using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace RevitApiOnline.PutAirTerminal
{
    public static class AirTerminalAppShow
    {
        public static PutAirTerminalWpf formPutAirTermianl;
        public static void ShowForm()
        {
            GetFamilyTypeHandler familyTypeHandler = new GetFamilyTypeHandler();
            ExternalEvent familyTypeEvent= ExternalEvent.Create(familyTypeHandler);

            PutAirTerminalHandler putAirHandler= new PutAirTerminalHandler();
            ExternalEvent putAirEvent = ExternalEvent.Create(putAirHandler);

            formPutAirTermianl= new PutAirTerminalWpf(familyTypeEvent,putAirEvent);
            formPutAirTermianl.Show();
        }
    }
}
