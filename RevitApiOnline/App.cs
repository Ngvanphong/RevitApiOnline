using Autodesk.Revit.UI;
using RevitApiOnline.Buttons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            new CreatePipeButton().CreatePipe(application);
            new DemoSplitButton().DemoSplit(application);
            new DemoPulldownButton().DemoPulldown(application);
            new DemoSmallButton().DemoSmall(application);
            new DemoCombineButton().DemoCombine(application);
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
