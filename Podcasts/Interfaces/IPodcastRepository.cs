using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Podcasts.Entities;

namespace Podcasts.Interfaces
{
    public interface IPodcastRepository
    {
        Task Add(Podcast podcast);
        Podcast Get(Guid id);
        Task Delete(Guid id);
        IReadOnlyCollection<Podcast> GetAll();
    }
}