// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//


using System;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: CLSCompliant(false)]
[assembly: AssemblyProduct("metaCall")]
[assembly: AssemblyCompany("seNAsa / MaDaNet")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif BETA
[assembly: AssemblyConfiguration("Beta")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCopyright("seNAsa / MaDaNet 2006-2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.3.7.*")]
