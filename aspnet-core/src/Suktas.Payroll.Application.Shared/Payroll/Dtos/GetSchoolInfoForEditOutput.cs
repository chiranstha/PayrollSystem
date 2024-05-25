using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetSchoolInfoForEditOutput : EntityDto<Guid>
    {
        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public byte[] ImageBytes { get; set; }
        public string ImageFileName { get; set; }
        public Guid? Image { get; set; }
    }
}