using System;
using System.Collections.Generic;
using System.Text;
using metatop.Applications.metaCall.BusinessLayer;

namespace metatop.Applications.metaCall.WinForms
{
    /// <summary>
    /// Diese Klasse ermöglicht den gemeinsamen Zugriff auf
    /// den MetaCall-BusinessLayer
    /// </summary>
    public static class MetaCall
    {
        public static readonly MetaCallBusiness Business = new MetaCallBusiness();
    }
}
