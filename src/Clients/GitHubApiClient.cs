namespace GitHubGrabber.Clients
{
    public class GitHubApiClient
    {
        private readonly HttpClient _httpClient;

        public GitHubApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [ExcludeFromCodeCoverage]
        public GitHubApiClient(string accessToken)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = SettingsHelper.Get(SettingKey.GitHubApiBaseUrl).ToUri()
            };

            SetHeaders(accessToken);
        }

        [ExcludeFromCodeCoverage]
        private void SetHeaders(string accessToken)
        {
            var applicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            _httpClient.DefaultRequestHeaders.Add("User-Agent", applicationName);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
        }

        public Uri BuildGetPullRequestsUri(
            string ownerName,
            string repositoryName,
            uint startPageNumber,
            uint itemsPerPage,
            PullRequestStatus status)
        {
            if (string.IsNullOrEmpty(ownerName))
            {
                throw new ArgumentException(nameof(ownerName));
            }

            if (string.IsNullOrEmpty(repositoryName))
            {
                throw new ArgumentException(nameof(repositoryName));
            }

            if (itemsPerPage > 100)
            {
                throw new ArgumentException(
                    "Invalid number of items per page. Limit = 100 items");
            }

            var builder = new UriBuilder($"{_httpClient.BaseAddress}repos/{ownerName}/{repositoryName}/pulls");

            var queryParameters = HttpUtility.ParseQueryString(builder.Query);
            queryParameters["page"] = startPageNumber.ToString();
            queryParameters["per_page"] = itemsPerPage.ToString();
            queryParameters["state"] = status.ToString().ToLower();
            builder.Query = queryParameters.ToString();

            return builder.ToString().ToUri();
        }

        [ExcludeFromCodeCoverage]
        public async Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }
    }
}
