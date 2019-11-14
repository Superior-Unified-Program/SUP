using System;
using System.Collections.Generic;
using System.IO;
using SUP_Library.DBComponent;
using DocumentFormat.OpenXml.Packaging;
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
    }
    /// <summary>
    /// The merge class
    /// </summary>
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
          
        private static string replaceValue(Client client, string match)
        {
            string r;
            switch (match)
            {
                case "firstname":
                    r = client.First_Name;
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

            string fileName = templateName.Substring(0, templateName.Length - 5) + "_" + client.Last_Name + "_" + client.First_Name + ".docx";
           
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
            string zipShortName = templateName.Substring(0, templateName.Length - 5);

            string zipPathLong = zipPath + @"\" + zipShortName + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("h_mm_ss_tt") + ".zip";
             
            ZipFile.CreateFromDirectory(savePath, zipPathLong);

            return zipPathLong;
            
        }
        public static void merge(List<Client> clientList, string template, out string exportFile)
        {
            List<String> fileNames = new List<string>();
            bool searched = false;
            MatchCollection mc;
            List<MergeResult> matches = new List<MergeResult>();
            mergeToken = new token();
            mergeToken.open = "&lt;";
            mergeToken.close = "&gt;";

            foreach (Client client in clientList)
            {
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
                        Regex regex = new Regex(  mergeToken.open + "(.)*?" + mergeToken.close, RegexOptions.Compiled);

                        mc = regex.Matches(docText);

                        for (int i = 0; i < mc.Count; i++)
                        {
                            Regex r = new Regex("<(.)*?>", RegexOptions.Compiled);
                           
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
            File.Delete(savePath);
            /*
            for (int i = 0; i<files.Count; i++)
            {
                File.Delete(files[i]);
            }
            */
        }
        public static List<string> getTemplateNames()
        {
            List<string> templateNames = new List<string>();
            if (Directory.Exists(templatePath))
            {
               string[] tNames = Directory.GetFiles(templatePath, "*.docx");
                foreach (string file in tNames)
                {
                    string[] fileSplit = file.Split('\\');
                    string fileName = fileSplit[fileSplit.Length - 1];
                    templateNames.Add(fileName);
                }
            }
           
            return templateNames;
        }
        public static string getTemplatePath()
        {
            return templatePath;
        }
    }
}
