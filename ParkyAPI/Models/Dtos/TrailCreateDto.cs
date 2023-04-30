using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    // This DTO is used for Create only, we will not provide Trail Id for this use case.
    public class TrailCreateDto
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public double Distance { get; set; }
        [Required]
        public double Elevation { get; set; }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }

    }
}
