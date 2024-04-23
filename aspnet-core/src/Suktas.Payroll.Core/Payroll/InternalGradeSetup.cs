using Suktas.Payroll.Municipality.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_InternalGradeSetup")]
    public class InternalGradeSetup : Entity<Guid>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        public virtual EmployeeCategory Category { get; set; }

        public virtual EmployeeGrade Grade { get; set; }

        public virtual bool IsPercent { get; set; }

        public virtual decimal Value { get; set; }

    }
}