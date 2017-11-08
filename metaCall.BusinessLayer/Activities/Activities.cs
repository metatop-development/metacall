using metatop.Applications.metaCall.BusinessLayer;
using MaDaNet.Common.AppFrameWork.Activities;

using metatop.Applications.metaCall.DataObjects;

namespace metatop.Applications.metaCall.BusinessLayer.Activities
{

    public class ProjectLogOn : ActivityBase
    {
        public readonly ProjectInfo project;

        public ProjectLogOn() { }

        public ProjectLogOn(ProjectInfo project)  :base()
        {
            this.project = project;
        }
    }

    public class ProjectLogOff : ActivityBase
    {
    }

    public class NewCustomer : ActivityBase
    {
        
        public NewCustomer(Call call)
        {
            this.call = call;
        }

        private Call call;
        public Call Call
        {
            get { return call; }
        }
    }

    public class Dial : ActivityBase
    {
        public Dial() { }

        public Dial(Call call)  :base()
        {
            this.call = call;
        }

        private Call call;
        public Call Call
        {
            get { return call; }
        }
    }

    public class DialConnected : ActivityBase
    {
        public DialConnected() { }

        public DialConnected(Call call)
            : base()
        {
            this.call = call;
        }

        private Call call;
        public Call Call
        {
            get { return call; }
        }
    }

    public class HangUp : ActivityBase
    {
        public HangUp() { }
    }

    public class AgentNotice : ActivityBase
    {
        public AgentNotice() { }
    }

    public class SaveCustomer : ActivityBase
    {
        public SaveCustomer() { }
    }

    public class CancelCustomer : ActivityBase
    {
        public CancelCustomer() { }

    }

    public class StartPause : ActivityBase
    {
    }

    public class StopPause : ActivityBase
    {
    }

    public class StartTraining : ActivityBase
    {
    }

    public class StopTraining : ActivityBase
    {
        private string trainingGrundItem;
        private string trainingNotice;

        public StopTraining(string trainingGrundItem)
        {
            this.trainingGrundItem = trainingGrundItem;
            this.trainingNotice = null;
        }

        public StopTraining(string trainingGrundItem, string trainingNotice)
        {
            this.trainingGrundItem = trainingGrundItem;
            this.trainingNotice = trainingNotice;
        }

        public string TrainingGrundItem
        {
            get { return this.trainingGrundItem; }
        }

        public string TrainingNotice
        {
            get { return this.trainingNotice; }
        }
    }

    public class DurringChanged : ActivityBase
    {
        public DurringChanged(bool durringActive)
        {
            this.durringActive = durringActive;
        }
        
        private bool durringActive;
        public bool DurringActive
        {
            get { return durringActive; }
        }
	
    }
}