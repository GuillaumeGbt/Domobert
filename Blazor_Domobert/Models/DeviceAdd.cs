using System.ComponentModel.DataAnnotations;

namespace Blazor_Domobert.Models
{
    public class DeviceAdd
    {
        [Required(ErrorMessage = "Nom requis!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Type requis!")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Localisation requise!")]
        public string Location { get; set; }
    }
}
