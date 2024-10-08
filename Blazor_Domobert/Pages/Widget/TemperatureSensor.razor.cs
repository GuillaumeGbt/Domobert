using Blazor_Domobert.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Globalization;

namespace Blazor_Domobert.Pages.Widget
{
    public partial class TemperatureSensor
    {
        [Parameter]
        public Device Device { get; set; }


        void ShowNotification(NotificationMessage message)
        {
            NotificationService.Notify(message);

            Console.WriteLine($"{message.Severity} notification");
        }

        class DataItem
        {
            public string Date { get; set; }
            public double Revenue { get; set; }
        }

        string FormatAsUSD(object value)
        {
            return ((double)value).ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
        }

        DataItem[] revenue2023 = new DataItem[] {
        new DataItem
        {
            Date = "Jan",
            Revenue = 234000
        },
        new DataItem
        {
            Date = "Feb",
            Revenue = 269000
        },
        new DataItem
        {
            Date = "Mar",
            Revenue = 233000
        },
        new DataItem
        {
            Date = "Apr",
            Revenue = 244000
        },
        new DataItem
        {
            Date = "May",
            Revenue = 214000
        },
        new DataItem
        {
            Date = "Jun",
            Revenue = 253000
        },
        new DataItem
        {
            Date = "Jul",
            Revenue = 274000
        },
        new DataItem
        {
            Date = "Aug",
            Revenue = 284000
        },
        new DataItem
        {
            Date = "Sept",
            Revenue = 273000
        },
        new DataItem
        {
            Date = "Oct",
            Revenue = 282000
        },
        new DataItem
        {
            Date = "Nov",
            Revenue = 289000
        },
        new DataItem
        {
            Date = "Dec",
            Revenue = 294000
        }
    };
    }
}
