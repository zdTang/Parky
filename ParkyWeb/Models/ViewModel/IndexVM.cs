namespace ParkyWeb.Models.ViewModel
{
    public class IndexVM
    {
        public IEnumerable<NationalPark>? NationalPark { get; set; }
        public IEnumerable<Trail>? Trails { get; set; }
    }
}
