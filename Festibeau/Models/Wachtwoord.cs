using System.ComponentModel.DataAnnotations;

namespace SendAndStore.Models
{
    public class Wachtwoord
    {
        [Required(ErrorMessage = "Verplicht in te vullen")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Verplicht in te vullen")]
        public string wachtwoord { get; set; }
    }
}
