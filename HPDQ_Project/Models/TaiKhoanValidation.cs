using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Models
{
    public class TaiKhoanValidation
    {
        public int TKID { get; set; }
        public int IDNV { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string TenPB { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Nhập tên tài khoản.")]
        [MaxLength(50, ErrorMessage = "Vượt quá số kí tự 50.")]
       // [Remote("IsAlready", "Account", HttpMethod = "POST", ErrorMessage = "Tên tài khoản đã tồn tại.")]
        public string TenTK { get; set; }
        [Required(ErrorMessage = "Chọn phần mềm.")]
        public Nullable<int> MaPM { get; set; }
        public string TenPM { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string GhiChu { get; set; }
        [Required(ErrorMessage = "Chọn tình trạng tài khoản.")]
        public Nullable<int> MaTinhTrang { get; set; }

        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgayCap { get; set; }
    }
}