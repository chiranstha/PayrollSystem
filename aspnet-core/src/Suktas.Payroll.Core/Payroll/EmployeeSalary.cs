using Suktas.Payroll.Municipality.Enum;
using Suktas.Payroll.Payroll;
using Suktas.Payroll.Payroll;
using Suktas.Payroll.Payroll;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Suktas.Payroll.Payroll
{
    [Table("tbl_EmployeeSalary")]
    public class EmployeeSalary : Entity<Guid>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual Months Month { get; set; }

        public virtual string DateMiti { get; set; }

        public virtual decimal BasicSalary { get; set; }

        public virtual decimal GradeAmount { get; set; }

        public virtual decimal TechnicalAmount { get; set; }

        public virtual decimal TotalGradeAmount { get; set; }

        public virtual decimal TotalBasicSalary { get; set; }

        public virtual decimal InsuranceAmount { get; set; }

        public virtual decimal TotalSalary { get; set; }

        public virtual decimal DearnessAllowance { get; set; }

        public virtual decimal PrincipalAllowance { get; set; }

        public virtual decimal TotalWithAllowance { get; set; }

        public virtual int TotalMonth { get; set; }

        public virtual decimal TotalSalaryAmount { get; set; }

        public virtual decimal FestiableAllowance { get; set; }

        public virtual decimal GovernmentAmount { get; set; }

        public virtual decimal InternalAmount { get; set; }

        public virtual decimal PaidSalaryAmount { get; set; }

        public virtual Guid SchoolInfoId { get; set; }

        [ForeignKey("SchoolInfoId")]
        public SchoolInfo SchoolInfoFk { get; set; }

        public virtual Guid EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee EmployeeFk { get; set; }

        public virtual Guid EmployeeLevelId { get; set; }

        [ForeignKey("EmployeeLevelId")]
        public EmployeeLevel EmployeeLevelFk { get; set; }

    }
}