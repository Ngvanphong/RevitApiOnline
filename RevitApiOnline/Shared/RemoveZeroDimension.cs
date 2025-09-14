using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitApiOnline.Shared
{
    public class RemoveZeroDimension
    {
        public static Dimension RemoveZeroDim(Document doc,Dimension dimension)
        {
            // dim don
            if(dimension.Segments==null || dimension.Segments.Size == 0)
            {
                if (dimension.Value < 0.000001)
                {
                    using (Transaction t = new Transaction(doc, "RemoveDim"))
                    {
                        t.Start();
                        doc.Delete(dimension.Id);
                        t.Commit();
                    }
                    return null;
                }
                return dimension;
            }
            else // dim array
            {
                ReferenceArray newReferenceArray = new ReferenceArray();
                var segmentIterator= dimension.Segments.GetEnumerator();
                var refereentIterator = dimension.References.GetEnumerator();
                segmentIterator.Reset();
                refereentIterator.Reset();
                while(segmentIterator.MoveNext())
                {
                    refereentIterator.MoveNext();
                    DimensionSegment currentSegment = segmentIterator.Current as DimensionSegment;
                    Reference currentRef = refereentIterator.Current as Reference;
                    if (currentSegment.Value > 0.0001)
                    {
                        Element hostElement = doc.GetElement(currentRef.ElementId);
                        if(hostElement is Grid)
                        {
                            newReferenceArray.Append(new Reference(hostElement));
                        }
                        else
                        {
                            newReferenceArray.Append(currentRef);
                        }
                            
                    }
                }
                
                var lastRef = refereentIterator.MoveNext();
                Reference endRef= refereentIterator.Current as Reference;
                Element lastHost = doc.GetElement(endRef.ElementId);
                if(lastHost is Grid)
                {
                    newReferenceArray.Append(new Reference(lastHost));
                }
                else
                {
                    newReferenceArray.Append(endRef);
                }
                   
                Dimension newDim = null;
                using (Transaction t= new Transaction(doc, "CreateDim"))
                {
                    t.Start();
                    if (newReferenceArray.Size > 1)
                    {
                        Line putDim = dimension.Curve as Line;
                        newDim = doc.Create.NewDimension(doc.ActiveView, putDim, newReferenceArray);

                    }
                    doc.Delete(dimension.Id);
                    t.Commit();
                }
                return newDim;
            }
        }

    }
}
