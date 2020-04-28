using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationDemo.Domain;

namespace NotificationDemo.Mapping
{
    public class UserMap : EntityMap<User>
    {
        public override void Map(EntityTypeBuilder<User> entityBuilder)
        {
            base.Map(entityBuilder);

            entityBuilder.Property(x => x.Login).HasMaxLength(50).IsRequired();
            entityBuilder.Property(x => x.Name).HasMaxLength(50).IsRequired();
        }
    }
}
