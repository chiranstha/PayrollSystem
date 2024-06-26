﻿using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Suktas.Payroll.Web.Public.Views
{
    public abstract class PayrollRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected PayrollRazorPage()
        {
            LocalizationSourceName = PayrollConsts.LocalizationSourceName;
        }
    }
}
