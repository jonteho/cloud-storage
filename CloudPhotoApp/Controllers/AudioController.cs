﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CloudPhotoApp.Storage;
using PagedList;

namespace CloudPhotoApp.Controllers
{
    public class AudioController : Controller
    {
        //
        // GET: /Audio/
        string tableName = "audioinformation";
        public ActionResult Index(int? page)
        {
            if (ModelState.IsValid)
            {
                TableAccess tableAccess = new TableAccess(tableName);
                var audios = tableAccess.GetAllAudios().OrderByDescending(x => x.DateAdded);
                var pageSize = 12;
                var pageNumber = (page ?? 1);
                return View(audios.ToPagedList(pageNumber, pageSize));
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