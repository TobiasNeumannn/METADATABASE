﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace METADATABASE.Models
{
    public class Posts
    {
        [Key]
        public int PostsID { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime Creation { get; set; }

        public string Title { get; set; }

        public string Pfp { get; set; }

        [Required]
        public bool Locked { get; set; }

        [Required]
        public int UserID { get; set; }

        // Navigation properties
        public Users User { get; set; }
        public ICollection<Likes> Likes { get; set; }
        public ICollection<Reports> Reports { get; set; }
        public ICollection<Comments> Comments { get; set; }
    }
}