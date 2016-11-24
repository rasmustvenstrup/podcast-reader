using System;
using System.ServiceModel.Syndication;
using System.Xml;

namespace Podcasts.Entities
{
    public class Podcast
    {
        public Podcast(Guid id, string feedUrl, string title, string description, string imageUrl)
        {
            Id = id;
            FeedUrl = feedUrl;
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string FeedUrl { get; private set; }
        public string ImageUrl { get; private set; }

        public static Podcast Create(string feedUrl)
        {
            if (string.IsNullOrEmpty(feedUrl)) throw new ArgumentException("Argument cannot be null or empty.", nameof(feedUrl));

            try
            {
                using (XmlReader xmlReader = XmlReader.Create(feedUrl))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                    var podcast = new Podcast(Guid.NewGuid(), feedUrl, feed?.Title?.Text, feed?.Description?.Text, feed?.ImageUrl?.AbsoluteUri);
                    return podcast;
                }
            }
            catch (Exception)
            {
                throw new ArgumentException("Argument is not a valid RSS feed url.", nameof(feedUrl));
            }
            
        }

        public SyndicationFeed GetFeed()
        {
            using (XmlReader xmlReader = XmlReader.Create(FeedUrl))
            {
                SyndicationFeed feed = SyndicationFeed.Load(xmlReader);
                return feed;
            }
        }
    }
}