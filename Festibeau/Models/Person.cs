using System.ComponentModel.DataAnnotations;

namespace SendAndStore.Models
{
    public class Person
    {
        [Required(ErrorMessage = "Verplicht in te vullen")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Verplicht in te vullen")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Verplicht in te vullen")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Verplicht in te vullen")]
        public string Wachtwoord { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }

    }
}
