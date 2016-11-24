using System;

namespace Podcasts.Models
{
    public class PodcastModel
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string FeedUrl { get; private set; }
        public string ImageUrl { get; private set; }
    }
}