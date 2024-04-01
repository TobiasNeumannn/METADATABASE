using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Users
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(256)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Pfp { get; set; }

        [StringLength(255)]
        public string ProjName { get; set; }

        public string ProjThumbnailImg { get; set; }

        public string ProjDesc { get; set; }

        // Navigation properties
        public ICollection<Posts> Posts { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Likes> Likes { get; set; }
        public ICollection<Reports> Reports { get; set; }
    }
}
