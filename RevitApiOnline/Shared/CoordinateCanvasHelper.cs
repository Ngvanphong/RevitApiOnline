using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RevitApiOnline.Shared
{
    public class CoordinateCanvasHelper
    {
        public static void GetMinMaxBoundary(List<List<Autodesk.Revit.DB.Line>> listLineColumn, out double xMin, out double xMax, out double yMin, out double yMax)
        {
            double minX = 10000000000;
            double maxX = -10000000000;
            double minY = 10000000000;
            double maxY = -10000000000;
            foreach (List<Autodesk.Revit.DB.Line> listLine in listLineColumn)
            {
                foreach (Autodesk.Revit.DB.Line line in listLine)
                {
                    List<Autodesk.Revit.DB.XYZ> points = new List<Autodesk.Revit.DB.XYZ> { line.GetEndPoint(0), line.GetEndPoint(1) };
                    foreach (Autodesk.Revit.DB.XYZ p in points)
                    {
                        if (p.X < minX) minX = p.X;
                        if (p.X > maxX) maxX = p.X;
                        if (p.Y < minY) minY = p.Y;
                        if (p.Y > maxY) maxY = p.Y;
                    }
                }
            }
            xMin = minX;
            xMax = maxX;
            yMin = minY;
            yMax = maxY;
        }
        private static Line CreateLine(Canvas canvas, double x1, double x2, double y1, double y2, Brush color, double thickness)
        {
            Line line = new Line();
            line.X1 = x1; line.Y1 = y1;
            line.X2 = x2; line.Y2 = y2;
            line.Stroke = color;
            line.StrokeThickness = thickness;
            canvas.Children.Add(line);
            return line;
        }

        private static Autodesk.Revit.DB.XYZ ConvertToCoordinateCanvas(Autodesk.Revit.DB.XYZ pointRevit,
            double xMin, double xMax, double yMin, double yMax)
        {
            double xMili = (pointRevit.X - xMin) * 304.8 + (xMax - xMin) * 304.8 * 0.1;
            double yMili = (yMax - pointRevit.Y) * 304.8 + (yMax - yMin) * 304.8 * 0.1;
            return new Autodesk.Revit.DB.XYZ(xMili, yMili, pointRevit.Z * 304.8);
        }

        public static Line CreateLineFromLineRevit(Canvas canvas, Autodesk.Revit.DB.Line lineRevit, double xMin, double xMax, double yMin, double yMax)
        {
            Autodesk.Revit.DB.XYZ spRevit = lineRevit.GetEndPoint(0);
            Autodesk.Revit.DB.XYZ epRevit = lineRevit.GetEndPoint(1);
            Autodesk.Revit.DB.XYZ spCanvas = ConvertToCoordinateCanvas(spRevit, xMin, xMax, yMin, yMax);
            Autodesk.Revit.DB.XYZ epCanvas = ConvertToCoordinateCanvas(epRevit, xMin, xMax, yMin, yMax);
            return CreateLine(canvas, spCanvas.X, epCanvas.X, spCanvas.Y, epCanvas.Y, Brushes.Red,2);

        }

    }
}
