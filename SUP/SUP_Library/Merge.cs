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
		private static string templatePath = (Directory.GetCurrentDirectory()).Replace("SUP-MVC", "SUP_Library\\Templates");
        private static string savePath = ""; // (Directory.GetCurrentDirectory()).Replace("SUP-MVC", "SUP_Library\\Temp");
		private static string zipPath = (Directory.GetCurrentDirectory()).Replace("SUP-MVC", "SUP_Library\\ExportFileFolder");

		/*private static string templatePath = @"C:\Users\Public\Documents\Templates";
		private static string savePath = ""; // @"C:\Users\Public\Documents\Temp";
		private static string zipPath = @"C:\Users\Public\Documents\ExportFileFolder";*/
		private struct token
        {
            public string open;
            public string close;
        }
        private static token mergeToken;
          
        private static string replaceValue(Client client, string match)
        {           
            string r;
            match = match.ToLower();
            switch (match)
            {
                case "prefix":
                    r = client?.Prefix;
                    break;
                case "primary_addressee":
                    r = "";
                    if (client?.Prefix?.Trim() != "") r = client?.Prefix + " ";
                    r += client?.First_Name + " ";
                    r += client?.Last_Name;
                    break;
                case "firstname":
                    r = client?.First_Name;
                    break;
                case "lastname":
                    r = client?.Last_Name;
                    break;
                case "Title":
                    r = client?.Primary_Organization?.Title;
                    break;
                case "organization":
                    r = client?.Primary_Organization?.Org_Name;
                    break;
                case "permit":
                    r = client?.Permit_Num;
                    break;
                case "addr1":
                case "address1":
                    r = client?.Address?.Line1;
                    break;
                case "addr2":
                case "address2":
                    r = client?.Address?.Line2;
                    break;
                case "city":
                    r = client?.Address?.City;
                    break;
                case "State":
                case "state":
                    r = client?.Address?.State;
                    break;
                case "zip":
                case "zipcode":
                    r = client?.Address?.Zip;
                    break;
                case "addressblock": 
                    r = client?.First_Name + " " + client?.Last_Name + "<w:br/>";
                    r += client?.Address.Line1 + "<w:br/>";
                    if (client?.Address.Line2.Trim() != "") r += "<w:t>" + client?.Address.Line2 + "<w:br/>";
                    r += client?.Address.City + ", " + client?.Address.State + " " + client?.Address?.Zip + "<w:br/>";
                    break;
                case "email_personal":
                    r = client?.Email?.Personal_Email;
                    break;
                case "phone_personal":
                    r = client?.Phone?.Personal_Phone;
                    break;
                case "email_business":
                case "email":
                    r = client?.Email?.Business_Email;
                    break;
                case "phone_business":
                case "phone":
                    r = client?.Phone?.Business_Phone;
                    break;
                case "date":
                    CultureInfo info = new CultureInfo( "en-US",false );
                    r = info.DateTimeFormat.GetMonthName(DateTime.Now.Month);
                    r += " " + DateTime.Now.Day;
                    r += ", " + DateTime.Now.Year;
                    break;
                case "assistant_firstname":
                    r = client?.Assistant_First_Name;
                    break;
                case "assistant_lastname":
                    r = client?.Assistant_Last_Name;
                    break;
                case "assistant_phone":
                    r = client?.Phone?.Assistant_Phone;
                    break;
                case "assistant_email":
                    r = client?.Email?.Assistant_Email;
                    break;
                case "assistant_name":
                    r = client?.Assistant_First_Name + " " + client?.Assistant_Last_Name;
                    break;
                case "next record":                 //Duy: should it be _ between ?
                    r = "";
                    break;
                default:
                    r = "Not Found";
                    break;

            }
            return r;
        }

        private static string createFileFromTemplate(string templateName, Client client)
        {

            
            if (!Directory.Exists(zipPath))
            {
                Directory.CreateDirectory(zipPath);
            }
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            const string EXTENSION = ".docx";
            string fileName = templateName.Substring(0, templateName.Length - EXTENSION.Length);
            fileName = fileName.Replace("Template", "");
            if (!fileName.EndsWith("_")) fileName += "_";
            fileName += client.Last_Name + "_" + client.First_Name + EXTENSION;
            fileName = savePath + "\\" + fileName;

            
            for(int i=2; File.Exists(fileName); i++) // file already exists...is there more than one db entry with this name?
            {
                //fileName = savePath + "\\" + templateName.Substring(0, templateName.Length - EXTENSION.Length) + "_" + client.Last_Name + "_" + client.First_Name + i + EXTENSION;
                fileName = templateName.Substring(0, templateName.Length - EXTENSION.Length);
                fileName = fileName.Replace("Template", "");
                if (!fileName.EndsWith("_")) fileName += "_";
                fileName += client.Last_Name + "_" + client.First_Name + i + EXTENSION;
                fileName = savePath + "\\" + fileName;
            }   

            try
            {
                File.Copy(templatePath + "\\" + templateName, fileName);//savePath + "\\" + fileName);
            }
            catch (Exception e)
            {
                return null;
            }
            return fileName;//savePath + "\\" + fileName;

        }
        public static string compress(string templateName)
        {
            string zipShortName = templateName.Substring(0, templateName.Length - 5);

            zipShortName = zipShortName.Replace("Template", "");
            if (!zipShortName.EndsWith("_")) zipShortName += "_";

            string zipPathLong = zipPath + @"\" + zipShortName + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + "_" + DateTime.Now.ToString("h_mm_ss_tt") + ".zip";
             
            ZipFile.CreateFromDirectory(savePath, zipPathLong);

            return zipPathLong;
            
        }
        public static int merge(List<Client> clientList, string template, out string exportFile) // returns the number of documents created
        {
            List<String> fileNames = new List<string>();
            bool searched = false;
            MatchCollection mc;
            List<MergeResult> matches = new List<MergeResult>();
            mergeToken = new token();
            mergeToken.open = "&lt;";
            mergeToken.close = "&gt;";

            var rand = new Random();
            savePath = zipPath + "\\TEMP" + rand.Next(0, 1000);

            //foreach (Client client in clientList)
            for (int c = 0; c < clientList.Count; c++)
            {
                Client client = clientList[c];

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
                            if (replaceValue(client,mr.Result)!="Not Found")
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

                            if (matches[i].Result== "Next Record")
                            {
                                if (c < clientList.Count - 1)
                                    client = clientList[++c];
                                else if (c == clientList.Count - 1)
                                {
                                    client = new Client(); // this should blank all remaining fields
                                    
                                }
                            }

                        }
                       
                        sw.Write(docText.Substring(s, docText.Length - s));
                    }
                }
    
            }
            if (fileNames.Count == 1) // we performed a merge with multiple clients on a single document
            {
                const string EXTENSION = ".docx";
                string fileName = template.Substring(0, template.Length - EXTENSION.Length);
                fileName = fileName.Replace("Template", "");
                if (fileName.EndsWith("_")) fileName = fileName.Substring(0,fileName.Length-1);
                fileName += EXTENSION;
                fileName = savePath + "\\" + fileName;
                try
                {
                    File.Move(fileNames[0], fileName);
                    fileNames.RemoveAt(0);
                    fileNames.Add(fileName);
                }
                catch (Exception e) { }
                
            }
            exportFile = compress(template);
            deleteTempFiles(fileNames);
            Directory.Delete(savePath);
            return fileNames.Count;
        }
        private static void deleteTempFiles(List<string> files)
        {
            for (int i = 0; i<files.Count; i++)
            {
                File.Delete(files[i]);
            }
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
		public static List<string> getFileModificationDates(List<string> files)
		{
			List<string> results = new List<string>();
			foreach (string file in files)
			{
				string modification = File.GetLastWriteTime(templatePath + "//" + file).ToShortDateString();
				results.Add(file + " " + modification);
			}
			return results;
		}
    }
}
