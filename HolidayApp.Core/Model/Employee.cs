using System;
using System.ComponentModel.DataAnnotations;

namespace HolidayApp.Core.Model
{
    public class Employee
    {
        public virtual int EmployeeId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public virtual DateTime BirthDate { get; set; }
        //public virtual Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public virtual DateTime HireDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public virtual DateTime? TerminationDate { get; set; }

        public virtual string Address { get; set; }

        public virtual long ContactNumber { get; set; }

        public virtual string PersonalEmail { get; set; }

        public virtual string Username { get; set; }

     //   public virtual EmployeeType EmployeeType { get; set; }

       // public virtual ICollection<Project> Projects { get; set; }
        public virtual string JobTitle { get; set; }

    }
}