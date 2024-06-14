using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetSchoolLevelForEdit : EntityDto<Guid>
    {
        public string AliasName { get; set; }
        public string Name { get; set; }

    }
}
