using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Users : IdentityUser
    {
        public string Pfp { get; set; } //add validation

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
