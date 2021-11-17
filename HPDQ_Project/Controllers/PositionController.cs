using HPDQ_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HPDQ_Project.Controllers
{
    public class PositionController : Controller
    {
        // GET: Position
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index(int? page)
        {
            var model = db.ChucVus.ToList();
            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(model.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(ChucVuValidation _DO)
        {
            db.Chucvu_insert(_DO.TenCV);
            return RedirectToAction("Index","Position");
        }
        public ActionResult Edit(int ID)
        {
            var model = db.ChucVus.Where(x => x.MaCV == ID).SingleOrDefault();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(ChucVu _DO)
        {
            db.Chucvu_update(_DO.MaCV, _DO.TenCV);
            return RedirectToAction("Index", "Position");
        }
        public ActionResult Delete(int ID)
        {
            db.Chucvu_delete(ID);
            return RedirectToAction("Index", "Position");
        }
    }
}