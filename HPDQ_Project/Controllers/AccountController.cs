using ClosedXML.Excel;
using ExcelDataReader;
using HPDQ_Project.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        HPDQEntities db = new HPDQEntities();
        List<SelectListItem> ddTrangthai = new List<SelectListItem>();
        public ActionResult Index(int? page, int? MaPB,int? MaPM,string search)
        {
            if (MaPB == null) MaPB = 0;
            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB", MaPB);

            if (MaPM == null) MaPM = 0;
            List<PhanMem> pm = db.PhanMems.ToList();
            ViewBag.PMList = new SelectList(pm, "MaPM", "TenPM", MaPM);

            if (search == null) search = "";

            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            var model = from a in db.Taikhoan_danhsach(MaPB, MaPM,search)
                        select new TaiKhoanValidation
                        {
                            TKID=a.TKID,
                            IDNV = a.IDNV,
                            MaNV = a.MaNV,
                            TenNV = a.TenNV,
                            TenPB = a.TenPB,
                            TenTK = a.TenTK,
                            TrangThai = a.TrangThai,
                            GhiChu = a.GhiChu,
                            TenPM = a.TenPM
                        };
            
            return View(model.ToList().ToPagedList(pageNumber, pageSize));
            
        }
        private SelectList TrangThai(int? Value)
        {
            ddTrangthai.Add(new SelectListItem { Text = "Un Lock", Value = "0" });
            ddTrangthai.Add(new SelectListItem { Text = "Lock", Value = "1" });
            return new SelectList(ddTrangthai, "Value", "Text", Value);
        }
        public ActionResult ImportExcel()
        {
            List<PhanMem> pb = db.PhanMems.ToList();
            ViewBag.MaPM = new SelectList(pb, "MaPM", "TenPM");
            
            return PartialView();

        }
        [HttpPost]
        public ActionResult ImportExcel(TaiKhoanValidation _DO)
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
                        return View();
                    }
                    int MaPM = 0;
                    if (_DO.MaPM != null)
                    {
                        MaPM = Convert.ToInt32(_DO.MaPM);
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert('Vui lòng chọn phần mềm import');</script>";
                        return View();
                    }
                    DataSet result = reader.AsDataSet();
                    DataTable dt = result.Tables[0];
                    reader.Close();
                    int dtc = 0, dtrung = 0;
                   
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            for (int i = 4; i < dt.Rows.Count; i++)
                            {
                                if (IsAvailable(dt.Rows[i][1].ToString()) == false)
                                {
                                    if (dt.Rows[i][3].ToString() != "")
                                    {
                                        //if(MaPM > 0 && CheckAcc(MaPM, dt.Rows[i][3].ToString()))
                                        //{
                                        //    db.Taikhoan_insert(dt.Rows[i][1].ToString(), _DO.MaPM, dt.Rows[i][3].ToString(), DateTime.Now, 0, "", 1);
                                        //}

                                        db.Taikhoan_insert(dt.Rows[i][1].ToString(), _DO.MaPM, dt.Rows[i][3].ToString(), DateTime.Now, 0, "", 1);
                                        dtc++;
                                    }
                                }
                            }
                            string msg = "Import được " + dtc + " dòng dữ liệu";
                            TempData["msgSuccess"] = "<script>alert('" + msg + "');</script>";
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

            return RedirectToAction("Index", "Account");
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

        public bool CheckAcc(int MaPM, string TenTK)
        {
            var Regsbb = (from u in db.TaiKhoans
                          where u.MaPM == MaPM && u.TenTK.Contains(TenTK)
                          select new { u.TKID }).FirstOrDefault();
           
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
        public ActionResult Create()
        {
            List<PhanMem> pm = db.PhanMems.ToList();
            ViewBag.PMList = new SelectList(pm, "MaPM", "TenPM");
            List<TinhTrang> tt = db.TinhTrangs.ToList();
            ViewBag.TTList = new SelectList(tt, "MaTinhTrang", "TenTinhTrang");
            ViewBag.TrangThai = TrangThai(0);
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(TaiKhoanValidation _DO)
        {
            if (IsAvailable(_DO.MaNV))
            {
                TempData["msgSuccess"] = "<script>alert('Mã nhân viên không đúng');</script>";
            }
            else
            {
                db.Taikhoan_insert(_DO.MaNV, _DO.MaPM, _DO.TenTK, _DO.NgayCap, _DO.TrangThai, _DO.GhiChu, _DO.MaTinhTrang);
                TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            }

            return RedirectToAction("Index", "Account", new { MaPB = Request.QueryString["MaPB"], MaPM = _DO.MaPM });
        }

        public ActionResult Edit(int ID)
        {
            var model = (from a in db.TaiKhoans.Where(x => x.TKID == ID)
                         join b in db.NhanViens on a.MaNV equals b.MaNV
                         select new TaiKhoanValidation
                         {
                             TKID=a.TKID,
                             IDNV = b.IDNV,
                             MaNV = a.MaNV,
                             TenNV = b.TenNV,
                             TenTK = a.TenTK,
                             MaPM = a.MaPM,
                             NgayCap = a.NgayCap,
                             TrangThai=a.TrangThai,
                             MaTinhTrang=a.MaTinhTrang,
                             GhiChu=a.GhiChu
                         }).SingleOrDefault();
            List<PhanMem> pm = db.PhanMems.ToList();
            ViewBag.PMList = new SelectList(pm, "MaPM", "TenPM");
            List<TinhTrang> tt = db.TinhTrangs.ToList();
            ViewBag.TTList = new SelectList(tt, "MaTinhTrang", "TenTinhTrang");
            ViewBag.TrangThai = TrangThai(model.TrangThai);
            return PartialView(model);

        }
        [HttpPost]
        public ActionResult Edit(TaiKhoanValidation _DO)
        {
            db.Taikhoan_update(_DO.TKID, _DO.NgayCap, _DO.TrangThai, _DO.GhiChu, _DO.MaTinhTrang);
            TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            return RedirectToAction("Index", "Account", new { MaPB = Request.QueryString["MaPB"], MaPM = _DO.MaPM });
        }
        public ActionResult Lock(int ID)
        {
            var rs = db.TaiKhoans.Where(x => x.TKID == ID).SingleOrDefault();
            if(rs!=null)
            {
                db.Taikhoan_lock(ID);
                TempData["msgSuccess"] = "<script>alert('Khóa tài khoản thành công');</script>";
                return RedirectToAction("Index", "Account", new { MaPB=Request.QueryString["MaPB"], MaPM=Request.QueryString["MaPM"], page= Request.QueryString["page"] });
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Tài khoản không tồn tại');</script>";
                return RedirectToAction("Index", "Account", new { MaPB = Request.QueryString["MaPB"], MaPM = Request.QueryString["MaPM"], page = Request.QueryString["page"] });
            }
        }
        public ActionResult UnLock(int ID)
        {
            var rs = db.TaiKhoans.Where(x => x.TKID == ID).SingleOrDefault();
            if (rs != null)
            {
                db.Taikhoan_unlock(ID);
                TempData["msgSuccess"] = "<script>alert('Mở khóa tài khoản thành công');</script>";
                return RedirectToAction("Index", "Account", new { MaPB = Request.QueryString["MaPB"], MaPM = Request.QueryString["MaPM"], page = Request.QueryString["page"] });
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Tài khoản không tồn tại');</script>";
                return RedirectToAction("Index", "Account", new { MaPB = Request.QueryString["MaPB"], MaPM = Request.QueryString["MaPM"], page = Request.QueryString["page"] });
            }
        }
        public ActionResult Delete(int ID)
        {
            var rs = db.TaiKhoans.Where(x => x.TKID == ID).SingleOrDefault();
            if (rs != null)
            {
                db.Taikhoan_delete(ID);
                TempData["msgSuccess"] = "<script>alert('Xóa tài khoản thành công');</script>";
                return RedirectToAction("Index", "Account", new { MaPB = Request.QueryString["MaPB"], MaPM = Request.QueryString["MaPM"], page = Request.QueryString["page"] });
            }  
            else
            {
                TempData["msgSuccess"] = "<script>alert('Tài khoản không tồn tại');</script>";
                return RedirectToAction("Index", "Account", new { MaPB = Request.QueryString["MaPB"], MaPM = Request.QueryString["MaPM"], page = Request.QueryString["page"] });
            }    
        }
        public List<TaiKhoanValidation> GetLTK(int? MaPB, int? MaPM)
        {
           
            string search = "";
            var model = from a in db.Taikhoan_danhsach(MaPB, MaPM, search)
                        select new TaiKhoanValidation
                        {
                            TKID = a.TKID,
                            IDNV = a.IDNV,
                            MaNV = a.MaNV,
                            TenNV = a.TenNV,
                            TenPB = a.TenPB,
                            TenTK = a.TenTK,
                            TrangThai = a.TrangThai,
                            GhiChu = a.GhiChu,
                            TenPM = a.TenPM
                        };

            return model.ToList();
        }
        public ActionResult ExportToExcel(int? MaPB,int? MaPM)
        {
            try
            {
                string fileNamemau = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_taikhoan.xlsx";
                string fileNamemaunew = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_taikhoan_temp.xlsx";
                XLWorkbook Workbook = new XLWorkbook(fileNamemau);
                IXLWorksheet Worksheet = Workbook.Worksheet("Name");
                if (MaPB == null) MaPB = 0;
                if (MaPM == null) MaPM = 0;
                IList<TaiKhoanValidation> lDB = GetLTK(MaPB,MaPM);
                if (lDB.Count > 0)
                {
                    int row = 3, stt = 0;
                    foreach (var item in lDB)
                    {
                        row++; stt++;
                        Worksheet.Cell("A" + row).Value = stt;
                        Worksheet.Cell("A" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        Worksheet.Cell("A" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                        Worksheet.Cell("B" + row).Value = item.MaNV;
                        Worksheet.Cell("B" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        Worksheet.Cell("B" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                        Worksheet.Cell("C" + row).Value = item.TenNV;
                        Worksheet.Cell("C" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        Worksheet.Cell("C" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;


                        Worksheet.Cell("D" + row).Value = item.TenPB;
                        Worksheet.Cell("D" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        Worksheet.Cell("D" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                        Worksheet.Cell("D" + row).Style.Alignment.WrapText = true;

                        Worksheet.Cell("E" + row).Value = item.TenTK;
                        Worksheet.Cell("E" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        Worksheet.Cell("E" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                        Worksheet.Cell("E" + row).Style.Alignment.WrapText = true;

                        Worksheet.Cell("F" + row).Value = item.TenPM;
                        Worksheet.Cell("F" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        Worksheet.Cell("F" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                    }
                    Worksheet.Range("A4:F" + (row)).Style.Font.SetFontName("Times New Roman");
                    Worksheet.Range("A4:F" + (row)).Style.Font.SetFontSize(13);
                    Worksheet.Range("A4:F" + (row)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    Worksheet.Range("A4:F" + (row)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    // Worksheet.Cell("B2").Value = "Ngày cập nhật: " + DateTime.Now.Date.ToString("dd/MM/yyyy");
                    //Worksheet.Name = MaPB;
                    Workbook.SaveAs(fileNamemaunew);
                    byte[] fileBytes = System.IO.File.ReadAllBytes(fileNamemaunew);
                    string fileName = "Danhsachtaikhoan" + ".xlsx";
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else
                {
                    //TempData["msg"] = "<script>alert('Không có dữ liệu');window.location.href = '/Progress/Department/" + MaPB + "'</script>";
                    return View(TempData);
                }
                //if (Session["TK"] != null)
                //{
                //    string fileNamemau = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_danhba.xlsx";
                //    string fileNamemaunew = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_danhba_temp.xlsx";
                //    XLWorkbook Workbook = new XLWorkbook(fileNamemau);
                //    IXLWorksheet Worksheet = Workbook.Worksheet("Name");
                //    if (MaPB == null) MaPB = 0;
                //    IList<DanhBaValidation> lDB = GetLDB(MaPB);
                //    if (lDB.Count > 0)
                //    {
                //        int row = 4, stt = 0;
                //        foreach (var item in lDB)
                //        {
                //            row++; stt++;
                //            Worksheet.Cell("A" + row).Value = stt;
                //            Worksheet.Cell("A" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //            Worksheet.Cell("A" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                //            Worksheet.Cell("B" + row).Value = item.MaNV;
                //            Worksheet.Cell("B" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //            Worksheet.Cell("B" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                //            Worksheet.Cell("C" + row).Value = item.TenNV;
                //            Worksheet.Cell("C" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //            Worksheet.Cell("C" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;


                //            Worksheet.Cell("D" + row).Value = item.ViTri;
                //            Worksheet.Cell("D" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //            Worksheet.Cell("D" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                //            Worksheet.Cell("D" + row).Style.Alignment.WrapText = true;

                //            Worksheet.Cell("E" + row).Value = item.TenPB;
                //            Worksheet.Cell("E" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                //            Worksheet.Cell("E" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                //            Worksheet.Cell("E" + row).Style.Alignment.WrapText = true;

                //            Worksheet.Cell("F" + row).Value = item.SoMayLe;
                //            Worksheet.Cell("F" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                //            Worksheet.Cell("F" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

                //        }
                //        Worksheet.Range("A5:F" + (row)).Style.Font.SetFontName("Times New Roman");
                //        Worksheet.Range("A5:F" + (row)).Style.Font.SetFontSize(13);
                //        Worksheet.Range("A5:F" + (row)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //        Worksheet.Range("A5:F" + (row)).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                //       // Worksheet.Cell("B2").Value = "Ngày cập nhật: " + DateTime.Now.Date.ToString("dd/MM/yyyy");
                //        //Worksheet.Name = MaPB;
                //        Workbook.SaveAs(fileNamemaunew);
                //        byte[] fileBytes = System.IO.File.ReadAllBytes(fileNamemaunew);
                //        string fileName = MaPB + ".xlsx";
                //        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                //    }
                //    else
                //    {
                //        //TempData["msg"] = "<script>alert('Không có dữ liệu');window.location.href = '/Progress/Department/" + MaPB + "'</script>";
                //        return View(TempData);
                //    }
                //}
                //else
                //{
                //    //Chưa đăng nhập
                //    return RedirectToAction("", "Login");
                //}
            }
            catch (Exception ex)
            {
                //TempData["msg"] = "<script>alert('" + ex + "');window.location.href = '/Progress/Department/" + MaPB + "'</script>";
                return View(TempData);
            }

        }
    }
}