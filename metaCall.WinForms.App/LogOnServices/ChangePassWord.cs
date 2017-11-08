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
    public delegate void ChangePassHandler(User user, string oldPassword, string newPassword);

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
            
            this.Height = 215;


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
            
            base.UserName = MetaCall.Business.Users.CurrentUser.UserName;
            base.txtUserName.Enabled = false;

        }

        protected override void OkMethod()
        {
            
            if (this.txtPwEdit.Text.Equals(this.txtPwEditConfirm.Text))
            {
                
                changePasswordMethod(MetaCall.Business.Users.CurrentUser, this.PassWord, this.txtPwEdit.Text);
                
            }
            else
            {
                throw new LogOnFailedException("Passwörter sind nicht gleich!");
            }

        }

        protected override void ObjectsArrange()
        {
            base.btnCancel.Top = 155;
            base.btnLogIn.Top = 155;

            base.lblInfo.Top = 125;
            this.Height = 225;
        }
    }
}

