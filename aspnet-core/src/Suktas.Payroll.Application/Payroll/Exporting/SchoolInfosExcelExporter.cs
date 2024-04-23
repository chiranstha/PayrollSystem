using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class SchoolInfosExcelExporter : NpoiExcelExporterBase, ISchoolInfosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SchoolInfosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSchoolInfoForViewDto> schoolInfos)
        {
            return CreateExcelPackage(
                    "SchoolInfos.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet("SchoolInfos");

                        AddHeader(
                            sheet,
                        "Name",
                        "Address",
                        "PhoneNo",
                        "Email"
                            );

                        AddObjects(
                            sheet, schoolInfos,
                        _ => _.Name,
                        _ => _.Address,
                        _ => _.PhoneNo,
                        _ => _.Email
                            );

                    });

        }
    }
}