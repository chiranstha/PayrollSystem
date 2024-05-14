using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using System.Collections.Generic;

namespace Suktas.Payroll.Payroll
{
    public interface IGradeUpgradesAppService : IApplicationService
    {
        Task<PagedResultDto<GetGradeUpgradeForViewDto>> GetAll(GetAllGradeUpgradesInput input);

        Task<GetGradeUpgradeForViewDto> GetGradeUpgradeForView(Guid id);

        Task<GetGradeUpgradeForEditOutput> GetGradeUpgradeForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditGradeUpgradeDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetGradeUpgradesToExcel(GetAllGradeUpgradesForExcelInput input);

        Task<List<GradeUpgradeEmployeeLookupTableDto>> GetAllEmployeeForTableDropdown();

    }
}