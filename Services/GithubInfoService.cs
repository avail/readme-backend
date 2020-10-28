using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace backend.Services
{
    public class GithubInfoService
    {
        // generated via https://json2csharp.com
        public class GithubInfo
        {
            [JsonProperty("login")]
            public string Login { get; set; }

            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("node_id")]
            public string NodeId { get; set; }

            [JsonProperty("avatar_url")]
            public string AvatarUrl { get; set; }

            [JsonProperty("gravatar_id")]
            public string GravatarId { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }

            [JsonProperty("html_url")]
            public string HtmlUrl { get; set; }

            [JsonProperty("followers_url")]
            public string FollowersUrl { get; set; }

            [JsonProperty("following_url")]
            public string FollowingUrl { get; set; }

            [JsonProperty("gists_url")]
            public string GistsUrl { get; set; }

            [JsonProperty("starred_url")]
            public string StarredUrl { get; set; }

            [JsonProperty("subscriptions_url")]
            public string SubscriptionsUrl { get; set; }

            [JsonProperty("organizations_url")]
            public string OrganizationsUrl { get; set; }

            [JsonProperty("repos_url")]
            public string ReposUrl { get; set; }

            [JsonProperty("events_url")]
            public string EventsUrl { get; set; }

            [JsonProperty("received_events_url")]
            public string ReceivedEventsUrl { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("site_admin")]
            public bool SiteAdmin { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("company")]
            public object Company { get; set; }

            [JsonProperty("blog")]
            public string Blog { get; set; }

            [JsonProperty("location")]
            public string Location { get; set; }

            [JsonProperty("email")]
            public object Email { get; set; }

            [JsonProperty("hireable")]
            public object Hireable { get; set; }

            [JsonProperty("bio")]
            public string Bio { get; set; }

            [JsonProperty("twitter_username")]
            public string TwitterUsername { get; set; }

            [JsonProperty("public_repos")]
            public int PublicRepos { get; set; }

            [JsonProperty("public_gists")]
            public int PublicGists { get; set; }

            [JsonProperty("followers")]
            public int Followers { get; set; }

            [JsonProperty("following")]
            public int Following { get; set; }

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("updated_at")]
            public DateTime UpdatedAt { get; set; }

            [JsonProperty("private_gists")]
            public int PrivateGists { get; set; }

            [JsonProperty("total_private_repos")]
            public int TotalPrivateRepos { get; set; }

            [JsonProperty("owned_private_repos")]
            public int OwnedPrivateRepos { get; set; }

            [JsonProperty("disk_usage")]
            public int DiskUsage { get; set; }

            [JsonProperty("collaborators")]
            public int Collaborators { get; set; }

            [JsonProperty("two_factor_authentication")]
            public bool TwoFactorAuthentication { get; set; }

            // we don't really care about anything in here
            //[JsonProperty("plan")]
            //public Plan Plan { get; set; }
        }
        public class Range
        {
            [JsonProperty("start")]
            public string Start { get; set; }

            [JsonProperty("end")]
            public string End { get; set; }
        }

        public class Year
        {
            [JsonProperty("year")]
            public string Year_ { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("range")]
            public Range Range { get; set; }
        }

        public class Contribution
        {
            [JsonProperty("date")]
            public string Date { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("color")]
            public string Color { get; set; }

            [JsonProperty("intensity")]
            public int Intensity { get; set; }
        }

        public class ContributionInfo
        {
            [JsonProperty("years")]
            public List<Year> Years { get; set; }

            [JsonProperty("contributions")]
            public List<Contribution> Contributions { get; set; }
        }



        public GithubInfoService()
        {
            m_client = new HttpClient();
            m_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", $"{Environment.GetEnvironmentVariable("GITHUB_API_TOKEN")}");
            m_client.DefaultRequestHeaders.UserAgent.ParseAdd("HelloWorldThisIsAvail");
        }

        DateTime m_lastUpdate;
        GithubInfo m_cachedResponse;
        HttpClient m_client;

        async Task<T> GetContent<T>(string url)
        {
            var res = await m_client.GetAsync(url);
            var content = await res.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<GithubInfo> FetchInfo()
        {
            var now = DateTime.Now;

            if (m_lastUpdate == null || (now - m_lastUpdate).Minutes > 5)
            {
                m_cachedResponse = await GetContent<GithubInfo>("https://api.github.com/user");
                m_lastUpdate = now;
            }

            return m_cachedResponse;
        }


        DateTime m_lastContribUpdate;
        ContributionInfo m_cachedContribResponse;

        public async Task<ContributionInfo> FetchContributions()
        {
            var now = DateTime.Now;

            if (m_lastContribUpdate == null || (now - m_lastContribUpdate).Minutes > 5)
            {
                m_cachedContribResponse = await GetContent<ContributionInfo>("https://ghcapi.now.sh/api/v1/avail");
                m_lastContribUpdate = now;
            }

            return m_cachedContribResponse;
        }
    }
}
