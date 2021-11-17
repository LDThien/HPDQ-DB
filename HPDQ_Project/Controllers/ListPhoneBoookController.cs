using HPDQ_Project.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Controllers
{
    public class ListPhoneBoookController : Controller
    {
        // GET: ListPhoneBoook
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index(int? page, int? MaPB)
        {
            List<PhongBan> pb = db.PhongBans.ToList();
            if (MaPB == null) MaPB = 0;
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB", MaPB);
            var model = from a in db.thongtin_danhba_DC(MaPB)
                        select new DanhBaValidation()
                        {
                            IDNV = a.IDNV,
                            MaNV = a.MaNV,
                            TenPB = a.TenPB,
                            TenCV = a.Vitri,
                            TenNV = a.TenNV,
                            SoMayLe = a.SoMayLe,
                            TrangThai = a.TrangThai
                        };
            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(model.OrderBy(x => x.MaNV).ToList().ToPagedList(pageNumber, pageSize));

        }
        public ActionResult Create()
        {
           
            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            var rs = (from u in db.NhanViens
                      where u.MaNV.Contains("DANHBA")
                      orderby u.MaNV descending
                      select u).FirstOrDefault();
            if (rs == null)
            {
                ViewBag.MaNV = "DANHBA001";
            }
            else
            {
                ViewBag.MaNV = NextID(rs.MaNV, "DANHBA");
            }

            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(DanhBaValidation _DO)
        {
            int MaCV = 1173;
            int MaGT = 6;
            db.Nhanvien_insert_danhba(_DO.MaNV, _DO.TenNV, MaCV, _DO.MaPB, MaGT);
            db.Nhanvien_update_danhba(_DO.MaNV, _DO.SoMayLe, _DO.ViTri, 2, 0);
            TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            return RedirectToAction("Index", "ListPhoneBoook", new { id = _DO.MaPB });
        }
        [HttpPost]
        public JsonResult IsAlready(string MaNV)
        {
            return Json(IsAvailable(MaNV.Replace(" ", "")));
        }
        public bool IsAvailable(string MaNV)
        {
            var Regsbb = (from u in db.NhanViens
                          where u.MaNV.ToLower() == MaNV.ToLower()
                          select new { u.MaNV }).FirstOrDefault();
            bool status;
            if (Regsbb != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }
            return status;
        }
        public string NextID(string lastID, string prefixID)
        {
            if (lastID == "")
            {
                return prefixID + "001";  // fixwidth default
            }
            int nextID = int.Parse(lastID.Remove(0, prefixID.Length)) + 1;
            int lengthNumerID = lastID.Length - prefixID.Length;
            string zeroNumber = "";
            for (int i = 1; i <= lengthNumerID; i++)
            {
                if (nextID < Math.Pow(10, i))
                {
                    for (int j = 1; j <= lengthNumerID - i; i++)
                    {
                        zeroNumber += "0";
                    }
                    return prefixID + zeroNumber + nextID.ToString();
                }
            }
            return prefixID + nextID;

        }

        public ActionResult Delete(int ID)
        {
            var rs = db.NhanViens.Where(x => x.IDNV == ID && x.TrangThai == 2).SingleOrDefault();
            if (rs != null)
            {
                db.Nhanvien_update_danhba(rs.MaNV, 0, "", 0, 2);
                db.Nhanvien_delete(ID);
                TempData["msgError"] = "<script>alert('Xóa danh bạ thành công');</script>";
                return RedirectToAction("Index", "ListPhoneBoook", new { MaPB = rs.MaPB });
            }
            else
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi xóa danh bạ');</script>";
                return RedirectToAction("Index", "ListPhoneBoook");
            }

        }

        public ActionResult Edit(int ID)
        {
            var model = (from a in db.NhanViens.Where(x => x.IDNV == ID)
                         join b in db.DanhBas on a.MaNV equals b.MaNV
                         select new DanhBaValidation
                         {
                             IDNV = a.IDNV,
                             MaNV = a.MaNV,
                             TenNV = a.TenNV,
                             MaCV = a.MaCV,
                             MaPB = a.MaPB,
                             SoMayLe = b.SoMayLe,
                             ViTri=b.Vitri
                         }).SingleOrDefault();
            
            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult Edit(DanhBaValidation _DO)
        {
            var rsnv = db.NhanViens.Where(x => x.MaNV == _DO.MaNV).SingleOrDefault();
            if (rsnv != null)
            {
               
                db.Nhanvien_update_danhba(_DO.MaNV, _DO.SoMayLe, _DO.ViTri, 2, 1);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
                return RedirectToAction("Index", "ListPhoneBoook", new { MaPB = rsnv.MaPB });
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Có lỗi khi cập nhập danh bạ');</script>";
                return RedirectToAction("Index", "ListPhoneBoook", new { MaPB = _DO.MaPB });
            }

            //return RedirectToAction("Index", "PhoneBook", new { MaPB = _DO.MaPB });
        }

    }
}