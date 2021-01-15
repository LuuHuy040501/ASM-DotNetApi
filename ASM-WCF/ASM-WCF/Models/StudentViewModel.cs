using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASM_WCF.Models
{
    public class StudentViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "MSSV không thể để trống")]
        [Display(Name ="MSSV")]
        public string RolleNumber { get; set; }

        [Required(ErrorMessage = "Tên sinh viên không thể để trống")]
        [Display(Name = "Tên Sinh Viên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ngày sinh không thể bỏ trống")]
        [Display(Name = "Ngày sinh")]
        public DateTime Birthday { get; set; }


        [Display(Name = "Giới tính")]
        public StudentGenre Genre { get; set; }

        [Required(ErrorMessage = "Hãy nhập email")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Email không hợp lệ vd: sinhvien@gmail.com")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mục này không được để trống")]
        [Display(Name = "Giới thiệu bản thân")]
        public string Introduction { get; set; }
   
        public enum StudentGenre
        {
            [Display(Name = "Nữ")]
            Female = 0,
            [Display(Name = "Nam")]
            Male = 1
        }
    }
}