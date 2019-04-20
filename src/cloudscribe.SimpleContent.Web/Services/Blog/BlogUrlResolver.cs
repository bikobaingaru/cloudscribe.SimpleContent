﻿using cloudscribe.SimpleContent.Models;
using cloudscribe.SimpleContent.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class BlogUrlResolver : IBlogUrlResolver
    {
        public BlogUrlResolver(
            IHttpContextAccessor contextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccesor,
            IContentProcessor contentProcessor,
            IBlogRoutes blogRoutes
            )
        {
            _contextAccessor = contextAccessor;
            _urlHelperFactory = urlHelperFactory;
            _actionContextAccesor = actionContextAccesor;
            _contentProcessor = contentProcessor;
            _blogRoutes = blogRoutes;
        }

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccesor;
        private readonly IContentProcessor _contentProcessor;
        private readonly IBlogRoutes _blogRoutes;

        public Task<string> ResolveBlogUrl(IProjectSettings blog)
        {
            //await EnsureBlogSettings().ConfigureAwait(false);

            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            var result = urlHelper.RouteUrl(_blogRoutes.BlogIndexRouteName);

            return Task.FromResult(result);
        }

        public Task<string> ResolvePostUrl(IPost post, IProjectSettings projectSettings)
        {
            
            var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
            string postUrl;
            if (projectSettings.IncludePubDateInPostUrls)
            {
                DateTime? pubDate = post.PubDate;
                if (!pubDate.HasValue)
                {
                    pubDate = DateTime.UtcNow;
                }
                postUrl = urlHelper.RouteUrl(_blogRoutes.PostWithDateRouteName,
                    new
                    {
                        year = pubDate.Value.Year,
                        month = pubDate.Value.Month.ToString("00"),
                        day = pubDate.Value.Day.ToString("00"),
                        slug = post.Slug
                    });
            }
            else
            {
                postUrl = urlHelper.RouteUrl(_blogRoutes.PostWithoutDateRouteName,
                    new { slug = post.Slug });
            }

            return Task.FromResult(postUrl);

        }

        //public async Task ConvertToRelativeUrls(IPost post, IProjectSettings projectSettings)
        //{
        //    var httpContext = _contextAccessor.HttpContext;
        //    var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccesor.ActionContext);
        //    var imageAbsoluteBaseUrl = urlHelper.Content("~" + projectSettings.LocalMediaVirtualPath);
        //    if (httpContext != null)
        //    {
        //        imageAbsoluteBaseUrl = httpContext.Request.AppBaseUrl() + projectSettings.LocalMediaVirtualPath;
        //    }

        //    // open live writer passes in posts with absolute urls
        //    // we want to change them to relative to keep the files portable
        //    // to a different root url
        //    post.DraftContent = await _contentProcessor.ConvertMediaUrlsToRelative(
        //        projectSettings.LocalMediaVirtualPath,
        //        imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
        //        post.DraftContent);
        //    // olw also adds hard coded style to images
        //    post.DraftContent = _contentProcessor.RemoveImageStyleAttribute(post.DraftContent);

        //    post.Content = await _contentProcessor.ConvertMediaUrlsToRelative(
        //        projectSettings.LocalMediaVirtualPath,
        //        imageAbsoluteBaseUrl, //this shold be resolved from virtual using urlhelper
        //        post.Content);

        //    post.Content = _contentProcessor.RemoveImageStyleAttribute(post.Content);
        //}

        public async Task ConvertMediaToRelativeUrls(IPost post)
        {
            var baseUrl = string.Concat(
                        _contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        _contextAccessor.HttpContext.Request.Host.ToUriComponent()
                        );
            
            // open live writer passes in posts with absolute urls
            // we want to change them to relative to keep the files portable
            // to a different root url
            post.DraftContent = await _contentProcessor.ConvertMediaUrlsToRelative(
                baseUrl, //this shold be resolved from virtual using urlhelper
                post.DraftContent);
            // olw also adds hard coded style to images
            post.DraftContent = _contentProcessor.RemoveImageStyleAttribute(post.DraftContent);

            post.Content = await _contentProcessor.ConvertMediaUrlsToRelative(
                baseUrl, //this shold be resolved from virtual using urlhelper
                post.Content);

            post.Content = _contentProcessor.RemoveImageStyleAttribute(post.Content);
        }

        public Task ConvertMediaToAbsoluteUrls(IPost post, IProjectSettings projectSettings)
        {
            string baseUrl = projectSettings.CdnUrl;
            if(string.IsNullOrWhiteSpace(baseUrl))
            {
                baseUrl = string.Concat(
                        _contextAccessor.HttpContext.Request.Scheme,
                        "://",
                        _contextAccessor.HttpContext.Request.Host.ToUriComponent()
                        );
            }

            post.Content = _contentProcessor.ConvertUrlsToAbsolute(baseUrl, post.Content);
            post.DraftContent = _contentProcessor.ConvertUrlsToAbsolute(baseUrl, post.DraftContent);

            return Task.CompletedTask;
        }


    }
}
