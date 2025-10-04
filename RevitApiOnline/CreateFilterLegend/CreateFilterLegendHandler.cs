using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitApiOnline.CreateFilterLegend
{
    public class CreateFilterLegendHandler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            UIDocument uiDoc = app.ActiveUIDocument;
            Document doc= uiDoc.Document;
            CategoryFilter categoryFiter = CategoryFilter.Floor;
            TypeFilter typeFilter = TypeFilter.Elevation;

            var floors= new FilteredElementCollector(doc).OfClass(typeof(Floor)).Cast<Floor>().ToList();
            List<string> listNamePattern= new List<string>();   
            foreach(var floor in floors)
            {
                string namePattern = PatternFilterFloor.GetPatternFilterForFloor(doc, typeFilter, floor);
                if (listNamePattern.Exists(x => x == namePattern))
                {
                    listNamePattern.Add(namePattern);
                }
            }
            Dictionary<string, PatternColor> dictionPattenColor = ColorAndPattern.DictionaryForPatternAndColor(doc, listNamePattern);



        }

        public string GetName()
        {
            return "CreateFilterLegendHandler";
        }
    }
}
