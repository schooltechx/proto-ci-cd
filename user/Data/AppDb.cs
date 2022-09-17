using Microsoft.EntityFrameworkCore;
using App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace App.Data
{
    class AppDb : IdentityDbContext<UserProfile, IdentityRole, string>
    {
        public AppDb(DbContextOptions options) : base(options) { }
        public DbSet<UserProfile> UserProfiles => Set<UserProfile> ();
    }
}

