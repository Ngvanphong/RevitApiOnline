using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using RevitApiOnline.Shared.Interfaces;
using RevitApiOnline.Shared.Implements;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI.Selection;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Xml.Linq;
using System;
using System.Collections;
using System.Data.SqlTypes;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Net.WebSockets;
using RevitApiOnline.Wpf;

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            GridLearn form = new GridLearn();
            // form.Show(); // show nhung ma co the tuong tac duoc revit
            //form.ShowDialog(); // khong tuong tac duoc revit
            form.ShowDialog();

            string valueTextBox = form.textBoxDemo.Text;


            return Result.Succeeded;
        }
    }

    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem != null && elem is Wall)
            {
                return true;
            }
            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
