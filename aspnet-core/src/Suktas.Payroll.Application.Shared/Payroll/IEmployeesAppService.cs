using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll
{
    public interface IEmployeesAppService : IApplicationService
    {
        Task<PagedResultDto<GetEmployeeForViewDto>> GetAll(GetAllEmployeesInput input);

        Task<GetEmployeeForViewDto> GetEmployeeForView(Guid id);

        Task<CreateOrEditEmployeeDto> GetEmployeeForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditEmployeeDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetEmployeesToExcel(GetAllEmployeesForExcelInput input);

        Task<List<EmployeeEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown();

        Task<List<EmployeeSchoolInfoLookupTableDto>> GetAllSchoolInfoForTableDropdown();

    }
}