using System.Collections.Generic;

namespace CloudPhotoApp.WebApi.Controllers
{
    public class WorkerRoleLog
    {

        public static List<string> WorkLog { get; set; }

        public WorkerRoleLog()
        {
            WorkLog = new List<string>();
        }

        public void AddMsg(string msg)
        {
            WorkLog.Add(msg);
        }

        //private static WorkerRoleLog _instance;

        //private WorkerRoleLog()
        //{
        //}

        //public static WorkerRoleLog Instance
        //{
        //    get { return _instance ?? (_instance = new WorkerRoleLog()); }
        //}

        //public string TraceInformation { get; set; }

    }
}
