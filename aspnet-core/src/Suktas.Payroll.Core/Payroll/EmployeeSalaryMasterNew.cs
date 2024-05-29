using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_EmployeeSalaryMasterNew")]
    public class EmployeeSalaryMasterNew : Entity<Guid>, IMayHaveTenant
    {
        public virtual int Year { get; set; }
        public virtual string Months { get; set; }
        public int? TenantId { get; set; }

    }
}
