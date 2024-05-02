using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateOrEditEmployeeSalaryDto : EntityDto<Guid?>
    {

        public Months Month { get; set; }

        public string DateMiti { get; set; }

        public decimal BasicSalary { get; set; }

        public decimal GradeAmount { get; set; }

        public decimal TechnicalAmount { get; set; }

        public decimal TotalGradeAmount { get; set; }

        public decimal TotalBasicSalary { get; set; }

        public decimal InsuranceAmount { get; set; }

        public decimal TotalSalary { get; set; }

        public decimal DearnessAllowance { get; set; }

        public decimal PrincipalAllowance { get; set; }

        public decimal TotalWithAllowance { get; set; }

        public int TotalMonth { get; set; }

        public decimal TotalSalaryAmount { get; set; }

        public decimal FestiableAllowance { get; set; }

        public decimal GovernmentAmount { get; set; }

        public decimal InternalAmount { get; set; }

        public decimal PaidSalaryAmount { get; set; }

        public Guid SchoolInfoId { get; set; }

        public Guid EmployeeId { get; set; }

        public Guid EmployeeLevelId { get; set; }

    }
}