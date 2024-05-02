using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetEmployeeSalaryForEditOutput
    {
        public CreateOrEditEmployeeSalaryDto EmployeeSalary { get; set; }

        public string SchoolInfoName { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeLevelName { get; set; }

    }
}