using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.BusinessLayer;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;


namespace metatop.Applications.metaCall.WinForms.App.LogOnServices
{
    public delegate void ChangePassHandler(User user, string oldPassword, string newPassword, string eMailPassword, string eMailPasswordWiederholung);

    public partial class ChangePassWord : metatop.Applications.metaCall.WinForms.App.LogOnServices.LogOnForm
    {
        private ChangePassHandler changePasswordMethod;

        public ChangePassWord()
        {
            InitializeComponent();
        }

        public ChangePassWord(ChangePassHandler changePasswordMethod)
            :this()
        {
            this.changePasswordMethod = changePasswordMethod;
            this.ViewAsPassWordEdit();
            ObjectsArrange();
        }

        public void ViewAsPassWordEdit()
        {            
            this.Height = 310;

            this.txtPwEditConfirm.Left = 210;
            this.txtPwEdit.Left = 210;

            this.txtPwEdit.Top = 75;
            this.txtPwEditConfirm.Top = 105;

            this.lblPwEdit.Top = 78;
            this.lblPwEdit.Left = 100;

            this.lblPwEditConfirm.Top = 108;
            this.lblPwEditConfirm.Left = 100;

            this.txtPwEdit.Visible = true;
            this.txtPwEditConfirm.Visible = true;

            this.lblEMailPasswortUeberschrift.Left = 100;
            this.lblEmailPasswortEdit.Left = 100;
            this.lblEMailPasswortWiederholung.Left = 100;

            base.UserName = MetaCall.Business.Users.CurrentUser.UserName;
            base.txtUserName.Enabled = false;
        }

        protected override void OkMethod()
        {            
            if (this.txtPwEdit.Text.Equals(this.txtPwEditConfirm.Text))
            {

                if (this.txtEmailPasswort.Text.Equals(this.txtEmailPasswortWiederholung.Text))
                {
                    changePasswordMethod(MetaCall.Business.Users.CurrentUser, this.PassWord, this.txtPwEdit.Text, this.txtEmailPasswort.Text, this.txtEmailPasswortWiederholung.Text);
                }
                else
                {
                    throw new LogOnFailedException("E-Mail-Passwörter sind nicht gleich!");
                }
            }
            else
            {
                throw new LogOnFailedException("Anmelde-Passwörter sind nicht gleich!");
            }
        }

        protected override void ObjectsArrange()
        {
            base.btnCancel.Top = 240;
            base.btnLogIn.Top = 240;

            base.lblInfo.Top = 125;
        }
    }
}

