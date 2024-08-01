using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace METADATABASE.Models
{
    public class Comments
    {
        [Key]
        public int CommentsID { get; set; }
        public int? PostsID { get; set; }
        [Required]
        [StringLength(10000, ErrorMessage = "Do not enter more than ten thousand characters")] // arbitrary big number to not cross
        public string Content { get; set; }
        public DateTime Creation { get; set; } // default and hidden - doesn't need validation
        public string UserId { get; set; } //user.UserId
        public bool Correct { get; set; }

        // Navigation properties (all nullable to ensure it passes ModelState.IsValid in the controller > Create)
        public Posts? Post { get; set; }
        public Users? User { get; set; }
        public ICollection<Likes>? Likes { get; set; }
        public ICollection<Reports>? Reports { get; set; }

        [NotMapped]
        public int LikesCount { get; set; } // Number of Likes for the post
    }
}
