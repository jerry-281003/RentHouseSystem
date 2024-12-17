namespace RentHouseSystem.Models
{
    public class Facilities
    {
        public int Id { get; set; }
        public string HouseId { get; set; }
        public string InventoryEquipment { get; set; }
        public string UtilityCost { get; set; }
        public bool AirConditioning { get; set; }
        public bool MajorAppliances { get; set; }
        public int NumberOfBedroom { get; set; }
        public string ToiletAndBathroom { get; set; }
        public bool Pet { get; set; }
        public int DepositRequired { get; set; }
    }
    public class FacilitiesHouse
    {
        public int Id { get; set; }
        public string HouseId { get; set; }
        public string InventoryEquipment { get; set; }
        public string UtilityCost { get; set; }
        public bool AirConditioning { get; set; }
        public bool MajorAppliances { get; set; }
        public int NumberOfBedroom { get; set; }
        public string ToiletAndBathroom { get; set; }
        public bool Pet { get; set; }
        public int DepositRequired { get; set; }
        public string ownerId { get; set; }
        public string title { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string ContactPhone { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public vehicles AcceptableVehicles { get; set; }
        public TimeSpan CloseTime { get; set; }
        public List<Image> Image { get; set; } 
    }
    public class FacilitiesHouseViewModel
    {
        public Facilities Facilities { get; set; }
        public House House { get; set; }
    }
}
