using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CategorySalaryDto : EntityDto<Guid>
    {
        public decimal Salary { get; set; }

        public EmployeeCategory Category { get; set; }

        public decimal TechnicalAmount { get; set; }

        public Guid EmployeeLevelId { get; set; }

    }
}