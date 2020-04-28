using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationDemo.Domain;

namespace NotificationDemo.Mapping
{
    public class SubscriptionMap : EntityMap<Subscription>
    {
        public override void Map(EntityTypeBuilder<Subscription> entityBuilder)
        {
            base.Map(entityBuilder);

            entityBuilder.Property(x => x.Auth).HasMaxLength(100).IsRequired();
            entityBuilder.Property(x => x.Endpoint).HasMaxLength(500).IsRequired();
            entityBuilder.Property(x => x.P256Dh).HasMaxLength(100).IsRequired();
            entityBuilder.Property(x => x.ExpirationTime);

            entityBuilder.HasOne(x => x.User)
                .WithMany(x => x.Subscriptions)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
