using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitApiOnline.CreatePiping
{
    public class ColumnCanvasAppShow
    {
        public static CanvasColumnWpf frmColumnWpf;
        public static List<List<Line>> listColumnCurves;
        public static void ShowForm()
        {
            try { frmColumnWpf.Close(); } catch { }
            frmColumnWpf = new CanvasColumnWpf();
            frmColumnWpf.Show();
        }

    }
}
