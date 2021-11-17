using HPDQ_Project.Common;
using HPDQ_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace HPDQ_Project.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index()
        {
            List<PhongBan> MaPB = db.PhongBans.ToList();
            ViewBag.MaPB = new SelectList(MaPB, "MaPB", "TenPB");

            List<QuyenDangNhap> quyen = db.QuyenDangNhaps.ToList();
            ViewBag.QuyenList = new SelectList(quyen, "MaQuyen", "TenQuyen");
            return View();
        }
        public JsonResult GetDangKyList()
        {
            var DKList = (from a in db.DangKies
                         join b in db.PhongBans on a.MaPB equals b.MaPB
                         join c in db.QuyenDangNhaps on a.MaQuyen equals c.MaQuyen
                         select new DangKyValidation()
                         {
                             TKID = a.TKID,
                             TenDangNhap = a.TenDangNhap,
                             HoTen = a.HoTen,
                             Email = a.Email,
                             TenPB = b.TenPB,
                             TenQuyen = c.TenQuyen
                         }).ToList();

            //List<DangKyValidation> DKList = db.DangKies.Select(x => new DangKyValidation
            //{
            //    TKID = x.TKID,
            //    TenDangNhap = x.TenDangNhap,
            //    HoTen = x.HoTen,
            //    Email = x.Email,
            //    TenPB = x.PhongBan.TenPB,
            //    TenQuyen = x.QuyenDangNhap.TenQuyen

            //}).ToList();

            return Json(DKList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDangKyById(int TKID)
        {
            DangKy model = db.DangKies.Where(x => x.TKID == TKID).SingleOrDefault();
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public JsonResult InsertInDatabase(DangKyValidation model)
        {
            var result = false;
            try
            {
                if (model.TKID == 0)
                {
                    DangKy ct = new DangKy();
                    ct.TenDangNhap = model.TenDangNhap;
                    ct.MatKhau = Encryptor.MD5Hash(model.MatKhau);
                    //ct.MatKhau = model.MatKhau;
                    ct.Email = model.Email;
                    ct.HoTen = model.HoTen;
                    ct.MaQuyen = model.MaQuyen;
                    ct.MaPB = model.MaPB;
                    db.DangKies.Add(ct);
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

        public JsonResult EditInDatabase(DangKyValidation model)
        {
            var result = false;
            try
            {
                if (model.TKID != 0)
                {
                    DangKy ct = db.DangKies.SingleOrDefault(x => x.TKID == model.TKID);
                    ct.HoTen = model.HoTen;
                    ct.Email = model.Email;
                    ct.MaPB = model.MaPB;
                    ct.MaQuyen = model.MaQuyen;
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

        public JsonResult DeleteDangKyRecord(int TKID)
        {
            bool result = false;
            DangKy ct = db.DangKies.SingleOrDefault(x => x.TKID == TKID);
            if (ct != null)
            {
                db.DangKies.Remove(ct);
                db.SaveChanges();
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(DangKyValidation model)
        {
            if (Session["TK"] != null)
            {
                string mk = Encryptor.MD5Hash(model.MatKhauCu);
                DangKy Suser = (DangKy)Session["TK"];
                DangKy user = db.DangKies.SingleOrDefault(x => x.TenDangNhap == Suser.TenDangNhap && x.MatKhau == mk);
                if (user != null)
                {
                    //user.MatKhau = model.MatKhau;
                    mk = Encryptor.MD5Hash(model.MatKhau);
                    user.MatKhau = mk;
                    db.SaveChanges();
                    Session.Clear();
                    Session.Abandon();
                    //TempData["msg"] = "<script>alert('Cập nhập thành công')</script>";
                    ViewBag.Message = "<script>alert('Thay đổi mật khẩu thành công');window.location.href = '/Login</script>";
                    //Page.ClientScript.RegisterClientScriptBlock(GetType(), "alerta", "alert('Save records with success')", true);
                    //return RedirectToAction("Index", "Login");
                    return View();
                }
                else
                {
                    ViewBag.Message = "<script>alert('Mật khẩu cũ không đúng, vui lòng nhập lại')</script>";
                    //TempData["msg"] = "<script>alert('Mật khẩu cũ không đúng, vui lòng nhập lại')</script>";
                    return View();
                }
            }
            else
            {
                ViewBag.Message = "<script>alert('Lỗi thay đổi mật khẩu')</script>";
                //TempData["msg"] = "<script>alert('Lỗi thay đổi mật khẩu')</script>";
                return View();
            }

        }


        public ActionResult ResetPassword(int TKID)
        {

            if (Session["TK"] != null)
            {
                DangKy Suser = (DangKy)Session["TK"];
                if (Suser.MaQuyen == 1)
                {
                    DangKy ct = db.DangKies.SingleOrDefault(x => x.TKID == TKID);
                    if (ct.MaQuyen == 1)
                    {
                        TempData["msg"] = "<script>alert('Bạn không có quyền reset user này');window.location.href = '/Register'</script>";
                        return View(ct);
                    }
                    return View(ct);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(DangKy model)
        {
            if (model.MaQuyen != 1)
            {
                DangKy ct = db.DangKies.SingleOrDefault(x => x.TKID == model.TKID);
                string pw = Encryptor.MD5Hash(model.MatKhau);
                ct.MatKhau = pw;
                db.SaveChanges();
                TempData["msg"] = "<script>alert('Cập nhập thành công');window.location.href = '/Register'</script>";
            }
            else
            {
                TempData["msg"] = "<script>alert('Bạn không có quyền reset user này');window.location.href = '/Register'</script>";
            }
            return View();
        }
    }
}