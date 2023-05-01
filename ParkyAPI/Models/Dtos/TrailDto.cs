using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    // This DTO is used for All use cases other then Update and Insert
    // we will need the API can return an Trail Object and also its National Park as well

    public class TrailDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Elevation { get; set; }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalParkDto? NationalPark { set; get; }
    }
}