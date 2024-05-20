using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Reports
    {
        [Key]
        public int ReportsID { get; set; }

        public string? Id { get; set; } //user.Id
        public int? PostsID { get; set; }
        public int? CommentsID { get; set; }

        public string Content { get; set; }

        [Required]
        public DateTime Creation { get; set; }

        // Navigation properties
        public Users User { get; set; }
        public Posts Post { get; set; }
        public Comments Comment { get; set; }
    }
}
