using HPDQ_Project.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        HPDQEntities db = new HPDQEntities();


        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetList()
        {
            
            List<PhongBanValidation> DKList = db.PhongBans.Select(x => new PhongBanValidation
            {
                MaPB = x.MaPB,
                TenPB = x.TenPB

            }).ToList();

            return Json(DKList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetById(int PBID)
        {
            PhongBan model = db.PhongBans.Where(x => x.MaPB == PBID).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        public JsonResult SaveDataInDatabase(PhongBanValidation model)
        {
            var result = false;
            try
            {
                if (model.MaPB != null)
                {

                    PhongBan ct = db.PhongBans.SingleOrDefault(x => x.MaPB == model.MaPB);
                    ct.TenPB = model.TenPB;
                    db.SaveChanges();
                    result = true;
                }
                else
                {
                    PhongBan ct = new PhongBan();
                    ct.TenPB = model.TenPB;
                    db.PhongBans.Add(ct);
                    db.SaveChanges();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteRecord(int PBID)
        {
            bool result = false;
            PhongBan ct = db.PhongBans.SingleOrDefault(x => x.MaPB == PBID);
            if (ct != null)
            {
                db.PhongBans.Remove(ct);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}