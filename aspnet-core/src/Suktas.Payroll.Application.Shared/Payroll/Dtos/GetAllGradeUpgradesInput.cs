using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllGradeUpgradesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string DateMitiFilter { get; set; }

        public int? GradeFilter { get; set; }

        public string RemarksFilter { get; set; }

        public string EmployeeNameFilter { get; set; }

    }
}