using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using optimum.data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace optimum.data.Context
{
    public class OptimumDbContext : IdentityDbContext<ApplicationUser>
    {
        public OptimumDbContext(DbContextOptions<OptimumDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // تأكد من استدعاء الأساس لمنع الأخطاء
                                                // 

            modelBuilder.Entity<SupplierRequestItem>()
    .HasOne(sri => sri.SchoolConfirmedRequestItem)
    .WithMany() // أو WithMany(x => x.SupplierRequestItems) لو عندك Navigation
    .HasForeignKey(sri => sri.SchoolConfirmedRequestItemId)
    .OnDelete(DeleteBehavior.NoAction);  // هنا المهم

        }

        public DbSet<School> School { get; set; }
        
        public DbSet<Products> Products { get; set; }
        public DbSet<AIParsedRequestItems> AIParsedRequestItems { get; set; }
        public DbSet<SchoolConfirmedRequestItems> SchoolConfirmedRequestItems { get; set; }
        public DbSet<SchoolRequestItems> SchoolRequestItems { get; set; }
        public DbSet<SchoolRequests> SchoolRequests { get; set; }
        public DbSet<SupplierOfferItems> SupplierOfferItems { get; set; }
        public DbSet<SupplierOffers> SupplierOffers { get; set; }
        public DbSet<SupplierProducts> SupplierProducts { get; set; }
        public DbSet<Suppliers> Suppliers { get; set; }
        public DbSet<SupplierRequest> SupplierRequest { get; set; }
        public DbSet<SupplierRequestItem> SupplierRequestItem { get; set; }
    }
}
