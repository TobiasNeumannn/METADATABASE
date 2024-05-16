using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Likes
    {
        [Key]
        public int LikesID { get; set; }

        public int Id { get; set; } //user.Id
        public int? PostsID { get; set; }
        public int? CommentsID { get; set; }

        // Navigation properties
        public Users User { get; set; }
        public Posts Post { get; set; }
        public Comments Comment { get; set; }
    }
}
