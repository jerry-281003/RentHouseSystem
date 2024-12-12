using System.ComponentModel.DataAnnotations;

namespace RentHouseSystem.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public string HouseId { get; set; }
        public string UserId { get; set; }
        public string CommentContent { get; set; }
        public string email { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
