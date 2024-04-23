using Suktas.Payroll.Municipality.Enum;
using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetCategorySalaryForViewDto : EntityDto<Guid>
    {
        public decimal Salary { get; set; }

        public string Category { get; set; }

        public decimal TechnicalAmount { get; set; }

        public string EmployeeLevelName { get; set; }

    }
}