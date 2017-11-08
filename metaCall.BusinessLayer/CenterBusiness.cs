using System;
using System.Collections.Generic;
using System.Text;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;
using System.Security.Permissions;

namespace metatop.Applications.metaCall.BusinessLayer
{
    public class CenterBusiness
    {

        MetaCallBusiness metaCallBusiness;
        internal CenterBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metaCallBusiness = metaCallBusiness;    
        }

        /// <summary>
        /// Ermittelt das Center zum angegebenen metawareCenter
        /// Erstellt automatisch ein neues Center, wenn das angeforderte noch nicht vorhanden ist
        /// </summary>
        /// <param name="mwCenter"></param>
        /// <returns></returns>
        public Center GetCenter(mwCenter mwCenter)
        {
            if (mwCenter == null)
                throw new ArgumentNullException("mwCenter");

            Center center = metaCallBusiness.ServiceAccess.GetCenter(mwCenter);


            if (center == null)
                center = Create(mwCenter);

            return center;
        }
        /// <summary>
        /// Liefert das Center mit der angegebenen CenterId zur�ck
        /// </summary>
        /// <param name="centerId"></param>
        /// <returns></returns>
        public Center GetCenter(Guid centerId)
        {
            if (centerId == Guid.Empty)
                throw new ArgumentNullException("centerId");

            return metaCallBusiness.ServiceAccess.GetCenter(centerId);
        }

        /// <summary>
        /// Liefert eine Liste aller verf�gbaren Center f�r den angemeldeten Benutzer
        /// </summary>
        public List<CenterInfo> Centers
        {
            get
            {
                //Aktuellen Benutzer �berpr�fen;
                if (!metaCallBusiness.Users.IsLoggedOn)
                    return new List<CenterInfo>();

                MetaCallPrincipal principal = System.Threading.Thread.CurrentPrincipal as MetaCallPrincipal;

                //Wenn es sich um einen Administrator handelt, darf er alle Center sehen
                if (principal.IsInRole(MetaCallPrincipal.AdminRoleName))
                    return new List<CenterInfo>(metaCallBusiness.ServiceAccess.GetAllCenters());

                //Ansonsten werden nur die Center zur�ckgeliefert, auf die der Benutzer zugriff hat
                return metaCallBusiness.Centers.GetCentersForUser(principal.Identity.User);
            }
        }

        private List<CenterInfo> GetCentersForUser(User user)
        {
            return new List<CenterInfo>(metaCallBusiness.ServiceAccess.GetCentersForUser(user));
        }

        /// <summary>
        /// Erstelt ein neues Center auf dem Server
        /// Das metaware-Center dient als Vorlgae und wird automatisch mit dem Center verkn�pft
        /// </summary>
        /// <param name="mwCenter">Das metaware-Center mit demn das aktuelle Center Verkn�pft werden soll</param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role=MetaCallPrincipal.AdminRoleName)]
        public Center Create(mwCenter mwCenter)
        {
            Center center = new Center();
            center.CenterId = Guid.NewGuid();
            center.Bezeichnung = mwCenter.Bezeichnung;
            center.mwCenter = mwCenter;

            Create(center);

            return center;

        }

        /// <summary>
        /// Erstellt ein neues unabh�ngiges Center
        /// </summary>
        /// <param name="bezeichnung">Bezeichnung des Centers</param>
        /// <returns></returns>
        [PrincipalPermission(SecurityAction.Demand, Role=MetaCallPrincipal.AdminRoleName)]
        public Center Create(string bezeichnung)
        {
            if (string.IsNullOrEmpty(bezeichnung))
                throw new ArgumentOutOfRangeException("bezeichnung must be a valid string");
            
            Center center = new Center();
            center.CenterId = Guid.NewGuid();
            center.Bezeichnung = bezeichnung;
            center.Administratoren = new UserInfo[0];
            
            Create(center);

            return center;
        }

        /// <summary>
        /// Erstellt ein neues Center auf dem Server
        /// </summary>
        /// <param name="center">das Center das erstellt werden soll</param>
        [PrincipalPermission(SecurityAction.Demand, Role=MetaCallPrincipal.AdminRoleName)]
        public void Create(Center center)
        {
            //TODO: Pr�fungen ob das Center korrekt gef�llt ist
            if (center == null)
                throw new ArgumentNullException("center");

            if (center.CenterId == Guid.Empty)
                center.CenterId = Guid.NewGuid();

            if (string.IsNullOrEmpty(center.Bezeichnung))
                throw new InvalidOperationException("Center.Bezeichnung must have a valid string");

            if (center.Administratoren == null)
                center.Administratoren = new UserInfo[0];

            metaCallBusiness.ServiceAccess.CreateCenter(center);
        }

        /// <summary>
        /// Aktualisiert ein vorhandeners Center auf dem Server
        /// </summary>
        /// <param name="center"></param>
        [PrincipalPermission(SecurityAction.Demand, Role = MetaCallPrincipal.AdminRoleName)]
        public void Update(Center center)
        {

            if (center == null)
                throw new ArgumentNullException("center");

            //TODO: Pr�fungen ob das Center korekt gef�llt ist

            metaCallBusiness.ServiceAccess.UpdateCenter(center);
        }
        
        /// <summary>
        /// L�scht ein vorhandenes Center auf dem Server
        /// </summary>
        /// <param name="center"></param>
        [PrincipalPermission(SecurityAction.Demand, Role = MetaCallPrincipal.AdminRoleName)]
        public void Delete(CenterInfo center)
        {
            if (center == null)
                throw new ArgumentNullException("center");

            metaCallBusiness.ServiceAccess.DeleteCenter(center.CenterId);
        }

        /// <summary>
        /// Pr�ft, ob der �bergebene User f�r das angegebenen Center 
        /// Administrationsrechte hat
        /// </summary>
        /// <param name="center"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsCenterAdmin(CenterInfo center, User user)
        {
            if (center == null)
                throw new ArgumentNullException("center");

            if (user == null)
                throw new ArgumentNullException("user");

            //TODO: pr�fung ob CenterAdmin
            return false;
            ////Predicate um in den CenterAdministratoren den aktuellen zu finden
            //Predicate<UserInfo> FindCenterAdmin = new Predicate<UserInfo>(
            //    delegate(UserInfo x)
            //    {
            //        return x.UserId.Equals(user.UserId);
            //    }
            //    );

            //if (center.Administratoren == null || center.Administratoren.Length == 0)
            //    return false;

            //return Array.Exists<UserInfo>(center.Administratoren, FindCenterAdmin);
        }

        #region MetaWare Centers
        public List<mwCenter> MetaWareCenters
        {
            get
            {
                return new List<mwCenter>(metaCallBusiness.ServiceAccess.GetAllActiveMetaWareCenters());
            }
        }
        #endregion

        /// <summary>
        /// Liefert eine Liste von UserInfo-Objekten zum angegebenen Center
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public List<UserInfo> GetUsers(Center center)
        {

            if (center == null)
                throw new ArgumentNullException("center");

            return new List<UserInfo>(metaCallBusiness.ServiceAccess.GetUsersByCenter(center.CenterId));
        }

        /// <summary>
        /// Liefert eine Liste von UserInfo-Objekten zum angegebenen CenterInfo-Objekts
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        public List<UserInfo> GetUsers(CenterInfo center)
        {

            if (center == null)
                throw new ArgumentNullException("center");

            return new List<UserInfo>(metaCallBusiness.ServiceAccess.GetUsersByCenter(center.CenterId));
        }


        public CenterInfo GetCenterInfo(mwCenter mwCenter)
        {
            if (mwCenter == null)
                throw new ArgumentNullException("mwCenter");

            return this.metaCallBusiness.ServiceAccess.GetCenterInfo(mwCenter);
        
        }

        public CenterInfo GetCenterInfo(Guid centerId)
        {
            return this.metaCallBusiness.ServiceAccess.GetCenterInfo(centerId);
        }
    }
}
