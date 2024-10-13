using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Reports
    {
        [Key]
        public int ReportsID { get; set; }
        public string? UserId { get; set; } //user.UserId (nullable to fix a modelstate error)
        public int? PostsID { get; set; }
        public int? CommentsID { get; set; }
        [Required]
        [StringLength(2000, ErrorMessage = "Do not enter more than two thousand characters")]
        public string Content { get; set; }
        public DateTime Creation { get; set; }

        // Navigation properties (nullable to fix a modelstate error)
        public Users? User { get; set; }
        public Posts? Post { get; set; }
        public Comments? Comment { get; set; }

    }
}
