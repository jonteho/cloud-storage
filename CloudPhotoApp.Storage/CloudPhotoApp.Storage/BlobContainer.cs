using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace CloudPhotoApp.Storage
{
    public class BlobContainer
    {
        private CloudBlobContainer m_BlobContainer;
        private CloudBlobClient _client;

        public BlobContainer(string name)
        {
            // hämta connectionsträngen från config // RoleEnviroment bestämmer settingvalue runtime
            //var connectionString = RoleEnvironment.GetConfigurationSettingValue("PhotoAppStorage");
            //var connectionString = CloudConfigurationManager.GetSetting("CloudStorageApp");
            // hämtar kontot utfrån connectionsträngens värde
            //var account = CloudStorageAccount.Parse(connectionString);

            //var account = CloudStorageAccount.DevelopmentStorageAccount;

            var cred = new StorageCredentials("jholm",
                "/bVipQ2JxjWwYrZQfHmzhaBx1p1s8BoD/wX6VWOmg4/gpVo/aALrjsDUKqzXsFtc9utepPqe65NposrXt9YsyA==");
            var account = new CloudStorageAccount(cred, true);

            // skapar en blobclient
            _client = account.CreateCloudBlobClient();

            m_BlobContainer = _client.GetContainerReference(name);

            // Om det inte finns någon container med det namnet
            if (!m_BlobContainer.Exists())
            {
                // Skapa containern
                m_BlobContainer.Create();
                var permissions = new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                // Sätter public access till blobs
                m_BlobContainer.SetPermissions(permissions);
            }
        }

        public string AddThumbnailToContainer(string blobName, Image thumbnail, CloudBlockBlob blobRaw)
        {
            var account = CloudStorageAccount.DevelopmentStorageAccount;

            // skapar en blobclient
            var blobClient = account.CreateCloudBlobClient();

            // Retrieve reference to a previously created container
            CloudBlobContainer container = blobClient.GetContainerReference("thumbimages");

            // Add thumbnail to container
            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);
            blob.Properties.ContentType = blobRaw.Properties.ContentType;
            blob.Properties.ContentEncoding = blobRaw.Properties.ContentEncoding;

            // Encoder parameter for image quality
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

            MemoryStream s = new MemoryStream();
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            // Upload the image
            thumbnail.Save(s, jpegCodec, encoderParams);
            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, (int)s.Length);
            s.Position = 0;

            using (s)
            {
                blob.UploadFromStream(s);
            }

            return blob.Uri.AbsoluteUri;
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }

        public CloudBlockBlob GetBlob(string blobName)
        {
            // Retrieve reference to a blob
            CloudBlockBlob blob = m_BlobContainer.GetBlockBlobReference(blobName);
            return blob;
        }

        public decimal GetBlobContainerSize()
        {
            var list = m_BlobContainer.ListBlobs();
            decimal size = list.Cast<CloudBlockBlob>().Sum(blob => blob.Properties.Length);
            return size;
        }

        public int GetBlobCountInContainer()
        {
            return m_BlobContainer.ListBlobs().Count();
        }

        public string UploadBlob(Stream fileStream, string blobName, string contentType)
        {
            CloudBlockBlob blob = m_BlobContainer.GetBlockBlobReference(blobName);

            // Set the content type to image and
            blob.Properties.ContentType = contentType;

            // Upload the blob
            using (fileStream)
            {
                blob.UploadFromStream(fileStream);
            }

            return blob.Uri.AbsoluteUri;
        }

        public void DeleteBlob(string blobName)
        {
            // Get the blob and delete it..
            m_BlobContainer.GetBlockBlobReference(blobName).DeleteIfExists();
        }

        public string GetRootUri()
        {
            return m_BlobContainer.Uri.AbsoluteUri;
        }

    }
}
