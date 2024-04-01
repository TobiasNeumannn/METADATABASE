using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Likes
    {
        [Key]
        public int LikesID { get; set; }

        public int UserID { get; set; }
        public string Pfp { get; set; }
        public int? PostsID { get; set; }
        public int? CommentsID { get; set; }

        // Navigation properties
        public Users User { get; set; }
        public Posts Post { get; set; }
        public Comments Comment { get; set; }
    }
}
