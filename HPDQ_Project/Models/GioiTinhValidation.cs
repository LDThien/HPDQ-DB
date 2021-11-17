using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HPDQ_Project.Models
{
    public class GioiTinhValidation
    {

        [Required(ErrorMessage = "Nhập mã giới tính")]
        public int? MaGT { get; set; }

        [Required(ErrorMessage = "Nhập tên giới tính")]
        [MaxLength(50, ErrorMessage = "vượt quá số kí tự 50")]
        public string TenGT { get; set; }
    }
}