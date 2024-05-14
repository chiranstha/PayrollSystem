using Abp.Application.Services.Dto;
using System;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetAllFestivalBonusSettingsForExcelInput
    {
        public string Filter { get; set; }

        public int? MonthIdFilter { get; set; }

        public string RemarksFilter { get; set; }

    }
}