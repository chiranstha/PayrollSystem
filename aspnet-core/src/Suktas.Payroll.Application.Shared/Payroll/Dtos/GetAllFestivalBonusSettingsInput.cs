using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllFestivalBonusSettingsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? MonthIdFilter { get; set; }

        public string RemarksFilter { get; set; }

    }
}