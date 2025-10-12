using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.IO;
namespace RevitApiOnline.ReadFileData
{
    public class ExportDxfHelper
    {
        public static string ExportToDxf(Document doc, ImportInstance cadImport)
        {
            DXFExportOptions options = new DXFExportOptions();
            options.FileVersion = ACADVersion.R2013;
            string folder = Path.GetTempPath();
            string fileName = "RevitCad_" + Guid.NewGuid().ToString() + ".dxf";
            string fullPath = string.Empty;

            var fillterElement = new FilteredElementCollector(doc, doc.ActiveView.Id)
                .WhereElementIsNotElementType()
                .Cast<Element>()
                .Where(item => item.CanBeHidden(doc.ActiveView) && item.Id != cadImport.Id)
                .ToList();

            using (Transaction t = new Transaction(doc,"HideElements"))
            {
                t.Start();
                doc.ActiveView.HideElements(fillterElement.Select(x => x.Id).ToList());
                doc.Regenerate();
                bool exportSuccess = doc.Export(folder, fileName, new List<ElementId> { doc.ActiveView.Id }, options);
                if (exportSuccess)
                {
                    fullPath = Path.Combine(folder, fileName);
                }
                t.RollBack();
            }
            return fullPath;
        }
    }
}
