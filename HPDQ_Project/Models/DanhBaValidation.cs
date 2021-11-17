using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Models
{
    public class DanhBaValidation
    {
        public int IDNV { get; set; }
        [Required(ErrorMessage = "Nhập mã nhân viên.")]
        //[MaxLength(9, ErrorMessage = "Vượt quá số kí tự 9.")]
        //[Remote("IsAlready", "PhoneBook", HttpMethod = "POST", ErrorMessage = "Mã nhân viên đã tồn tại.")]
        public string MaNV { get; set; }

        [Required(ErrorMessage = "Nhập họ tên nhân viên.")]
        [MaxLength(50, ErrorMessage = "Vượt quá số kí tự 50.")]
        public string TenNV { get; set; }

        [Required(ErrorMessage = "Chọn chức vụ.")]
        public Nullable<int> MaCV { get; set; }

        [Required(ErrorMessage = "Chọn Bộ phận / Nhà máy.")]
        public Nullable<int> MaPB { get; set; }
        public Nullable<int> SoMayLe { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string TenCV { get; set; }
        public string TenPB { get; set; }
        public string TenGT { get; set; }
        public string ViTri { get; set; }
    }
}