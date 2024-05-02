using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllEmployeeSalaryForExcelInput
    {
        public string Filter { get; set; }

        public int? MonthFilter { get; set; }

        public decimal? MaxTechnicalAmountFilter { get; set; }
        public decimal? MinTechnicalAmountFilter { get; set; }

        public decimal? MaxTotalGradeAmountFilter { get; set; }
        public decimal? MinTotalGradeAmountFilter { get; set; }

        public decimal? MaxInsuranceAmountFilter { get; set; }
        public decimal? MinInsuranceAmountFilter { get; set; }

        public decimal? MaxTotalSalaryFilter { get; set; }
        public decimal? MinTotalSalaryFilter { get; set; }

        public decimal? MaxDearnessAllowanceFilter { get; set; }
        public decimal? MinDearnessAllowanceFilter { get; set; }

        public decimal? MaxTotalWithAllowanceFilter { get; set; }
        public decimal? MinTotalWithAllowanceFilter { get; set; }

        public decimal? MaxGovernmentAmountFilter { get; set; }
        public decimal? MinGovernmentAmountFilter { get; set; }

        public decimal? MaxInternalAmountFilter { get; set; }
        public decimal? MinInternalAmountFilter { get; set; }

        public decimal? MaxPaidSalaryAmountFilter { get; set; }
        public decimal? MinPaidSalaryAmountFilter { get; set; }

        public string SchoolInfoNameFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

        public string EmployeeLevelNameFilter { get; set; }

    }
}