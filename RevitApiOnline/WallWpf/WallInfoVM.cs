using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitApiOnline.WallWpf
{
    public class WallInfoVM
    {
        public WallInfoVM()
        {
            
        }
        public WallInfoVM(string wallTypeName, ElementId wallTypeId, double wallHeight, ElementId wallId)
        {
            (WallTypeName, WallTypeId,WallHeight,WallId)=(wallTypeName,wallTypeId,wallHeight, wallId);
        }
        public string WallTypeName { set; get; }
        public ElementId WallTypeId { set; get; }


        public double WallHeight { set;get; }
        public ElementId WallId { set; get; }

        public List<ParameterVm> ListPara { set; get; }

        public ParameterVm SelectedParameter { set; get; }  

    }
    public class ParameterVm
    {
        public ParameterVm()
        {
            
        }
        public ParameterVm(ElementId parameterId, string parameterName)
        {
            (ParamterId, ParamterName)=(parameterId, parameterName);
        }
        public ElementId ParamterId { set; get; }

        public string ParamterName { set; get; }    
    }
}
