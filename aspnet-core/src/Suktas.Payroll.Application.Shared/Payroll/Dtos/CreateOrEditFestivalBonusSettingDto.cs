using Suktas.Payroll.Municipality.Enum;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateOrEditFestivalBonusSettingDto : EntityDto<Guid?>
    {

        public Months MonthId { get; set; }

        public string Remarks { get; set; }

    }
}