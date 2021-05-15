using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacies.Server.Controls
{
    public class ConfirmBase: ComponentBase
    {
        public bool ShowConfirmation { get; set; }

        [Parameter]
        public string ConfirmationTitle { get; set; } = "Delete Confirmation";

        [Parameter]
        public string ConfirmationMessage { get; set; } = "Are you sure you want to delete?";

        [Parameter]
        public string ConfirmationButton { get; set; } = "Delete";


        public void Show()
        {
            ShowConfirmation = true;
            this.StateHasChanged();
        }

        [Parameter]
        public EventCallback<bool> ConfirmationChanged { get; set; }

        protected async Task OnConfirmationChange(bool value)
        {
            ShowConfirmation = false;
            await ConfirmationChanged.InvokeAsync(value);
        }
    }
}
