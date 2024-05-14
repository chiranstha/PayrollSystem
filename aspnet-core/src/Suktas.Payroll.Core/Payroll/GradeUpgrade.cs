using Suktas.Payroll.Municipality.Enum;
using Suktas.Payroll.Payroll;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("GradeUpgrades")]
    public class GradeUpgrade : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual string DateMiti { get; set; }

        public virtual EmployeeGrade Grade { get; set; }

        public virtual string Remarks { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Guid? EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

    }
}