using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllEmployeeLevelsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

    }
}