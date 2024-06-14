using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllSchoolLevelsForExcelInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
