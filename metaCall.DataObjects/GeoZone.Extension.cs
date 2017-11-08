using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class GeoZone
    {
        static GeoZone unknownGeoZone;

        /// <summary>
        /// Unbekannte Zone mit Zone = 99
        /// </summary>
        public static GeoZone Unknown
        {
            get
            {
                if (GeoZone.unknownGeoZone == null)
                {
                    GeoZone unknown = new GeoZone();
                    unknown.Zone = 99;

                    GeoZone.unknownGeoZone = unknown;
                }

                return GeoZone.unknownGeoZone;
            }
        }

    }
}
