using Autodesk.Revit.DB;
using RevitApiOnline.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.CreatePiping
{
    public class FindGridAroundFoundation
    {
        public static GridInfo GetGridAroundFoundation(Document doc, FamilyInstance foundation,
            XYZ horizonDiretion, XYZ verticalDirection)
        {
            Location location = foundation.Location;
            if (location == null) return null;
            LocationPoint locaitonPoint = location as LocationPoint;
            if (locaitonPoint == null) return null;
            XYZ pointFound = locaitonPoint.Point;

            var allGrid = new FilteredElementCollector(doc, doc.ActiveView.Id).OfClass(typeof(Grid)).Cast<Grid>();
            double distanceHorizontalMin = 10000000;
            Grid horizonGrid = null;
            double distanceVerticalMin = 100000000;
            Grid verticalGrid = null;
            foreach (Grid grid in allGrid)
            {
                var allCurve = grid.GetCurvesInView(DatumExtentType.ViewSpecific, doc.ActiveView);
                Line lineGrid = null;
                foreach (Curve curve in allCurve)
                {
                    if (curve is Line)
                    {
                        lineGrid = curve as Line;
                        break;
                    }
                }
                if (lineGrid != null)
                {
                    XYZ directionGrid = lineGrid.Direction.Normalize();
                    XYZ perpendicularGrid = directionGrid.CrossProduct(XYZ.BasisZ).Normalize();
                    Plane planeGrid = Plane.CreateByNormalAndOrigin(perpendicularGrid, lineGrid.GetEndPoint(0));

                    if (directionGrid.IsAlmostEqualTo(horizonDiretion, 0.000001) ||
                        directionGrid.IsAlmostEqualTo(-horizonDiretion, 0.000001))
                    {
                        XYZ intersect = XYZCalculation.IntersectionPointPlanebyVector(planeGrid, pointFound, verticalDirection);
                        if (intersect != null)
                        {
                            double d = intersect.DistanceTo(pointFound);
                            if (d < distanceHorizontalMin)
                            {
                                distanceHorizontalMin = d;
                                horizonGrid = grid;
                            }
                        }
                    }
                    if (directionGrid.IsAlmostEqualTo(verticalDirection, 0.000001) ||
                        directionGrid.IsAlmostEqualTo(-verticalDirection, 0.000001))
                    {
                        XYZ intersect = XYZCalculation.IntersectionPointPlanebyVector(planeGrid, pointFound, horizonDiretion);
                        if (intersect != null)
                        {
                            double d = intersect.DistanceTo(pointFound);
                            if (d < distanceVerticalMin)
                            {
                                distanceVerticalMin = d;
                                verticalGrid = grid;
                            }
                        }
                    }
                }


            }
            return new GridInfo(horizonGrid, verticalGrid);
        }
    }
    public class GridInfo
    {
        public GridInfo(Grid horGrid, Grid verGrid)
        {
            (HorizontalGrid, VerticalGrid) = (horGrid, verGrid);
        }
        public Grid HorizontalGrid { get; set; }
        public Grid VerticalGrid { get; set; }
    }
}
