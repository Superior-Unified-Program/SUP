using System;
using System.Collections.Generic;
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

            string eFileName = "ExcelFile" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("h_mm_ss_tt") + ".xlsx";
            //string savePath = @"C:\Users\%USERPROFILE%\source\repos\Superior - Unified - Program\SUP\SUP\SUP_Library\ExportFileFolder";
            //string savePath = @"\Export\";
            string savePath = @"C:\Users\Public\Documents\SUPExport";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            eWorkbook.SaveAs(savePath + "\\" + eFileName);
            eWorkbook.Close(true, eFileName, misValue);
            eApp.Quit();
            Marshal.ReleaseComObject(eWorkSheet);
            Marshal.ReleaseComObject(eWorkbook);
            Marshal.ReleaseComObject(eApp);

            #endregion
        }
        private static void WriteCSV(List<Client> clientList)
        {
            const char DELIM = ','; // CSVs can technically have a different delimiter such as tab

            using (TextWriter output = File.CreateText("test.csv"))
            {
                
                output.Write("First Name" + DELIM);
                output.Write("Last Name" + DELIM);
                output.Write("Category" + DELIM);
                output.Write("Email" + DELIM);
                output.Write("Phone Number");
                output.WriteLine();

                for (int i = 0; i < clientList.Count; i++)
                {
                    output.Write(clientList[i].First_Name + DELIM);
                    output.Write(clientList[i].Last_Name + DELIM);
                    output.Write(clientList[i].Org.Org_Name + DELIM);
                    output.Write(clientList[i].Email.Email + DELIM);
                    output.Write(clientList[i].Phone.Number);
                    output.WriteLine();
                }
            }
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

        public static void CreateExcelFile2(List<Client> clientList, out string fileName)
        {
            string savePath = @"C:\Users\Public\Documents\SUPExport";
            //string savePath = @"C:\Users\%USERNAME%\Documents\SUPExport";
            fileName = "ExcelFile" + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("h_mm_ss_tt") + ".xlsx";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            using (SpreadsheetDocument spreadDoc = SpreadsheetDocument.Create(savePath + "\\" + fileName, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
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

                #region Junk
                /*
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
               */

                /* Auto Fit Part
                Columns colList = worksheetPart.Worksheet.GetFirstChild<Columns>();
                Column c = new Column();
                c.BestFit = true;
                colList.Append(c);
                worksheetPart.Worksheet.Append(colList);
                */
                #endregion

                Row row = new Row();
                row.RowIndex = 1;
				row.AppendChild(AddCellWithText("Prefix"));
				row.AppendChild(AddCellWithText("First Name"));
                row.AppendChild(AddCellWithText("Last Name"));
				row.AppendChild(AddCellWithText("Middle Initial"));
				row.AppendChild(AddCellWithText("Address Line 1"));
				row.AppendChild(AddCellWithText("Address Line 2"));
				row.AppendChild(AddCellWithText("City"));
				row.AppendChild(AddCellWithText("State"));
				row.AppendChild(AddCellWithText("Zip"));
				row.AppendChild(AddCellWithText("Category"));
				row.AppendChild(AddCellWithText("Title"));
				row.AppendChild(AddCellWithText("Business Email"));
				row.AppendChild(AddCellWithText("Personal Email"));
				row.AppendChild(AddCellWithText("Business Phone"));
				row.AppendChild(AddCellWithText("Personal Phone"));
				row.AppendChild(AddCellWithText("Assistant's First Name"));
				row.AppendChild(AddCellWithText("Assistant's Last Name"));
				row.AppendChild(AddCellWithText("Assistant's Email"));
				row.AppendChild(AddCellWithText("Assistant's Phone"));
				row.AppendChild(AddCellWithText("Permit Number"));
				sheetData.AppendChild(row);

                for (int i = 2; i < clientList.Count+2; i++)
                {
                    Row newRow = new Row();
                    newRow.RowIndex = (UInt32)i;
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Prefix));
					newRow.AppendChild(AddCellWithText(clientList[i-2].First_Name));
                    newRow.AppendChild(AddCellWithText(clientList[i-2].Last_Name));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Middle_initial));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Address.Line1));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Address.Line2));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Address.City));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Address.State));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Address.Zip));
					newRow.AppendChild(AddCellWithText(clientList[i-2].Org.Org_Name));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Org.Title));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Email.Business_Email));
					newRow.AppendChild(AddCellWithText(clientList[i-2].Email.Personal_Email));
                    newRow.AppendChild(AddCellWithText(clientList[i-2].Phone.Business_Phone));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Phone.Personal_Phone));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Assistant_First_Name));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Assisntant_Last_Name));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Email.Assistant_Email));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Phone.Assistant_Phone));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Permit_Num));
					sheetData.AppendChild(newRow);
                }

                wbPart.Workbook.Sheets.AppendChild(sheet);

                #region Auto Fit
                /* Auto Fit Part
                Row r1 = new Row
                {
                    RowIndex = 1,
                    CustomHeight = true,
                    Height = 71.25 //change height based on info
                };
                sheetData.Append(r1);
                Cell refCell = null;
                foreach (Cell cell in r1.Elements<Cell>())
                {
                    if (string.Compare(cell.CellReference.Value, "A1", true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }

                // Add the cell to the cell table at A1.
                Cell newCell = new Cell()
                {
                    CellReference = "A1",
                };
                r1.InsertBefore(newCell, refCell);

                // Set the cell value to be a numeric value of 100.
                newCell.CellValue = new CellValue("100");

                //Set Border
                //wbPark
                */
                #endregion

                wbPart.Workbook.Save();
                spreadDoc.Close();
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
