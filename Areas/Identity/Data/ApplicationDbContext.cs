﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisualStudio.Areas.Identity.Data;
using VisualStudio.Models;

namespace VisualStudio.Areas.Identity.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UploadedFile> UploadedFiles { get; set; }
    public DbSet<PendingFile> PendingFiles { get; set; }
    public DbSet<FeedbackData> FeedbackDatas { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<EmailRequest> EmailRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UploadedFile>()
       .HasOne(u => u.User)
       .WithMany()
       .HasForeignKey(u => u.UserId)
       .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<PendingFile>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
    }
}

public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FirstName);
        builder.Property(x => x.LastName);
    }
}


