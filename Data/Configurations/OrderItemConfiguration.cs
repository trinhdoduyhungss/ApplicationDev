using ApplicationDev.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationDev.Data.Configuration
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.OderItems)
                .HasForeignKey(x => x.UserId)
                .IsRequired(false);
            builder.HasMany(x => x.OrderDetails)
                .WithOne(x => x.OrderItem)
                .HasForeignKey(x => x.OrderId);
            
        }
        
    }
}