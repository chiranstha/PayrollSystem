﻿using Microsoft.AspNetCore.Components;
using Suktas.Payroll.Authorization.Accounts;
using Suktas.Payroll.Authorization.Accounts.Dto;
using Suktas.Payroll.Core.Dependency;
using Suktas.Payroll.Core.Threading;
using Suktas.Payroll.Mobile.MAUI.Models.Login;
using Suktas.Payroll.Mobile.MAUI.Shared;

namespace Suktas.Payroll.Mobile.MAUI.Pages.Login
{
    public partial class EmailActivationModal : ModalBase
    {
        public override string ModalId => "email-activation-modal";

        [Parameter] public EventCallback OnSave { get; set; }

        public EmailActivationModel emailActivationModel { get; set; } = new EmailActivationModel();

        private readonly IAccountAppService _accountAppService;

        public EmailActivationModal()
        {
            _accountAppService = DependencyResolver.Resolve<IAccountAppService>();
        }

        protected virtual async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequestExecuter.Execute(
                async () =>
                    await _accountAppService.SendEmailActivationLink(new SendEmailActivationLinkInput
                    {
                        EmailAddress = emailActivationModel.EmailAddress
                    }),
                    async () =>
                    {
                        await OnSave.InvokeAsync();
                    }
                );
            });
        }

        protected virtual async Task Cancel()
        {
            await Hide();
        }
    }
}
