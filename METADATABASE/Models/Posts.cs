using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace METADATABASE.Models
{
    public class Posts
    {
        [Key]
        public int PostsID { get; set; }

        [Required]
        [StringLength(2000, ErrorMessage = "Do not enter more than two thousand characters")]
        public string Description { get; set; }

        public DateTime Creation { get; set; } // default and hidden - doesn't need validation

        [Required]
        [StringLength(50, ErrorMessage = "Do not enter more than fifty thousand characters")]
        public string Title { get; set; }

        public bool Locked { get; set; }

        public string? UserId { get; set; } //user.UserId (nullable to get past a strange ModelState error)

        // Navigation properties - nullable because every post doesnt necessarily have a comment/like/report (fixes a modelstate error)
        public Users? User { get; set; }
        public ICollection<Likes>? Likes { get; set; }
        public ICollection<Reports>? Reports { get; set; }
        public ICollection<Comments>? Comments { get; set; }

        [NotMapped] // This property is not stored in the database
        public int? CommentsCount { get; set; } // Number of comments for the post
        [NotMapped]
        public int? LikesCount { get; set; } // Number of Likes for the post
        [NotMapped]
        public int? ReportsCount { get; set; } // Number of reports for the post

    }
    public class PostIndexViewModel
    {
        public IEnumerable<Posts> Posts { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
        public string SortOrder { get; set; }
        public string SearchString { get; set; }
    }

    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }

}
