using Blazor_Domobert.Models;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace Blazor_Domobert.Pages.Widget
{
    public partial class Camera
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
