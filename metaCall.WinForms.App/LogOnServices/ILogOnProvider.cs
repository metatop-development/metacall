using System;
namespace metatop.Applications.metaCall.WinForms.App.LogOnServices
{
    public interface ILogOnProvider
    {
        bool LogOn(string username, string password);
    }
}
