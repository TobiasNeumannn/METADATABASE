using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace METADATABASE.Models
{
    public class Users : IdentityUser
    {
        // ! MOST VALIDATIONS are inside Register.cshtml.cs because that is where the data is actually input.
        public string PfpName { get; set; }
        [NotMapped]
        [DisplayName("Upload PFP")]
        public IFormFile PfpFile { get; set; }

        [StringLength(255)]
        public string ProjName { get; set; }
        public string ThumbName { get; set; }

        [NotMapped]
        [DisplayName("Upload Thumbnail")]
        public IFormFile ThumbFile { get; set; }

        public string ProjDesc { get; set; }

        // Navigation properties
        public ICollection<Posts>? Posts { get; set; }
        public ICollection<Comments>? Comments { get; set; }
        public ICollection<Likes>? Likes { get; set; }
        public ICollection<Reports>? Reports { get; set; }
    }
}
