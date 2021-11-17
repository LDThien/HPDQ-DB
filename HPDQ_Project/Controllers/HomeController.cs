using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HPDQ_Project.Models;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace HPDQ_Project.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Index1()
        {
            return View();
        }
        public class NVList
        {
            public string name { get; set; }
            public int[] data { get; set; }
        }
        public ActionResult GetNV()
        {
            //var query = db.NhanViens.Include("PhongBan")
            //        .Where(x => x.TrangThai == 1)
            //       .GroupBy(p => p.PhongBan.TenPB)
            //       .Select(g => new { name = g.Key, count = g.Count() }).ToList();
            //return Json(query, JsonRequestBehavior.AllowGet);


            List<string> a = new List<string>();
            var pb = db.PhongBans.ToList();
            foreach (var item in pb)
            {
                a.Add(item.TenPB);
            }
            List<NVList> obj = new List<NVList>();
            NVList temp = new NVList();
            temp.name = "BP/NM";
            temp.data = CreateArray();
            obj.Add(temp);
            return Json(new { categories = a, series = obj }, JsonRequestBehavior.AllowGet);
        }
        private int[] CreateArray()
        {
            var pb = db.PhongBans.ToList();
            int[] m = new int[pb.Count];
            for (int i = 0; i < pb.Count; i++)
            {
                m[i] = CountSummary(pb[i].MaPB);
            }
            return m;
        }
        private int CountSummary(int MaPB)
        {
            return db.NhanViens.Where(x => x.MaPB == MaPB && x.TrangThai == 1).Count();
        }

        public JsonResult GetTK()
        {
            List<string> a = new List<string>();
            List<NVList> obj = new List<NVList>();
            NVList temp = null;
            var pb = db.PhongBans.ToList();
            foreach (var item in pb)
            {
                a.Add(item.TenPB);
                
            }
            var tk = db.PhanMems.ToList();
            foreach (var item in tk)
            {
                temp = new NVList();
                temp.name = item.TenPM;
                temp.data = CreateArrayTK(item.MaPM);
                obj.Add(temp);
                temp = null;
            }
            temp = new NVList();
            temp.name = "Danh Bạ";
            temp.data = CreateArrayTKDanhBa();

            obj.Add(temp);
            temp = null;
            temp = new NVList();
            temp.name = "Email";
            temp.data = CreateArrayTKEmail();
            obj.Add(temp);
            return Json(new { categories = a, series = obj }, JsonRequestBehavior.AllowGet);
        }
        private int Countsoft(int MaPB, int MaPM)
        {
            var model = (from a in db.TaiKhoans.Where(x => x.MaPM == MaPM)
                         join b in db.NhanViens on a.MaNV equals b.MaNV
                         join c in db.PhongBans.Where(x => x.MaPB == MaPB) on b.MaPB equals c.MaPB
                         select new DanhBaValidation{ });
            return model.Count();
        }
        private int[] CreateArrayTK(int MaPM)
        {
            var pb = db.PhongBans.ToList();
            int[] m = new int[pb.Count];
            for (int i = 0; i < pb.Count; i++)
            {
                m[i] = Countsoft(pb[i].MaPB, MaPM);
            }
            return m;
        }
        private int[] CreateArrayTKDanhBa()
        {
            var pb = db.PhongBans.ToList();
            int[] m = new int[pb.Count];
            for (int i = 0; i < pb.Count; i++)
            {
                m[i] = CountPhone(pb[i].MaPB);
            }
            return m;
        }
        private int CountPhone(int MaPB)
        {
            var model = (from a in db.DanhBas
                         join b in db.NhanViens on a.MaNV equals b.MaNV
                         join c in db.PhongBans.Where(x => x.MaPB == MaPB) on b.MaPB equals c.MaPB
                         select new DanhBaValidation { });
            return model.Count();
        }
        private int[] CreateArrayTKEmail()
        {
            var pb = db.PhongBans.ToList();
            int[] m = new int[pb.Count];
            for (int i = 0; i < pb.Count; i++)
            {
                m[i] = CountEmail(pb[i].MaPB);
            }
            return m;
        }
        private int CountEmail(int MaPB)
        {
            var model = (
                         from a in db.NhanViens.Where(x => x.Email != null && x.Email != "" && x.TrangThai == 1)
                         join b in db.PhongBans.Where(x => x.MaPB == MaPB) on a.MaPB equals b.MaPB
                         select new DanhBaValidation { });
            return model.Count();
        }

    }
}