using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CircleCanvas
{
    [Transaction(TransactionMode.Manual)]
    public class CircleCanvasBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var form = new CanvasWpf();
            form.Show();
            int.TryParse( form.txtRebarNumber.Text,out int numberRebar);
            CanvasRebarHelper.CreateRebarCanvas(numberRebar, 150-20, form.canvas);
            Document doc = null;
            Family family = null;
            Document familyDoc = doc.EditFamily(family);
            CombinableElementArray combinables = new CombinableElementArray();
            Extrusion extruc1 = null;
            combinables.Append(extruc1);
            combinables.Append(extruc1);
            familyDoc.CombineElements(combinables);

            ICollection<ElementId> elementsToCombine = new List<ElementId>
        {
            
        };
            familyDoc.AutoJoinElements();
            return Result.Succeeded;
        }
    }
}
