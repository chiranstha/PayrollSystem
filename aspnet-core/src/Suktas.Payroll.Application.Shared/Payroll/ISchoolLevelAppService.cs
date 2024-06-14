using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Payroll.Dtos;
using System;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    public interface ISchoolLevelAppService : IApplicationService
    {
        Task<PagedResultDto<GetSchoolLevelForView>> GetAll(GetAllSchoolLevelsInput input);

        Task<GetSchoolLevelForView> GetSchoolLevelForView(Guid id);

        Task<GetSchoolLevelForEdit> GetSchoolLevelForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditSchoolLevelDto input);

        Task Delete(EntityDto<Guid> input);

   //     Task<FileDto> GetSchoolLevelsToExcel(GetAllSchoolLevelsForExcelInput input);

    }
}
