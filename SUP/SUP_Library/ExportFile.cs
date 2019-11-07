using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using SUP_Library.DBComponent;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

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
            //string savePath = @"C:\Users\%USERPROFILE%\source\repos\Superior - Unified - Program\SUP\SUP\SUP_Library\ExportFileFolder";
            //string savePath = @"\Export\";
            string savePath = @"C:\Users\Public\Documents\SUPExport";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            eWorkbook.SaveAs(savePath + "\\" + eFileName + ".xlsx");
            eWorkbook.Close(true, eFileName, misValue);
            eApp.Quit();
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

        public static void CreateExcelFile2(List<Client> clientList)
        {
            string savePath = @"C:\Users\Public\Documents\SUPExport";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            using (SpreadsheetDocument spreadDoc = SpreadsheetDocument.Create(savePath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart wbPart = spreadDoc.WorkbookPart;
                if (wbPart == null)
                {
                    wbPart = spreadDoc.AddWorkbookPart();
                    wbPart.Workbook = new Workbook();
                }

                string sheetName = "Client Information";
                WorksheetPart worksheetPart = wbPart.AddNewPart<WorksheetPart>();
                SheetData sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                if (wbPart.Workbook.Sheets == null)
                {
                    wbPart.Workbook.AppendChild<Sheets>(new Sheets());
                }

                Sheet sheet = new Sheet()
                {
                    Id = wbPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = sheetName
                };

                var workingSheet = ((WorksheetPart)wbPart.GetPartById(sheet.Id)).Worksheet;

                int rowindex = 1;
                foreach (var client in clientList)
                {
                    Row row = new Row();
                    row.RowIndex = (UInt32)rowindex;

                    if (rowindex == 1) //Header
                    {
                        row.AppendChild(AddCellWithText("First Name"));
                        row.AppendChild(AddCellWithText("Last Name"));
                        row.AppendChild(AddCellWithText("Category"));
                        row.AppendChild(AddCellWithText("Email"));
                        row.AppendChild(AddCellWithText("Phone#"));
                    }
                    else //Data
                    {
                        row.AppendChild(AddCellWithText(client.First_Name));
                        row.AppendChild(AddCellWithText(client.Last_Name));
                        row.AppendChild(AddCellWithText(client.Org.Org_Name));
                        row.AppendChild(AddCellWithText(client.Email.Email));
                        row.AppendChild(AddCellWithText(client.Phone.Number));
                    }
                    sheetData.AppendChild(row);
                    rowindex++;
                }
                wbPart.Workbook.Sheets.AppendChild(sheet);

                //Set Border
                //wbPark

                wbPart.Workbook.Save();
            }
        }
        private static Cell AddCellWithText(string text)
        {
            Cell c1 = new Cell();
            c1.DataType = CellValues.InlineString;

            InlineString inlineString = new InlineString();
            Text t = new Text();
            t.Text = text;
            inlineString.AppendChild(t);

            c1.AppendChild(inlineString);

            return c1;
        }
    }
}
