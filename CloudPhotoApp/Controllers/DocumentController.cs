using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudPhotoApp.Storage;
using PagedList;

namespace CloudPhotoApp.Controllers
{
    public class DocumentController : Controller
    {
        //
        // GET: /Document/
        string tableName = "documentinformation";
        public ActionResult Index(int? page)
        {
            if (ModelState.IsValid)
            {
                TableAccess tableAccess = new TableAccess(tableName);
                var documents = tableAccess.GetAllDocuments().OrderByDescending(x => x.DateAdded);
                var pageSize = 15;
                var pageNumber = (page ?? 1);
                return View(documents.ToPagedList(pageNumber, pageSize));
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