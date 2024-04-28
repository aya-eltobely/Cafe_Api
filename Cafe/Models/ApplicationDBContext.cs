using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Cafe.Models
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration config;

        public ApplicationDBContext(){}

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, IConfiguration _configuration) : base(options)
        {
            config = _configuration;
        }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images  { get; set; }
        public DbSet<Order> Orders  { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products  { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(config.GetConnectionString("Default") );
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Configure Identity-related entities
            builder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<IdentityUserRole<string>>().HasKey(r => new { r.UserId, r.RoleId });
            builder.Entity<IdentityUserClaim<string>>().HasKey(c => c.Id);
            builder.Entity<IdentityUserToken<string>>().HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            builder.Entity<IdentityRoleClaim<string>>().HasKey(rc => rc.Id);
            ////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///

            //builder.Entity<ApplicationUser>().HasMany(u => u.Addresses).WithOne(a => a.AppUser);
            builder.Entity<ApplicationUser>().HasMany(u => u.Orders).WithOne(a => a.AppUser);

            builder.Entity<Delivery>().HasMany(u => u.Orders).WithOne(a => a.Delivery).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Delivery>().HasOne(u => u.AppUser).WithOne(a => a.Delivery).HasForeignKey<Delivery>(d => d.AppUserId);
            builder.Entity<Order>().Property(d => d.DeliveryId).IsRequired(false);


            //builder.Entity<Address>().HasMany(a => a.Orders).WithOne(o => o.Address).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(a => a.Address).WithOne(o => o.Order).HasForeignKey<Address>(a=>a.OrderId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Order>().HasMany(o => o.OrderItems).WithOne(o => o.Order).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>().HasMany(p => p.OrderItems).WithOne(o => o.Product).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Category>().HasMany(c => c.SubCategories).WithOne(s => s.Category);

            builder.Entity<Image>().HasOne(i => i.SubCategory).WithOne(s => s.Image).HasForeignKey<SubCategory>(s=>s.ImageId).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SubCategory>().HasMany(s => s.Products).WithOne(p => p.SubCategory);

            builder.Entity<Product>().HasOne(p => p.Image).WithOne(i => i.Product).HasForeignKey<Product>(p => p.ImageId);
        





        }

    }
}
