using HPDQ_Project.Common;
using HPDQ_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginUser(DangKyValidation model)
        {
            try
            {
                string mk = Encryptor.MD5Hash(model.MatKhau);
                DangKy user = db.DangKies.SingleOrDefault(x => x.TenDangNhap == model.TenDangNhap && x.MatKhau == mk);
                //string result = "fail";
                if (user != null)
                {
                    Session["TK"] = user;
                    Session["HoTen"] = user.HoTen;

                    Session["MaQuyen"] = user.MaQuyen;
                    //if (user.MaQuyen == 1)
                    //{
                    //    result = "Admin";
                    //}
                    //else if (user.MaQuyen == 2)
                    //{
                    //    result = "User";
                    //}
                    //else if (user.MaQuyen == 3)
                    //{
                    //    result = "Manager";
                    //}
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["msg"] = "<script>alert('Sai tên đăng nhập hoặc mật khẩu');window.location.href = '/Login'</script>";
                    //return View(TempData);
                    return RedirectToAction("", "Login");
                }
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Logout()
        {

            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");

        }
    }
}