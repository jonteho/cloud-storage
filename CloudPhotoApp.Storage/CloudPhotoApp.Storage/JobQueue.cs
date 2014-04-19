using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CloudPhotoApp.Storage
{
    public class JobQueue
    {
        private CloudQueue _cloudQueue;
        private string _queueName = "cloudphotoappqueue";

        public JobQueue()
        {
           // var connectionString = RoleEnvironment.GetConfigurationSettingValue("PhotoAppStorage");
            //var connectionString = ConfigurationManager.ConnectionStrings["PhotoAppStorage"].ConnectionString;
            //var account = CloudStorageAccount.Parse(connectionString);

            //var account = CloudStorageAccount.DevelopmentStorageAccount;

            var cred = new StorageCredentials("jholm",
            "/bVipQ2JxjWwYrZQfHmzhaBx1p1s8BoD/wX6VWOmg4/gpVo/aALrjsDUKqzXsFtc9utepPqe65NposrXt9YsyA==");
            var account = new CloudStorageAccount(cred, true);

            var cloudQueueClient = account.CreateCloudQueueClient();
            _cloudQueue = cloudQueueClient.GetQueueReference(_queueName);
            _cloudQueue.CreateIfNotExists();
        }

        public void AddJob(string jobName)
        {
            _cloudQueue.AddMessage(new CloudQueueMessage(jobName));
        }
        public CloudQueueMessage Get()
        {
            return _cloudQueue.GetMessage(TimeSpan.FromMinutes(30));
        }

        public void Delete(CloudQueueMessage message)
        {
            _cloudQueue.DeleteMessage(message);
        }
    }
}
