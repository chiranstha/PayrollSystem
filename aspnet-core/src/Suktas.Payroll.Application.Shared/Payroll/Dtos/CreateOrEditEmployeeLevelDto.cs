using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateOrEditEmployeeLevelDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

    }
}