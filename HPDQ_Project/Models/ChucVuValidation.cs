using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HPDQ_Project.Models
{
    public class ChucVuValidation
    {
        [Required(ErrorMessage = "Nhập mã chức vụ")]
        public int? MaCV { get; set; }

        [Required(ErrorMessage = "Nhập tên chức vụ")]
        [MaxLength(50, ErrorMessage = "vượt quá số kí tự 50")]
        public string TenCV { get; set; }
    }
}