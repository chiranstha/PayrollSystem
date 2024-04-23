using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetEmployeeLevelForEdit: EntityDto<Guid>
    {
        public string Name { get; set; }

    }
}