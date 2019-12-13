using System;
using System.Collections.Generic;
using SUP_Library.DBComponent;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SUP_Library
{
    public class ExportFile
    {
        public static void CreateExcelFile(List<Client> clientList, out string fileName)
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

                #region Export File Header

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
				row.AppendChild(AddCellWithText("Organization"));
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
                    newRow.AppendChild(AddCellWithText(clientList[i - 2].Primary_Organization.Org_Type));
                    newRow.AppendChild(AddCellWithText(clientList[i-2].Primary_Organization.Org_Name));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Org.Title));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Email.Business_Email));
					newRow.AppendChild(AddCellWithText(clientList[i-2].Email.Personal_Email));
                    newRow.AppendChild(AddCellWithText(clientList[i-2].Phone.Business_Phone_Formatted));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Phone.Personal_Phone_Formatted));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Assistant_First_Name));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Assisntant_Last_Name));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Email.Assistant_Email));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Phone.Assistant_Phone_Formatted));
					newRow.AppendChild(AddCellWithText(clientList[i - 2].Permit_Num));
					sheetData.AppendChild(newRow);
                }

                #endregion

                wbPart.Workbook.Sheets.AppendChild(sheet);
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
