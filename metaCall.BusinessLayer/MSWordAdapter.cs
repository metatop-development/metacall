using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;



//using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using metatop.Applications.metaCall.BusinessLayer;
using System.Windows.Forms;
using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Word = Microsoft.Office.Interop.Word;


namespace metatop.Applications.metaCall
{
    public class MSWordAdapter   : IDisposable
    {

        Word._Application msWord = null;
        Process msWordProcess = null;

        string tempFileName = null;
        ArrayList  dataFieldTable = new ArrayList();

        /// <summary>
        /// Erstellt eine Instanz der Klasse MSWordAdapter
        /// </summary>
        /// <param name="metaCallBusiness"></param>
        public MSWordAdapter()
        {
            LogMsWordAdapterInformation("MsWordAdapter constructor");
        }

        public ArrayList DataFieldTable
        {
            get { return this.dataFieldTable; }
            set { this.dataFieldTable = value; }
        }

        /// <summary>
        /// Instanziert ein COM-Objekt des Typs Word.Application und 
        /// öffnet ein neues Dokument
        /// </summary>
        /// <param name="fileName">Pfad zur Datei die geöffnet werden soll</param>
        /// <param name="fromTemplate">true gibt an, dass ein neues Dokument erstellt werden soll
        /// und das angegebene lediglich als Vorlage dient</param>
        /// <param name="showWordInstance">true gibt an, dass die erstellte Wordinstanz angezeigt werden soll</param>
        public void Open(string fileName, bool fromTemplate, bool showWordInstance)
        {
            object _fileName = fileName;
            object _false = false;
            object _true = true;
            object _missing = System.Reflection.Missing.Value;

            try
            {
                this.msWord = new Word.Application();

                if (this.msWord == null)
                    LogMsWordAdapterInformation("Cannot instantiate MS Word");

                if (showWordInstance)
                    this.msWord.Visible = true;

                IDictionary<string, object> logInfos = new Dictionary<string,object>();
                logInfos.Add("Filename", _fileName);

                if (fromTemplate)
                {
                    this.msWord.Documents.Add(ref _fileName, ref _missing, ref _missing, ref _true);


                    LogMsWordAdapterInformation("Document successfully opened from Template", logInfos);

                }
                else
                {
                    this.msWord.Documents.Open(ref _fileName, ref _missing, ref _missing, ref _missing, ref _missing,
                        ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                        ref _missing, ref _missing, ref _missing);

                    LogMsWordAdapterInformation("Document successfully opened as new Document", logInfos);
                
                }
                this.msWordProcess = FindWordProcess(this.msWord);

                if (this.msWordProcess == null)
                    LogMsWordAdapterInformation("no WordProcess found");
                else
                    LogMsWordAdapterInformation(string.Format("WordProcess found {0}", this.msWordProcess.Id));

            }
            catch (COMException ex)
            {
                LogMsWordAdapterInformation(string.Format("MsWordAdapter error in Open-Method: {0}", ex));
                throw new MSWordAdapterException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                LogMsWordAdapterInformation(string.Format("MsWordAdapter error in Open-Method: {0}", ex));
                throw new MSWordAdapterException(ex.Message, ex);
            }
            finally
            {
                this.tempFileName = null;
            }
        }

        public static void WordImage()
        {
        }

        public void ProcessDataFields(DataFieldMethodParameter param)
        {
            InsertDelegate insertMethod;
            DataFieldManager dataFieldManager = new DataFieldManager();
            object _missing = System.Reflection.Missing.Value;
            object _true = true;


            IDictionary<string, object> logInfos = new Dictionary<string, object>();

            try
            {
                Word.Document doc = this.msWord.ActiveDocument;
                Word._Application word = this.msWord;


                LogMsWordAdapterInformation("Processing Datafields:");

                foreach (string dataField in DataFieldManager.AvailableDataFields)
                {
                    object value = dataFieldManager.GetValue(dataField, param);
                    
                    if (value != null &&  value.GetType() == typeof(string))
                    {
                        insertMethod = new InsertDelegate(InsertText);
                    }
                    else if (value != null && value.GetType() == typeof(WordImages))
                    {
                        insertMethod = new InsertDelegate(InsertImage);
                    }
                    else
                    {
                        LogMsWordAdapterInformation(string.Format("unknown dataFieldType {0}", dataField.ToString()));

                        continue;
                    }

                    logInfos.Clear();
                    logInfos.Add("DataFieldType", value.GetType());
                    logInfos.Add("DataVieldValue", value);
                    logInfos.Add("Method to Use", insertMethod.Method.Name);

                    LogMsWordAdapterInformation("CurrentDataField", logInfos);


                    //Das Find-Object initialisieren 
                    Word.Find findObject = InitFindClass(doc, dataField);

                    //Startposition für das Suchen setzen
                    SetStartSelection(doc);

                    while (findObject.Execute(ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                        ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                        ref _missing, ref _missing, ref _missing, ref _missing))
                    {

                        insertMethod(word, dataField, value);
                    }


                    foreach (Word.Shape shape in doc.Shapes)
                    {

                        if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
                        {

                            shape.Select(ref _true);
                            //Startposition auf den Anfang setzen 

                            while (findObject.Execute(ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                                                ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                                                ref _missing, ref _missing, ref _missing, ref _missing))
                            {

                                insertMethod(word, dataField, value);
                            }
                        }
                    }


                }
            }
            catch (COMException ex)
            {
                throw new MSWordAdapterException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new MSWordAdapterException(ex.Message, ex);
            }
        }

        public void PrintDocument()
        {
            PrintDocument(null, null);
        }
        
        public void PrintDocument(string printer)
        {
            PrintDocument(printer, null);
        }

        public void PrintDocument(string printer, string printerFilename)
        {

            object _missing = System.Reflection.Missing.Value;
            object _false = false;

            if (this.msWord == null)
            {
                throw new MSWordAdapterException("Cannot access MS Word instance");
            }

            IDictionary<string, object> logInfos = new Dictionary<string, object>();
            logInfos.Add("used Printer", printer);

            try
            {


                string tempFilename = null;
                tempFilename = SaveAsTemporaryFile(this.msWord.ActiveDocument, printerFilename);

                if (!string.IsNullOrEmpty(printer))
                {
                    this.msWord.ActivePrinter = printer;
                }

                logInfos.Add("File to print", tempFilename);

                LogMsWordAdapterInformation("Try to Print MS Word document", logInfos);

                Word.Document doc = this.msWord.ActiveDocument;

                doc.PrintOut(ref _false, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing, ref _missing);

                LogMsWordAdapterInformation("MSWord-Printing was successfully");

            }
            catch (COMException ex)
            {

                logInfos.Add("Exception", ex);
                LogMsWordAdapterInformation("An COMException occoured while printing", logInfos);
                throw new MSWordAdapterException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                logInfos.Add("Exception", ex);
                LogMsWordAdapterInformation("An System.Exception occoured while printing", logInfos);

                throw new MSWordAdapterException(ex.Message, ex);
            }
            catch
            {
                LogMsWordAdapterInformation("An unknown Exception occoured while printing", logInfos);
                throw;
            }
        }

        public void Quit(bool closeWord, bool deleteTempFiles)
        {

            object _missing = System.Reflection.Missing.Value;
            object _false = false;

            //versuchen Word wieder schließen
            if (this.msWord != null)
            {
                try
                {
                    if (closeWord)
                    {
                        //Word schließen ohne zu speichern
                        this.msWord.Quit(ref _false, ref _missing, ref _missing);
                    }
                    this.msWord = null;
                    //Wenn der korrekte Prozess gefunden wurde, so wird auf dessen 
                    // Beendigung gewartet
                    if (this.msWordProcess != null)
                        this.msWordProcess.WaitForExit(1000);

                    this.msWordProcess = null;
                }
                catch(Exception ex)
                {
                    LogMsWordAdapterInformation("An System.Exception occoured while printing", ex);
                }
               
            }

            if (deleteTempFiles && 
                (this.tempFileName != null))
            {
                FileInfo fi = new FileInfo(this.tempFileName);

                // Löschen der temporären Datei
                try
                {
                    //aktuellen Thread kurz schlafen legen
                    // damit die Datei auch freigegeben ist.
                    System.Threading.Thread.Sleep(500);
                    //löschen der Datei
                    System.IO.File.Delete(this.tempFileName);
                }
                catch (Exception ex)
                {
                    LogMsWordAdapterInformation("An System.Exception occoured while printing", ex);
                }
               
            }
        }

        private Process FindWordProcess(Word._Application msWord)
        {

            Process[] processes = Process.GetProcessesByName("WINWORD"); // ("WINWORD.EXE");


            if (processes != null &&
                processes.Length > 0)
            {
                return processes[0];
            }
            return null;
        }

        public string SaveAsAndConvertDocumentToPdf(string targetFileName)
        {
            Word.Document doc = this.msWord.ActiveDocument;
            string path = Path.GetTempPath();
            object _filename = new object();

            if (string.IsNullOrEmpty(targetFileName))
            {
                targetFileName = doc.Name;

                string filename = Path.GetFileNameWithoutExtension(targetFileName);
                string extension = "pdf";

                _filename = path + filename + "." + extension;
            }
            else
            {
                _filename = targetFileName;
            }

            try
            {

                if (File.Exists((string)_filename))
                {
                    File.Delete((string)_filename);
                }

                //Speichern des Dokuments
                doc.SaveAs(_filename, Word.WdSaveFormat.wdFormatPDF);

                return (string) _filename;

            }
            catch (COMException ex)
            {
                LogMsWordAdapterInformation("An ComException occourred during Quit-Method", ex);
                throw new MSWordAdapterException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                LogMsWordAdapterInformation("An System.Exception occourred during Quit-Method", ex);
                throw new MSWordAdapterException(ex.Message, ex);
            }

        }

        private string SaveAsTemporaryFile(Word.Document doc, string fileName)
        {
            object _missing = System.Reflection.Missing.Value;

            string path = Path.GetTempPath(); 

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = doc.Name;
            }

            string filename = Path.GetFileNameWithoutExtension(fileName);
            string extension = "doc";

            object _filename = path + fileName + "." + extension;
            try
            {
                string originFilename = null;
                if (doc.Saved)
                {
                    originFilename = doc.Name;
                }

                if (File.Exists((string)_filename))
                {
                    File.Delete((string)_filename);
                }

 
                //Speichern des Dokuments, damit der Dokumentname für das Drucken richtig gesetzt ist.
                doc.SaveAs(ref _filename, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                    ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing, ref _missing);

 
                //Nachdem die Datei unter neuem Namen gespeichert wurde, 
                // muss die alte datei gelöscht werden
                if (originFilename != null)
                {
                    try
                    {
                        if (File.Exists(originFilename))
                        {
                            File.Delete(originFilename);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogMsWordAdapterInformation("An System.Exception occourred during Quit-Method", ex);
                    }
                }

                return (string)_filename;
            }
            catch (COMException ex)
            {
                LogMsWordAdapterInformation("An ComException occourred during Quit-Method", ex);
                throw new MSWordAdapterException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                LogMsWordAdapterInformation("An System.Exception occourred during Quit-Method", ex);
                throw new MSWordAdapterException(ex.Message, ex);
            }
        }

        private delegate void InsertDelegate(Word._Application word, string dataFields, object value);

        private static void InsertText(Word._Application word, string dataField, object value)
        {
            string text = value as string;


            //Selecttion ersetzen
            int start = word.Selection.Start;
            int ende = word.Selection.End;
            //word.Selection.MoveRight()
            Console.WriteLine("Variable {0} gefunden : Position {1} bis {2}", dataField, start, ende);


            if (string.IsNullOrEmpty(text) && (
                dataField == "$Projekt.Sponsorenpakete1$" || dataField == "$Projekt.Sponsorenpakete2$" || 
                dataField == "$Projekt.Sponsorenpakete3$" || dataField == "$Projekt.Sponsorenpakete4$" || 
                dataField == "$Projekt.Sponsorenpakete5$" || dataField == "$Projekt.Sponsorenpakete6$"))
            {
                object character = Word.WdUnits.wdCharacter;
                object count = 1;
                object extend = 1;

                word.Selection.MoveRight(ref character, ref count, ref count);
            }

            word.Selection.Cut();
            if (!string.IsNullOrEmpty(text))
            {
                word.Selection.InsertAfter(text);
            }

            object collapseDirection = Word.WdCollapseDirection.wdCollapseEnd;
            word.Selection.Collapse(ref collapseDirection);
        }

        private static void InsertImage(Word._Application word, string dataField, object value)
        {
            string imagePath = value.ToString();
            
            int start = word.Selection.Start;
            int ende = word.Selection.End;
            Console.WriteLine("Variable {0} gefunden : Position {1} bis {2}", dataField, start, ende);

            SizeF imageSize;
            if (!CheckDataFieldImageParameters(word, out imageSize))
                imageSize = SizeF.Empty;

            word.Selection.Start = start;

            //Markierung ersetzen
            word.Selection.Cut();

            object _false = false;
            object _true = true;
            object _missing = System.Reflection.Missing.Value;
            
            if (imagePath != null)
            {
            //    Clipboard.SetImage(image);

                word.Selection.InlineShapes.AddPicture(imagePath, ref _false, ref _true, ref _missing);

              //  word.Selection.Paste();
                
                //Bildgröße entsprechend den Parametern setzen
                if (imageSize != SizeF.Empty)
                {
                    // Markierung auf die alten Werte einstellen. Dann müsste eigentlich ein
                    // Bild als InlineShape vorhanden sein.
                    word.Selection.Start = start;
                    word.Selection.End = ende;

                    if (word.Selection.InlineShapes.Count > 0)
                    {
                        Word.InlineShape shape;
                        if (word.Application.Version == "14.0")
                            shape = word.Selection.InlineShapes[1];
                        else
                            shape = word.Selection.InlineShapes[0];

                        shape.Select();
                        
                        shape.Height = word.CentimetersToPoints(imageSize.Height);
                        shape.Width = word.CentimetersToPoints(imageSize.Width);
                    }
                }

               // Clipboard.Clear();
            }
            object collapseDirection = Word.WdCollapseDirection.wdCollapseEnd;
            word.Selection.Collapse(ref collapseDirection);
        }

        private static bool CheckDataFieldImageParameters(Word._Application word, out SizeF size)
        {
            //aktuelle markierung bis zur nächsten schließenden Klammer erweitern
            object character = ")";
            word.Selection.Extend(ref character);


            size = SizeF.Empty;
            //Auslesen der aktuellen Markierung
            string param = word.Selection.Text;

            if (param.IndexOf('(') < 0)
                return false;

            
            param = param.Substring(param.IndexOf('('));
            float width = 0f;
            float height = 0f;

            if (param != null &&
                param.StartsWith("(") &&
                param.EndsWith(")"))
            {
                string[] dimension = param.Trim('(', ')').Split(';');

                if (dimension.Length < 2)
                    return false;

                if (!float.TryParse(dimension[0], out width))
                    return false;

                if (!float.TryParse(dimension[1], out height))
                    return false;

                size = new SizeF(width, height);

                return true;
            }
            return false;
        }

        private void SetStartSelection(Word._Document document)
        {
            Word.Application wordApplication = document.Application;
            document.Select();
            object direction = Word.WdCollapseDirection.wdCollapseStart;
            wordApplication.Selection.Collapse(ref direction);
        }

        private Word.Find InitFindClass(Word._Document document, string searchExpression)
        {
            object _missing = System.Reflection.Missing.Value;
            object _true = true;
            object _false = false;


            Word.Application wordApplication = document.Application;

            Word.Find findClass = wordApplication.Selection.Find;


            findClass.ClearFormatting();
            findClass.Text = searchExpression;
            findClass.Forward = true;
            findClass.Wrap = Word.WdFindWrap.wdFindContinue;
            findClass.Format = false;
            findClass.MatchCase = false;
            findClass.MatchWholeWord = true;
            findClass.MatchWildcards = false;
            findClass.MatchSoundsLike = false;
            findClass.MatchAllWordForms = false;

            return findClass;

        }

        private void IterateText(Word._Document document, Word.Find findClass, Delegate ReplaceMethod, object replaceExpression)
        {

            object _missing = System.Reflection.Missing.Value;

            while (findClass.Execute(ref _missing, ref _missing, ref _missing, ref _missing,
                ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                ref _missing))
            {
                ReplaceMethod.DynamicInvoke(null);
            }
        }

        private void ReplaceVarText(Word._Document document, string textToReplace)
        {
            object _missing = System.Reflection.Missing.Value;
            object _true = true;
            object _false = false;

            Word.Application wordApplication = document.Application;


            wordApplication.Selection.Range.Delete(ref _missing, ref _missing);
            wordApplication.Selection.InsertAfter(textToReplace);

            /*Word.Find findClass = wordApplication.Selection.Find;

            document.Select();
            object direction = Word.WdCollapseDirection.wdCollapseStart;
            wordApplication.Selection.Collapse(ref direction);


            findClass.ClearFormatting();
            findClass.Replacement.ClearFormatting();
            findClass.Text = searchExpression;
            findClass.Replacement.Text = textToReplace;
            findClass.Forward = true;
            findClass.Wrap = Word.WdFindWrap.wdFindContinue;
            findClass.Format = false;
            findClass.MatchCase = false;
            findClass.MatchWholeWord = true;
            findClass.MatchWildcards = false;
            findClass.MatchSoundsLike = false;
            findClass.MatchAllWordForms = false;
            object replaceOption = Word.WdReplace.wdReplaceAll;

            findClass.GetValue(ref _missing, ref _missing, ref _missing, ref _missing,
                ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
                ref _missing, ref replaceOption, ref _missing, ref _missing, ref _missing,
                ref _missing);

            */



            //foreach (Word.Shape shape in document.Shapes)
            //{

            //    if (shape.Type == Microsoft.Office.Core.MsoShapeType.msoTextBox)
            //    {

            //        shape.Select(ref _true);
            //        findClass.GetValue(ref _missing, ref _missing, ref _missing, ref _missing,
            //        ref _missing, ref _missing, ref _missing, ref _missing, ref _missing,
            //        ref _missing, ref replaceOption, ref _missing, ref _missing, ref _missing,
            //        ref _missing);
            //    }
            //}
        }

        #region IDisposable Member

        public void Dispose()
        {
            object _false = false;
            object _missing = System.Reflection.Missing.Value;

            Quit(true, true);

        }

        #endregion

        private void LogMsWordAdapterInformation(string message, Exception ex)
        {
            IDictionary<string, object> loginfo = new Dictionary<string, object>();
            loginfo.Add("Exception", ex);
            LogMsWordAdapterInformation(message, loginfo);
        }
        private void LogMsWordAdapterInformation(string message)
        {
            LogMsWordAdapterInformation(message, (IDictionary<string, object>) null);
        }
        private void LogMsWordAdapterInformation(string message, IDictionary<string, object> logInformations)
        {
            if (!Logger.IsLoggingEnabled())
                return;

            IDictionary<string, object> logInfos = new Dictionary<string, object>();
            //IDictionary<string, string> sysInfos = metaCallBusiness.GetSystemInformation();
            //foreach (KeyValuePair<string, string> sysInfo in sysInfos)
            //{
            //    logInfos.Add(sysInfo.Key, sysInfo.Value);
            //}

            if ((logInformations != null) &&
                logInformations.Count > 0)
            {
                foreach (KeyValuePair<string, object> logInformation in logInformations)
                {
                    logInfos.Add(logInformation);
                }
            }

            LogEntry logEntry = new LogEntry(message, "MsWordAdapter", 20, 2001, System.Diagnostics.TraceEventType.Information, "MsWordAdapter", logInfos);

            Logger.Write(logEntry);

        }
    }
    
    [global::System.Serializable]
    public class MSWordAdapterException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public MSWordAdapterException() { }
        public MSWordAdapterException(string message) : base(message) { }
        public MSWordAdapterException(string message, Exception inner) : base(message, null) { }
        protected MSWordAdapterException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    public class WordImages 
    {
        private string wordImages;

        public WordImages(string value)
        {
            this.wordImages = value;
        }

        public override string ToString()
        {
            return this.wordImages;
        }


    }
            
}
