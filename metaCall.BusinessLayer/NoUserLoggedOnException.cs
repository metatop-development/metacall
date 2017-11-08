using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.BusinessLayer
{
    
    [global::System.Serializable]
    public class NoUserLoggedOnException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NoUserLoggedOnException() { }
        public NoUserLoggedOnException(string message) : base(message) { }
        public NoUserLoggedOnException(string message, Exception inner) : base(message, inner) { }
        protected NoUserLoggedOnException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
