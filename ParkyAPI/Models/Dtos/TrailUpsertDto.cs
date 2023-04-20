using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    // This DTO is used for Update and Insert
    // As at these two use case, We will provide a Trail object, and in this object. we will not provide a 
    // National Park object
    public class TrailUpsertDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }

    }
}
