﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using SUP_Library.DBComponent;
using System.IO;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;

namespace SUP_Library
{
    public class Merge
    {

        #region Old Merge
        /// <summary>
        /// Check the DocumentTemplate.Docx file, create a copy of it and modified it. Export the copy to a certain place.
        /// </summary>
        public static void oldMerge(string templateName)
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
        #endregion


        public static void newMerge()
        {
            string templatePath = @"C:\Users\Public\Documents";
            string templateName = "TestTemplateDuy.docx";
            string saveName = "MergeComplete.docx";

            /*
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(templatePath, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex("<<FirstName>>");
                docText = regexText.Replace(docText, "Duy");

                Regex regexText2 = new Regex("<<LastName>>");
                docText = regexText.Replace(docText, "Nguyen");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                
            }
            */
            using (WordprocessingDocument wordDocComplte = WordprocessingDocument.CreateFromTemplate(templatePath + "\\" + templateName, false))
            {
                wordDocComplte.SaveAs(templatePath + "\\" + saveName);
                wordDocComplte.Close();
            }


            // Break here since the path saveName is used in the process
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(templatePath + "\\" + saveName, true))
            {
                string docText = null;
                using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                {
                    docText = sr.ReadToEnd();
                }

                Regex regexText = new Regex("<<FirstName>>");
                docText = regexText.Replace(docText, "Duy");

                Regex regexText2 = new Regex("<<LastName>>");
                docText = regexText.Replace(docText, "Nguyen");

                using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                {
                    sw.Write(docText);
                }

                wordDoc.Save();
            }
        }

    }
}
