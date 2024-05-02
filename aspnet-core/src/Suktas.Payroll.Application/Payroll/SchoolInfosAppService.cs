using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Suktas.Payroll.Payroll.Exporting;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using Suktas.Campus.Configuration.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_SchoolInfos)]
    public class SchoolInfosAppService : PayrollAppServiceBase, ISchoolInfosAppService
    {
        private readonly IRepository<SchoolInfo, Guid> _schoolInfoRepository;
        private readonly ISchoolInfosExcelExporter _schoolInfosExcelExporter;

        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public SchoolInfosAppService(IRepository<SchoolInfo, Guid> schoolInfoRepository,
            ISchoolInfosExcelExporter schoolInfosExcelExporter
            , ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager)
        {
            _schoolInfoRepository = schoolInfoRepository;
            _schoolInfosExcelExporter = schoolInfosExcelExporter;

            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;

        }

        public virtual async Task<PagedResultDto<GetSchoolInfoForViewDto>> GetAll(GetAllSchoolInfosInput input)
        {

            var filteredSchoolInfos = _schoolInfoRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.PhoneNo.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Description.Contains(input.Filter));

            var pagedAndFilteredSchoolInfos = filteredSchoolInfos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var schoolInfos = from o in pagedAndFilteredSchoolInfos
                              select new
                              {

                                  o.Name,
                                  o.Address,
                                  o.PhoneNo,
                                  o.Email,
                                  o.Id
                              };

            var totalCount = await filteredSchoolInfos.CountAsync();

            var dbList = await schoolInfos.ToListAsync();
            var results = dbList.Select(o => new GetSchoolInfoForViewDto
                {
                    Name = o.Name,
                    Address = o.Address,
                    PhoneNo = o.PhoneNo,
                    Email = o.Email,
                    Id = o.Id,
                })
                .ToList();

            return new PagedResultDto<GetSchoolInfoForViewDto>(
                totalCount,
                results
            );

        }

        public virtual async Task<GetSchoolInfoForViewDto> GetSchoolInfoForView(Guid id)
        {
            var schoolInfo = await _schoolInfoRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == id);
            if (schoolInfo == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetSchoolInfoForViewDto
            {
                Id = schoolInfo.Id,
                Name = schoolInfo.Name,
                Address = schoolInfo.Address,
                PhoneNo = schoolInfo.PhoneNo,
                Email = schoolInfo.Email
            };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SchoolInfos_Edit)]
        public virtual async Task<GetSchoolInfoForEditOutput> GetSchoolInfoForEdit(EntityDto<Guid> input)
        {
            var schoolInfo = await _schoolInfoRepository.GetAll().AsNoTracking()
                .Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (schoolInfo == null)
                throw new UserFriendlyException("Data not found");

            var output = new GetSchoolInfoForEditOutput
            {
                Id = schoolInfo.Id,
                Name = schoolInfo.Name,
                Address = schoolInfo.Address,
                PhoneNo = schoolInfo.PhoneNo,
                Email = schoolInfo.Email,
                Description = schoolInfo.Description,
                Image = schoolInfo.Image
            };
            if (output.Image != null)
            {
                output.ImageBytes = await GetBinaryByte(output.Image);
                output.ImageFileName = await GetBinaryFileName(output.Image);
            }


            return output;
        }

        public virtual async Task CreateOrEdit(CreateOrEditSchoolInfoDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SchoolInfos_Create)]
        protected virtual async Task Create(CreateOrEditSchoolInfoDto input)
        {
            var schoolInfo = new SchoolInfo
            {
                Id = Guid.Empty,
                TenantId = AbpSession.GetTenantId(),
                Name = input.Name,
                Address = input.Address,
                PhoneNo = input.PhoneNo,
                Email = input.Email,
                Description = input.Description
            };


            var imageUpload = await GetBinaryObjectFromCache(input.ImageToken);
            if (imageUpload != null) schoolInfo.Image = imageUpload.Id;

            await _schoolInfoRepository.InsertAsync(schoolInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_SchoolInfos_Edit)]
        protected virtual async Task Update(CreateOrEditSchoolInfoDto input)
        {
            var schoolInfo = await _schoolInfoRepository.GetAll().Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (schoolInfo == null)
                throw new UserFriendlyException("Data not found");
            schoolInfo.Name = input.Name;
            schoolInfo.Address = input.Address;
            schoolInfo.PhoneNo = input.PhoneNo;
            schoolInfo.Email = input.Email;
            schoolInfo.Description = input.Description;

            if (!string.IsNullOrWhiteSpace(input.ImageToken))
            {
                if (schoolInfo.Image != null) await RemoveImageFile(new EntityDto<Guid> { Id = schoolInfo.Id });
                var imageUpload = await GetBinaryObjectFromCache(input.ImageToken);
                if (imageUpload != null) schoolInfo.Image = imageUpload.Id;
            }

            await _schoolInfoRepository.UpdateAsync(schoolInfo);

        }

        [AbpAuthorize(AppPermissions.Pages_SchoolInfos_Delete)]
        public virtual async Task Delete(EntityDto<Guid> input)
        {
            var schoolInfo = await _schoolInfoRepository.GetAll().Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (schoolInfo.Image != null) await RemoveImageFile(new EntityDto<Guid> { Id = schoolInfo.Id });
            await _schoolInfoRepository.DeleteAsync(schoolInfo);
        }

        public virtual async Task<FileDto> GetSchoolInfosToExcel(GetAllSchoolInfosForExcelInput input)
        {

            var filteredSchoolInfos = _schoolInfoRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e =>  e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.PhoneNo.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Description.Contains(input.Filter));

            var query = (from o in filteredSchoolInfos
                select new GetSchoolInfoForViewDto
                {
                    Name = o.Name,
                    Address = o.Address,
                    PhoneNo = o.PhoneNo,
                    Email = o.Email,
                    Id = o.Id

                });

            var schoolInfoListDtos = await query.ToListAsync();

            return _schoolInfosExcelExporter.ExportToFile(schoolInfoListDtos);
        }

        private async Task<GetUploadFileResultDto> GetBinaryObjectFromCache(string fileToken)
        {
            if (fileToken.IsNullOrWhiteSpace()) return null;

            var fileCache = _tempFileCacheManager.GetFileInfo(fileToken);

            if (fileCache == null) throw new UserFriendlyException("There is no such file with the token: " + fileToken);

            var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, fileCache.FileName);
            await _binaryObjectManager.SaveAsync(storedFile);

            return new GetUploadFileResultDto
            {
                Id = storedFile.Id,
                FileType = fileCache.FileType
            };
        }
        protected virtual async Task<string> GetBinaryFileName(Guid? fileId)
        {
            if (!fileId.HasValue)
            {
                return null;
            }

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Description;
        }

        private async Task<byte[]> GetBinaryByte(Guid? fileId)
        {
            if (!fileId.HasValue) return null;

            var file = await _binaryObjectManager.GetOrNullAsync(fileId.Value);
            return file?.Bytes;
        }

        [AbpAuthorize(AppPermissions.Pages_SchoolInfos_Edit)]
        public virtual async Task RemoveImageFile(EntityDto<Guid> input)
        {
            var schoolInfo = await _schoolInfoRepository.GetAll().Where(x => x.TenantId == AbpSession.GetTenantId())
                .FirstOrDefaultAsync(x => x.Id == input.Id);
            if (schoolInfo == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            if (!schoolInfo.Image.HasValue)
            {
                throw new UserFriendlyException(L("FileNotFound"));
            }

            await _binaryObjectManager.DeleteAsync(schoolInfo.Image.Value);
            schoolInfo.Image = null;
        }

    }
}