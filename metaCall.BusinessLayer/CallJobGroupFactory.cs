using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.BusinessLayer;
using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CallJobGroupFactory
    {
        /// <summary>
        /// Wird ausgelöst, wenn eine neue Anrufgruppe ermittelt wurde
        /// </summary>
        public event CallJobGroupCreatedEventHandler CallJobGroupCreated;

        /// <summary>
        /// Wird ausgelöst, wenn die Methode Analyse einen weiteren Sponsor bearbeitet
        /// </summary>
        public event AnalyseSponsorProgressChangedEventHandler AnalyseSponsorProgressChanged;

        #region private Members
        private MetaCallBusiness metaCallBusiness;
        /// <summary>
        /// speichert die CallJobGruppen zusammen mit den Sponsoren
        /// Die CallJobGruppe wird in einem Container-Element gehalten und die zugehörigen Sponsoren in 
        /// einer Liste
        /// </summary>
        private SortedDictionary<CallJobGroupContainer, List<Sponsor>> callJobGroupList;
        
        private Dictionary<CallJobGroupType, CallJobGroupTypeInfo> callJobGroupTypeInfos;
        #endregion

        internal CallJobGroupFactory(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;
            this.callJobGroupTypeInfos = GetCallJobGroupTypes();
        }


        private Dictionary<CallJobGroupType, CallJobGroupTypeInfo> GetCallJobGroupTypes()
        {
            List<CallJobGroupTypeInfo> infos = this.metaCallBusiness.CallJobGroups.GetCallJobGroupTypeInfos();
            Dictionary<CallJobGroupType, CallJobGroupTypeInfo> groupTypes = new Dictionary<CallJobGroupType, CallJobGroupTypeInfo>(infos.Count);
            foreach (CallJobGroupTypeInfo info in infos)
            {
                groupTypes.Add(info.CallJobGroupType, info);
            }

            return groupTypes;
        }

        /*
        public void Test()
        {
            Project project = this.metaCallBusiness.Projects.Get(new Guid("0E6E4E88-13C9-4A16-9D02-EF8DE3F86D69"));
            List<Sponsor> sponsors = this.metaCallBusiness.Addresses.GetSponsorsByProject(project);

            foreach (Sponsor sponsor in sponsors)
            {
                //Console.WriteLine(sponsor.AddressId.ToString());
                CallJobGroupType callJobGroupType;
                CallJobGroupContainer callJobGroup = null;

                List<SponsorOrderInfo> orderInfos = GetAllSponsorOrderInfos(sponsor, project);
                
                // Wurden Auftragsinformationen gefunden, wird der 
                // Sponsor in eine CallJobGruppe vom Typ Verein- oder ProjektSponsoren eingeordnet
                if (orderInfos.Count > 0)
                {
                    if (Enum.IsDefined(typeof(CallJobGroupType), orderInfos[0].Ranking))
                    {
                        callJobGroupType = (CallJobGroupType)Enum.ToObject(typeof(CallJobGroupType), orderInfos[0].Ranking);

                        Console.WriteLine(sponsor.AddressId.ToString() + "--" + callJobGroupType.ToString());
                        //callJobGroup = ProcessOrderInfo(orderInfos[0], project, callJobGroupType, sponsor);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Enum.Value {0} is not defined", orderInfos[0].Ranking));
                    }
                }
                else
                {
                    //ansonsten findet eine Einordnung in eine Geo-Zone statt.
                    GeoZone geoZone = GetGeoZone(sponsor, project);
                    if (geoZone != null)
                    {
                        callJobGroupType = CallJobGroupType.GeoCodedList;
                        Console.WriteLine(sponsor.AddressId.ToString() + "--" + callJobGroupType.ToString());
//                        callJobGroup = ProcessGeoZone(geoZone, project, callJobGroupType, sponsor);
                    }
                }

                
          //      Console.WriteLine(sponsor.AddressId.ToString() + "--" + callJobGroupType.ToString());

  //              int percentage = (int) (((float) Array.IndexOf(sponsors, sponsor) / (float) sponsors.Length) * 100);
  //              OnAnalyseSponsorProgressChanged(new AnalyseSponsorProgressChangedEventArgs(percentage, null, sponsor, callJobGroup.CallJobGroup, releaseInfos, project, startDate, stopDate));
            }

        }

*/


        /// <summary>
        /// Analysiert die zu diesem Projekt angegebenen Sponsoren
        /// und ermittelt die Zurodnung der jeweiligen Sponsoren zu einer CallJobGruppe
        /// Bei Bedarf wird eine neue CallJobGruppe erstellt
        /// </summary>
        /// <param name="project"></param>
        /// <param name="sponsors"></param>
        public void Analyze(Project project, Sponsor[] sponsors, List<AddressReleaseInfo> releaseInfos, DateTime startDate, DateTime stopDate)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            if (sponsors == null)
                throw new ArgumentNullException("sponsors");

            if (sponsors.Length == 0)
                return;

            bool secondCallListExists = false;
            bool tipAddressExists = false;

            this.callJobGroupList = new SortedDictionary<CallJobGroupContainer, List<Sponsor>>(new CallJobGroupComparer());
            if ((project.CallJobGroups != null) &&
                project.CallJobGroups.Length > 0)
            {
                foreach (CallJobGroup callJobGroup in project.CallJobGroups)
                {
                    if (callJobGroup.Type == CallJobGroupType.SecondCallList)
                        secondCallListExists = true;

                    if (callJobGroup.Type == CallJobGroupType.TipAddress)
                        tipAddressExists = true;

                    CallJobGroupContainer container = ConvertCallJobGroup(callJobGroup);
                    if (container != null)
                         this.callJobGroupList.Add(container, new List<Sponsor>());
                }
            }

            //Anrufgruppe "Zweitanrufe" hinzufügen
            if (!secondCallListExists)
            {
                this.callJobGroupList.Add(CreateSecondCallList(project), new List<Sponsor>());
            }

            //Anrufgruppe "Tipadresse" hinzufügen
            if (!tipAddressExists)
            {
                this.callJobGroupList.Add(CreateTipAddress(project), new List<Sponsor>());
            }

            foreach (Sponsor sponsor in sponsors)
            {
                CallJobGroupType callJobGroupType;
                CallJobGroupContainer callJobGroup = null;

                List<SponsorOrderInfo> orderInfos = GetAllSponsorOrderInfos(sponsor, project);
                
                // Wurden Auftragsinformationen gefunden, wird der 
                // Sponsor in eine CallJobGruppe vom Typ Verein- oder ProjektSponsoren eingeordnet
                if (orderInfos.Count > 0)
                {
                    if (Enum.IsDefined(typeof(CallJobGroupType), orderInfos[0].Ranking))
                    {
                        callJobGroupType = (CallJobGroupType)Enum.ToObject(typeof(CallJobGroupType), orderInfos[0].Ranking);
                        callJobGroup = ProcessOrderInfo(orderInfos[0], project, callJobGroupType, sponsor);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Enum.Value {0} is not defined", orderInfos[0].Ranking));
                    }
                }
                else
                {
                    //Abfragen ob Tip-Adresse

                    if (metaCallBusiness.Addresses.GetAddress_IsTip(sponsor.AdressenPoolNummer))
                    {
                        CallJobGroupTypeInfo typeInfo = this.callJobGroupTypeInfos[CallJobGroupType.TipAddress];

                        string key = Enum.GetName(typeof(CallJobGroupType), typeInfo.CallJobGroupType);

                        foreach (CallJobGroupContainer callJobGroupContainer in this.callJobGroupList.Keys)
                        {
                            if (callJobGroupContainer.Key.Equals(key))
                            {
                                List<Sponsor> sponsorList = this.callJobGroupList[callJobGroupContainer];
                                sponsorList.Add(sponsor);
                                callJobGroup = callJobGroupContainer;
                            }
                        }
                    }
                    else
                    {
                        //ansonsten findet eine Einordnung in eine Geo-Zone statt.
                        GeoZone geoZone = GetGeoZone(sponsor, project);
                        if (geoZone != null)
                        {
                            callJobGroupType = CallJobGroupType.GeoCodedList;
                            callJobGroup = ProcessGeoZone(geoZone, project, callJobGroupType, sponsor);
                        }
                    }
                }

                int percentage = (int) (((float) Array.IndexOf(sponsors, sponsor) / (float) sponsors.Length) * 100);
                OnAnalyseSponsorProgressChanged(new AnalyseSponsorProgressChangedEventArgs(percentage, null, sponsor, callJobGroup.CallJobGroup, releaseInfos, project, startDate, stopDate));
            }

            //Durchsortieren der CallJobGroupListe und festelegen des Ranking
            CallJobGroup[] callJobGroups = new CallJobGroup[this.callJobGroupList.Keys.Count];
            int i = 0;
            foreach (CallJobGroupContainer callJobGroupContainer in this.callJobGroupList.Keys)
            {
                callJobGroups[i] = callJobGroupContainer.CallJobGroup;
                callJobGroups[i].Ranking = i + 1;
                i++;
            }

        }

        private CallJobGroupContainer CreateTipAddress(Project project)
        {
            CallJobGroupTypeInfo typeInfo = this.callJobGroupTypeInfos[CallJobGroupType.TipAddress];

            string key = Enum.GetName(typeof(CallJobGroupType), typeInfo.CallJobGroupType);

            CallJobGroup callJobGroup = new CallJobGroup();
            callJobGroup.CallJobGroupId = Guid.NewGuid();
            callJobGroup.DisplayName = typeInfo.DisplayNameTemplate;
            callJobGroup.Description = typeInfo.Description;
            callJobGroup.Key = key;
            callJobGroup.Project = this.metaCallBusiness.Projects.Get(project);
            callJobGroup.Type = typeInfo.CallJobGroupType;
            callJobGroup.Teams = new TeamInfo[0];
            callJobGroup.Users = new UserInfo[0];
            callJobGroup.Ranking = typeInfo.Ranking;

            OnCallJobGroupCreated(new CallJobGroupCreatedEventArgs(callJobGroup, project));
            return new CallJobGroupContainer(callJobGroup, key, -1, -1);
        }

        private CallJobGroupContainer CreateSecondCallList(Project project)
        {
            CallJobGroupTypeInfo typeInfo = this.callJobGroupTypeInfos[CallJobGroupType.SecondCallList];

            string key = Enum.GetName(typeof(CallJobGroupType), typeInfo.CallJobGroupType);

            CallJobGroup callJobGroup = new CallJobGroup();
            callJobGroup.CallJobGroupId = Guid.NewGuid();
            callJobGroup.DisplayName = typeInfo.DisplayNameTemplate;
            callJobGroup.Description = typeInfo.Description;
            callJobGroup.Key = key;
            callJobGroup.Project = this.metaCallBusiness.Projects.Get(project);
            callJobGroup.Type = typeInfo.CallJobGroupType;
            callJobGroup.Teams = new TeamInfo[0];
            callJobGroup.Users = new UserInfo[0];
            callJobGroup.Ranking = typeInfo.Ranking;

            OnCallJobGroupCreated(new CallJobGroupCreatedEventArgs(callJobGroup, project));
            return new CallJobGroupContainer(callJobGroup, key, -1, -1);
        }

        private CallJobGroupContainer ProcessGeoZone(GeoZone geoZone, Project project, CallJobGroupType callJobGroupType, Sponsor sponsor)
        {
            string key = GetGeoZoneKey(geoZone, callJobGroupType);

            foreach (CallJobGroupContainer callJobGroup in this.callJobGroupList.Keys)
            {
                if (callJobGroup.Key.Equals(key))
                {
                    List<Sponsor> sponsorList = this.callJobGroupList[callJobGroup];
                    sponsorList.Add(sponsor);
                    return callJobGroup;
                }
            }
            {
                //neue CallJob-Gruppe erstellen
                CallJobGroupContainer callJobGroup = CreateCallJobGroup(geoZone, key, project, callJobGroupType);
                List<Sponsor> sponsorList = new List<Sponsor>(new Sponsor[] { sponsor });
                this.callJobGroupList.Add(callJobGroup, sponsorList);
                return callJobGroup;
            }
        }

        private CallJobGroupContainer ProcessOrderInfo(SponsorOrderInfo sponsorOrderInfo, Project project, CallJobGroupType callJobGroupType, Sponsor sponsor)
        {
            string key = GetOrderInfoKey(sponsorOrderInfo, callJobGroupType);

            foreach (CallJobGroupContainer callJobGroup in this.callJobGroupList.Keys)
            {
                if (callJobGroup.Key.Equals(key))
                {
                    List<Sponsor> sponsorList = this.callJobGroupList[callJobGroup];
                    sponsorList.Add(sponsor);
                    return callJobGroup;
                }
            }

            //neue CallJob-Gruppe erstellen
            {
                CallJobGroupContainer callJobGroup = CreateCallJobGroup(sponsorOrderInfo, key, project, callJobGroupType);
                List<Sponsor> sponsorList = new List<Sponsor>(new Sponsor[] { sponsor });
                this.callJobGroupList.Add(callJobGroup, sponsorList);

                return callJobGroup;
            }
        }

        private CallJobGroupContainer CreateCallJobGroup(SponsorOrderInfo sponsorOrderInfo, string key, Project project, CallJobGroupType callJobGroupType)
        {

            CallJobGroup callJobGroup = new CallJobGroup();

            StringBuilder sb = new StringBuilder();
            CallJobGroupTypeInfo info = this.callJobGroupTypeInfos[callJobGroupType];
            if (info.DisplayNameTemplate != null && info.DisplayNameTemplate.Length > 0)
            {
                sb.AppendFormat(info.DisplayNameTemplate, sponsorOrderInfo.ProjectYear, sponsorOrderInfo.ProjectMonth);
            }



            callJobGroup.CallJobGroupId = Guid.NewGuid();
            callJobGroup.Project = this.metaCallBusiness.Projects.Get(project);
            callJobGroup.Type = callJobGroupType;
            callJobGroup.DisplayName = sb.Length > 0 ? sb.ToString() : key;
            callJobGroup.Description = callJobGroup.DisplayName;
            callJobGroup.Ranking = -1; //TODO: Ranking ermitteln
            callJobGroup.Teams = new TeamInfo[0];
            callJobGroup.Users = new UserInfo[0];
            callJobGroup.Key = key;

            OnCallJobGroupCreated(new CallJobGroupCreatedEventArgs(callJobGroup, project));

            return new CallJobGroupContainer(callJobGroup, key, sponsorOrderInfo.ProjectYear, sponsorOrderInfo.ProjectMonth);

        }

        private CallJobGroupContainer CreateCallJobGroup(GeoZone geoZone, string key, Project project, CallJobGroupType callJobGroupType)
        {

            CallJobGroup callJobGroup = new CallJobGroup();
            StringBuilder sb = new StringBuilder();
            CallJobGroupTypeInfo info = this.callJobGroupTypeInfos[callJobGroupType];
            if (info.DisplayNameTemplate != null && info.DisplayNameTemplate.Length > 0)
            {
                sb.AppendFormat(info.DisplayNameTemplate, geoZone.Zone);
            }

            callJobGroup.CallJobGroupId = Guid.NewGuid();
            callJobGroup.Project = this.metaCallBusiness.Projects.Get(project);
            callJobGroup.Type = callJobGroupType;
            callJobGroup.DisplayName = sb.Length > 0 ? sb.ToString() : key;
            callJobGroup.Description = callJobGroup.DisplayName;
            callJobGroup.Ranking = geoZone.Zone;
            callJobGroup.Teams = new TeamInfo[0];
            callJobGroup.Users = new UserInfo[0];
            callJobGroup.Key = key;
            
            OnCallJobGroupCreated(new CallJobGroupCreatedEventArgs(callJobGroup, project));

            return new CallJobGroupContainer(callJobGroup, key, geoZone.Zone);

        }

        private List<SponsorOrderInfo> GetAllSponsorOrderInfos(Sponsor sponsor, Project project)
        {
            return new List<SponsorOrderInfo>(this.metaCallBusiness.ServiceAccess.GetAllSponsorOrderInfos(sponsor, project.ProjectId));

        }

        private GeoZone GetGeoZone(Sponsor sponsor, Project project)
        {
            return this.metaCallBusiness.ServiceAccess.GetGeoZone(sponsor, project.ProjectId);
        }

        private string GetOrderInfoKey(SponsorOrderInfo sponsorOrderInfo, CallJobGroupType callJobGroupType)
        {

            if (callJobGroupType == CallJobGroupType.CommonSponsorList)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}", Enum.GetName(typeof(CallJobGroupType), callJobGroupType));
                return sb.ToString();
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}", Enum.GetName(typeof(CallJobGroupType), callJobGroupType));
                sb.Append("-");
                sb.AppendFormat("{0:0000}-{1:00}", sponsorOrderInfo.ProjectYear, sponsorOrderInfo.ProjectMonth);
                return sb.ToString();
            }
        }

        private string GetGeoZoneKey(GeoZone geoZone, CallJobGroupType callJobGroupType)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}", Enum.GetName(typeof(CallJobGroupType), callJobGroupType));
            sb.Append("-");
            sb.AppendFormat("{0:00}", geoZone.Zone);
            return sb.ToString();
        }

        private CallJobGroupContainer ConvertCallJobGroup(CallJobGroup callJobGroup)
        {
            string [] keys = callJobGroup.Key.Split(new char[]{'-'}, StringSplitOptions.RemoveEmptyEntries);

            if (callJobGroup.Type == CallJobGroupType.GeoCodedList)
                return new CallJobGroupContainer(callJobGroup, callJobGroup.Key, int.Parse(keys[1]));
            else if (callJobGroup.Type == CallJobGroupType.ProjectSponsorList)
                return new CallJobGroupContainer(callJobGroup, callJobGroup.Key, int.Parse(keys[1]), int.Parse(keys[2]));
            else if (callJobGroup.Type == CallJobGroupType.CommonSponsorList)
                return new CallJobGroupContainer(callJobGroup, callJobGroup.Key, -1, -1);
            else if (callJobGroup.Type == CallJobGroupType.CustomerSponsorList)
                return new CallJobGroupContainer(callJobGroup, callJobGroup.Key, int.Parse(keys[1]), int.Parse(keys[2]));
            else if (callJobGroup.Type == CallJobGroupType.TipAddress)
                return new CallJobGroupContainer(callJobGroup, callJobGroup.Key, -1, -1);
            else if (callJobGroup.Type == CallJobGroupType.SecondCallList)
                return new CallJobGroupContainer(callJobGroup, callJobGroup.Key, -1, -1);
            else if (callJobGroup.Type == CallJobGroupType.ManualList)
                return new CallJobGroupContainer(callJobGroup, callJobGroup.Key, -1, -1);
            else
                return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected void OnCallJobGroupCreated(CallJobGroupCreatedEventArgs e)
        {
            if (CallJobGroupCreated != null)
                CallJobGroupCreated(this, e);
        }

        protected void OnAnalyseSponsorProgressChanged(AnalyseSponsorProgressChangedEventArgs e)
        {
            if (AnalyseSponsorProgressChanged != null)
                AnalyseSponsorProgressChanged(this, e);
        }

        /// <summary>
        /// private Klasse zum aufnehmen der ermittelten Informationen
        /// </summary>
        private class CallJobGroupContainer
        {
            internal CallJobGroupContainer(CallJobGroup callJobGroup, string key, int orderInfoYear, int orderInfoMonth)
            {
                this.callJobGroup = callJobGroup;
                this.callJobGroupType = callJobGroup.Type;
                this.orderInfoYear = orderInfoYear;
                this.orderInfoMonth = orderInfoMonth;
                this.geoZoneNumber = -1;
                this.key = key;
            }

            internal CallJobGroupContainer(CallJobGroup callJobGroup, string key, int geoZoneNumber)
            {
                this.callJobGroup = callJobGroup;
                this.callJobGroupType = callJobGroup.Type;
                this.orderInfoYear = -1;
                this.orderInfoMonth = -1;
                this.geoZoneNumber = geoZoneNumber;
                this.key = key;
            }

            private string key;
            public string Key
            {
                get { return key; }
            }


            private CallJobGroup callJobGroup;
            public CallJobGroup CallJobGroup
            {
                get { return callJobGroup; }
            }

            private CallJobGroupType callJobGroupType;
            public CallJobGroupType CallJobGroupType
            {
                get { return callJobGroupType; }
            }

            private int geoZoneNumber;
            public int GeoZoneNumber
            {
                get { return geoZoneNumber; }
            }

            private int orderInfoYear;
            public int OrderInfoYear
            {
                get { return orderInfoYear; }
            }

            private int orderInfoMonth;
            public int OrderInfoMonth
            {
                get { return orderInfoMonth; }
            }

        }

        /// <summary>
        /// Vergleich von CallJobGruppen anhand CallJobGroupTypes und des Rankings 
        /// </summary>
        private class CallJobGroupComparer : IEqualityComparer<CallJobGroupContainer>, IComparer<CallJobGroupContainer>
        {
            #region IComparer<CallJobGroup> Member

            public int Compare(CallJobGroupContainer x, CallJobGroupContainer y)
            {

                int result = 0;
                switch (x.CallJobGroupType)
                {
                    case CallJobGroupType.ProjectSponsorList:
                        if (y.CallJobGroupType != CallJobGroupType.ProjectSponsorList)
                            return -1;

                        result = x.OrderInfoYear.CompareTo(y.OrderInfoYear) * -1;

                        if (result == 0)
                            return x.OrderInfoMonth.CompareTo(y.OrderInfoMonth) * -1;
                        else
                            return result;

                    case CallJobGroupType.CustomerSponsorList:
                        if (y.CallJobGroupType == CallJobGroupType.ProjectSponsorList)
                            return 1;
                        if (y.CallJobGroupType == CallJobGroupType.CustomerSponsorList)
                        {
                            result = x.OrderInfoYear.CompareTo(y.OrderInfoYear) * -1;

                            if (result == 0)
                                return x.OrderInfoMonth.CompareTo(y.OrderInfoMonth) * -1;
                            else
                                return result;
                        }
                        return -1;
                    case CallJobGroupType.CommonSponsorList:
                        if ((y.CallJobGroupType == CallJobGroupType.ProjectSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CustomerSponsorList))
                            return 1;

                        if (y.CallJobGroupType == CallJobGroupType.CommonSponsorList)
                        {
                            result = x.OrderInfoYear.CompareTo(y.OrderInfoYear) * -1;

                            if (result == 0)
                                return x.OrderInfoMonth.CompareTo(y.OrderInfoMonth) * -1;
                            else
                                return result;
                        }

                        return -1;
                    case CallJobGroupType.GeoCodedList:
                        if ((y.CallJobGroupType == CallJobGroupType.ProjectSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CustomerSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CommonSponsorList))
                            return 1;

                        if (y.CallJobGroupType == CallJobGroupType.GeoCodedList)
                            return x.GeoZoneNumber.CompareTo(y.GeoZoneNumber);

                        return -1;
                    case CallJobGroupType.ManualList:
                        if ((y.CallJobGroupType == CallJobGroupType.ProjectSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CustomerSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CommonSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.GeoCodedList))
                            return 1;

                        if (y.CallJobGroupType == CallJobGroupType.ManualList)
                            return x.Key.CompareTo(y.Key);

                        return -1;
                    case CallJobGroupType.SecondCallList:
                        if ((y.CallJobGroupType == CallJobGroupType.ProjectSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CustomerSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CommonSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.GeoCodedList) ||
                            (y.CallJobGroupType == CallJobGroupType.ManualList))
                            return 1;

                        if (y.CallJobGroupType == CallJobGroupType.SecondCallList)
                            return x.Key.CompareTo(y.Key);

                        return -1;
                    case CallJobGroupType.TipAddress:
                        if ((y.CallJobGroupType == CallJobGroupType.ProjectSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CustomerSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.CommonSponsorList) ||
                            (y.CallJobGroupType == CallJobGroupType.GeoCodedList) ||
                            (y.CallJobGroupType == CallJobGroupType.SecondCallList) ||
                            (y.CallJobGroupType == CallJobGroupType.ManualList))
                            return 1;

                        if (y.CallJobGroupType == CallJobGroupType.TipAddress)
                            return x.Key.CompareTo(y.Key);

                        return -1;
                    default:
                        throw new InvalidOperationException(string.Format("CallJobGroupType {0} is not sortable", x.CallJobGroupType.ToString()));
                }
            }

            #endregion

            #region IEqualityComparer<CallJobGroupContainer> Member

            public bool Equals(CallJobGroupContainer x, CallJobGroupContainer y)
            {

                return (this.Compare(x, y) == 0);
            }

            public int GetHashCode(CallJobGroupContainer obj)
            {
                return obj.GetHashCode();
            }

            #endregion
        }

    }

    public delegate void CallJobGroupCreatedEventHandler(object sender, CallJobGroupCreatedEventArgs e);
    public class CallJobGroupCreatedEventArgs : EventArgs
    {
        public CallJobGroupCreatedEventArgs(CallJobGroup callJobGroup, Project project)
        {
            this.callJobGroup = callJobGroup;
            this.project = project;
        }
        
        private CallJobGroup callJobGroup;
        public CallJobGroup CallJobGroup
        {
            get { return callJobGroup; }
        }

        private Project project;
        public Project Project
        {
            get { return project; }
        }
	
    }


    public delegate void AnalyseSponsorProgressChangedEventHandler(object sender, AnalyseSponsorProgressChangedEventArgs e);
    public class AnalyseSponsorProgressChangedEventArgs : System.ComponentModel.ProgressChangedEventArgs
    {

        public AnalyseSponsorProgressChangedEventArgs(int percentage, object userState, Sponsor sponsor, CallJobGroup callJobGroup, List<AddressReleaseInfo> releaseInfos, Project project, DateTime startDate, DateTime stopDate)
            : base(percentage, userState)
        {
            if (sponsor == null)
                throw new ArgumentNullException("sponsor");

            if (callJobGroup == null)
                throw new ArgumentNullException("callJobGroup");
            
            this.sponsor = sponsor;
            this.callJobGroup = callJobGroup;
            this.releaseInfos = releaseInfos;
            this.startDate = startDate;
            this.stopDate = stopDate;
            this.project = project;
        }
        
        private Sponsor sponsor;
        public Sponsor Sponsor
        {
            get { return sponsor; }
        }

        private CallJobGroup callJobGroup;
        public CallJobGroup CallJobGroup
        {
            get { return callJobGroup; }
        }

        private List<AddressReleaseInfo> releaseInfos;
        public List<AddressReleaseInfo> ReleaseInfos
        {
            get { return releaseInfos; }
        }

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
        }

        private DateTime stopDate;
        public DateTime StopDate
        {
            get { return stopDate; }
        }

        private Project  project;
        public Project  Project
        {
            get { return project; }
        }
	
	
    }
}
