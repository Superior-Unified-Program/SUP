using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using SUP_Library.DBComponent;

using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO.Compression;


namespace SUP_Library
{
   
    /// <summary>
    /// Class to store search results in
    /// </summary>
    class MergeResult
    {
        public string Result { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        // public int length { get; set; }
    }
    public class Merge
    {
        private const string templatePath = @"C:\Users\Public\Documents\Templates";
        private const string savePath = @"C:\Users\Public\Documents\SUPExport\TEMP";
        private const string zipPath = @"C:\Users\Public\Documents\SUPExport";
        private struct token
        {
            public string open;
            public string close;
        }
        private static token mergeToken;
        public Merge()
        {
            
        }
        /// <summary>
        /// Check the DocumentTemplate.Docx file, create a copy of it and modified it. Export the copy to a certain place.
        /// </summary>
        public static void mergeold(string templateName)
        {
            try
            {
                //if (File.Exists(@"C:\Users\hoang\Desktop\" + templateName + ".docx"))      //check for the DocumentTemplate.Docx file in computer
                {
                    #region Open Template

                    Word.Document template = null;
                    Word.Application wordAppTemplate = new Word.Application();
                    wordAppTemplate.ShowAnimation = false;
                    wordAppTemplate.Visible = false;

                    #endregion

                    #region Create Document

                    Word.Application wordApp = new Word.Application();
                    Word.Document document = wordApp.Documents.Open(Properties.Resources.Guest_Parking_Letter_Template);
                    wordApp.ShowAnimation = false;
                    wordApp.Visible = false;

                    #endregion

                    #region Properties

                    object openFileName = AppDomain.CurrentDomain.BaseDirectory + @"DocumentTemplate.Docx";     //check for the DocumentTemplate.Docx file in computer
                    string savePath = @"C:\Users\Public\CCApp";
                    object saveFileName = savePath + "\\" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + ".Docx";

                    #endregion

                    #region Document = Copy of Template

                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }

                    template = wordAppTemplate.Documents.Open(ref openFileName);
                    template.Activate();
                    template.SaveAs(ref saveFileName);
                    template.Close();
                    wordAppTemplate.Quit();

                    #endregion

                    document = wordApp.Documents.Open(Properties.Resources.Guest_Parking_Letter_Template);
                    document.Activate();

                    //Word.MailMerge.CreateDataSource();
                    /*
                     * Find text in template in here
                     */
                    List<Client> clientList = new List<Client>();
                    //Edit file here



                    document.Save();
                    document.Close();

                    wordApp.Quit();
                }
                //else throw new Exception();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <summary>
        /// Find and replace the text in the document.
        /// Modified the Sellection and Find method in Word library.
        /// </summary>
        /// <param name="certification"></param>
        /// <param name="findText"></param>
        /// <param name="replaceText"></param>
        private void FindAndReplace(Word.Application template, object findText, object replaceText)
        {
            #region Replace Options

            object matchCase = true;
            object matchWholeWord = true;
            object matchWildCards = false;
            object matchSoundsLike = false;
            object matchAllWordForms = false;
            object forward = true;
            object format = false;
            object matchKashida = false;
            object matchDiacritics = false;
            object matchAlefHamza = false;
            object matchControl = false;
            object read_only = false;
            object visible = true;
            object replace = 2;
            object wrap = 1;

            #endregion

            template.Selection.Find.Execute(ref findText, ref matchCase, ref matchWholeWord, ref matchWildCards,
                ref matchSoundsLike, ref matchAllWordForms, ref forward, ref wrap, ref format, ref replaceText,
                ref replace, ref matchKashida, ref matchDiacritics, ref matchAlefHamza, ref matchControl);
        }

        private static string replaceValue(Client client, string match)
        {
            string r;
            switch (match)
            {
                case "firstname":
                    r = client.First_Name;
                   // Console.WriteLine("Attempting to replace first name..");
                    break;
                case "lastname":
                    r = client.Last_Name;
                    break;
                case "organization":
                    r = client.Org.Org_Name;
                    break;
                case "permit":
                    r = client?.Permit_Num;
                    break;
                case "address1":
                    r = client.Address.Line1;
                    break;
                case "address2":
                    r = client.Address.Line2;
                    break;
                case "city":
                    r = client.Address.City;
                    break;
                case "state":
                    r = client.Address.State;
                    break;
                case "zipcode":
                    r = client.Address.Zip;
                    break;
                case "email":
                    r = client.Email?.Business_Email;
                    break;
                case "phone":
                    r = client.Phone?.Business_Phone;
                    break;
                case "date":
                    CultureInfo info = new CultureInfo( "en-US",false );
                    r = info.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    r += " " + DateTime.Now.Day;
                    r += ", " + DateTime.Now.Year;
                    break;
                default:
                    r = "";
                    break;

            }
            return r;
        }

        private static string createFileFromTemplate(string templateName, Client client)
        {
          
            string fileName = templateName.Substring(0,templateName.Length-4) + client.First_Name + client.Last_Name + 
                "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("h_mm_ss_tt") + ".docx";

            if (!Directory.Exists(zipPath))
            {
                Directory.CreateDirectory(zipPath);
            }
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            try
            {
                File.Copy(templatePath + "\\" + templateName, savePath + "\\" + fileName);
            }
            catch (Exception e)
            {
                return null;
            }
            return savePath + "\\" + fileName;

        }
        public static string compress(string templateName)
        {
            //string startPath = @".\start";

            string zipShortName = templateName.Substring(0, templateName.Length - 4);

            string zipPathLong = zipPath + @"\" + zipShortName + ".zip";
                //= @".\result.zip";
            //string extractPath = @".\extract";
            ZipFile.CreateFromDirectory(savePath, zipPathLong);

            return zipPathLong;
            //ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
        public static void merge(List<Client> clientList, string template, out string exportFile)
        //    , out List<string> fileNames)
        {
            List<String> fileNames = new List<string>();
            bool searched = false;
            MatchCollection mc;
            List<MergeResult> matches=new List<MergeResult>();
            mergeToken = new token();
            mergeToken.open = "&lt;";
            mergeToken.close = "&gt;";
            foreach (Client client in clientList)
            {
                //string newFile = client.First_Name + client.Last_Name + documentLocation;
                //File.Copy(documentLocation, newFile);
                string newFile = createFileFromTemplate(template, client);
                if (newFile == null) throw new Exception("Error Creating File!");
                fileNames.Add(newFile);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(newFile, true))
                {
                    string docText = null;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }
                   
                    if (!searched)
                    {
                        //Regex regex = new Regex("&lt;(.)*?&gt;", RegexOptions.Compiled);
                        Regex regex = new Regex(  mergeToken.open + "(.)*?" + mergeToken.close, RegexOptions.Compiled);

                        mc = regex.Matches(docText);

                        for (int i = 0; i < mc.Count; i++)
                        {
                           // Console.WriteLine("Result " + i + ": ");
                           // Console.WriteLine("\tat: " + mc[i].Index);
                           // Console.WriteLine("\tvalue: " + mc[i].Value);
                            Regex r = new Regex("<(.)*?>", RegexOptions.Compiled);
                           // string p = mc[i].Value.Substring(4, mc[i].Length - 8);
                           string p = mc[i].Value.Substring(mergeToken.open.Length, (mc[i].Length - (mergeToken.open.Length + mergeToken.close.Length)));

                            MergeResult mr = new MergeResult();
                            mr.Result = r.Replace(p, "");
                            mr.Start = mc[i].Index;
                            mr.End = mc[i].Index + mc[i].Length;
                            matches.Add(mr);
                        }
                        searched = true;
                    }
                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        int s = 0;
                        for (int i = 0; i < matches.Count; i++)
                        {
                            Console.WriteLine(matches[i].Result);
                            string toWrite = docText.Substring(s, matches[i].Start - s);
                            string tag = replaceValue(client, matches[i].Result);
                            s = matches[i].End;
                            sw.Write(toWrite + tag);

                        }
                       
                        sw.Write(docText.Substring(s, docText.Length - s));
                    }
                }
    
            }


            exportFile = compress(template);
            deleteTempFiles(fileNames);
            

        }
        private static void deleteTempFiles(List<string> files)
        {
            for (int i = 0; i<files.Count; i++)
            {
                File.Delete(files[i]);
            }
        }

    }
}
