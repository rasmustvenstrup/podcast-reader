using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using AutoMapper;
using Podcasts.Entities;
using Podcasts.Interfaces;
using Podcasts.Models;

namespace Podcasts.Services
{
    public class PodcastService : IPodcastService
    {
        private readonly IMapper _mapper;
        private readonly IPodcastRepository _podcastRepository;

        public PodcastService(IMapper mapper, IPodcastRepository podcastRepository)
        {
            _mapper = mapper;
            _podcastRepository = podcastRepository;
        }

        public async Task AddPodcast(string feedUrl)
        {
            Podcast podcast = Podcast.Create(feedUrl);
            await _podcastRepository.Add(podcast);
        }

        public async Task RemovePodcast(Guid id)
        {
            await _podcastRepository.Delete(id);
        }

        public IReadOnlyCollection<PodcastModel> GetPodcasts()
        {
            IReadOnlyCollection<Podcast> podcasts = _podcastRepository.GetAll();
            return _mapper.Map<IReadOnlyCollection<PodcastModel>>(podcasts);
        }

        public FeedModel GetFeed(Guid podcastId)
        {
            Podcast podcast = _podcastRepository.Get(podcastId);
            SyndicationFeed feed = podcast.GetFeed();
            return _mapper.Map<FeedModel>(feed);
        }
    }
}