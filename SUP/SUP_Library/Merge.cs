using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace SUP_Library
{
    public class Merge
    {
        /// <summary>
        /// Check the DocumentTemplate.Docx file, create a copy of it and modified it. Export the copy to a certain place.
        /// </summary>
        public static void merge()
        {
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"DocumentTemplate.Docx"))      //check for the DocumentTemplate.Docx file in computer
                {
                    #region Open Template

                    Word.Document template = null;
                    Word.Application wordAppTemplate = new Word.Application();
                    wordAppTemplate.ShowAnimation = false;
                    wordAppTemplate.Visible = false;

                    #endregion

                    #region Create Document

                    Word.Document document = null;
                    Word.Application wordApp = new Word.Application();
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

                    document = wordApp.Documents.Open(ref saveFileName);
                    document.Activate();

                    /*
                     * Find text in template in here
                     */

                    document.Save();
                    document.Close();

                    wordApp.Quit();
                }
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
    }
}
