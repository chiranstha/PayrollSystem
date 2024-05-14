using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class GradeUpgradesExcelExporter : NpoiExcelExporterBase, IGradeUpgradesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public GradeUpgradesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetGradeUpgradeForViewDto> gradeUpgrades)
        {
            return CreateExcelPackage(
                    "GradeUpgrades.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("GradeUpgrades"));

                        AddHeader(
                            sheet,
                        L("DateMiti"),
                        L("Grade"),
                        L("Remarks"),
                        (L("Employee")) + L("Name")
                            );

                        AddObjects(
                            sheet, gradeUpgrades,
                        _ => _.DateMiti,
                        _ => _.Grade,
                        _ => _.Remarks,
                        _ => _.EmployeeName
                            );

                    });

        }
    }
}