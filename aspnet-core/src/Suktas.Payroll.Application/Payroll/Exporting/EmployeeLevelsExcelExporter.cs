using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Suktas.Payroll.DataExporting.Excel.NPOI;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;
using Suktas.Payroll.Storage;

namespace Suktas.Payroll.Payroll.Exporting
{
    public class EmployeeLevelsExcelExporter : NpoiExcelExporterBase, IEmployeeLevelsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public EmployeeLevelsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetEmployeeLevelForView> employeeLevels)
        {
            return CreateExcelPackage(
                    "EmployeeLevels.xlsx",
                    excelPackage =>
                    {

                        var sheet = excelPackage.CreateSheet(L("EmployeeLevels"));

                        AddHeader(
                            sheet,
                        L("Name")
                            );

                        AddObjects(
                            sheet, employeeLevels,
                        v => v.Name
                            );

                    });

        }
    }
}