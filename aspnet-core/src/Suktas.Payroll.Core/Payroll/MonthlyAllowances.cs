using Abp.Domain.Entities;
using Suktas.Payroll.Municipality.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_MonthlyAllowances")]
    public class MonthlyAllowances : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public EmployeeCategory EmployeeCategory { get; set; }
    }
}
