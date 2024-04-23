using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Suktas.Payroll.Payroll.Exporting;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Authorization;
using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Suktas.Payroll.Municipality.Enum;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_InternalGradeSetup)]
    public class InternalGradeSetupAppService : PayrollAppServiceBase, IInternalGradeSetupAppService
    {
        private readonly IRepository<InternalGradeSetup, Guid> _internalGradeSetupRepository;
        private readonly IInternalGradeSetupExcelExporter _internalGradeSetupExcelExporter;

        public InternalGradeSetupAppService(IRepository<InternalGradeSetup, Guid> internalGradeSetupRepository,
            IInternalGradeSetupExcelExporter internalGradeSetupExcelExporter)
        {
            _internalGradeSetupRepository = internalGradeSetupRepository;
            _internalGradeSetupExcelExporter = internalGradeSetupExcelExporter;

        }

        public virtual async Task<PagedResultDto<GetInternalGradeSetupForViewDto>> GetAll(GetAllInternalGradeSetupInput input)
        {

            var filteredInternalGradeSetup = _internalGradeSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false);

            var pagedAndFilteredInternalGradeSetup = filteredInternalGradeSetup
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var internalGradeSetup = from o in pagedAndFilteredInternalGradeSetup
                                     select new
                                     {

                                         o.Category,
                                         o.Grade,
                                         o.IsPercent,
                                         o.Value,
                                         o.Id
                                     };

            var totalCount = await filteredInternalGradeSetup.CountAsync();

            var dbList = await internalGradeSetup.ToListAsync();
            var results = new List<GetInternalGradeSetupForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetInternalGradeSetupForViewDto()
                {
                    Category = o.Category.ToString(),
                    Grade = o.Grade.ToString(),
                    IsPercent = o.IsPercent,
                    Value = o.Value,
                    Id = o.Id,

                };

                results.Add(res);
            }

            return new PagedResultDto<GetInternalGradeSetupForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetInternalGradeSetupForViewDto> GetInternalGradeSetupForView(Guid id)
        {
            var internalGradeSetup = await _internalGradeSetupRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == id);
            if (internalGradeSetup == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetInternalGradeSetupForViewDto
            {
                Id = internalGradeSetup.Id,
                Category = internalGradeSetup.Category.ToString(),
                Grade = internalGradeSetup.Grade.ToString(),
                IsPercent = internalGradeSetup.IsPercent,
                Value = internalGradeSetup.Value
            };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_InternalGradeSetup_Edit)]
        public virtual async Task<GetInternalGradeSetupForEditOutput> GetInternalGradeSetupForEdit(EntityDto<Guid> input)
        {
            var internalGradeSetup = await _internalGradeSetupRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (internalGradeSetup == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetInternalGradeSetupForEditOutput
            {
                Id = internalGradeSetup.Id,
                Category = internalGradeSetup.Category,
                Grade = internalGradeSetup.Grade,
                IsPercent = internalGradeSetup.IsPercent,
                Value = internalGradeSetup.Value
            };
            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditInternalGradeSetupDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_InternalGradeSetup_Create)]
        protected virtual async Task Create(CreateOrEditInternalGradeSetupDto input)
        {
            var internalGradeSetup = new InternalGradeSetup
            {
                Id = Guid.Empty,
                TenantId = AbpSession.GetTenantId(),
                Category = input.Category,
                Grade = input.Grade,
                IsPercent = input.IsPercent,
                Value = input.Value
            };

            await _internalGradeSetupRepository.InsertAsync(internalGradeSetup);

        }

        [AbpAuthorize(AppPermissions.Pages_InternalGradeSetup_Edit)]
        protected virtual async Task Update(CreateOrEditInternalGradeSetupDto input)
        {
            var internalGradeSetup = await _internalGradeSetupRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (internalGradeSetup == null)
                throw new UserFriendlyException("Data not found");

            internalGradeSetup.Category = input.Category;
            internalGradeSetup.Grade = input.Grade;
            internalGradeSetup.IsPercent = input.IsPercent;
            internalGradeSetup.Value = input.Value;

            await _internalGradeSetupRepository.UpdateAsync(internalGradeSetup);
        }

        [AbpAuthorize(AppPermissions.Pages_InternalGradeSetup_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            var internalGradeSetup = await _internalGradeSetupRepository.GetAll()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (internalGradeSetup == null)
                throw new UserFriendlyException("Data not found");
            await _internalGradeSetupRepository.DeleteAsync(internalGradeSetup.Id);
        }

        public virtual async Task<FileDto> GetInternalGradeSetupToExcel(GetAllInternalGradeSetupForExcelInput input)
        {

            var filteredInternalGradeSetup = _internalGradeSetupRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false);

            var query = (from o in filteredInternalGradeSetup
                select new GetInternalGradeSetupForViewDto()
                {
                    Category = o.Category.ToString(),
                    Grade = o.Grade.ToString(),
                    IsPercent = o.IsPercent,
                    Value = o.Value,
                    Id = o.Id

                });

            var internalGradeSetupListDtos = await query.ToListAsync();

            return _internalGradeSetupExcelExporter.ExportToFile(internalGradeSetupListDtos);
        }

    }
}