using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class FestivalBonusSettingsExcelExporter : NpoiExcelExporterBase, IFestivalBonusSettingsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public FestivalBonusSettingsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetFestivalBonusSettingForViewDto> festivalBonusSettings)
        {
            return CreateExcelPackage(
                    "FestivalBonusSettings.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("FestivalBonusSettings"));

                        AddHeader(
                            sheet,
                        L("MonthId"),
                        L("Remarks")
                            );

                        AddObjects(
                            sheet, festivalBonusSettings,
                        _ => _.MonthId,
                        _ => _.Remarks
                            );

                    });

        }
    }
}