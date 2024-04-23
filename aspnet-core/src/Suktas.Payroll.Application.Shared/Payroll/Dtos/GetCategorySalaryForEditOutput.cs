using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetCategorySalaryForEditOutput : EntityDto<Guid>
    {
        public decimal Salary { get; set; }

        public EmployeeCategory Category { get; set; }

        public decimal TechnicalAmount { get; set; }

        public Guid EmployeeLevelId { get; set; }
    }
}