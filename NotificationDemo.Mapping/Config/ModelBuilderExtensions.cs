using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace NotificationDemo.Mapping.Config
{
    public static class ModelBuilderExtensions
    {
        public static void AddEntityConfigurationsFromAssembly(this ModelBuilder modelBuilder, Assembly assembly)
        {
            var mappingTypes = assembly.GetMappingTypes(typeof(IEntityMappingConfiguration<>));
            foreach (var config in mappingTypes.Select(Activator.CreateInstance).Cast<IEntityMappingConfiguration>())
            {
                config.Map(modelBuilder);
            }
        }

        private static IEnumerable<Type> GetMappingTypes(this Assembly assembly, Type mappingInterface)
        {
            return assembly.GetTypes()
                .Where(x => !x.GetTypeInfo().IsAbstract &&
                            x.GetInterfaces().Any(y => y.GetTypeInfo().IsGenericType &&
                                                       y.GetGenericTypeDefinition() ==
                                                       mappingInterface));
        }
    }
}
