using Abp.Domain.Entities;
using Suktas.Payroll.Municipality.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_EmployeeSalaryMasterMonthNew")]
    public class EmployeeSalaryMasterMonthNew : Entity<Guid>, IMayHaveTenant
    {
        public Months Month { get; set; }
        public Guid EmployeeSalaryMasterNewId { get; set; }
        [ForeignKey("EmployeeSalaryMasterNewId")]
        public EmployeeSalaryMasterNew EmployeeSalaryMasterNew { get; set; }
        public int? TenantId { get; set; }
    }
}
