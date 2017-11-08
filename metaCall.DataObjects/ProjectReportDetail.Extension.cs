using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public class ProjectReportDetail
    {
        private List<ProjectReportDetailSummen> summen;
        private List<ProjectReportDetailDaten> daten;
        private List<ProjectReportGeozonen> geoZonen;

        public List<ProjectReportDetailSummen> Summen
        {
            get{ return this.summen;}
            set { this.summen = value; }
        }

        public List<ProjectReportDetailDaten> Daten
        {
            get { return this.daten; }
            set { this.daten = value; }
        }

        public List<ProjectReportGeozonen> GeoZonen
        {
            get { return this.geoZonen; }
            set { this.geoZonen = value; }
        }
    }
}
