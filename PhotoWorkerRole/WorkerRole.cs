using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using CloudPhotoApp.Storage;
using CloudPhotoApp.Storage.Entities;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using System.Drawing;

namespace PhotoWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.TraceInformation("PhotoWorkerRole entry point called", "Information");
            var jobQueue = new JobQueue();
            
            while (true)
            {
                CloudQueueMessage jobMessage = jobQueue.Get();

                if (jobMessage != null)
                {
                    // Om det finns ett message
                    string jobName = jobMessage.AsString;
                    Trace.TraceInformation("Found job: " + jobName, "Information");

                    try
                    {
                        // Create a new / reference to a image blob
                        BlobContainer rawContainer = new BlobContainer("rawimages");
                        BlobContainer thumbContainer = new BlobContainer("thumbimages");

                        // Hämta bloben från rawimages containern
                        CloudBlockBlob rawBlob = rawContainer.GetBlob(jobName);
                        Trace.TraceInformation("Image to process: " + rawBlob.Name, "Information");

                        // Get the image information from table
                        TableAccess tableAccess = new TableAccess("photoinformation");
                        Photo photo = tableAccess.Get(jobName);

                        // Create a new image processor for the image
                        ImageProcessor imgProc = new ImageProcessor(photo.RawUri, photo.Title);

                        // Skapa en thumbnail
                        Trace.WriteLine("Processing job " + jobName);
                        Image thumb = imgProc.CreateTile(new Rectangle(0, 0, imgProc.Width, imgProc.Height));

                        // Encoder parameter for image quality
                        EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

                        // Set the Jpeg image codec
                        //ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
                        ImageCodecInfo jpegCodec = GetEncoderInfo(photo.ContentType);

                        EncoderParameters encoderParams = new EncoderParameters(1);
                        encoderParams.Param[0] = qualityParam;

                        // Upload the image
                        MemoryStream s = new MemoryStream();
                        thumb.Save(s, jpegCodec, encoderParams);
                        byte[] buffer = new byte[s.Length];
                        s.Read(buffer, 0, (int)s.Length);
                        s.Position = 0;

                        // Lägg till thumben i thumbcontainer
                        Trace.WriteLine("Thumbnail to add: " + photo.RowKey);
                        var thumbUri = thumbContainer.UploadBlob(s, photo.RowKey, photo.ContentType);
                        //var thumbUri = thumbContainer.AddThumbnailToContainer(photo.RowKey, thumb, rawBlob);

                        // Update the information of photo in the table
                        photo.ThumbUri = thumbUri;
                        tableAccess.Update(photo);

                        // Delete the message
                        jobQueue.Delete(jobMessage);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Error processing job " + jobName + ": " +
                                        ex.Message, "Error");
                    }
                }
                else
                {
                    Trace.TraceInformation("Polling for job...", "Information");
                    Thread.Sleep(1000);
                }
            }
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

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            return base.OnStart();
        }
    }
}
