using cloudscribe.DateTimeUtils;
using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Web.Services;
using cloudscribe.SimpleContent.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web
{
    public class FeaturedPostsViewComponent : ViewComponent
    {
        public FeaturedPostsViewComponent(
            IProjectService projectService,
            IPostQueries postQueries,
            IContentProcessor contentProcessor,
            ITimeZoneHelper timeZoneHelper
            )
        {
            _projectService = projectService;
            _postQueries = postQueries;
            _timeZoneHelper = timeZoneHelper;
            _contentProcessor = contentProcessor;
        }

        private readonly IProjectService _projectService;
        private readonly IPostQueries _postQueries;
        private readonly ITimeZoneHelper _timeZoneHelper;
        private readonly IContentProcessor _contentProcessor;

        public async Task<IViewComponentResult> InvokeAsync(string viewName = "FeaturedPosts", int numberToShow = 5)
        {
            var model = new RecentPostsViewModel(_contentProcessor);
            var settings = await _projectService.GetCurrentProjectSettings().ConfigureAwait(false);
            var list = await _postQueries.GetFeaturedPosts(settings.Id, numberToShow);
            model.ProjectSettings = settings;
            model.Posts = list;
            model.TimeZoneHelper = _timeZoneHelper;
            model.TimeZoneId = model.ProjectSettings.TimeZoneId;

            return View(viewName, model);
        }
    }
}
