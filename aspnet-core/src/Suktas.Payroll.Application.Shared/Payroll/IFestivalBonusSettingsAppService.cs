using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;

namespace Suktas.Payroll.Payroll
{
    public interface IFestivalBonusSettingsAppService : IApplicationService
    {
        Task<PagedResultDto<GetFestivalBonusSettingForViewDto>> GetAll(GetAllFestivalBonusSettingsInput input);

        Task<GetFestivalBonusSettingForViewDto> GetFestivalBonusSettingForView(Guid id);

        Task<GetFestivalBonusSettingForEditOutput> GetFestivalBonusSettingForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditFestivalBonusSettingDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetFestivalBonusSettingsToExcel(GetAllFestivalBonusSettingsForExcelInput input);

    }
}