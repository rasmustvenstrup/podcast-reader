using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Podcasts.Entities;
using Podcasts.Interfaces;

namespace Podcasts.Repositories
{
    public class PodcastFileRepository : IPodcastRepository
    {
        private readonly string _directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
        private readonly string _fileName = "podcasts.json";
        private string _filepath;
        private ICollection<Podcast> _podcasts;

        public PodcastFileRepository()
        {
            Initialize();
        }

        public async Task Add(Podcast podcast)
        {
            _podcasts.Add(podcast);
            await Save();
        }

        public Podcast Get(Guid id)
        {
            return _podcasts.Single(p => p.Id == id);
        }

        public async Task Delete(Guid id)
        {
            _podcasts.Remove(_podcasts.Single(p => p.Id == id));
            await Save();
        }

        public IReadOnlyCollection<Podcast> GetAll()
        {
            return new ReadOnlyCollection<Podcast>(_podcasts.ToList());
        }

        private async Task Save()
        {
            using (StreamWriter writer = File.CreateText(_filepath))
            {
                await writer.WriteAsync(JsonConvert.SerializeObject(_podcasts));
            }
        }

        private void Initialize()
        {
            if (!Directory.Exists(_directoryPath)) Directory.CreateDirectory(_directoryPath);
            _filepath = Path.Combine(_directoryPath, _fileName);

            _podcasts = File.Exists(_filepath)
                ? JsonConvert.DeserializeObject<ICollection<Podcast>>(File.ReadAllText(_filepath))
                : new List<Podcast>();
        }
    }
}