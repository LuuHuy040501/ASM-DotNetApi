using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
public class Service : IService
{
    private StudentDbContext _dbContext;
    public Service()
    {
        this._dbContext = new StudentDbContext();
    }
    public Student CreateStudent(Student student)
    {
        _dbContext.Students.Add(student);
        _dbContext.SaveChanges();

        return student;
    }

    public Student DeleteStudent(int id)
    {
        var toDelStd = GetStudentById(id);
        if(toDelStd == null)
        {
            return null;
        }
        _dbContext.Students.Remove(toDelStd);
        _dbContext.SaveChanges();
        return toDelStd;
    }

    public IEnumerable<Student> GetListStudents()
    {
        var listStudent = from s in _dbContext.Students select s;
       
        return listStudent;
    }

    public Student GetStudentById(int id)
    {
        var student = from s in _dbContext.Students where s.Id == id select s;
        return student.FirstOrDefault();
    }

    public string Hello(string name)
    {
        return "hello " + name;
    }

    public Student UpdateStudent(int id, Student newStudent)
    {
        var existStudent = _dbContext.Students.First(student => student.Id == id);
        if(existStudent == null)
        {
            return null;
        }
        //update
        existStudent.Rollnumber = newStudent.Rollnumber;
        existStudent.Introduction = newStudent.Introduction;
        existStudent.Genre = newStudent.Genre;
        existStudent.Email = newStudent.Email;
        existStudent.Name = newStudent.Name;
        existStudent.Birthday = newStudent.Birthday;
        _dbContext.SaveChanges();
        return existStudent;
    }
}
