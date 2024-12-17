using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentHouseSystem.Models
{
    public enum status
    {
        pencding,
        approved,
        rejected
    }
    public enum vehicles
    {
        bicycle,
        motorbike,
        noVehicles,
        both
    }
    public class House
    {
        [Key]
        public string HouseId { get; set; }
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
        public bool? Invisible { get; set; }
        public ICollection<Image> Images { get; set; } = new List<Image>();
    }

    public class HouseViewModel
    {
        public string HouseId { get; set; }
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
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
    public class HouseSearchModel
    {
        public string HouseId { get; set; }
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
        public string ImageUrls { get; set; } 
        
    }
}
