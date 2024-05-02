using System;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll.Dtos
{
    public class GetEmployeeForViewDto: EntityDto<Guid>
    {
        public string Category { get; set; }

        public string ProvidentFund { get; set; }

        public string PanNo { get; set; }

        public string Name { get; set; }

        public string BankAccountNo { get; set; }

        public string DateOfJoinMiti { get; set; }

        public bool IsDearnessAllowance { get; set; }

        public bool IsPrincipal { get; set; }

        public bool IsGovernment { get; set; }

        public bool IsInternal { get; set; }


        public string EmployeeLevelName { get; set; }

        public string SchoolInfoName { get; set; }

    }
}