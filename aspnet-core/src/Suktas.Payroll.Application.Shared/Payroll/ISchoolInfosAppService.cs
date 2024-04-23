using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;

namespace Suktas.Payroll.Payroll
{
    public interface ISchoolInfosAppService : IApplicationService
    {
        Task<PagedResultDto<GetSchoolInfoForViewDto>> GetAll(GetAllSchoolInfosInput input);

        Task<GetSchoolInfoForViewDto> GetSchoolInfoForView(Guid id);

        Task<GetSchoolInfoForEditOutput> GetSchoolInfoForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditSchoolInfoDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetSchoolInfosToExcel(GetAllSchoolInfosForExcelInput input);

        Task RemoveImageFile(EntityDto<Guid> input);

    }
}