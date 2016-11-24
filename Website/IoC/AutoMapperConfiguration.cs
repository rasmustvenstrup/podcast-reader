using System.Collections.Generic;
using AutoMapper;
using StructureMap;

namespace Website.IoC
{
    public static class AutoMapperConfiguration
    {
        public static void Configure(IContainer container)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                IEnumerable<Profile> profiles = container.GetAllInstances<Profile>();
                foreach (Profile profile in profiles) config.AddProfile(profile);
            });

            container.Configure(c =>
            {
                c.For<IConfigurationProvider>().Use(mapperConfiguration);
                c.For<IMapper>().Use(mapperConfiguration.CreateMapper());
            });
        }
    }
}