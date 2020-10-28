using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace backend.Controllers
{
    [ApiController]
    [Route("/")]
    public class ServeSvg : Controller
    {
        VisitorCountService m_visitor;
        TemplatingService m_templating;
        GithubInfoService m_github;

        public ServeSvg(VisitorCountService visitor, TemplatingService templating, GithubInfoService github)
        {
            m_visitor = visitor;
            m_templating = templating;
            m_github = github;
        }

        [HttpGet]
        public async Task<ContentResult> Get()
        {
            m_visitor.Increase();

            Response.ContentType = "image/svg+xml";
            Response.Headers.Add("Cache-Control", "no-cache,max-age=0,no-store,s-maxage=0,proxy-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "-1");

            var gh = await m_github.FetchInfo();
            var contrib = await m_github.FetchContributions();

            var svg = System.IO.File.ReadAllText("serve.svg");
            svg = m_templating.Perform(svg, "visitors", m_visitor.Visitors);
            svg = m_templating.Perform(svg, "repos_total", $"{gh.PublicRepos + gh.OwnedPrivateRepos}");
            svg = m_templating.Perform(svg, "repos_private", $"{gh.OwnedPrivateRepos}");
            svg = m_templating.Perform(svg, "gists_total", $"{gh.PublicGists + gh.PrivateGists}");
            svg = m_templating.Perform(svg, "gists_private", $"{gh.PrivateGists}");
            svg = m_templating.Perform(svg, "contributions_count", $"{contrib.Years.First().Total}");

            return Content(svg, "image/svg+xml");
        }
    }
}
