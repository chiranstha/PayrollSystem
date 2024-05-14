using Abp.Domain.Entities;
using Suktas.Payroll.Municipality.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_EmployeeLevel")]
    public class EmployeeLevel : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        public virtual string Name { get; set; }


        [Required]
        public virtual EmployeeGrade Grade { get; set; }

    }
}