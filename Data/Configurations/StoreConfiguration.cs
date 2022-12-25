using ApplicationDev.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationDev.Data.Configuration
{
    public class StoreConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder.HasOne(x => x.ApplicationUser)
                .WithOne(x => x.Store)
                .HasForeignKey<Store>(x => x.UserId)
                .IsRequired(false);
        }
    }
}