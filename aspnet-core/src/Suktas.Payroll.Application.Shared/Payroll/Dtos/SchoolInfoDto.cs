using System;
using Abp.Application.Services.Dto;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class SchoolInfoDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

    }
}