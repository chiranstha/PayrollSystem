using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CategorySalaryEmployeeLevelLookupTableDto
    {
        public Guid Id { get; set; }

        public string DisplayName { get; set; }
    }
}