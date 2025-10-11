using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;
using Newtonsoft.Json;
using System.IO;

namespace RevitApiOnline.ReadFileData
{
    [Transaction(TransactionMode.Manual)]
    public class ReadExcelBinding : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Excel File (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            dialog.RestoreDirectory = true;
            var dialogResult = dialog.ShowDialog();
            List<JsonData> listObjectFromJson = new List<JsonData>();
            if (dialogResult == DialogResult.OK)
            {
                string filePath = dialog.FileName;

                #region excel
                //Excel.Application excelApp = new Excel.Application();
                //excelApp.Visible = false;
                //Excel.Workbook excelWorkbook = excelApp.Workbooks.Open(filePath);
                //Excel._Worksheet excelWorksheet = excelWorkbook.Worksheets["FamilyType"];
                //var useRange = excelWorksheet.UsedRange;
                //int totalRow = useRange.Rows.Count;
                //int totalCol = useRange.Columns.Count;

                //List<ExcelData> listDataFromExcel= new List<ExcelData>();
                //for (int row = 2; row <= totalRow; row++)
                //{
                //    ExcelData dataExcel = new ExcelData();
                //    for (int col = 1; col <= totalCol; col++)
                //    {
                //        var valueCell = ((Excel.Range)useRange.Cells[row, col]).Value2;
                //        if (col == 1) 
                //            dataExcel.Type = valueCell.GetType() == typeof(string) ? valueCell.ToString() : string.Empty;
                //        else if (col == 2) 
                //            dataExcel.Family = valueCell.GetType() == typeof(string) ? valueCell.ToString() : string.Empty;
                //        else if (col == 3)
                //            dataExcel.Width = valueCell.GetType() == typeof(double) ? (double)valueCell : double.Parse(valueCell.ToString());

                //    }
                //    listDataFromExcel.Add(dataExcel);
                //}

                ////write
                //for(int row= totalRow+1; row< totalRow+2; row++)
                //{
                //    for(int col= 1; col<= totalCol; col++)
                //    {
                //        if (col == 1) ((Excel.Range)useRange.Cells[row, col]).Value2 = "200x300";
                //        else if (col == 2) ((Excel.Range)useRange.Cells[row, col]).Value2 = "Family G";
                //        else if (col == 3)
                //        {
                //            var cell= (Excel.Range)useRange.Cells[row, col];
                //            DropdownExcel(ref cell, "E");
                //        }
                //    }
                //}


                //excelWorkbook.Save();
                //excelWorkbook.Close();


                //excelApp.Quit();
                #endregion

                #region json

                //read json
                string dataJson= File.ReadAllText(filePath);
                listObjectFromJson= JsonConvert.DeserializeObject<List<JsonData>>(dataJson);

                #endregion
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            var saveDialogResult= saveDialog.ShowDialog();
            if(saveDialogResult== DialogResult.OK)
            {
                string fileName = saveDialog.FileName;
                var textSaveFile = JsonConvert.SerializeObject(listObjectFromJson);
                File.WriteAllText(fileName, textSaveFile);
            }


            return Result.Succeeded;
        }
        public static void DropdownExcel(ref Excel.Range cell, string currentValue)
        {
            cell.Validation.Delete();
            cell.Validation.Add(XlDVType.xlValidateList, XlDVAlertStyle.xlValidAlertInformation,
                XlFormatConditionOperator.xlBetween, "A,B,C,E,F", Type.Missing);
            cell.Validation.IgnoreBlank = true;
            cell.Value = currentValue;
            cell.Validation.InCellDropdown = true;
        }
       
    }
    public class ExcelData
    {
        public string Type { set; get; }
        public string Family { set; get; }
        public double Width { set; get; }
    }

    public class JsonData
    {
        public string PassengerId { set; get; }
        public string Survived { set; get; }
        public string Pclass { set; get; }
        public string Name { set; get; }
        public string Sex { set; get; }
        public int Age { set; get; }
        public string SibSp { set; get; }
        public string Parch { set; get; }
        public string Ticket { set; get; }
        public double Fare { set; get; }
        public string Cabin { set; get; }
        public string Embarked { set; get; }
    }

}
