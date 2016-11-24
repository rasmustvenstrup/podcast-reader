using Podcasts.Interfaces;
using Podcasts.Repositories;
using StructureMap.Configuration.DSL;

namespace Website.IoC
{
    public class PodcastsRegistry : Registry
    {
        public PodcastsRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<PodcastFileRepository>();
                    scan.WithDefaultConventions();
                });

            For<IPodcastRepository>().Use<PodcastFileRepository>().Singleton();
        }
    }
}