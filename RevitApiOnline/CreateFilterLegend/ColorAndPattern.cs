using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaClock= System.Windows.Media;

namespace RevitApiOnline.CreateFilterLegend
{
    public class ColorAndPattern
    {
        public static List<FillPatternElement> GetFillternDraft(Document doc)
        {
            var collection = new FilteredElementCollector(doc).OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>()
                .Where(x=>x.GetFillPattern().Target== FillPatternTarget.Drafting && !x.GetFillPattern().IsSolidFill).ToList();
            return collection;
        }
        public static IEnumerable<Color> GetColors()
        {
            List<MediaClock.Color> colors = new List<MediaClock.Color> { MediaClock.Colors.Gray, MediaClock.Colors.Green,
                                    MediaClock.Colors.Violet, MediaClock.Colors.Blue, MediaClock.Colors.Aqua, 
                                     MediaClock.Colors.Brown};
            foreach (var color in colors)
                yield return new Color(color.R, color.G, color.B);

        }

        public static Dictionary<string, PatternColor> DictionaryForPatternAndColor(Document doc,List<string> listNamePattern)
        {
            Dictionary<string,PatternColor> keyValuePairs = new Dictionary<string,PatternColor>();
            int index = 0;
            foreach(var pattern in GetFillternDraft(doc))
            {
                foreach (var color in GetColors())
                {
                    PatternColor pattenColor = new PatternColor();
                    pattenColor.Pattern= pattern;
                    pattenColor.Color = color;
                    keyValuePairs.Add(listNamePattern[index], pattenColor);
                    index++;
                    if (index >= listNamePattern.Count) break;
                }
                if(index>= listNamePattern.Count) break;
            }
            return keyValuePairs;
        }



    }
    public class PatternColor
    {
        public FillPatternElement Pattern { set; get; }
        public Color Color { set; get; }
    }
}
