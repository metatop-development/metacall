using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MaDaNet.Common.AppFrameWork.ApplicationModul;
using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Globalization;
using MaDaNet.Common.AppFrameWork.Validation;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ModulIndex(666)]
    public partial class Stammdatenverwaltung : UserControl, IModulMainControl
    {
        Setting setting = new Setting();
        FormStateType formstate = new FormStateType();

        public enum FormStateType
        {
            ModeEdit,
            ModeView
        }

        private FormStateType FormState
        {
            get
            {
                return this.formstate;
            }

            set
            {
                this.formstate = value;
                switch(this.formstate)
                {
                    case FormStateType.ModeEdit:
                    {
                        editToolStripButton.Enabled = false;
                        toolStripLabelCancel.Enabled = true;
                        toolStripLabelSave.Enabled = true;
                        break;
                    }
                    case FormStateType.ModeView:
                    {
                        editToolStripButton.Enabled = true;
                        toolStripLabelCancel.Enabled = false;
                        toolStripLabelSave.Enabled = false;
                        break;
                    }
                }
            }
        }

        public Stammdatenverwaltung()
        {
            InitializeComponent();
        }

        #region IModulMainControl Member

        public event ModulInfoMessageHandler StatusMessage;

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        public void Initialize(IModulMainControl caller)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        public void UnloadModul(out bool canUnload)
        {
            //throw new Exception("The method or operation is not implemented.");
            canUnload = true;
        }

        public ToolStrip CreateToolStrip()
        {
            return this.toolStripMenue;
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {
            return null;
        }

        #endregion

        private class Configuration : ModulConfigBase
        {

            public override StartUpMenuItem GetStartUpMenuItem()
            {
                return new StartUpMenuItem("Stammdaten", "Verwaltung");
            }
            public override bool HasStartupMenuItem
            {
                get { return true; }
            }

            public override bool HasMainMenuItems
            {
                get { return false; }
            }

            public override bool HasToolStrip
            {
                get { return false; }
            }
        }

        #region IModulMainControl Member


        public bool CanPauseApplication
        {
            get { return true; }
        }

        #endregion

        private void editToolStripButton_Click(object sender, EventArgs e)
        {
             MasterDataEdit();
        }


        #region IModulMainControl Member


        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return principal.IsInRole(MetaCallPrincipal.AdminRoleName);
        }

        #endregion


        private void MasterDataEdit()
        {
            EnableDataFields(true);
            FormState = FormStateType.ModeEdit;
        }

        private void Stammdatenverwaltung_Load(object sender, EventArgs e)
        {
            this.setting = MetaCall.Business.Settings.GetSetting();
            this.newToolStripButton.Enabled = false;
            EnableDataFields(false);
            SetDataFields();

            FormState = FormStateType.ModeView;
        }

        private void SetDataFields()
        {
            this.textBoxActivitiesMaxSec.Text = this.setting.ActivitiesMaxSec.ToString();
            this.textBoxShutDownCountdownSec.Text = this.setting.ShutDownCountdownSec.ToString();
            this.textBoxAddressSafeTime.Text = this.setting.AddressSafeTime.ToString();

            if (this.setting.PaymentTargetVisible == true)
            {
                this.radioButtonPaymentTargetVisibleFalse.Checked = false;
                this.radioButtonPaymentTargetVisibleTrue.Checked = true;
            }
            else
            {
                this.radioButtonPaymentTargetVisibleFalse.Checked = true;
                this.radioButtonPaymentTargetVisibleTrue.Checked = false;
            }

            if (this.setting.AddressSafeActiv == true)
            {
                this.radioButtonAddressSafeActivTrue.Checked = true;
                this.radioButtonAddressSafeActivFalse.Checked = false;
            }
            else
            {
                this.radioButtonAddressSafeActivTrue.Checked = false;
                this.radioButtonAddressSafeActivFalse.Checked = true;
            }

            this.textBoxAbortDialingTime.Text = this.setting.AbortDialingTime.ToString();
        }

        private void EnableDataFields(bool enable)
        {
            this.textBoxActivitiesMaxSec.Enabled = enable;
            this.textBoxShutDownCountdownSec.Enabled = enable;
            this.textBoxAddressSafeTime.Enabled = enable;
            this.radioButtonPaymentTargetVisibleFalse.Enabled = enable;
            this.radioButtonPaymentTargetVisibleTrue.Enabled = enable;
            this.radioButtonAddressSafeActivFalse.Enabled = enable;
            this.radioButtonAddressSafeActivTrue.Enabled = enable;
            this.textBoxAbortDialingTime.Enabled = enable;
        }

        private void toolStripLabelSave_Click(object sender, EventArgs e)
        {
            //Speichern
            if (this.radioButtonPaymentTargetVisibleTrue.Checked == true)
                this.setting.PaymentTargetVisible = true;
            else
                this.setting.PaymentTargetVisible = false;

            if (this.radioButtonAddressSafeActivTrue.Checked == true)
                this.setting.AddressSafeActiv = true;
            else
                this.setting.AddressSafeActiv = false;

            this.setting.ActivitiesMaxSec = UIValidator.Validate<int>(this.textBoxActivitiesMaxSec);
            this.setting.ShutDownCountdownSec = UIValidator.Validate<int>(this.textBoxShutDownCountdownSec);
            this.setting.AddressSafeTime = UIValidator.Validate<int>(this.textBoxAddressSafeTime);

            this.setting.AbortDialingTime = UIValidator.Validate<int>(this.textBoxAbortDialingTime);

            MetaCall.Business.Settings.UpdateSettings(this.setting);

            EnableDataFields(false);
            FormState = FormStateType.ModeView;
        }

        private void toolStripLabelCancel_Click(object sender, EventArgs e)
        {
            //Abbrechen
            SetDataFields();
            EnableDataFields(false);
            FormState = FormStateType.ModeView;

        }

        private void btnBlockAddresses_Click(object sender, EventArgs e)
        {
            MetaCall.Business.Addresses.BlockCallJobsWithMissingAddresses();
            MessageBox.Show("Die fehlerhaften Anrufjobs wurden gesperrt. Bitte informieren Sie " +
                "Agents, die Probleme mit WV hatten, dass Sie metacall neu starten!");
        }


    }
}
