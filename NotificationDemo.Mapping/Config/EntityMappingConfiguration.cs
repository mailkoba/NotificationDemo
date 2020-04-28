using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NotificationDemo.Mapping.Config
{
    public abstract class EntityMappingConfiguration<T> : IEntityMappingConfiguration<T>
        where T : class
    {
        public abstract void Map(EntityTypeBuilder<T> entityBuilder);

        public void Map(ModelBuilder modelBuilder)
        {
            Map(modelBuilder.Entity<T>());
        }
    }
}
