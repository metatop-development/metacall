#if DEBUG
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using MaDaNet.Common.AppFrameWork.ApplicationModul;
using metatop.Applications.metaCall.BusinessLayer;


namespace metatop.Applications.metaCall.WinForms.Modules
{
    [ModulIndex(10)]
    public partial class Mitarbeiterstatistik : UserControl, IModulMainControl
    {
        public Mitarbeiterstatistik()
        {
            InitializeComponent();
        }

        #region IModulMainControl Member

        public event ModulInfoMessageHandler StatusMessage;

        public event QueryPermissionHandler QueryPermisson;

        public event ModuleStateChangedHandler StateChanged;

        public void Initialize(IModulMainControl caller)
        {
            if (caller != null)
            {
                MessageBox.Show (string.Format("Called from {0}", caller.GetType()));

            }
        }

        public void UnloadModul(out bool canUnload)
        {
            //throw new Exception("The method or operation is not implemented.");

            //if (this.textBox1.Text.Length > 0)
            //    this.dataLayer.CurrentProjekt = this.textBox1.Text;

            canUnload = true;

        }
        public ToolStrip CreateToolStrip()
        {
            ToolStrip toolstrip = new ToolStrip(
                new ToolStripButton("Symbolleiste Button1"),
                new ToolStripButton("Symbolleiste Button2")); 
            
            return toolstrip;
        }

        public ToolStripMenuItem[] CreateMainMenuItems()
        {

            ToolStripMenuItem[] menuItems = new ToolStripMenuItem[]{
                new ToolStripMenuItem("Telefonie", null, 
                new ToolStripButton("Test 1 Button"), 
                new ToolStripButton("Test 2 Button"), 
                new ToolStripComboBox("Test 3 ComboBox"))};
            return menuItems;
        }

        private class Configuration : ModulConfigBase
        {
            public Configuration()
            {


            }

            public override StartUpMenuItem GetStartUpMenuItem()
            {
                return null;
            //    return new StartUpMenuItem("Mitarbeiterstatistik", "Auswertungen"); ;
            }

            public override bool HasStartupMenuItem
            {
                get { return true; }
            }

            public override bool HasMainMenuItems
            {
                get { return true; }
            }

            public override bool HasToolStrip
            {
                get { return true; }
            }
        }

        #endregion

        #region IModulMainControl Member


        public bool CanPauseApplication
        {
            get { return true; }
        }

        #endregion

        #region IModulMainControl Member


        public bool CanAccessModul(System.Security.Principal.IPrincipal principal)
        {
            return principal.IsInRole(MetaCallPrincipal.AdminRoleName);
        }

        #endregion
    }
}
#endif