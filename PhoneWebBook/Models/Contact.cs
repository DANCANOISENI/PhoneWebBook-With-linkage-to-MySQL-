using System.ComponentModel.DataAnnotations;

namespace PhoneWebBook.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
