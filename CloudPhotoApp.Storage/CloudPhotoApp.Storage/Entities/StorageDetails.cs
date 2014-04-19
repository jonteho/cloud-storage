namespace CloudPhotoApp.Storage.Entities
{
    public class StorageDetails
    {
        public StorageType StorageType { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public int TotalEntrys { get; set; }
    }

    public enum StorageType
    {
        Table,
        Blob
    }
}
