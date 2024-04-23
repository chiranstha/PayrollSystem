using System;
using Abp.Domain.Repositories;
using Suktas.Payroll.Payroll.Exporting;
using Suktas.Payroll.Authorization;
using Abp.Authorization;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll
{
    [AbpAuthorize(AppPermissions.Pages_EmployeeLevels)]
    public class EmployeeLevelsAppService : EmployeeLevelsAppServiceBase, IEmployeeLevelsAppService, IEmployeeLevelsAppServiceExtended
    {
        public EmployeeLevelsAppService(IRepository<EmployeeLevel, Guid> employeeLevelRepository, IEmployeeLevelsExcelExporter employeeLevelsExcelExporter)
        : base(employeeLevelRepository, employeeLevelsExcelExporter
        )
        {
        }

        // Write your custom code here. 
        // ASP.NET Zero Power Tools will not overwrite this class when you regenerate the related entity.
    }
}