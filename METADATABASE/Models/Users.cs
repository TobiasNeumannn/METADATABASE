using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace METADATABASE.Models
{
    public class Users : IdentityUser
    {
        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Profile Picture Name")]
        public string pfpName { get; set; }

        [NotMapped]
        [DisplayName("Upload PFP")]
        public IFormFile pfpFile { get; set; }

        [StringLength(255)]
        public string ProjName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Project Thumbnail Image Name")]
        public string thumbName { get; set; }

        [NotMapped]
        [DisplayName("Upload Thumbnail")]
        public IFormFile thumbFile { get; set; }

        public string ProjDesc { get; set; }

        // Navigation properties
        public ICollection<Posts> Posts { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Likes> Likes { get; set; }
        public ICollection<Reports> Reports { get; set; }
    }
}
