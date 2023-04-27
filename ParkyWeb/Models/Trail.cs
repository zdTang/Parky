
using System.ComponentModel.DataAnnotations;

namespace ParkyWeb.Models

{
    // This DTO is used for All use cases other then Update and Insert
    // we will need the API can return an Trail Object and also its National Park as well


    public class Trail
    {

        [Required]
        public string Name { get; set; } = "";
        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        public NationalPark? NationalPark { set; get; }
    }
    public enum DifficultyType { Easy, Moderate, Difficult, Expert }
}
