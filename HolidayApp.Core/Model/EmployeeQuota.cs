using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using HolidayApp.Validations;
using SalesFirst.Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace HolidayApp.Core.Model
{
    public class EmployeeQuota
    {
        [Key]
        public virtual int EmployeeQuotaId { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int PaidQuota { get; set; }

        public int NonPaidQuota { get; set; }


    }
}
