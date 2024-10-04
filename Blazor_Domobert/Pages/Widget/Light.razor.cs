using Blazor_Domobert.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;

namespace Blazor_Domobert.Pages.Widget
{
    public partial class Light
    {
        [Parameter]
        public Device Device { get; set; }


        void ShowNotification(NotificationMessage message)
        {
            NotificationService.Notify(message);

            Console.WriteLine($"{message.Severity} notification");
        }
    }
}
