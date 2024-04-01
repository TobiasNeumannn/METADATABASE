using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Comments
    {
        [Key]
        public int CommentsID { get; set; }

        [Required]
        public int PostsID { get; set; }

        public string Content { get; set; }

        [Required]
        public DateTime Creation { get; set; }

        public int UserID { get; set; }

        public string Pfp { get; set; }

        [Required]
        public bool Correct { get; set; }

        // Navigation properties
        public Posts Post { get; set; }
        public Users User { get; set; }
        public ICollection<Likes> Likes { get; set; }
        public ICollection<Reports> Reports { get; set; }
    }
}
