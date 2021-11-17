using HPDQ_Project.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HPDQ_Project.Models
{
    public class DangKyValidation
    {
        public int TKID { get; set; }

        public string HoTen { get; set; }

        [Required(ErrorMessage = "Nhập tài khoản")]
        [RegularExpression(@"[A-Za-z0-9]*$", ErrorMessage = "Tài khoản chứa kí tự đặc biệt")]
        [MaxLength(30, ErrorMessage = "Vượt quá số kí tự 30")]
        public string TenDangNhap { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Nhập mật khẩu")]
        [MaxLength(50, ErrorMessage = "Vượt quá số kí tự 50")]
        public string MatKhau { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Xác nhận mật khẩu")]
        [MaxLength(50, ErrorMessage = "Vượt quá số kí tự 50")]
        [Compare("MatKhau", ErrorMessage = "Xác nhận không khớp")]
        public string NhapLaiMatKhau { get; set; }

        public string MatKhauCu { get; set; }

        [Required(ErrorMessage = "Email")]
        public string Email { get; set; }


        public int? MaQuyen { get; set; }

        public string TenQuyen { get; set; }

        public int? MaPB { get; set; }

        public string TenPB { get; set; }

        public virtual QuyenDangNhap QuyenDangNhap { get; set; }

        public virtual PhongBan PhongBan { get; set; }




    }
}