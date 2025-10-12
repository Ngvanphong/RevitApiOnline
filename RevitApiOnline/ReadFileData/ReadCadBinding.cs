using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.ReadFileData
{
    [Transaction(TransactionMode.Manual)]
    public class ReadCadBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc= uiDoc.Document;
            ImportInstance cadImport = null;
            try
            {
                Reference pickElement = uiDoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element,
                    new CadFilter(), "Pick a cadfile");
                cadImport = doc.GetElement(pickElement) as ImportInstance;
            }
            catch { }
            Options options= new Options();
            options.View = doc.ActiveView;
            GeometryElement geoElement = cadImport.get_Geometry(options);
            string layerName = "Beam";
            List<Line> listLine = new List<Line>();
            Transform transformCad = null;
            foreach(GeometryObject geoObj in geoElement)
            {
                if(geoObj is GeometryInstance)
                {
                    GeometryInstance geoInstance= (GeometryInstance)geoObj;
                    transformCad = geoInstance.Transform;
                    foreach(GeometryObject geoObj2 in geoInstance.GetInstanceGeometry())
                    {
                        if(geoObj2 is Line line)
                        {
                            GraphicsStyle graphicStyle= doc.GetElement( line.GraphicsStyleId) as GraphicsStyle;
                            if (graphicStyle.GraphicsStyleCategory.Name == layerName)
                            {
                                listLine.Add(line);
                            }
                        }
                        else if(geoObj2 is Arc arc)
                        {
                            GraphicsStyle graphicStyle = doc.GetElement(arc.GraphicsStyleId) as GraphicsStyle;
                            if (arc.IsCyclic)
                            {
                                Plane planeArc = Plane.CreateByNormalAndOrigin(arc.Normal, arc.Center);
                                Arc arc1 = Arc.Create(planeArc, arc.Radius, 0, Math.PI);
                                Arc arc2 = Arc.Create(planeArc, arc.Radius, Math.PI, 2 *Math.PI);
                            }
                            else
                            {
                                if(graphicStyle.Name == layerName)
                                {

                                }
                            }
                                
                        }
                        else if(geoObj2 is PolyLine polyLine)
                        {
                            GraphicsStyle graphicStyle = doc.GetElement(polyLine.GraphicsStyleId) as GraphicsStyle;
                            if(graphicStyle.GraphicsStyleCategory.Name == layerName)
                            {
                                for (int i = 0; i < polyLine.NumberOfCoordinates-1; i++)
                                {
                                    Line lineItem = Line.CreateBound(polyLine.GetCoordinate(i), polyLine.GetCoordinate(i + 1));
                                    listLine.Add(lineItem);
                                }
                            }
                            
                        }
                    }
                }
            }

            string fullPathDxf = ExportDxfHelper.ExportToDxf(doc, cadImport);

            List<TextPosition> listText = ReadTextFromDxf.GetText(fullPathDxf, transformCad, "Text");

            return Result.Succeeded;
        }
        public class CadFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                if (elem is ImportInstance) return true;
                return false;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return true;
            }
        }
    }
}
