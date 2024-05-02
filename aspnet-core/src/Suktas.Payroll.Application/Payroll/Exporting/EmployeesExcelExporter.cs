using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class EmployeesExcelExporter : NpoiExcelExporterBase, IEmployeesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeForViewDto> employees)
        {
            return CreateExcelPackage(
                    "Employees.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Employees"));

                        AddHeader(
                            sheet,
                        L("Category"),
                        L("ProvidentFund"),
                        L("PanNo"),
                        L("Name"),
                        L("BankAccountNo"),
                        L("DateOfJoinMiti"),
                        L("IsDearnessAllowance"),
                        L("IsPrincipal"),
                        L("IsGovernment"),
                        L("IsInternal"),
                        (L("EmployeeLevel")) + L("Name"),
                        (L("SchoolInfo")) + L("Name")
                            );

                        AddObjects(
                            sheet,  employees,
                        d => d.Category,
                        d => d.ProvidentFund,
                        d => d.PanNo,
                        d => d.Name,
                        d => d.BankAccountNo,
                        d => d.DateOfJoinMiti,
                        d => d.IsDearnessAllowance,
                        d => d.IsPrincipal,
                        d => d.IsGovernment,
                        d => d.IsInternal,
                        d => d.EmployeeLevelName,
                        d => d.SchoolInfoName
                            );

                    });

        }
    }
}