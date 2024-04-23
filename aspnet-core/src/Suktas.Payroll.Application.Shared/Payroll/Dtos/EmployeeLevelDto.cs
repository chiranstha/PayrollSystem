using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public abstract class EmployeeLevelDtoBase : EntityDto<Guid>
    {
        public string Name { get; set; }

    }
}