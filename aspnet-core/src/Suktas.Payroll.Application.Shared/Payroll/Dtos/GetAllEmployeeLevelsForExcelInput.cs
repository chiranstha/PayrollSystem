using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public abstract class GetAllEmployeeLevelsForExcelInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}