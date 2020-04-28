using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationDemo.Domain;
using NotificationDemo.Mapping.Config;

namespace NotificationDemo.Mapping
{
    public abstract class EntityMap<TEntity> : EntityMappingConfiguration<TEntity>
        where TEntity : Entity
    {
        public override void Map(EntityTypeBuilder<TEntity> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
