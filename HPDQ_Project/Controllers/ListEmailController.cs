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
    public class ListEmailController : Controller
    {
        // GET: ListEmail
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index(int? page, int? MaPB, string search)
        {
            List<PhongBan> pb = db.PhongBans.ToList();
            if (MaPB == null) MaPB = 0;
            if (search == null) search = "";
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB", MaPB);

            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(GetlistEmail(MaPB,search).ToPagedList(pageNumber, pageSize));

        }
        List<EmailValidation> GetlistEmail(int? MaPB,string search)
        {
            if(search=="")
            {     
                if (MaPB == 0)
                {
                    var model = (from a in db.NhanViens.Where(x => x.Email != null && x.Email != "" && x.TrangThai == 1)
                                 join b in db.ChucVus on a.MaCV equals b.MaCV
                                 join c in db.PhongBans on a.MaPB equals c.MaPB
                                 select new EmailValidation()
                                 {
                                     IDNV = a.IDNV,
                                     MaNV = a.MaNV,
                                     TenPB = c.TenPB,
                                     TenCV = b.TenCV,
                                     TenNV = a.TenNV,
                                     Email = a.Email,
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
                else
                {
                    var model = (from a in db.NhanViens.Where(x => x.Email != null && x.Email != "" &&x.MaPB==MaPB && x.TrangThai == 1)
                                 join b in db.ChucVus on a.MaCV equals b.MaCV
                                 join c in db.PhongBans on a.MaPB equals c.MaPB
                                 select new EmailValidation()
                                 {
                                     IDNV = a.IDNV,
                                     MaNV = a.MaNV,
                                     TenPB = c.TenPB,
                                     TenCV = b.TenCV,
                                     TenNV = a.TenNV,
                                     Email = a.Email,
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
            }
            else
            {
                if (MaPB == 0)
                {
                    var model = (from a in db.NhanViens.Where(x => x.Email != null && x.Email != "" && x.TrangThai == 1 && (x.TenNV.Contains(search) || x.TenNVNONE.Contains(search)))
                                 join b in db.ChucVus on a.MaCV equals b.MaCV
                                 join c in db.PhongBans on a.MaPB equals c.MaPB
                                 select new EmailValidation()
                                 {
                                     IDNV = a.IDNV,
                                     MaNV = a.MaNV,
                                     TenPB = c.TenPB,
                                     TenCV = b.TenCV,
                                     TenNV = a.TenNV,
                                     Email = a.Email,
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
                else
                {
                    var model = (from a in db.NhanViens.Where(x => x.Email != null && x.Email != "" && x.MaPB == MaPB && x.TrangThai == 1 && (x.TenNV.Contains(search) || x.TenNVNONE.Contains(search)))
                                 join b in db.ChucVus on a.MaCV equals b.MaCV
                                 join c in db.PhongBans on a.MaPB equals c.MaPB
                                 select new EmailValidation()
                                 {
                                     IDNV = a.IDNV,
                                     MaNV = a.MaNV,
                                     TenPB = c.TenPB,
                                     TenCV = b.TenCV,
                                     TenNV = a.TenNV,
                                     Email = a.Email,
                                 }).OrderBy(x => x.MaNV);
                    return model.ToList();
                }
            }    

        }
        public ActionResult ImportExcel()
        {
            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            return PartialView();

        }
        [HttpPost]
        public ActionResult ImportExcel(DanhBaValidation _DO)
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
                                    if (dt.Rows[i][7].ToString() != "")
                                    {
                                        db.Nhanvien_update_Email(dt.Rows[i][1].ToString(), dt.Rows[i][7].ToString());
                                    }

                                }

                                dtc++;
                            }

                            string msg = "";
                            if (dtc != 0 && dtrung != 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu, " + "Có " + dtrung + " dòng trùng Mã HM cập nhập nội dung";
                            }
                            else if (dtc != 0 && dtrung == 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu";
                            }
                            else if (dtc == 0 && dtrung != 0)
                            {
                                msg = "Có " + dtrung + " dòng trùng Mã HM cập nhập nội dung";
                            }
                            else { msg = "File import không có dữ liệu"; }

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

            return RedirectToAction("Index", "ListEmail");
        }

        public JsonResult GetNhanvien(string search)
        {
            var result = (from u in db.Viev_NhanVien.Where(x=>x.MaNV.Contains(search) || x.TenCV.Contains(search) || x.Ho_Ten.Contains(search))
                          select new EmailValidation
                          {
                              IDNV = u.IDNV,
                              MaNV = u.MaNV,
                              TenNV = u.TenNV,
                              MaCV = u.MaCV,
                              MaPB = u.MaPB,
                              Email = u.Email
                          }).Take(30).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
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



        public ActionResult UpdateEmail()
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
        public ActionResult UpdateEmail(EmailValidation _DO)
        {
            db.Nhanvien_update_Email(_DO.MaNV,_DO.Email);
            TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            return RedirectToAction("Index", "ListEmail", new { MaPB = _DO.MaPB });
        }
        public ActionResult Edit(int ID)
        {
            var model = (from a in db.NhanViens.Where(x => x.IDNV == ID)
                         select new EmailValidation
                         {
                             IDNV = a.IDNV,
                             MaNV = a.MaNV,
                             TenNV = a.TenNV,
                             MaCV = a.MaCV,
                             MaPB = a.MaPB,
                             Email = a.Email,
                         }).SingleOrDefault();
            List<ChucVu> cv = db.ChucVus.ToList();
            ViewBag.CVList = new SelectList(cv, "MaCV", "TenCV");
            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            return PartialView(model);

        }
        [HttpPost]
        public ActionResult Edit(EmailValidation _DO)
        {
            db.Nhanvien_update_Email(_DO.MaNV, _DO.Email);
            TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
            return RedirectToAction("Index", "ListEmail", new { MaPB = _DO.MaPB });
        }
        public List<DanhBaValidation> GetLDB(int? MaPB)
        {
            if (MaPB == null) MaPB = 0;
            string search = "";
            var model = from a in db.thongtin_danhba_NV(MaPB, search)
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
            return model.ToList();
        }
        public ActionResult ExportToExcel(int? MaPB)
        {
            try
            {
                string fileNamemau = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_email.xlsx";
                string fileNamemaunew = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_email_temp.xlsx";
                XLWorkbook Workbook = new XLWorkbook(fileNamemau);
                IXLWorksheet Worksheet = Workbook.Worksheet("Name");
                if (MaPB == null) MaPB = 0;
                IList<EmailValidation> lDB = GetlistEmail(MaPB,"");
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


                        Worksheet.Cell("D" + row).Value = item.TenCV;
                        Worksheet.Cell("D" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        Worksheet.Cell("D" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                        Worksheet.Cell("D" + row).Style.Alignment.WrapText = true;

                        Worksheet.Cell("E" + row).Value = item.TenPB;
                        Worksheet.Cell("E" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        Worksheet.Cell("E" + row).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
                        Worksheet.Cell("E" + row).Style.Alignment.WrapText = true;

                        Worksheet.Cell("F" + row).Value = item.Email;
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
                    string fileName = "Danhsachemail" + ".xlsx";
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