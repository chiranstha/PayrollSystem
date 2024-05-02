using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class InternalGradeSetupExcelExporter : NpoiExcelExporterBase, IInternalGradeSetupExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public InternalGradeSetupExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetInternalGradeSetupForViewDto> internalGradeSetup)
        {
            return CreateExcelPackage(
                    "InternalGradeSetup.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("InternalGradeSetup"));

                        AddHeader(
                            sheet,
                        L("Category"),
                        L("Grade"),
                        L("IsPercent"),
                        L("Value")
                            );

                        AddObjects(
                            sheet, internalGradeSetup,
                        d => d.Category,
                        d => d.Grade,
                        d => d.IsPercent,
                        d => d.Value
                            );

                    });

        }
    }
}