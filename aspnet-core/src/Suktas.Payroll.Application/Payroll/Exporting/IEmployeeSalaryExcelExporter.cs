using System.Collections.Generic;
using Suktas.Payroll.Payroll.Dtos;
using Suktas.Payroll.Dto;

namespace Suktas.Payroll.Payroll.Exporting
{
    public interface IEmployeeSalaryExcelExporter
    {
        FileDto ExportToFile(List<GetEmployeeSalaryForViewDto> employeeSalary);
    }
}