using HPDQ_Project.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Controllers
{
    public class SoftwareController : Controller
    {
        // GET: Software
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index(int? page)
        {
            var model = db.PhanMems.ToList();
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(PhanMem _DO)
        {
            db.Phanmem_insert(_DO.TenPM);
            return RedirectToAction("Index", "Software");
        }
        public ActionResult Edit(int ID)
        {
            var model = db.PhanMems.Where(x => x.MaPM == ID).SingleOrDefault();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(PhanMem _DO)
        {
            db.Phanmem_update(_DO.MaPM, _DO.TenPM);
            return RedirectToAction("Index", "Software");
        }
        public ActionResult Delete(int ID)
        {
            db.Phanmem_delete(ID);
            return RedirectToAction("Index", "Software");
        }
    }
}