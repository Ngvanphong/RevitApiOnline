using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using netDxf;
using netDxf.Entities;
using DB= Autodesk.Revit.DB;

namespace RevitApiOnline.ReadFileData
{
    public class ReadTextFromDxf
    {
        public static List<TextPosition> GetText(string pathDxf, DB.Transform transformToRevit,string layerName)
        {
            List<TextPosition> listResult= new List<TextPosition>();
            DxfDocument dxfDocument= netDxf.DxfDocument.Load(pathDxf);
            double scale = 1 / 304.8;
            foreach(var block in dxfDocument.Blocks)
            {
                foreach(var entity in block.Entities)
                {
                    if(entity.Layer.Name== layerName)
                    {
                        if(entity is Text|| entity is MText)
                        {
                            if (entity is Text text)
                            {
                                var position = text.Position;
                                DB.XYZ positionRevit= new DB.XYZ(position.X *scale,position.Y* scale, position.Z* scale);
                                positionRevit= transformToRevit.OfPoint(positionRevit);
                                TextPosition textPosition= new TextPosition(text.Value, positionRevit);
                                listResult.Add(textPosition);
                            }
                            else
                            {
                                MText mText = entity as MText;
                                var position = mText.Position;
                                DB.XYZ positionRevit = new DB.XYZ(position.X * scale, position.Y * scale, position.Z * scale);
                                positionRevit = transformToRevit.OfPoint(positionRevit);
                                TextPosition textPosition = new TextPosition(mText.Value, positionRevit);
                                listResult.Add(textPosition);
                            }
                        }
                    }
                }
            }
            return listResult;
        }
    }
    public class TextPosition
    {
        public TextPosition(string value,DB.XYZ position)
        {
            (TextValue, Position) = (value, position);
        }
        public string TextValue { set;get; }
        public DB.XYZ Position { set; get; }
    }
}
