using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Podcasts.Interfaces;
using Podcasts.Models;
using Website.Models;

namespace Website.Controllers
{
    public class PodcastController : Controller
    {
        private readonly IPodcastService _podcastService;

        public PodcastController(IPodcastService podcastService)
        {
            _podcastService = podcastService;
        }

        public ActionResult Index()
        {
            IReadOnlyCollection<PodcastModel> podcasts = _podcastService.GetPodcasts();
            var model = new PodcastViewModel { Podcasts = podcasts };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(PodcastViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // TODO: Ensure podcasts are posted instead of getting them from service.
                model.Podcasts = _podcastService.GetPodcasts();
                return View(model);
            }

            await _podcastService.AddPodcast(model.FeedUrl);
            return RedirectToAction("Index");
        }

        public ActionResult Feed(Guid podcastId)
        {
            FeedModel feed = _podcastService.GetFeed(podcastId);
            return View(feed);
        }
    }
}