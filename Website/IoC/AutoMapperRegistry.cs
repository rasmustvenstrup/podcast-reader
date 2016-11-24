using AutoMapper;
using Podcasts.Profiles;
using StructureMap.Configuration.DSL;

namespace Website.IoC
{
    public class AutoMapperRegistry : Registry
    {
        public AutoMapperRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<PodcastProfile>();
                    scan.AddAllTypesOf<Profile>();
                });
        }
    }
}