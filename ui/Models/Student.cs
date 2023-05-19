using System;
using System.Collections.Generic;
namespace myWebApp.Models
{
  public class Student
  {
    public int id { get; set; }
    public string lastName { get; set; }
    public string firstMidName { get; set; }
    public DateTime EnrollmentDate { get; set; }
  }
}