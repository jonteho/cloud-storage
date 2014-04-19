using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;

namespace CloudPhotoApp.Storage
{
    public class StorageInformation
    {
        private CloudBlobClient _blobClient;
        private CloudTableClient _tableClient;
        public StorageInformation()
        {
            //var account = CloudStorageAccount.DevelopmentStorageAccount;
            var cred = new StorageCredentials("jholm",
                "/bVipQ2JxjWwYrZQfHmzhaBx1p1s8BoD/wX6VWOmg4/gpVo/aALrjsDUKqzXsFtc9utepPqe65NposrXt9YsyA==");
            var account = new CloudStorageAccount(cred, true);

            _blobClient = account.CreateCloudBlobClient();
            _tableClient = account.CreateCloudTableClient();
        }

        public IEnumerable<CloudBlobContainer> GetContainers()
        {
            return _blobClient.ListContainers();
        }
        public IEnumerable<CloudTable> GetTables()
        {
            return _tableClient.ListTables();
        }
    }
}
