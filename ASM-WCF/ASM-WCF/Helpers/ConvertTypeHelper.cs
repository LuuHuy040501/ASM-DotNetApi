using ASM_WCF.Models;
using ASM_WCF.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASM_WCF.Helpers
{
    public class ConvertTypeHelper
    {

        public static StudentViewModel ConvertServiceStudentToViewStudent(Student serviceStudent)
        {
            var stdView = new StudentViewModel
            {
                Id = serviceStudent.Id,
                RolleNumber = serviceStudent.Rollnumber,
                Name = serviceStudent.Name,
                Birthday = serviceStudent.Birthday,
                Email = serviceStudent.Email,
                Genre = (StudentViewModel.StudentGenre)serviceStudent.Genre,
                Introduction = serviceStudent.Introduction
            };
            return stdView;
        }

        public static Student ConvertViewStudentToServiceStudent(StudentViewModel studentViewModel)
        {
            var student = new Student
            {
               
                Rollnumber = studentViewModel.RolleNumber,
                Birthday = studentViewModel.Birthday,
                Email = studentViewModel.Email,
                Genre = (int)studentViewModel.Genre,
                Introduction = studentViewModel.Introduction,
                Name = studentViewModel.Name
            };
            return student;
        }
    }
}