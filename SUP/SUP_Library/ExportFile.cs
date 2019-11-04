using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using SUP_Library.DBComponent;

namespace SUP_Library
{
    public class ExportFile
    {
        public static void CreateExcelFile(List<Client> clientList)
        {
            #region Setup new Excel File

            object misValue = Missing.Value;

            Excel.Application eApp = new Excel.Application();
            Excel.Workbook eWorkbook = eApp.Workbooks.Add(misValue);
            Excel.Worksheet eWorkSheet = (Excel.Worksheet)eWorkbook.Worksheets.get_Item(1);

            #endregion

            #region Input data into Excel File

            eWorkSheet.Name = "Client Information";             //Name of the work sheet here
            WriteExcelFile(clientList, eWorkSheet);

            #endregion

            #region Save and clean up Excel File

            string eFileName = "ExcelFile" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("h_mm_ss_tt") + ".xlsm";
            string savePath = @"C:\Users\dnguye14\Source\Repos\Superior - Unified - Program\SUP\SUP\SUP_Library\ExportFileFolder";
            eWorkbook.SaveAs(savePath + eFileName + ".xlsx");
            eWorkbook.Close(true, eFileName, misValue);
            eApp.Quit();
            % USERPROFILE %
            Marshal.ReleaseComObject(eWorkSheet);
            Marshal.ReleaseComObject(eWorkbook);
            Marshal.ReleaseComObject(eApp);

            #endregion
        }

        private static void WriteExcelFile(List<Client> clientList, Excel.Worksheet eWorkSheet)
        {
            eWorkSheet.Cells[1, 1] = "First Name";  //Excel SpreadSheet's index start at 1 instead of 0
            eWorkSheet.Cells[1, 2] = "Last Name";
            eWorkSheet.Cells[1, 3] = "Category";
            eWorkSheet.Cells[1, 4] = "Email";
            eWorkSheet.Cells[1, 5] = "Phone#";

            for (int i = 0; i < clientList.Count; i++)
            {
                eWorkSheet.Cells[i + 2, 1] = clientList[i].First_Name;
                eWorkSheet.Cells[i + 2, 2] = clientList[i].Last_Name;
                eWorkSheet.Cells[i + 2, 3] = clientList[i].Org.Org_Name;
                eWorkSheet.Cells[i + 2, 4] = clientList[i].Email.Email;
                eWorkSheet.Cells[i + 2, 5] = clientList[i].Phone.Number;
            }
            eWorkSheet.Columns.AutoFit();
        }
    }
}
