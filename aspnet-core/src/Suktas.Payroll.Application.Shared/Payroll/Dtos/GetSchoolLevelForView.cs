using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetSchoolLevelForView : EntityDto<Guid>
    {
        public string Name { get; set; }
        public string AliasName { get; set; }
    }
}
