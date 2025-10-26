using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using DB = Autodesk.Revit.DB;

namespace RevitApiOnline.CircleCanvas
{
    public class CanvasRebarHelper
    {
        public static void CreateCircleRebar(Canvas canvas, double positionX, double positionY, Brush color)
        {
            double radiusRebar = 20;
            Ellipse arc = new Ellipse();
            arc.Width = radiusRebar * 2;
            arc.Height = radiusRebar * 2;
            arc.Stroke = color;
            arc.StrokeThickness = 1;
            arc.Fill = color;
            Canvas.SetLeft(arc, positionX- radiusRebar);
            Canvas.SetBottom(arc, positionY- radiusRebar);
            canvas.Children.Add(arc);
        }
        public static void CreateCircleRC(Canvas canvas, double radius, Brush color)
        {
            double widthCanvas = canvas.Width;
            double heightCanvas = canvas.Height;
            Ellipse arc = new Ellipse();
            arc.Width = radius * 2;
            arc.Height = radius * 2;
            arc.Stroke = color;
            arc.StrokeThickness = 4;
            
            Canvas.SetLeft(arc, widthCanvas / 2 - radius);
            Canvas.SetBottom(arc, heightCanvas / 2 - radius);
            //CreateCircleRebar(canvas, arc.Width/2, arc.Height/2, Brushes.Red);
            canvas.Children.Add(arc);
        }

        public static void CreateLine(Canvas canvas, double x1, double x2, double y1, double y2, Brush color,
                                    double thickness, double angle = 0)
        {

            Line line = new Line();
            line.X1 = x1; line.Y1 = y1;
            line.X2 = x2; line.Y2 = y2;
            line.Stroke = color;
            line.StrokeThickness = thickness;
            line.RenderTransform = new RotateTransform(angle);
            canvas.Children.Add(line);
        }

        public static void CreateTextBlock(Canvas canvas, double x, double y, string valueText, double angle = 0)
        {
            double widthCanvas = canvas.Width;
            double heightCanvas = canvas.Height;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "D= " + valueText;
            textBlock.RenderTransform = new RotateTransform(angle);
            double offset = 10;
            Canvas.SetLeft(textBlock, widthCanvas / 2 - offset);
            Canvas.SetRight(textBlock, heightCanvas / 2 - offset);
            canvas.Children.Add(textBlock);
        }


        public static void CreateRebarCanvas(int reberNumber, double radiusInner, Canvas canvas)
        {
            double widthCanvas = canvas.Width;
            double heightCanvas = canvas.Height;
            double widthFeet = widthCanvas * 0.26 / 304.8;
            double heightFeet = heightCanvas * 0.26 / 304.8;
            double radiusFeet = radiusInner * 0.26 / 304.8;
            DB.XYZ center = new DB.XYZ(widthFeet / 2, heightFeet / 2, 0);
            DB.XYZ endLine = new DB.XYZ(center.X + radiusFeet, center.Y, 0);
            double angle = 2 * Math.PI / reberNumber;
            List<DB.XYZ> points = new List<DB.XYZ>();
            double totalAngle = 0;
            for (int i = 0; i < reberNumber; i++)
            {
                DB.XYZ pointEnd = endLine;
                if (totalAngle > 0)
                {
                    DB.Transform rotateTransform = DB.Transform.CreateRotationAtPoint(DB.XYZ.BasisZ, totalAngle, center);
                    pointEnd = rotateTransform.OfPoint(pointEnd);
                }
                points.Add(pointEnd);
                totalAngle += angle;
            }

            foreach (DB.XYZ point in points)
            {
                double xPixel = point.X * 304.8 / 0.26;
                double yPixel = point.Y * 304.8 / 0.26;
                CreateCircleRebar(canvas, xPixel, yPixel, Brushes.Red);
            }
            CreateCircleRC(canvas, 150, Brushes.Black);




        }



    }
}
