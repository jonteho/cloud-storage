using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using CloudPhotoApp.Storage;
using CloudPhotoApp.Storage.Entities;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using PagedList;

namespace CloudPhotoApp.Controllers
{
    public class HomeController : Controller
    {
        private string dev_uploadRedirect = "https://localhost:44300/Home/Upload";
        private string rel_uploadRedirect = "http://jholm.azurewebsites.net/Home/Upload";
        string tableName = "photoinformation";

        public ActionResult Index(int? page)
        {
            if (ModelState.IsValid)
            {
                TableAccess tableAccess = new TableAccess("photoinformation");
                var photos = tableAccess.GetAllPhotos().OrderByDescending(x => x.DateAdded);
                var pageSize = 8;
                var pageNumber = (page ?? 1);
                return View(photos.ToPagedList(pageNumber, pageSize));
            }
            return View();
        }

        //[Authorize] funkar inte när deployat ?
        public ActionResult Upload()
        {
            if (!Request.IsAuthenticated)
            {
                var signInRequest = FederatedAuthentication.WSFederationAuthenticationModule.CreateSignInRequest("", rel_uploadRedirect,
                    false);
                return Redirect(signInRequest.RequestUrl);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Upload(string title, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var mimeTypeExtension = new MimeTypeExtension();
                    MimeType mimeType = mimeTypeExtension.GetType(file.ContentType);

                    if (mimeType == MimeType.Photo)
                    {
                        var newPhoto = new Photo()
                        {
                            Id = file.FileName + Guid.NewGuid().ToString(),
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            Title = title,
                            DateAdded = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                            UploadedBy = User.Identity.Name
                        };

                        // Lätt till ett jobb för worker rolen som ska skapa en thumbnail
                        JobQueue jobQueue = new JobQueue();
                        jobQueue.AddJob(newPhoto.RowKey);

                        // Ladda upp till blob
                        var blobContainer = new BlobContainer("rawimages");
                        newPhoto.RawUri = blobContainer.UploadBlob(file.InputStream, newPhoto.RowKey, file.ContentType);

                        // Lägg till i tabellen
                        TableAccess tableAccess = new TableAccess("photoinformation");
                        tableAccess.Add(newPhoto);

                        ViewData["Success"] = "Your photo were uploaded successfully!";
                        return RedirectToAction("Index");
                    }
                    if (mimeType == MimeType.Document)
                    {
                        var newDocument = new Document()
                        {
                            Id = file.FileName + Guid.NewGuid().ToString(),
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            Title = title,
                            DateAdded = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                            ContentLength = file.ContentLength / 1024 + "kb",
                            UploadedBy = User.Identity.Name
                        };

                        // Ladda upp till blob
                        var blobContainer = new BlobContainer("documents");
                        newDocument.RawUri = blobContainer.UploadBlob(file.InputStream, newDocument.RowKey, file.ContentType);

                        // Lägg till i tabellen
                        TableAccess tableAccess = new TableAccess("documentinformation");
                        tableAccess.Add(newDocument);

                        return RedirectToAction("Index", "Document");
                    }
                    if (mimeType == MimeType.Video)
                    {
                        var newVideo = new Video()
                        {
                            Id = file.FileName + Guid.NewGuid().ToString(),
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            Title = title,
                            DateAdded = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                            UploadedBy = User.Identity.Name
                        };

                        // Ladda upp till blob
                        var blobContainer = new BlobContainer("videos");
                        newVideo.RawUri = blobContainer.UploadBlob(file.InputStream, newVideo.RowKey, file.ContentType);

                        // Lägg till i tabellen
                        TableAccess tableAccess = new TableAccess("videoinformation");
                        tableAccess.Add(newVideo);

                        return RedirectToAction("Index", "Video");
                    }
                    if (mimeType == MimeType.Audio)
                    {
                        var newAudio = new Audio()
                        {
                            Id = file.FileName + Guid.NewGuid().ToString(),
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            Title = title,
                            DateAdded = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),
                            UploadedBy = User.Identity.Name
                        };

                        // Ladda upp till blob
                        var blobContainer = new BlobContainer("audios");
                        newAudio.RawUri = blobContainer.UploadBlob(file.InputStream, newAudio.RowKey, file.ContentType);

                        // Lägg till i tabellen
                        TableAccess tableAccess = new TableAccess("audioinformation");
                        tableAccess.Add(newAudio);

                        return RedirectToAction("Index", "Audio");
                    }
                    if (mimeType == MimeType.NotAccepted)
                        ViewData["Error"] = "Unsupported content, try another! (maybe i´ve missed to add it as supported)";

                }
                catch (Exception ex)
                {
                    ViewData["Error"] = "Error: " + ex.Message;
                }
            }

            return View("Upload");
        }

        [HttpPost]
        public ActionResult Delete(string rowKey)
        {
            TableAccess tableAccess = new TableAccess(tableName);
            tableAccess.Delete(rowKey, tableName);
            return RedirectToAction("Index");
        }
    }
}