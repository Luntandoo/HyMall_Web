using HyMall_App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HyMall_App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Fix non-nullable property warning by making it nullable
        public DbSet<ContactMessage> ContactMessages { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<TenantProfile> TenantProfiles { get; set; }

        //Tenant
        public DbSet<InventoryCheck> InventoryChecks { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<ProductShortageReport> ProductShortageReport { get; set; }
        public DbSet<ShopDetail> ShopDetail { get; set; }


    }
}
