using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RevitApiOnline.CreateFilterLegend
{
    public class PatternFilterFloor
    {
        public const string UnderLine = "_";
        public const string Phay = "; ";
        public static string GetPatternFilterForFloor(Document doc, TypeFilter typeFilter, Floor floor)
        {
            string namePattern = CategoryFilter.Floor.ToString();
            switch (typeFilter)
            {
                case TypeFilter.Type:
                    namePattern += UnderLine + floor.FloorType.Name;
                    break;
                case TypeFilter.Elevation:
                    {
                        Level level = doc.GetElement(floor.LevelId) as Level;
                        double offsetLevel = floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble();
                        offsetLevel = Math.Round(offsetLevel * 304.8, 1);
                        string nameLevel = level.Name;
                        namePattern += UnderLine + nameLevel + UnderLine + offsetLevel;
                        break;
                    }
                case TypeFilter.TypeElevation:
                    {
                        namePattern += UnderLine + floor.FloorType.Name;

                        Level level = doc.GetElement(floor.LevelId) as Level;
                        double offsetLevel = floor.get_Parameter(BuiltInParameter.FLOOR_HEIGHTABOVELEVEL_PARAM).AsDouble();
                        offsetLevel = Math.Round(offsetLevel * 304.8, 1);
                        string nameLevel = level.Name;
                        namePattern += UnderLine + nameLevel + UnderLine + offsetLevel;
                        break;
                    }
            }
            return namePattern;
        }

        public static string GetTextFromPatternFloor(FillPattern fillPattern, TypeFilter typeFilter)
        {
            string textComment = string.Empty;
            string namePattern = fillPattern.Name;
            List<string> listString = namePattern.Split(UnderLine.ToCharArray()).ToList();
            switch (typeFilter)
            {
                case TypeFilter.Type:
                    textComment = listString[1];
                    break;
                case TypeFilter.Elevation:
                    {
                        List<string> listStringLevel = new List<string>(listString);
                        listStringLevel.RemoveAt(0);
                        listStringLevel.RemoveAt(listStringLevel.Count - 1);
                        string nameLevel = string.Join(UnderLine,listStringLevel);
                        textComment += nameLevel + Phay+ "Offset= " +listString.Last();
                        break;
                    }
                case TypeFilter.TypeElevation:
                    {
                        List<string> listStringLevel = new List<string>(listString);
                        listStringLevel.RemoveAt(0);
                        listStringLevel.RemoveAt(0);
                        listStringLevel.RemoveAt(listStringLevel.Count - 1);
                        string nameLevel = string.Join(UnderLine, listStringLevel);
                        string nameFloorType = listString[1];
                        textComment += nameFloorType + Phay+  nameLevel +Phay + "Offset= " + listString.Last();
                        break;
                    }
            }
            return textComment;

        }





    }
}
