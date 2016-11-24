using AutoMapper;
using Podcasts.Entities;
using Podcasts.Models;

namespace Podcasts.Profiles
{
    public class PodcastProfile : Profile
    {
        public PodcastProfile()
        {
            CreateMap<Podcast, PodcastModel>();
        }
    }
}