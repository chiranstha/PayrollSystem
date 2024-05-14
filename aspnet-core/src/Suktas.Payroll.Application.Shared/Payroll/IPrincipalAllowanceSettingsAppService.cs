using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll
{
    public interface IPrincipalAllowanceSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetPrincipalAllowanceSettingForViewDto>> GetAll(GetAllPrincipalAllowanceSettingsInput input);

        Task<GetPrincipalAllowanceSettingForViewDto> GetPrincipalAllowanceSettingForView(Guid id);

        Task<GetPrincipalAllowanceSettingForEditOutput> GetPrincipalAllowanceSettingForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditPrincipalAllowanceSettingDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetPrincipalAllowanceSettingsToExcel(GetAllPrincipalAllowanceSettingsForExcelInput input);

        Task<List<PrincipalAllowanceSettingEmployeeLevelLookupTableDto>> GetAllEmployeeLevelForTableDropdown();

    }
}