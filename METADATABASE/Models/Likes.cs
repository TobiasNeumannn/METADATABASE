using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Likes
    {
        // ! LIKES ARE a simple click-done thing. no inputs, no validations needed
        [Key]
        public int LikesID { get; set; }
        public string Id { get; set; } //user.Id
        public int? PostsID { get; set; }
        public int? CommentsID { get; set; }

        // Navigation properties
        public Users? User { get; set; }
        public Posts? Post { get; set; }
        public Comments? Comment { get; set; }
    }
}
