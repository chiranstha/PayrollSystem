using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class CreateOrEditSchoolInfoDto : EntityDto<Guid?>
    {

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNo { get; set; }

        public string Email { get; set; }

        public int WardNo { get; set; }
        public string Description { get; set; }
        public Guid SchooolLevelId { get; set; }

        public Guid? Image { get; set; }

        public string ImageToken { get; set; }

    }
}