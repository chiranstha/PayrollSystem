using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class FestivalBonusSettingDto : EntityDto<Guid>
    {
        public Months MonthId { get; set; }

        public string Remarks { get; set; }

    }
}