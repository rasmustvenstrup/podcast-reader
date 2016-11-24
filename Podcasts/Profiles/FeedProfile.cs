using System.Linq;
using System.ServiceModel.Syndication;
using AutoMapper;
using Podcasts.Models;

namespace Podcasts.Profiles
{
    public class FeedProfile : Profile
    {
        public FeedProfile()
        {
            CreateMap<SyndicationFeed, FeedModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Text))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description.Text))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl != null ? src.ImageUrl.AbsoluteUri : null))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Links.Any() ? src.Links.FirstOrDefault().Uri.AbsoluteUri : null));

            CreateMap<SyndicationItem, ItemModel>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Text))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => ((TextSyndicationContent)src.Content).Text))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.DateTime))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Links.Any() ? src.Links.FirstOrDefault().Uri.AbsoluteUri : null));
        }
    }
}