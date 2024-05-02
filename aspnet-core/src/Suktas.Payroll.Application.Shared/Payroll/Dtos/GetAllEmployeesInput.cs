using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllEmployeesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? CategoryFilter { get; set; }

        public string PanNoFilter { get; set; }

        public string InsuranceNoFilter { get; set; }

        public string NameFilter { get; set; }

        public string EmployeeLevelNameFilter { get; set; }

        public string SchoolInfoNameFilter { get; set; }

    }
}