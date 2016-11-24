using System.Collections.Generic;

namespace Podcasts.Models
{
    public class FeedModel
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }
        public string Url { get; private set; }
        public ICollection<ItemModel> Items { get; private set; }
    }
}