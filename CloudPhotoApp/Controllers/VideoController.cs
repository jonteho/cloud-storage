using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudPhotoApp.Storage;
using PagedList;

namespace CloudPhotoApp.Controllers
{
    public class VideoController : Controller
    {
        //
        // GET: /Video/
        string tableName = "videoinformation";
        public ActionResult Index(int? page)
        {
            if (ModelState.IsValid)
            {
                TableAccess tableAccess = new TableAccess(tableName);
                var videos = tableAccess.GetAllVideos().OrderByDescending(x => x.DateAdded);
                var pageSize = 12;
                var pageNumber = (page ?? 1);
                return View(videos.ToPagedList(pageNumber, pageSize));
            }
            return View();
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