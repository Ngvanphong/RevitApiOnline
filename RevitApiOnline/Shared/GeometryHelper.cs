using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.Shared
{
    public class GeometryHelper
    {
        public static void GeoSolidElement(Document doc,Element element,ref List<Solid> listSolid, 
            ref List<Line> listLine)
        {
            Options options= new Options();
            options.ComputeReferences = true;
            options.IncludeNonVisibleObjects = false;
            options.View = doc.ActiveView;
            //options.DetailLevel = ViewDetailLevel.Medium;

            //Get Solid of this element
            GeometryElement geoElement = element.get_Geometry(options);
            foreach (GeometryObject geoObj in geoElement)
            {
                if(geoObj is Solid)
                {
                    Solid solid = (Solid)geoObj;
                    if (solid.Volume > 0.000000001)
                    {
                        listSolid.Add(solid);   
                    }
                }
                else if(geoObj is Line)
                {
                    listLine.Add(geoObj as Line);
                }
                else if (geoObj is GeometryInstance)
                {
                    GeometryInstance geoInstance = (GeometryInstance)geoObj;
                    if (geoInstance.GetInstanceGeometry() != null)
                    {
                        foreach (GeometryObject geoObj2 in geoInstance.GetInstanceGeometry())
                        {
                            if (geoObj2 is Solid)
                            {
                                Solid solid = (Solid)geoObj2;
                                if (solid.Volume > 0.000000001)
                                {
                                    listSolid.Add(solid);
                                }
                            }
                            else if(geoObj2 is Line)
                            {
                                listLine.Add(geoObj2 as Line);
                            }
                        }
                    }
                }
            }


            //Get Solid of SubComponnet;
            if(element is FamilyInstance)
            {
                FamilyInstance instance= (FamilyInstance)element;
                var subIds= instance.GetSubComponentIds();
                if (subIds != null)
                {
                    foreach (var subId in subIds)
                    {
                        Element elementSub = doc.GetElement(subId);
                        GeoSolidElement(doc, elementSub, ref listSolid, ref listLine);
                    }
                }
                
            }
            
        }
    }
}
