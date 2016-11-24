using System;
using System.Text.RegularExpressions;

namespace Podcasts.Models
{
    public class ItemModel
    {
        public string Title { get; private set; }
        public string Content { get; private set; }
        public DateTime PublishDate { get; private set; }
        public string Url { get; private set; }

        public string GetDownloadLink()
        {
            if (string.IsNullOrEmpty(Url)) return null;
            Match match = Regex.Match(Url, "^.*.mp(3|4)");
            return match.Success ? match.Value : null;
        }
    }
}