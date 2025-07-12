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
using RevitApiOnline.WallWpf;

namespace RevitApiOnline
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var pickElement = uiDoc.Selection.PickObject(ObjectType.Element, "Pick a wall");
            Wall wall = doc.GetElement(pickElement) as Wall;
            WallType wallType= wall.WallType;
            Parameter heighPara= wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);
            double wallHeight=Math.Round(UnitUtils.ConvertFromInternalUnits(heighPara.AsDouble(), UnitTypeId.Millimeters));
            WallInfoVM wallInfoVm = new WallInfoVM(wallType.Name, wallType.Id, wallHeight, wall.Id);
            ParameterSet listParameter = wall.Parameters;
            List<ParameterVm> listParameterVm= new List<ParameterVm>();
            foreach(Parameter param in listParameter)
            {
                ParameterVm parameterVm = new ParameterVm(param.Id, param.Definition.Name);
                listParameterVm.Add(parameterVm);
            }
            wallInfoVm.ListPara = listParameterVm;

            WallInfoWpf form = new WallInfoWpf();
            form.DataContext = wallInfoVm;
            bool? formResult= form.ShowDialog();
            if (formResult == true)
            {
                WallInfoVM dataContextForm = form.DataContext as WallInfoVM;

            }


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
