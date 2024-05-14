using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Authorization;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Municipality.Enum;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Payroll.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_FestivalBonusSettings)]
    public class FestivalBonusSettingsAppService : PayrollAppServiceBase, IFestivalBonusSettingsAppService
    {
        private readonly IRepository<FestivalBonusSetting, Guid> _festivalBonusSettingRepository;
        private readonly IFestivalBonusSettingsExcelExporter _festivalBonusSettingsExcelExporter;

        public FestivalBonusSettingsAppService(IRepository<FestivalBonusSetting, Guid> festivalBonusSettingRepository, IFestivalBonusSettingsExcelExporter festivalBonusSettingsExcelExporter)
        {
            _festivalBonusSettingRepository = festivalBonusSettingRepository;
            _festivalBonusSettingsExcelExporter = festivalBonusSettingsExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetFestivalBonusSettingForViewDto>> GetAll(GetAllFestivalBonusSettingsInput input)
        {
            var monthIdFilter = input.MonthIdFilter.HasValue
                        ? (Months)input.MonthIdFilter
                        : default;

            var filteredFestivalBonusSettings = _festivalBonusSettingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MonthIdFilter.HasValue && input.MonthIdFilter > -1, e => e.MonthId == monthIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks.Contains(input.RemarksFilter));

            var pagedAndFilteredFestivalBonusSettings = filteredFestivalBonusSettings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var festivalBonusSettings = from o in pagedAndFilteredFestivalBonusSettings
                                        select new
                                        {

                                            o.MonthId,
                                            o.Remarks,
                                            Id = o.Id
                                        };

            var totalCount = await filteredFestivalBonusSettings.CountAsync();

            var dbList = await festivalBonusSettings.ToListAsync();
            var results = new List<GetFestivalBonusSettingForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetFestivalBonusSettingForViewDto()
                {
                    MonthId = o.MonthId,
                    Remarks = o.Remarks,
                    Id = o.Id,

                };

                results.Add(res);
            }

            return new PagedResultDto<GetFestivalBonusSettingForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetFestivalBonusSettingForViewDto> GetFestivalBonusSettingForView(Guid id)
        {
            var festivalBonusSetting = await _festivalBonusSettingRepository.GetAsync(id);

            var output = new GetFestivalBonusSettingForViewDto
            {
                MonthId = festivalBonusSetting.MonthId,
                Remarks = festivalBonusSetting.Remarks,
                Id = festivalBonusSetting.Id,
            };
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FestivalBonusSettings_Edit)]
        public virtual async Task<GetFestivalBonusSettingForEditOutput> GetFestivalBonusSettingForEdit(EntityDto<Guid> input)
        {
            var festivalBonusSetting = await _festivalBonusSettingRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFestivalBonusSettingForEditOutput
            {
                Id = festivalBonusSetting.Id,
                MonthId = festivalBonusSetting.MonthId,
                Remarks = festivalBonusSetting.Remarks
            };

            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditFestivalBonusSettingDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_FestivalBonusSettings_Create)]
        protected virtual async Task Create(CreateOrEditFestivalBonusSettingDto input)
        {
            var festivalBonusSetting = new FestivalBonusSetting
            {
                MonthId = input.MonthId,
                Remarks = input.Remarks
            };

            if (AbpSession.TenantId != null)
            {
                festivalBonusSetting.TenantId = (int?)AbpSession.TenantId;
            }

            await _festivalBonusSettingRepository.InsertAsync(festivalBonusSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_FestivalBonusSettings_Edit)]
        protected virtual async Task Update(CreateOrEditFestivalBonusSettingDto input)
        {
            var festivalBonusSetting = await _festivalBonusSettingRepository.FirstOrDefaultAsync((Guid)input.Id);
            if (festivalBonusSetting == null)
                throw new UserFriendlyException("Data not found");
            else
            {
                festivalBonusSetting.Remarks = input.Remarks;
                festivalBonusSetting.MonthId = input.MonthId;
                await _festivalBonusSettingRepository.UpdateAsync(festivalBonusSetting);
            }
            ObjectMapper.Map(input, festivalBonusSetting);

        }

        [AbpAuthorize(AppPermissions.Pages_FestivalBonusSettings_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            await _festivalBonusSettingRepository.DeleteAsync(input.Id);
        }

        public virtual async Task<FileDto> GetFestivalBonusSettingsToExcel(GetAllFestivalBonusSettingsForExcelInput input)
        {
            var monthIdFilter = input.MonthIdFilter.HasValue
                        ? (Months)input.MonthIdFilter
                        : default;

            var filteredFestivalBonusSettings = _festivalBonusSettingRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Remarks.Contains(input.Filter))
                        .WhereIf(input.MonthIdFilter.HasValue && input.MonthIdFilter > -1, e => e.MonthId == monthIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RemarksFilter), e => e.Remarks.Contains(input.RemarksFilter));

            var query = (from o in filteredFestivalBonusSettings
                         select new GetFestivalBonusSettingForViewDto()
                         {
                             MonthId = o.MonthId,
                             Remarks = o.Remarks,
                             Id = o.Id

                         });

            var festivalBonusSettingListDtos = await query.ToListAsync();

            return _festivalBonusSettingsExcelExporter.ExportToFile(festivalBonusSettingListDtos);
        }

    }
}