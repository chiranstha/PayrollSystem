using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;

namespace Suktas.Payroll.Payroll
{
    public interface IInternalGradeSetupAppService : IApplicationService
    {
        Task<PagedResultDto<GetInternalGradeSetupForViewDto>> GetAll(GetAllInternalGradeSetupInput input);

        Task<GetInternalGradeSetupForViewDto> GetInternalGradeSetupForView(Guid id);

        Task<GetInternalGradeSetupForEditOutput> GetInternalGradeSetupForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditInternalGradeSetupDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetInternalGradeSetupToExcel(GetAllInternalGradeSetupForExcelInput input);

    }
}