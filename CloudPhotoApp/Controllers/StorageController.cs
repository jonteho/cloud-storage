using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudPhotoApp.Storage;
using CloudPhotoApp.Storage.Entities;
using Microsoft.WindowsAzure.Storage.Blob;
using CloudPhotoApp.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace CloudPhotoApp.Controllers
{
    public class StorageController : Controller
    {
        //
        // GET: /Storage/
        public ActionResult Index()
        {
            decimal totalSize = 0m;
            var listInfo = new List<StorageDetails>();
            var storageInfo = new StorageInformation();
            var containers = storageInfo.GetContainers();
            //var tables = storageInfo.GetTables();

            //foreach (CloudTable table in tables)
            //{
            //    var tableInfo = new StorageDetails()
            //    {
            //        StorageType = StorageType.Table,
            //        Name = table.Name
            //    };
            //    listInfo.Add(tableInfo);
            //}

            foreach (CloudBlobContainer container in containers)
            {
                var c = new BlobContainer(container.Name);
                var size = Math.Round(c.GetBlobContainerSize());
                totalSize += size/1048576;
                var containerInfo = new StorageDetails()
                {
                    StorageType = StorageType.Blob,
                    Name = container.Name,
                    Size = (size / 1048576).ToString("#.##") + " Mb",
                    TotalEntrys = c.GetBlobCountInContainer()
                };
                listInfo.Add(containerInfo);
            }
            ViewData["TotalSize"] = totalSize.ToString("#.##") + " Mb";
            return View(listInfo);
        }
	}
}