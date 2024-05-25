using Suktas.Payroll.Municipality.Enum;
using Suktas.Payroll.Payroll;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_CategorySalary")]
    public class CategorySalary : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual decimal Salary { get; set; }

        public virtual EmployeeCategory Category { get; set; }

        public virtual decimal TechnicalAmount { get; set; }

        [Required]
        public virtual EmployeeGrade Grade { get; set; }
        public virtual Guid EmployeeLevelId { get; set; }

        [ForeignKey("EmployeeLevelId")]
        public EmployeeLevel EmployeeLevelFk { get; set; }

    }
}