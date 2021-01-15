using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

/// <summary>
/// Summary description for Student
/// </summary>
/// 
[DataContract]
public class Student
{
    [DataMember]
    public int Id { get; set; }
    [DataMember]
    public string Rollnumber { get; set; }
    [DataMember]
    public string Name { get; set; }
    [DataMember]
    public DateTime Birthday { get; set; }
    [DataMember]
    
    public string Email { get; set; }
    [DataMember]

    public string Introduction { get; set; }
    [DataMember]

    public int Genre { get; set; } // 1 nam 0 nữ
    public Student()
    {
        //
        // TODO: Add constructor logic here
        //
    }
}