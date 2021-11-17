using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HPDQ_Project.Models
{
    public class PhongBanValidation
    {
        [Required(ErrorMessage = "Nhập mã phòng ban")]
        public int? MaPB { get; set; }

        [Required(ErrorMessage = "Nhập tên phòng ban")]
        [MaxLength(50, ErrorMessage = "vượt quá số kí tự 50")]
        public string TenPB { get; set; }
    }
}