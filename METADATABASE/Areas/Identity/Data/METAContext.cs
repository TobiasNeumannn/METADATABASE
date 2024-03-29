﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using METADATABASE.Models;

namespace METADATABASE.Areas.Identity.Data;

public class  METAContext: IdentityDbContext<IdentityUser>
{
    public METAContext(DbContextOptions<METAContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

public DbSet<METADATABASE.Models.Feedback> Feedback { get; set; } = default!;

public DbSet<METADATABASE.Models.Bug> Bug { get; set; } = default!;
}
