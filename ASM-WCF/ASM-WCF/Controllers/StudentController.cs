using ASM_WCF.Helpers;
using ASM_WCF.Models;
using ASM_WCF.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASM_WCF.Controllers
{
    public class StudentController : Controller
    {

        // GET: Student
        public StudentController()
        {

        }
        public ActionResult Index()
        {
            List<StudentViewModel> listStudent = new List<StudentViewModel>();
            ServiceClient serviceClient = new ServiceClient();
            var inDbListStd = serviceClient.GetListStudents();
            //convert to view model
            foreach (var std in inDbListStd)
            {
                var studentViewModel =  ConvertTypeHelper.ConvertServiceStudentToViewStudent(std);
                listStudent.Add(studentViewModel);
            }
            serviceClient.Close();
            return View(listStudent);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                ServiceClient client = new ServiceClient();
                //convert to servive student

                var inDatabaseStudent = ConvertTypeHelper.ConvertViewStudentToServiceStudent(student);
                var res = client.CreateStudent(inDatabaseStudent);
                if (res != null)
                {
                    return RedirectToAction("Index");
                }
                client.Close();
            }
            return View(student);

        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            ServiceClient client = new ServiceClient();
            var serviceStd = client.GetStudentById(id);
            var viewStd = ConvertTypeHelper.ConvertServiceStudentToViewStudent(serviceStd);
            if(viewStd == null)
            {
                return RedirectToAction("Index");
            }
            return View(viewStd);
        }
        [HttpPost]
        public ActionResult Edit(StudentViewModel student)
        {
            if(ModelState.IsValid)
            {
                ServiceClient client = new ServiceClient();
                client.UpdateStudent(student.Id, ConvertTypeHelper.ConvertViewStudentToServiceStudent(student));
                client.Close();
                return RedirectToAction("Index");
                
            }
            return View(student);
        }

        public ActionResult Delete(int id)
        {
            ServiceClient client = new ServiceClient();
            client.DeleteStudent(id);
            client.Close();
            return RedirectToAction("Index");
        }
    }
}