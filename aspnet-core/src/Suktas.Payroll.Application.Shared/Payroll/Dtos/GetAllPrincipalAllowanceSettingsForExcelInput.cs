using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllPrincipalAllowanceSettingsForExcelInput
    {
        public string Filter { get; set; }

        public decimal? MaxAmountFilter { get; set; }
        public decimal? MinAmountFilter { get; set; }

        public string EmployeeLevelNameFilter { get; set; }

    }
}