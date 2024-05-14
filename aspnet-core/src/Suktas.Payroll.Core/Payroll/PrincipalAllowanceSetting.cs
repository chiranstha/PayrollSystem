using Suktas.Payroll.Payroll;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_PrincipalAllowanceSettings")]
    public class PrincipalAllowanceSetting : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual Guid EmployeeLevelId { get; set; }

        [ForeignKey("EmployeeLevelId")]
        public EmployeeLevel EmployeeLevelFk { get; set; }

    }
}