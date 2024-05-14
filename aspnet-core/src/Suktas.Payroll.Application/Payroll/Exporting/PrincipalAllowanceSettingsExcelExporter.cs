using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class PrincipalAllowanceSettingsExcelExporter : NpoiExcelExporterBase, IPrincipalAllowanceSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PrincipalAllowanceSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPrincipalAllowanceSettingForViewDto> tbl_PrincipalAllowanceSettings)
        {
            return CreateExcelPackage(
                    "Tbl_PrincipalAllowanceSettings.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("Tbl_PrincipalAllowanceSettings"));

                        AddHeader(
                            sheet,
                        L("Amount"),
                        (L("EmployeeLevel")) + L("Name")
                            );

                        AddObjects(
                            sheet, tbl_PrincipalAllowanceSettings,
                        _ => _.Amount,
                        _ => _.EmployeeLevelName
                            );

                    });

        }
    }
}