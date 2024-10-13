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
        public int PostsID { get; set; }
        [Required]
        [StringLength(1000, ErrorMessage = "Do not enter more than one thousand characters")]
        public string Content { get; set; }
        public DateTime Creation { get; set; } // default and hidden - doesn't need validation
        public string? UserId { get; set; } //user.UserId - nullable to fix the modelState bug
        public bool Correct { get; set; }

        // Navigation properties (all nullable to ensure it passes ModelState.IsValid in the controller > Create)
        public Posts? Post { get; set; }
        public Users? User { get; set; }
        public ICollection<Likes>? Likes { get; set; }
        public ICollection<Reports>? Reports { get; set; }
        [NotMapped]
        public int? LikesCount { get; set; } //number of likes for the comment. same system as in posts
        [NotMapped]
        public int? ReportsCount { get; set; } // Number of reports for the comment
    }
}
