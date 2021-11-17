using ExcelDataReader;
using HPDQ_Project.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;

namespace HPDQ_Project.Controllers
{
    public class PhoneBookController : Controller
    {
        // GET: PhoneBook
        HPDQEntities db = new HPDQEntities();
        public ActionResult Index(int? page,int? MaPB,string search)
        {
            List<PhongBan> pb = db.PhongBans.ToList();
            if (MaPB == null) MaPB = 0;
            if (search == null) search = "";
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB", MaPB);
            var model = from a in db.thongtin_danhba_NV(MaPB,search)
                        select new DanhBaValidation()
                        {
                            IDNV=a.IDNV,
                            MaNV = a.MaNV,
                            TenPB = a.TenPB,
                            TenCV= a.Vitri,
                            TenNV = a.TenNV,
                            SoMayLe = a.SoMayLe,
                            TrangThai=a.TrangThai
                        };
            if (page == null) page = 1;
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(model.ToList().ToPagedList(pageNumber, pageSize));
         }

        public ActionResult Delete(int ID)
        {
            var result = db.NhanViens.Where(x => x.IDNV == ID && x.TrangThai == 1).SingleOrDefault();
           
            if (result != null)
            {
                var rsdb = db.DanhBas.Where(x => x.MaNV == result.MaNV).SingleOrDefault();
                if(rsdb!=null)
                {
                    db.Nhanvien_update_danhba(rsdb.MaNV, 0, "", 1, 2);
                    TempData["msgSuccess"] = "<script>alert('Xóa danh bạ thành công');</script>";
                }
                else
                {
                    TempData["msgSuccess"] = "<script>alert('Có lỗi khí xóa danh bạ');</script>";
                }    
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Có lỗi khí xóa danh bạ');</script>";
            }
            return RedirectToAction("Index", "PhoneBook");
        }
        public ActionResult Edit(int ID)
        {
            var model = (from a in db.NhanViens.Where(x => x.IDNV == ID)
                         join b in db.DanhBas on a.MaNV equals b.MaNV
                         select new DanhBaValidation
                         {
                             IDNV=a.IDNV,
                             MaNV = a.MaNV,
                             TenNV = a.TenNV,
                             MaCV = a.MaCV,
                             MaPB = a.MaPB,
                             SoMayLe = b.SoMayLe
                         }).SingleOrDefault();
            List<ChucVu> cv = db.ChucVus.ToList();
            ViewBag.CVList = new SelectList(cv, "MaCV", "TenCV");
            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            return PartialView(model);
           
        }
        [HttpPost]
        public ActionResult Edit(DanhBaValidation _DO)
        {
            var rsnv = db.NhanViens.Where(x => x.MaNV == _DO.MaNV).SingleOrDefault();
            if(rsnv!=null)
            {
                string vitri=GetTenCV(Convert.ToInt32(rsnv.MaCV));
                db.Nhanvien_update_danhba(_DO.MaNV, _DO.SoMayLe, vitri, 1, 1);
                TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
                return RedirectToAction("Index", "PhoneBook", new { MaPB = rsnv.MaPB });
            }    
            else
            {
                TempData["msgSuccess"] = "<script>alert('Có lỗi khi cập nhập danh bạ');</script>";
                return RedirectToAction("Index", "PhoneBook", new { MaPB = _DO.MaPB });
            }    
           
            //return RedirectToAction("Index", "PhoneBook", new { MaPB = _DO.MaPB });
        }
        public string GetTenCV(int MaCV)
        {
            var model = db.ChucVus.Where(x => x.MaCV == MaCV).SingleOrDefault();
            if (model == null)
                return "";
            return model.TenCV;

        }
        public ActionResult MenuleftPhone()
        {
            List<PhongBan> pb = db.PhongBans.ToList();
            return PartialView(pb);
        }
        public ActionResult Department(int id, int? page)
        {

            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            var model = from a in db.NhanViens.Where(x=>x.MaPB==id)
                        join c in db.PhongBans on a.MaPB equals c.MaPB
                        join b in db.ChucVus on a.MaCV equals b.MaCV
                        orderby a.MaNV descending
                        select new NhanVienValidation()
                        {
                            MaNV = a.MaNV,
                            TenPB = c.TenPB,
                            TenCV = b.TenCV,
                            TenNV = a.TenNV,
                            //SoMayLe = a.SoMayLe,
                            SDT = a.SDT,
                            Email = a.Email
                        };
            if (page == null) page = 1;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(model.ToList().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Create()
        {
            List<ChucVu> cv = db.ChucVus.ToList();
            ViewBag.CVList = new SelectList(cv, "MaCV", "TenCV");

            List<PhongBan> pb = db.PhongBans.ToList();
            ViewBag.PBList = new SelectList(pb, "MaPB", "TenPB");
            var rs = (from u in db.NhanViens
                      where u.MaNV.Contains("DANHBA")
                      orderby u.MaNV descending select u).FirstOrDefault();
            if(rs==null)
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
            //db.Nhanvien_insert_danhba(_DO.MaNV, _DO.TenNV, _DO.MaCV, _DO.MaPB, _DO.SoMayLe);
            TempData["msgSuccess"] = "<script>alert('Thêm mới thành công');</script>";
            return RedirectToAction("Index", "PhoneBook",new{id=_DO.MaPB});
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
                    int snb;
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            
                            for (int i = 3; i < dt.Rows.Count; i++)
                            {
                                string MaNV = dt.Rows[i][1].ToString();
                                if (IsAvailable(dt.Rows[i][1].ToString())==false)
                                {
                                    if(dt.Rows[i][8].ToString()!="")
                                    {
                                        snb = Convert.ToInt32(dt.Rows[i][8].ToString());
                                        var rsdb = db.DanhBas.Where(x => x.MaNV == MaNV).SingleOrDefault();
                                        if(rsdb==null)
                                        {
                                            db.Nhanvien_update_danhba(MaNV, snb, dt.Rows[i][4].ToString(), 1, 0);
                                        }
                                        else
                                        {
                                            db.Nhanvien_update_danhba(MaNV, snb, dt.Rows[i][4].ToString(), 1, 1);
                                        }    
                                    }
                                  
                                }    
                                
                                dtc++;
                            }
                            
                            string msg = "";
                            if (dtc != 0 && dtrung != 0)
                            {
                                msg = "Import được " + dtc + " dòng dữ liệu, " + "Có " + dtrung + " dòng trùng Mã NV cập nhập nội dung";
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

            return RedirectToAction("Index", "PhoneBook");
        }

        public JsonResult GetNhanvien(string search)
        {
            var result = (from u in db.Nhanvien_search_danhba(search)
                          select new DanhBaValidation
                          {
                              IDNV = u.IDNV,
                              MaNV = u.MaNV,
                              TenNV = u.TenNV,
                              MaCV = u.MaCV,
                              MaPB = u.MaPB,
                          }).ToList();
            //var result= (from u in db.Viev_NhanVien.Where(x=>x.TenNV.Contains(search) || x.MaNV.Contains(search) || x.Ho_Ten.Contains(search)))
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetSNB(string MaNV)
        {
            var result = (from u in db.DanhBas.Where(x=>x.MaNV==MaNV)
                          select new DanhBaValidation
                          {
                              MaNV = u.MaNV,
                              SoMayLe = u.SoMayLe
                          }).ToList();
            //var result= (from u in db.Viev_NhanVien.Where(x=>x.TenNV.Contains(search) || x.MaNV.Contains(search) || x.Ho_Ten.Contains(search)))
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

        public ActionResult UpdatePhoneBook()
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
        public ActionResult UpdatePhoneBook(DanhBaValidation _DO)
        {
            var rsnv = db.NhanViens.Where(x => x.MaNV == _DO.MaNV).SingleOrDefault();
            if(rsnv!=null)
            {
                var rscv = db.ChucVus.Where(x => x.MaCV == rsnv.MaCV).SingleOrDefault();
                if(rscv!=null)
                {  
                    var rsdb = db.DanhBas.Where(x => x.MaNV == _DO.MaNV).SingleOrDefault();
                    if(rsdb==null)
                    {
                        db.Nhanvien_update_danhba(_DO.MaNV, _DO.SoMayLe, rscv.TenCV, 1, 0);
                    }  
                    else
                    {
                        db.Nhanvien_update_danhba(_DO.MaNV, _DO.SoMayLe, rscv.TenCV, 1, 1);
                    }    
                    TempData["msgSuccess"] = "<script>alert('Cập nhập thành công');</script>";
                }
                else
                {
                    TempData["msgSuccess"] = "<script>alert('Có lỗi khi cập nhập danh bạ');</script>";
                }    
                return RedirectToAction("Index", "PhoneBook", new { MaPB = rsnv.MaPB });
            }
            else
            {
                TempData["msgSuccess"] = "<script>alert('Có lỗi khi cập nhập danh bạ');</script>";
                return RedirectToAction("Index", "PhoneBook", new { MaPB = _DO.MaPB });
            }

        }

        public ActionResult AddPhone()
        {
            return PartialView();
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
                string fileNamemau = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_danhba.xlsx";
                string fileNamemaunew = AppDomain.CurrentDomain.BaseDirectory + @"App_Data\Mau_danhba_temp.xlsx";
                XLWorkbook Workbook = new XLWorkbook(fileNamemau);
                IXLWorksheet Worksheet = Workbook.Worksheet("Name");
                if (MaPB == null) MaPB = 0;
                IList<DanhBaValidation> lDB = GetLDB(MaPB);
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

                        Worksheet.Cell("F" + row).Value = item.SoMayLe;
                        Worksheet.Cell("F" + row).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
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
                    string fileName = "Danhbanhanvien" + ".xlsx";
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
                TempData["msg"] = "<script>alert('" + ex + "');window.location.href = '/Progress/Department/" + MaPB + "'</script>";
                return View(TempData);
            }

        }
    }
}