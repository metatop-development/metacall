using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.BusinessLayer
{
    
    [global::System.Serializable]
    public class NoMetawareUserException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NoMetawareUserException() { }
        public NoMetawareUserException(string message) : base(message) { }
        public NoMetawareUserException(string message, Exception inner) : base(message, inner) { }
        protected NoMetawareUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
