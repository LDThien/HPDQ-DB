using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPDQ_Project.Models
{
    public class NhanVienValidation
    {
        public int IDNV { get; set; }
        [Required(ErrorMessage = "Nhập mã nhân viên.")]
        [MaxLength(9, ErrorMessage = "Vượt quá số kí tự 9.")]
        [Remote("IsAlready", "Employees", HttpMethod = "POST", ErrorMessage = "Mã nhân viên đã tồn tại.")]
        public string MaNV { get; set; }

        [Required(ErrorMessage = "Nhập họ tên nhân viên.")]
        [MaxLength(50, ErrorMessage = "Vượt quá số kí tự 50.")]
        public string TenNV { get; set; }

        [Required(ErrorMessage = "Chọn chức vụ.")]
        public Nullable<int> MaCV { get; set; }

        [Required(ErrorMessage = "Chọn Bộ phận / Nhà máy.")]
        public Nullable<int> MaPB { get; set; }

        [MaxLength(200, ErrorMessage = "Vượt quá số kí tự 200.")]
        public string DiaChi { get; set; }
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",ErrorMessage = "Số điện thoại không đúng.")]
        [MaxLength(11, ErrorMessage = "Vượt quá số kí tự 11.")]
        public string SDT { get; set; }

        public Nullable<int> SoMayLe { get; set; }

        [Required(ErrorMessage = "Chọn giới tính.")]
        public Nullable<int> MaGT { get; set; }

        //[Required(ErrorMessage = "Nhập Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không đúng.")]
        public string Email { get; set; }

        [DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? NgayVaoLam { get; set; }
        //public DateTime DateChosen { get; set; }
        public string TenCV { get; set; }
        public string TenPB { get; set; }
        public string TenGT { get; set; }
    }
   
}