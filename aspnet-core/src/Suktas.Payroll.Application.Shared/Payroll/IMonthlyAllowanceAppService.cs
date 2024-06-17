using Suktas.Payroll.Payroll.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Suktas.Payroll.Payroll
{
    public interface IMonthlyAllowanceAppService
    {
        Task CreateOrEdit(CreateOrEditMontlyAllowanceDto input);
    }
}
