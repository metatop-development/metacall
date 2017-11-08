using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;


namespace metatop.Applications.metaCall.WinForms.App.LogOnServices
{
    /// <summary>
    /// delegate which is called to handle the LoggedOn-Procedure
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public delegate bool LogOnHandler(string username, string password);
    
    public partial class LogOnForm : Form
    {
        #region Class Data
        #region Const Data
        public const int MaxLogOnTrys = 3;
        private const string LogOnInfo = "Anmeldung fehlgeschlagen. Sie haben noch {0} Versuche.";
        #endregion

        private LogOnHandler logOnMethod;
        private int logOnCounter;

       
        #endregion

        public LogOnForm()
        {
            InitializeComponent();

            this.Text = Application.ProductName + " Anmeldung";

            this.txtUserName.Enter+=new EventHandler(TextBox_Enter);
            this.txtPwd.Enter += new EventHandler(TextBox_Enter);


            //Load the Image from Resources
            
            Bitmap image = Properties.Resources.metaCall64;
            image.MakeTransparent(Color.White);

            this.pictureBox1.Image = image;
            ObjectsArrange();

        }

        public LogOnForm(LogOnHandler logOnMethod)
            :this()
        {
            this.logOnMethod = logOnMethod;
            
        }



 




        protected virtual void OkMethod()
        {
            try
            {
                if (this.logOnMethod != null)
                {

                    logOnMethod(this.txtUserName.Text, this.txtPwd.Text);
                }
            }
            catch (LogOnFailedException ex)
            {
                if (this.logOnCounter < MaxLogOnTrys)
                {
                    this.txtUserName.Focus();
                    return;
                }

                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                if (rethrow)
                {
                    throw;
                }
            }
        }

        private void TextBox_Enter(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                textbox.SelectionStart = 0;
                textbox.SelectionLength = textbox.Text.Length;
            }

        }

        #region Properties
        /// <summary>
        /// sets or gets the current LogInCounter
        /// </summary>
        public int LogOnCounter
        {
            get { return logOnCounter; }
            set { 
                logOnCounter = value;
                if (logOnCounter > 0)
                {
                    this.lblInfo.Visible = true;
                    this.lblInfo.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, LogOnInfo, (MaxLogOnTrys - logOnCounter));
                }
                else
                {
                    this.lblInfo.Visible = false;
                    this.lblInfo.Text = string.Empty;
                }
            }
        }

        public string UserName
        {
            get { return this.txtUserName.Text; }
            set { this.txtUserName.Text = value; }
        }

        protected string PassWord
        {
            get { return this.txtPwd.Text; }
        }
	
	
        #endregion

        protected virtual void ObjectsArrange()
        {
            this.btnCancel.Top = 105;
            this.btnLogIn.Top = 105;

            this.lblInfo.Top = 75;
            this.Height = 175;
        }

        private void LogOnForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                try
                {
                    OkMethod();
                }
                catch(Exception ex)
                {
                    bool rethrow = ExceptionPolicy.HandleException(ex, "UI Policy");
                    if (rethrow)
                    {
                        throw;
                    }
                    e.Cancel = true;
                }
        }

    }
}