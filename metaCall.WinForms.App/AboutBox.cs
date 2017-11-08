using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Text;

namespace metatop.Applications.metaCall.WinForms.App
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();

            //  Initialisieren Sie AboutBox, um die Produktinformationen aus den Assemblyinformationen anzuzeigen.
            //  Ändern Sie die Einstellungen für Assemblyinformationen für Ihre Anwendung durch eine der folgenden Vorgehensweisen:
            //  - Projekt->Eigenschaften->Anwendung->Assemblyinformationen
            //  - AssemblyInfo.cs
            this.Text = String.Format("Info über {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0} - {1}", AssemblyVersion , AssemblyConfiguration);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assemblyattributaccessoren

        public string AssemblyConfiguration
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyConfigurationAttribute), false);

                // Wenn mindestens ein Configuration-Attribut vorhanden ist
                if (attributes.Length > 0)
                {
                    // Erstes auswählen
                    AssemblyConfigurationAttribute configurationAttribute = (AssemblyConfigurationAttribute)attributes[0];
                    // Zurückgeben, wenn es keine leere Zeichenfolge ist
                    if (configurationAttribute.Configuration != "")
                        return configurationAttribute.Configuration;
                }
                // Wenn kein Configuration-Attribut vorhanden oder das Configuration-Attribut eine leere Zeichenfolge war, einen leeren String zurückgeben
                return string.Empty;
            }
        }

        public string AssemblyTitle
        {
            get
            {
                // Alle Title-Attribute in dieser Assembly abrufen
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // Wenn mindestens ein Title-Attribut vorhanden ist
                if (attributes.Length > 0)
                {
                    // Erstes auswählen
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // Zurückgeben, wenn es keine leere Zeichenfolge ist
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // Wenn kein Title-Attribut vorhanden oder das Title-Attribut eine leere Zeichenfolge war, den EXE-Namen zurückgeben
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                if (sb.Length > 0) sb.AppendLine();
                sb.AppendFormat("Aktueller Benutzer     {0}", Environment.UserName);

                string session = Environment.GetEnvironmentVariable("SESSIONNAME");
                if (!string.IsNullOrEmpty(session))
                {
                    if (sb.Length > 0) sb.AppendLine(); 
                    sb.AppendFormat("Session       {0}", session);
                    
                }

                if (sb.Length > 0) sb.AppendLine();
                sb.Append("===============================");
                if (sb.Length > 0) sb.AppendLine(); 

                
                if (MetaCall.Business.Users.CurrentUser != null)
                {
                    if (sb.Length > 0) sb.AppendLine(); 
                    sb.AppendFormat("angemeldet als      {0}", MetaCall.Business.Users.CurrentUser.DisplayName);
                    
                }

                if (MetaCall.Business.Projects.Current != null)
                {
                    if (sb.Length > 0) sb.AppendLine();
                    sb.AppendFormat("aktuelles Projekt   {0}", MetaCall.Business.Projects.Current.Bezeichnung);
                }

                Dictionary<string, string> systemInformation = MetaCall.Business.GetSystemInformation();
                if (sb.Length > 0) sb.AppendLine();
                sb.Append("===============================");
                if (sb.Length > 0) sb.AppendLine(); 

                foreach (string key in systemInformation.Keys)
                {
                    if (sb.Length > 0) sb.AppendLine();
                    sb.AppendFormat("{0}   : {1}", key, systemInformation[key]);
                }



                return sb.ToString();

            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Alle Product-Attribute in dieser Assembly abrufen
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // Eine leere Zeichenfolge zurückgeben, wenn keine Product-Attribute vorhanden sind
                if (attributes.Length == 0)
                    return "";
                // Den Wert des Product-Attributs zurückgeben, wenn eines vorhanden ist
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Alle Copyright-Attribute in dieser Assembly abrufen
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // Eine leere Zeichenfolge zurückgeben, wenn keine Copyright-Attribute vorhanden sind
                if (attributes.Length == 0)
                    return "";
                // Den Wert des Copyright-Attributs zurückgeben, wenn eines vorhanden ist
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Alle Company-Attribute in dieser Assembly abrufen
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // Eine leere Zeichenfolge zurückgeben, wenn keine Company-Attribute vorhanden sind
                if (attributes.Length == 0)
                    return "";
                // Den Wert des Company-Attributs zurückgeben, wenn eines vorhanden ist
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion
    }
}
