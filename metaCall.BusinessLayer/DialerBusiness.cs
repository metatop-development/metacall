// Legt fest ob die TAPI für den Anwahlvoprgang verwendet werden soll.
//#define UseTAPI 

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging;

using metatop.Applications.metaCall.DataObjects;
using metatop.Applications.metaCall.ServiceAccessLayer;


using Tapi2Lib;


namespace metatop.Applications.metaCall.BusinessLayer
{
    
    public class DialerBusiness: IDisposable
    {
        public event DialingEventHandler WantConnect;
        public event DialingEventHandler Connected;
        public event DialingEventHandler HangedUp;
 
        private MetaCallBusiness metacallBusiness;
        private DialStates state;

        public readonly bool IsVirtualMode;

        private string phoneNumber;
        private Call call;

        private CLine cLine;
        //private TapiCall tCall;
        private CTapi cTapi;
        //private int tapiLineId = -1;
        private IntPtr hCall_PlaceHolder;

        bool lineIsOpen;

        private bool isHangedUp;

        
        private void InitializeTapi()
        {
            IntPtr ptr = System.IntPtr.Zero;
            cTapi = new CTapi("metaCall",
                                 0x00020000,
                                 ptr,
                                 CTapi.LineInitializeExOptions.LINEINITIALIZEEXOPTION_USEEVENT);

            lineIsOpen = false;
            cTapi.CallStateEvent += new CTapi.CallStateEventHandler(cTapi_CallStateChanged);

            //line.CallStateChanged += new EventHandler<CallStateEventArgs>(cTapi_CallStateChanged);
            //line.NewCall += new EventHandler<NewCallEventArgs>(cTapi_NewCall);
            //line.CallInfoChanged += new EventHandler<CallInfoChangeEventArgs>(cTapi_CallInfoChanged);

            OpenLine();
            if (lineIsOpen == true)
            {
                Thread th = new Thread(new ThreadStart(LineGetMessageLoop));
                th.IsBackground = true;
                th.Start();

            }
        }

        
        private void RegisterEvents()
        {
        }

        public DialerBusiness(MetaCallBusiness metaCallBusiness)
        {
            this.metacallBusiness = metaCallBusiness;
            this.state = DialStates.Ready;
            this.isHangedUp = true;

            if (this.metacallBusiness.Users.DomainUser_UsesDialer(Environment.UserName))
            {
                //#if UseTAPI
                try
                {
                    InitializeTapi();

                }
                catch
                {
                    // wenn bei der initialisierung ein Fehler auftritt wird 
                    // der virtuelle Modus eingeschaltet
                    IsVirtualMode = true;
                }
                finally
                {

                }
            }
            else
                IsVirtualMode = true;

            //Prüfen, ob cTapi oder Line ungültig sind
            //if (this.cTapi == null || this.cLine.hLine == IntPtr.Zero)
            if (!lineIsOpen)
            {
                IsVirtualMode = true;
            }

        }

        public void LineGetMessageLoop()
        {
            CTapi.LineErrReturn ret;
            while (lineIsOpen)
            {
                ret = cTapi.LineGetMessage();
                if (ret != CTapi.LineErrReturn.LINEERR_OK)
                    Console.WriteLine("MessageLoop return: " + ret.ToString());
            }
        } 
        
        void cTapi_CallStateChanged(object sender, CTapi.CallStateEventArgs e)
        {
            if (this.call == null)
            {
                hCall_PlaceHolder = e.hcall;
                return;
            }

            CallJobPhoneEvent phoneEvent = new CallJobPhoneEvent(this.call.CallJob.CallJobId, 
                this.metacallBusiness.Users.CurrentUser.UserId, e.CallState.ToString(),
                System.DateTime.Now, this.phoneNumber);
            this.metacallBusiness.CallJobPhoneEvents.CreateAsync(phoneEvent);
                
            /* Anwahlversuch -> Freizeichen */
            if (e.CallState == CTapi.LineCallState.LINECALLSTATE_DIALTONE)
            {
                this.state = DialStates.DialTone;
                hCall_PlaceHolder = e.hcall;
                //OnWantConnect(new DialingEventArgs(e.Call.CalledId, this.call, DialStates.DialTone));
                return;
            }

            /* Anwahlversuch -> Beim anderen klingelts */
            if (e.CallState == CTapi.LineCallState.LINECALLSTATE_RINGBACK)
            {
                this.state = DialStates.RingBack;
                OnWantConnect(new DialingEventArgs(phoneNumber, this.call, DialStates.RingBack));
                return;
            }

            /* !!! Anruf !!! -> das gegenüber hat abgenommen */
            if (e.CallState == CTapi.LineCallState.LINECALLSTATE_CONNECTED)
            {
                this.state = DialStates.Connected;
                OnConnected(new DialingEventArgs(phoneNumber, this.call, DialStates.Connected));
                return;
            }
            
            // Anrufer hat aufgelegt
            if (e.CallState == CTapi.LineCallState.LINECALLSTATE_DISCONNECTED)
            {
                try
                {
                    if (hCall_PlaceHolder == e.hcall)
                    {
                        HangUp();
                    }
                }
                catch (ObjectDisposedException exSafeHandle)
                {
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                return;
            }

            /* Anruf unterbrochen/beendet */
            if (e.CallState == CTapi.LineCallState.LINECALLSTATE_IDLE)
            {
                try
                {
                    if (hCall_PlaceHolder == e.hcall)
                    {
                        this.state = DialStates.Ready;
                        HangUp();
                    }
                    //OnHangedUp(new DialingEventArgs(e.Call.CalledId, this.call, DialStates.Ready));
                }
                catch (ObjectDisposedException exSafeHandle)
                {
                    //SafeHandleError wird ignoriert
                    //System.Windows.Forms.MessageBox.Show("mcDef: " + exSafeHandle.Message);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return;
            }

        }
        
        public void Dial(string phoneNumber, Call call)
        {

            // Der Dialer muss sich im Zustand "Ready" befinden, damit gewählt werden kann
            this.state = CheckCallState();


            if (this.state != DialStates.Ready)
                return;

            this.call = call;

            //TODO: über Anwendungseinstellungen eine zusätzliche Vorwahl ermöglichen
            string externVorwahl = "0";
            string dialingPrefixNumber = call.CallJob.Project.DialingPrefixNumber;

            this.phoneNumber = phoneNumber;
            this.phoneNumber = this.metacallBusiness.Addresses.ClearPhoneNumber(externVorwahl + phoneNumber);


            //Ausführen der Anwahl
            if (!IsVirtualMode &&
                (MetaCallPrincipal.Current.Identity.User.DialMode == DialMode.AutoSoftwareDialing
                    || MetaCallPrincipal.Current.Identity.User.DialMode == DialMode.AutoDialingImmediately))
            {
                if (this.cTapi == null)
                    throw new InvalidOperationException("TAPI is not available");

                if (this.cLine.hLine == IntPtr.Zero)
                    throw new InvalidOperationException("No Line available");

                try
                {
                    isHangedUp = false;

                    cTapi.MakeCall(cLine, this.phoneNumber, out hCall_PlaceHolder);
                }

                catch (ObjectDisposedException exSafeHandle)
                {
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (hCall_PlaceHolder != IntPtr.Zero) //und connected!
                    {
                        OnConnected(new DialingEventArgs(this.phoneNumber, this.call, this.state));
                    }
                }

            }
            else
            {
                //virtueller Modus
                this.state = DialStates.DialTone;
                OnWantConnect(new DialingEventArgs(this.phoneNumber, this.call, this.state));

                this.state = DialStates.Connected;
                OnConnected(new DialingEventArgs(this.phoneNumber, this.call, this.state));

                isHangedUp = false;
            }
        }


        
        private DialStates CheckCallState()
        {

            return this.state;
        }

        
        public DialStates XPhoneState()
        {
            return CheckCallState();
        }

        public void HangUp()
        {
            this.state = DialStates.Ready;

            if (!IsVirtualMode)
            {
                if (hCall_PlaceHolder != IntPtr.Zero)
                {
                    cTapi.HangUp(hCall_PlaceHolder);
                    hCall_PlaceHolder = IntPtr.Zero;
                }

                if (!isHangedUp)
                {
                    isHangedUp = true;
                    OnHangedUp(new DialingEventArgs(this.phoneNumber, this.call, this.state));
                }
            }
            else
            {
                //virtueller Modus
                hCall_PlaceHolder = IntPtr.Zero;
                if (!isHangedUp)
                {
                    isHangedUp = true;
                    OnHangedUp(new DialingEventArgs(this.phoneNumber, this.call, this.state));
                }
            }
        }

        private void OpenLine()
        {

            //switch (Environment.UserName.ToUpper())
            //{
            //    case "STGTAGENTTT03":
            //        partOfLineName = "-16";
            //        break;
            //    case "STGTAGENTTT28":
            //        partOfLineName = "-445";
            //        break;
            //    case "ADMIN":
            //        partOfLineName = "-28";
            //        break;
            //    default:
            //        partOfLineName = Environment.UserName;
            //        break;
            //}
            
            string partOfLineName = this.metacallBusiness.Users.DomainUser_GetLine(Environment.UserName);
            if (partOfLineName != null)
            {
                foreach (CLine line in cTapi.LinesHt.Values)
                {

                    if (line.LineName != null && line.LineName.IndexOf(partOfLineName) > -1)
                    {

                        CTapi.LineErrReturn ret = line.FillAddressCaps();
                        if (ret != CTapi.LineErrReturn.LINEERR_OK)
                        {
                            throw new Exception();
                        }

                        if ((int)line.hLine == 0)
                        {
                            ret = line.LineOpen(line.DeviceID,
                                                 CTapi.LineCallPrivilege.LINECALLPRIVILEGE_MONITOR |
                                                 CTapi.LineCallPrivilege.LINECALLPRIVILEGE_OWNER);

                            if (ret != CTapi.LineErrReturn.LINEERR_OK)
                            {
                                throw new Exception();
                            }
                            cLine = line;
                            lineIsOpen = true;
                            return;
                        }
                    }
                }
            }
            else
            {
                lineIsOpen = false;
                return;
            }
        }

        public DialStates State
        {
            get { return this.state;}
        }

        
        protected void OnWantConnect(DialingEventArgs e)
        {
            if (WantConnect != null)
                WantConnect(this, e);
        }

        
        protected void OnConnected(DialingEventArgs e)
        {
            if (Connected != null)
                Connected(this, e);
        }

        
        protected void OnHangedUp(DialingEventArgs e)
        {
            if (HangedUp != null)
                HangedUp(this, e);
        }

        
        public void ShutDown()
        {
            if (!IsVirtualMode)
            {
                lineIsOpen = false;
                if (cTapi != null)
                {
                    cTapi.CallStateEvent -= new CTapi.CallStateEventHandler(cTapi_CallStateChanged);
                    cTapi.ShutDown();
                }
            }
        }

        ~DialerBusiness()
        {
            Dispose();
        }

        #region IDisposable Member
        private bool isDisposed;
        private bool disposing;

        /// <summary>
        /// Gibt alle von der Komponente verwendeten Resourcen frei
        /// </summary>
        
        public void Dispose()
        {
            if (disposing ||
                isDisposed)
                return;

            disposing = true;

            try
            {
                ShutDown();
            }
            catch (Exception){}

            disposing = false;
            isDisposed = true;
        }

        #endregion
    }

    /// <summary>
    /// Gibt den Status der DialerBusiness-Komponente an
    /// </summary>
    public enum DialStates
    {
        /// <summary>
        /// Der Dialer befindet sich im Status Ready und kann Anrufe durchführen
        /// </summary>
        Ready,
        /// <summary>
        /// Der Dialer befindet sich im Status DialTone. Ein Anruf wurde eingeleitet
        /// es wurde jedoch noch kein RingBack erhalten
        /// </summary>
        DialTone,
        /// <summary>
        /// Der Dialer befindet sich im Status RingBack. Ein Anruf wurde eingeleitet
        /// und die Gegenstelle bekommt das Klingelzeichen zu hören
        /// </summary>
        RingBack,
        /// <summary>
        /// Der Dialer bedindet sich im Status Connected. Der Anrufer hat den 
        /// Anruf entgegengenommen.
        /// </summary>
        Connected,
        /// <summary>
        /// Der Dialer befindet sich im Status Busy. Der Anruf wurde unterbrochen.
        /// </summary>
        Busy,
    }

    public delegate void DialingEventHandler(object sender, DialingEventArgs e);
        
    public class DialingEventArgs : EventArgs
    {
        public DialingEventArgs(string phoneNumber, Call call, DialStates state )
        {
            this.phoneNumber = phoneNumber;
            this.call = call;
            this.state = state;
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get { return this.phoneNumber; }
        }

        private Call call;
        public Call Call
        {
            get { return this.call; }
        }

        private DialStates state;
        public DialStates State
        {
            get { return this.state; }
        }
	
    }

    
    /// <summary>
    /// Die Ausnahme, die ausgelöst wird, wenn eine Operation auf dem Dialer durchgeführt
    /// werden soll, die eine freie Leitung vorraussetzt, der Dialer aber keine freie Leitung hat.
    /// </summary>
    [global::System.Serializable]
    public class LineNotFreeException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public LineNotFreeException() { }
        public LineNotFreeException(string message) : base(message) { }
        public LineNotFreeException(string message, Exception inner) : base(message, inner) { }
        protected LineNotFreeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    
    /// <summary>
    /// Die Ausnahme, die ausgelöst wird, wenn eine Operation auf dem Dialer durchgeführt 
    /// werden soll, die eine vebundene Leitung vorraussetzt, der Dialer aber nicht verbunden ist.
    /// </summary>
    [global::System.Serializable]
    public class LineNotConnectedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public LineNotConnectedException() { }
        public LineNotConnectedException(string message) : base(message) { }
        public LineNotConnectedException(string message, Exception inner) : base(message, inner) { }
        protected LineNotConnectedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
