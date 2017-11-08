using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public class UserSignature
    {
        private byte[] signature;

        public byte[] Signature
        {
            get { return signature; }
            set { signature = value; }
        }

        private string filename;

        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
	
	

    }
}
