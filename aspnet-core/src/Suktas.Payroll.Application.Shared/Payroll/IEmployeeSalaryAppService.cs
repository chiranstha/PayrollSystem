using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll
{
    public interface IEmployeeSalaryAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeeSalaryForViewDto>> GetAll(GetAllEmployeeSalaryInput input);

        Task<GetEmployeeSalaryForViewDto> GetEmployeeSalaryForView(Guid id);

        Task<CreateOrEditEmployeeSalaryDto> GetEmployeeSalaryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditEmployeeSalaryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetEmployeeSalaryToExcel(GetAllEmployeeSalaryForExcelInput input);

        Task<List<EmployeeSalarySchoolInfoLookupTableDto>> GetAllSchoolInfoForTableDropdown();

        Task<List<EmployeeSalaryEmployeeLookupTableDto>> GetAllEmployeeForTableDropdown();

        Task<List<EmployeeSalaryEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown();

    }
}