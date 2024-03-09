using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethods>
{
    public void Configure(EntityTypeBuilder<DeliveryMethods> builder)
    {
        builder.Property(d => d.Price)
            .HasColumnType("decimal(18,2)");
    }
}
