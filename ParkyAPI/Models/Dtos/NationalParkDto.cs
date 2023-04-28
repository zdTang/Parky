using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class NationalParkDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";  // "" is space, which is not same as Null
        [Required]
        public string State { get; set; } = "";
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
        public byte[]? Picture { get; set; }
    }
}
