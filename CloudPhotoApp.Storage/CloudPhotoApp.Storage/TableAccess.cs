using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPhotoApp.Storage.Entities;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace CloudPhotoApp.Storage
{
    public class TableAccess
    {
        private CloudTable _table;

        public TableAccess(string name)
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["PhotoAppStorage"].ConnectionString;
            //var connectionString = RoleEnvironment.GetConfigurationSettingValue("PhotoAppStorage");
            //var account = CloudStorageAccount.Parse(connectionString);

            //var account = CloudStorageAccount.DevelopmentStorageAccount;

            var cred = new StorageCredentials("jholm",
                "/bVipQ2JxjWwYrZQfHmzhaBx1p1s8BoD/wX6VWOmg4/gpVo/aALrjsDUKqzXsFtc9utepPqe65NposrXt9YsyA==");
            var account = new CloudStorageAccount(cred, true);

            CloudTableClient tableClient = account.CreateCloudTableClient();
            _table = tableClient.GetTableReference(name);
            _table.CreateIfNotExists();
        }

        public void Add(TableEntity entity)
        {
            TableOperation opp = TableOperation.Insert(entity);
            _table.Execute(opp);
        }

        public List<Photo> GetAllPhotos()
        {
            string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Images");
            TableQuery<Photo> query = new TableQuery<Photo>().Where(pkFilter);
            IEnumerable<Photo> images = _table.ExecuteQuery(query);
            return images.ToList();
        }
        public List<Document> GetAllDocuments()
        {
            string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Documents");
            TableQuery<Document> query = new TableQuery<Document>().Where(pkFilter);
            IEnumerable<Document> documents = _table.ExecuteQuery(query);
            return documents.ToList();
        }
        public List<Video> GetAllVideos()
        {
            string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Videos");
            TableQuery<Video> query = new TableQuery<Video>().Where(pkFilter);
            IEnumerable<Video> videos = _table.ExecuteQuery(query);
            return videos.ToList();
        }
        public List<Audio> GetAllAudios()
        {
            string pkFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Audios");
            TableQuery<Audio> query = new TableQuery<Audio>().Where(pkFilter);
            IEnumerable<Audio> audios = _table.ExecuteQuery(query);
            return audios.ToList();
        }
        public ITableEntity GetPhotoEntity(string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<Photo>("Images", rowKey);
            TableEntity image = (TableEntity)_table.Execute(retrieve).Result;
            return image;
        }
        public ITableEntity GetDocumentEntity(string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<Document>("Documents", rowKey);
            TableEntity document = (TableEntity)_table.Execute(retrieve).Result;
            return document;
        }
        public ITableEntity GetVideoEntity(string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<Video>("Videos", rowKey);
            TableEntity video = (TableEntity)_table.Execute(retrieve).Result;
            return video;
        }
        public ITableEntity GetAudioEntity(string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<Audio>("Audios", rowKey);
            TableEntity audio = (TableEntity)_table.Execute(retrieve).Result;
            return audio;
        }

        public void Delete(string rowKey, string tableName)
        {
            TableAccess tableAccess = new TableAccess(tableName);
            ITableEntity tableEntity = new TableEntity();

            if (tableName == "photoinformation")
            {
                tableEntity = tableAccess.GetPhotoEntity(rowKey);

                // Ta bort från container
                var rawContainer = new BlobContainer("rawimages");
                rawContainer.DeleteBlob(rowKey);

                var thuContainer = new BlobContainer("thumbimages");
                thuContainer.DeleteBlob(rowKey);
            }
            if (tableName == "documentinformation")
            {
                tableEntity = tableAccess.GetDocumentEntity(rowKey);
                var container = new BlobContainer("documents");
                container.DeleteBlob(rowKey);
            }
            if (tableName == "videoinformation")
            {
                tableEntity = tableAccess.GetVideoEntity(rowKey);
                var container = new BlobContainer("videos");
                container.DeleteBlob(rowKey);
            }
            if (tableName == "audioinformation")
            {
                tableEntity = tableAccess.GetAudioEntity(rowKey);
                var container = new BlobContainer("audios");
                container.DeleteBlob(rowKey);
            }
            // Ta bort från tabellen
            tableEntity.ETag = "*";
            TableOperation opp = TableOperation.Delete(tableEntity);
            _table.Execute(opp);
        }

        public void Update(Photo photo)
        {
            TableOperation opp = TableOperation.Replace(photo);
            _table.Execute(opp);
        }

        public Photo Get(string rowKey)
        {
            TableOperation retrieve = TableOperation.Retrieve<Photo>("Images", rowKey);
            Photo image = (Photo)_table.Execute(retrieve).Result;
            return image;
        }

    }
}
