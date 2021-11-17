using ExcelDataReader;
using HPDQ_Project.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Net;
using Newtonsoft.Json;

namespace HPDQ_Project.Controllers
{
    public class EmployeesController : Controller
    {
        // GET: Employees
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index(int? page, int? MaPB)
        {
            List<PhongBan> pb = db.PhongBans.ToList();
            if (MaPB == null) MaPB = 0;
            var model = from a in db.Nhanvien_danhsach(MaPB)
                    select new NhanVienValidation
                    {
                        IDNV=a.IDNV,
                        MaNV = a.MaNV,
                        TenPB = a.TenPB,
                        TenCV = a.TenCV,
                        TenNV = a.TenNV,
                        NgayVaoLam=a.NgayVaoLam

                    };
            if (page == null) page = 1;
            
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB", MaPB);
            return View(model.OrderBy(x => x.MaNV).ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Edit(int ID)
        {
            var result = (from u in db.Nhanvien_DO(ID)
                          select new NhanVienValidation
                          {
                              IDNV = u.IDNV,
                              MaNV = u.MaNV,
                              TenNV = u.TenNV,
                              TenCV = u.TenCV,
                              TenPB = u.TenPB,
                              DiaChi = u.DiaChi,
                              SDT = u.SDT,
                              MaCV = u.MaCV,
                              MaPB = u.MaPB,
                              MaGT = u.MaGT,
                              TenGT = u.TenGT,
                              Email = u.Email,
                              NgayVaoLam = u.NgayVaoLam
                          }).ToList();
            NhanVienValidation DO = new NhanVienValidation();
            if (result.Count > 0)
            {
                foreach (var u in result)
                {
                    DO.IDNV = u.IDNV;
                    DO.MaNV = u.MaNV;
                    DO.TenNV = u.TenNV;
                    DO.TenCV = u.TenCV;
                    DO.MaCV = u.MaCV;
                    DO.MaPB = u.MaPB;
                    DO.MaGT = u.MaGT;
                    DO.TenPB = u.TenPB;
                    DO.DiaChi = u.DiaChi;
                    DO.SDT = u.SDT;
                    var m = db.DanhBas.Where(x => x.MaNV == u.MaNV).SingleOrDefault();
                    if (m != null)
                    {
                        DO.SoMayLe = m.SoMayLe;
                    }
                    else { DO.SoMayLe = null; }
                    DO.TenGT = u.TenGT;
                    DO.Email = u.Email;
                    DO.NgayVaoLam = u.NgayVaoLam;
                }
               
            }
            List<ChucVu> cv = db.ChucVus.ToList();
            ViewBag.CVList = new SelectList(cv, "MaCV", "TenCV",DO.MaCV);

            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB",DO.MaPB);

            List<GioiTinh> gt = db.GioiTinhs.ToList();
            ViewBag.GTList = new SelectList(gt, "MaGT", "TenGT",DO.MaGT);
            try
            {
                ViewBag.NgayVaoLam = DO.NgayVaoLam.Value.ToString("yyyy-MM-dd");
            }
            catch
            {
                ViewBag.NgayVaoLam = "NULL";
            }

            return PartialView(DO);
        }
        [HttpPost]
        public ActionResult Edit(NhanVienValidation _DO)
        {
            var model = db.ChucVus.Where(x => x.MaCV == _DO.MaCV).SingleOrDefault();
            if (model != null)
            {    
                db.Nhanvien_update(_DO.IDNV, _DO.TenNV, _DO.MaCV, _DO.MaPB, _DO.DiaChi, _DO.SDT,_DO.MaGT, _DO.Email, _DO.NgayVaoLam);
                db.Nhanvien_update_danhba(_DO.MaNV, _DO.SoMayLe, model.TenCV, 1,1);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            }
            return RedirectToAction("Index", "Employees", new { MaPB = _DO.MaPB });
        }
        public ActionResult Create()
        {
            List<ChucVu> cv = db.ChucVus.ToList();
            ViewBag.CVList = new SelectList(cv, "MaCV", "TenCV");

            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");

            List<GioiTinh> gt = db.GioiTinhs.ToList();
            ViewBag.GTList = new SelectList(gt, "MaGT", "TenGT");
            return PartialView();
        }
        [HttpPost]
        public ActionResult Create(NhanVienValidation _DO)
        {
            
            var model = db.ChucVus.Where(x => x.MaCV == _DO.MaCV).SingleOrDefault();
            if(model!=null)
            {
                db.Nhanvien_insert(_DO.MaNV, _DO.TenNV, _DO.MaCV, _DO.MaPB, _DO.DiaChi, _DO.SDT, _DO.MaGT, _DO.Email, _DO.NgayVaoLam, 1);
                db.Nhanvien_update_danhba(_DO.MaNV, _DO.SoMayLe, model.TenCV, 1,0);
                TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            }
            else
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi thêm mới nhân viên');</script>";
            }
            
            return RedirectToAction("Index", "Employees", new { MaPB = _DO.MaPB });
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


        public ActionResult ImportExcel()
        {
            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            return PartialView();

        }
        [HttpPost]
        public ActionResult ImportExcel(NhanVienValidation _DO)
        {
            string filePath = string.Empty;
            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["FileUpload"];
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string path = Server.MapPath("~/Uploads/");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    filePath = path + Path.GetFileName(file.FileName);

                    file.SaveAs(filePath);
                    Stream stream = file.InputStream;
                    // We return the interface, so that  
                    IExcelDataReader reader = null;
                    if (file.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (file.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Vui lòng chọn đúng định dạng file Excel');</script>"; 
                        return View(_DO);
                    }
                    DataSet result = reader.AsDataSet();
                    DataTable dt = result.Tables[0];
                    reader.Close();
                    int dtc = 0, dtrung = 0;
                    int MaCV, MaPB;
                    string MaNV = "";
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {

                            for (int i = 3; i < dt.Rows.Count; i++)
                            {
                                MaNV = dt.Rows[i][1].ToString();
                                var rsnv = db.NhanViens.Where(x => x.MaNV == MaNV).SingleOrDefault();
                                if(rsnv==null)
                                {    
                                    MaCV = 0; MaPB = 0;
                                    MaCV = GetMaCV(dt.Rows[i][4].ToString());
                                    MaPB = GetMaPB(dt.Rows[i][5].ToString());
                                    if (MaCV == 0)
                                    {
                                        db.Chucvu_insert(dt.Rows[i][4].ToString());
                                        MaCV = GetMaCV(dt.Rows[i][4].ToString());
                                    }

                                    if (MaPB == 0)
                                    {
                                        db.Phongban_insert(dt.Rows[i][5].ToString());
                                        MaPB = GetMaPB(dt.Rows[i][5].ToString());
                                    }
                                    string dtime=dt.Rows[i][6].ToString();
                                    DateTime NVL = DateTime.ParseExact(dt.Rows[i][6].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    db.Nhanvien_insert_Import(dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), MaCV, MaPB,6,NVL, 1);
                                    dtc++;
                                }
                                else
                                {
                                    MaCV = 0; MaPB = 0;
                                    MaCV = GetMaCV(dt.Rows[i][4].ToString());
                                    MaPB = GetMaPB(dt.Rows[i][5].ToString());
                                    if (MaCV == 0)
                                    {
                                        db.Chucvu_insert(dt.Rows[i][4].ToString());
                                        MaCV = GetMaCV(dt.Rows[i][4].ToString());
                                    }

                                    if (MaPB == 0)
                                    {
                                        db.Phongban_insert(dt.Rows[i][5].ToString());
                                        MaPB = GetMaPB(dt.Rows[i][5].ToString());
                                    }
                                    db.Nhanvien_update_import(MaNV, MaCV, MaPB);
                                    dtrung++;
                                }    
                            }

                            string msg = "";
                            if (dtc != 0 && dtrung != 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu, " + "Có " + dtrung + " dòng trùng MãNV cập nhập nội dung";
                            }
                            else if (dtc != 0 && dtrung == 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu";
                            }
                            else if (dtc == 0 && dtrung != 0)
                            {
                                msg = "Có " + dtrung + " dòng trùng Mã NV cập nhập nội dung";
                            }
                            else { msg = "File import không có dữ liệu"; }


                            TempData["msgSuccess"] = "<script>alert('"+ msg + "');</script>";
                        }
                        catch
                        {
                            TempData["msgSuccess"] = "<script>alert('File import không đúng định dạng. Vui lòng tải biểu mẫu file import');</script>";
                        }
                    }
                    else
                    {
                        TempData["msgSuccess"] = "<script>alert('File import không đúng định dạng. Vui lòng tải biểu mẫu file import');</script>";
                    }

                }
                else
                {
                    TempData["msgSuccess"] = "<script>alert('Vui lòng nhập file Import');</script>";
                }
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Vui lòng nhập file Import');</script>";
            }

            return RedirectToAction("Index", "Employees");
        }

        public JsonResult GetNhanvien(string search)
        {
            var result = (from u in db.Nhanvien_search(search)
                          select new NhanVienValidation
                          {
                              IDNV=u.IDNV,
                              MaNV = u.MaNV,
                              TenNV = u.TenNV
                          }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private string ConvertToUnSign(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            return str2;
        }

        public int GetMaCV(string TenCV)
        {
            var model = db.ChucVus.Where(x => x.TenCV == TenCV).SingleOrDefault();
            if (model == null)
                return 0;
            return model.MaCV;
          
        }
        public int GetMaPB(string TenPB)
        {
            var model = db.PhongBans.Where(x => x.TenPB == TenPB).SingleOrDefault();
            if (model == null)
                return 0;
            return model.MaPB;

        }

        public ActionResult Details(int ID)
        {
            var result = (from u in db.Nhanvien_DO(ID)
                          select new NhanVienValidation
                          {
                              IDNV=u.IDNV,
                              MaNV = u.MaNV,
                              TenNV = u.TenNV,
                              TenCV = u.TenCV,
                              TenPB = u.TenPB,
                              DiaChi = u.DiaChi,
                              SDT = u.SDT,
                              //SoMayLe = u.SoMayLe,
                              TenGT = u.TenGT,
                              Email = u.Email,
                              NgayVaoLam = u.NgayVaoLam
                          }).ToList();
            NhanVienValidation DO = new NhanVienValidation();
            if(result.Count>0)
            {
                foreach(var u in result)
                {
                    DO.IDNV = u.IDNV;
                    DO.MaNV = u.MaNV;
                    DO.TenNV = u.TenNV;
                    DO.TenCV = u.TenCV;
                    DO.TenPB = u.TenPB;
                    DO.DiaChi = u.DiaChi;
                    DO.SDT = u.SDT;
                    var m = db.DanhBas.Where(x => x.MaNV == u.MaNV).SingleOrDefault();
                    if(m!=null){
                        DO.SoMayLe = m.SoMayLe;
                    }
                    else { DO.SoMayLe = null; }    
                    DO.TenGT = u.TenGT;
                    DO.Email = u.Email;
                    DO.NgayVaoLam = u.NgayVaoLam;
                }
                return View(DO);
            }

            return View();
        }
       


        public ActionResult ListAccount(string ID)
        {
            var model = from a in db.TaiKhoans.Where(x => x.MaNV == ID)
                        join b in db.PhanMems on a.MaPM equals b.MaPM
                        join c in db.TinhTrangs on a.MaTinhTrang equals c.MaTinhTrang
                        select new TaiKhoanValidation
                        {
                            TKID = a.TKID,
                            MaNV = a.MaNV,
                            TenTK = a.TenTK,
                            TrangThai = a.TrangThai,
                            TenPM = b.TenPM
                        };
            
            return PartialView(model.ToList());
        }
        public ActionResult Timesheets()
        {
            //DateTime Since = DateTime.Now.AddDays(-3);
            //DateTime Todate = DateTime.Now.Date;
            //since = Since.Year.ToString() + Since.Month.ToString() + Since.Day.ToString();
            //todate = Todate.Year.ToString() + Todate.Month.ToString() + Todate.Day.ToString();
            //var webClient = new WebClient();
            //webClient.Headers["Content-type"] = "application/json";
            //webClient.Encoding = Encoding.UTF8;
            string since = "20200903";
            string todate = "20200903";
            string link= @"http://192.168.120.120/hoaphatdq/TsMobileAPI.asmx/GetDataArgJson?Procedure=SHP_HR_SEL_INF_WDATE_DETAIL&para="+ since + "|"+ todate + "&token=";
            //var json = webClient.DownloadString(link);
            //Employees_API.Rootobject[] root = JsonConvert.DeserializeObject<Employees_API.Rootobject[]>(json);


            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var json = webClient.DownloadString(link);
                var list = JsonConvert.DeserializeObject<List<Employees_API.Timesheets>>(json);
                return View(list.ToList());
            }
               
                
        }
        List<Employees_API.Timesheets> GetAPI()
        {
            string sy, sm, sd;
            sy = DateTime.Now.Year.ToString();
            if (DateTime.Now.Month <= 9)
                sm = "0" + DateTime.Now.Month;
            else
                sm = DateTime.Now.Month.ToString();

            if (DateTime.Now.Day <= 9)
                sd = "0" + DateTime.Now.Day;
            else
                sd = DateTime.Now.Day.ToString();
            DateTime dtt = DateTime.Now.AddDays(-3);
            string ty, tm, td;
            ty = dtt.Year.ToString();
            if (dtt.Month <= 9)
                tm = "0" + dtt.Month;
            else
                tm = dtt.Month.ToString();

            if (dtt.Day <= 9)
                td = "0" + dtt.Day;
            else
                td = dtt.Day.ToString();

            string since = ty + tm + td;
            string todate = sy + sm + sd;
            string link = @"http://192.168.120.120/hoaphatdq/TsMobileAPI.asmx/GetDataArgJson?Procedure=SHP_HR_SEL_INF_WDATE_DETAIL&para=" + since + "|" + todate + "&token=";
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var json = webClient.DownloadString(link);
                var list = JsonConvert.DeserializeObject<List<Employees_API.Timesheets>>(json);
                return list.ToList();
            }

        }
        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
        public JsonResult GetDataArgJson(string U, string P)
        {
            if(U=="tuanpv" && P=="123456a@" && GetIp()=="192.168.113.125")
            { 
                var rs = (from a in db.NhanViens.Where(x => x.TrangThai == 1)
                          join b in db.PhongBans on a.MaPB equals b.MaPB
                          join c in db.ChucVus on a.MaCV equals c.MaCV
                          join d in db.DanhBas on a.MaNV equals d.MaNV
                          select new Employees_API.E_API
                          {
                              MaNV = a.MaNV,
                              TenNV = a.TenNV,
                              MaPB = a.MaPB,
                              TenPB = b.TenPB,
                              MaCV = a.MaCV,
                              TenCV = c.TenCV,
                              SoMayLe = d.SoMayLe,
                              Email = a.Email
                          }).OrderBy(x => x.MaNV).ToList();

                return Json(rs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return null;
            }
        }
        public ActionResult Sync()
        {
            string MaNV,sMaNV;
            int MaCV, MaPB, MaGT;
            int dtc = 0;
            string msg = "";
            List<Employees_API.Timesheets> list = GetAPI();
            foreach(var item in list)
            {
                if(item.EMP_ID!=null)
                {
                    MaNV = item.EMP_ID;
                    MaCV = 1173;
                    MaGT = 6;
                    MaPB = GetMaPB(item.ORG_NM);
                    sMaNV = MaNV.Substring(0, 4);
                    var rsnv = db.NhanViens.Where(x => x.MaNV == MaNV).SingleOrDefault();
                    if(rsnv==null)
                    {
                        if(sMaNV == "HPDQ")
                        {  
                            db.Nhanvien_insert_API(MaNV, item.FULL_NAME, MaCV, MaPB, MaGT, 1, item.PHONE_NUMBER);
                            dtc++;
                        }
                    }
                    else
                    {
                        if (sMaNV == "HPDQ")
                        {
                            db.Nhanvien_update_API(MaNV, MaPB, item.PHONE_NUMBER);
                            dtc++;
                        }
                    }    
                    

                }    
            }
            if (dtc != 0 )
            {
                msg = "Đồng bộ được " + dtc + " dòng dữ liệu";
            }
            else
            {
                msg = "Dữ liệu hiện tại đã được đồng bộ";
            }
            TempData["msgSuccess"] = "<script>alert('"+ msg + "');</script>";
            return RedirectToAction("Index", "Employees");
        }
        
        public ActionResult Delete(int ID)
        {
            var rs = db.NhanViens.Where(x => x.IDNV == ID && x.TrangThai==1).SingleOrDefault();
            if(rs!=null)
            {
                db.Nhanvien_update_danhba(rs.MaNV,0, "", 0, 2);
                db.Taikhoan_delete_MaNV(rs.MaNV);
                db.Nhanvien_delete(ID);
                TempData["msgError"] = "<script>alert('Xóa nhân viên thành công');</script>";
                return RedirectToAction("Index", "Employees", new { MaPB = rs.MaPB });
            }
            else
            {
                TempData["msgError"] = "<script>alert('Có lỗi khi xóa nhân viên');</script>";
                return RedirectToAction("Index", "Employees");
            }
            
        }

    }
}