namespace RentHouseSystem.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string HouseId { get; set; } 
        public string ImageUrl { get; set; }

        public House House { get; set; }
    }
}
