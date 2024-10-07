using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Likes
    {
        // ! LIKES ARE a simple click-done thing. no inputs, no validations needed
        // EVERYTHING MUST BE NULLABLE because all the value-inserting is done in the backend AFTER the modelstate check. dont change this
        [Key]
        public int? LikesID { get; set; }
        public string? UserId { get; set; } //user.UserId
        public int? PostsID { get; set; }
        public int? CommentsID { get; set; }
        // Navigation properties
        public Users? User { get; set; }
        public Posts? Post { get; set; }
        public Comments? Comment { get; set; }

    }
}
