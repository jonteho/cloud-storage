using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace CloudPhotoApp.Storage.Entities
{
    public class Audio : TableEntity
    {
        public Audio()
        {
            PartitionKey = "Audios";
            RowKey = string.Format("{0:10}_{1}", DateTime.MaxValue.Ticks - DateTime.Now.Ticks, Guid.NewGuid());
        }
        public string Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Title { get; set; }
        public string DateAdded { get; set; }
        public string UploadedBy { get; set; }
        public string RawUri { get; set; }
    }
}
