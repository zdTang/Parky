using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models
{
    public class NationalPark
    {
        [Key]
        public int Id { get; set; }

        [Required] // Here the [Required] is to restrain frontEnd must input value, or ModelState will not be validated during Model binding
        public string Name { get; set; } = "";  // "" is space, which is not same as Null

        [Required]
        public string State { get; set; } = "";

        public DateTime Created { get; set; }
        public byte[]? Picture { get; set; }
        public DateTime Established { get; set; }
    }
}