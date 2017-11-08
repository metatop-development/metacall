using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using System.ComponentModel;

namespace metatop.Applications.metaCall.WinForms.Modules
{
    public class ExpandableUserControl: UserControl
    {
        public ExpandableUserControl()
        {
        }


        
        private Size expandedSize;
        [Category("Expanding"), 
        Description("Gibt die expandierte Größe des Steuerelements an."), 
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size ExpandedSize
        {
            get { return expandedSize; }
            set { expandedSize = value; }
        }

        private Size collapsedSize;

        [Category("Expanding"),
        Description("Gibt die reduzierte Größe des Steuerelements an."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size CollapsedSize
        {
            get { return collapsedSize; }
            set { collapsedSize = value; }
        }

        
        protected override void OnControlAdded(ControlEventArgs e)
        {

            e.Control.MouseMove += new MouseEventHandler(Control_MouseMove);
            
            
            base.OnControlAdded(e);
        }

        void Control_MouseMove(object sender, MouseEventArgs e)
        {
            //Umrechnen der Koordinaten in ClientKoordination
            Point mouse = PointToClient(new Point(e.X, e.Y));


            MouseEventArgs mea = new MouseEventArgs(
                e.Button, 
                e.Clicks, 
                mouse.X, 
                mouse.Y, 
                e.Delta);

            this.OnMouseMove(mea);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        /// <summary>
        /// erweitert das Steuelement auf seine maximale Größe
        /// </summary>
        public void Expand()
        {

            if (expandedSize.Equals(this.Size))
                return;

            //Maximieren, wenn der Focus auf das Steuerelement gesetzt wird
            if (!this.expandedSize.IsEmpty)
            {
                this.Size = this.expandedSize;
            }

            //isExpanded = true;
        }

        /// <summary>
        /// reduziert die Größe des Steuerelements auf seine minimale Größe
        /// </summary>
        public void Collapse()
        {
            if (this.collapsedSize.Equals(this.Size))
                return;
            
            if (!this.collapsedSize.IsEmpty)
            {
                this.Size = this.collapsedSize;
            }

            //isExpanded = false;
        }
   
    }
}
