using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreaetWallByPoint
{
    public static class CreateWallAppShow
    {
        public static CreateWallWpf formCreateWall;
        public static void ShowForm()
        {
            StartPointClickHandler startPointClickHandler = new StartPointClickHandler();
            ExternalEvent startPointClickEvent = ExternalEvent.Create(startPointClickHandler);

            EndPointClickHandler endPointClickHandler = new EndPointClickHandler();
            ExternalEvent endPointClickEvent = ExternalEvent.Create(endPointClickHandler);

            DrawWallHandler drawWallHandler = new DrawWallHandler();
            ExternalEvent drawWallEvent = ExternalEvent.Create(drawWallHandler);

            formCreateWall = new CreateWallWpf(startPointClickEvent, endPointClickEvent, drawWallEvent);

            formCreateWall.Show();
        }
    }
}
