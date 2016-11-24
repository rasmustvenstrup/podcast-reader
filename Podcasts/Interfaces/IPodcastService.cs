using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Podcasts.Models;

namespace Podcasts.Interfaces
{
    public interface IPodcastService
    {
        Task AddPodcast(string url);
        Task RemovePodcast(Guid id);
        IReadOnlyCollection<PodcastModel> GetPodcasts();
        FeedModel GetFeed(Guid podcastId);
    }
}