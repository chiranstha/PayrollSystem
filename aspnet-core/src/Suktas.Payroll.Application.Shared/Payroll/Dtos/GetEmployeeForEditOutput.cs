using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetEmployeeForEditOutput
    {
        public CreateOrEditEmployeeDto Employee { get; set; }

        public string EmployeeLevelName { get; set; }

        public string SchoolInfoName { get; set; }

    }
}