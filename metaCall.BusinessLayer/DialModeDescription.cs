using System;
//using System.Collections.Generic;
//using System.Text;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class DialModeDescription
    {
        private const string manuellesWaehlen = "manuelles W�hlen";
        private const string automatischesSofortWaehlen = "autom. Sofortw�hlen";
        private const string automatischesWaehlen = "autom. W�hlen";

        public DialModeDescription() { }

        public DialMode TranslateToDialMode(string description)
        {
            switch (description)
            {
                case manuellesWaehlen:
                    return DialMode.ManualDialing;
                case automatischesSofortWaehlen:
                    return DialMode.AutoDialingImmediately;
                case automatischesWaehlen:
                    return DialMode.AutoSoftwareDialing;
                default:
                    return DialMode.Unseeded;
            }

        }

        public string TranslateToDescription(DialMode dialMode)
        {
            switch (dialMode)
            {

                case DialMode.ManualDialing:
                    return manuellesWaehlen;
                case DialMode.AutoDialingImmediately:
                    return automatischesSofortWaehlen;
                case DialMode.AutoSoftwareDialing:
                    return automatischesWaehlen;
                default:
                    return null;
            }
        }
    }


}
