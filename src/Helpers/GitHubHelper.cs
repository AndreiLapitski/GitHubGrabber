namespace GitHubGrabber.Helpers
{
    public class GitHubHelper
    {
        private readonly string _accessToken;
        private readonly string _ownerName;

        public GitHubHelper(string accessToken, string ownerName)
        {
            _accessToken = accessToken;
            _ownerName = ownerName;
        }

        public async Task<IEnumerable<RepositoryModel>> GetRepositoriesAsync(
            IEnumerable<string> repositoryNames,
            uint itemsPerPage,
            PullRequestStatus status)
        {
            if (repositoryNames == null)
            {
                throw new ArgumentNullException(nameof(repositoryNames));
            }

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 10
            };

            var repositories = new ConcurrentBag<RepositoryModel>();
            async ValueTask Body(string repositoryName, CancellationToken token)
            {
                var pullRequests = await GetPullRequestsAsync(repositoryName, itemsPerPage, status);
                var pullRequestModels =
                    pullRequests.Select(pr =>
                            new PullRequestModel(
                                pr.Url.ToUri(),
                                pr.Title,
                                pr.Body))
                        .ToList();
                var repository = new RepositoryModel(repositoryName, pullRequestModels);
                repositories.Add(repository);

                ConsoleWriter.WriteLine($"{repositoryName} repository has {pullRequests.Count} PRs");
            }

            await Parallel.ForEachAsync(repositoryNames, parallelOptions, Body);

            return repositories;
        }

        public async Task<List<PullRequestDto>> GetPullRequestsAsync(
            string repositoryName,
            uint perPage,
            PullRequestStatus state)
        {
            if (string.IsNullOrEmpty(repositoryName))
            {
                throw new ArgumentNullException(nameof(repositoryName));
            }

            if (perPage > 100)
            {
                throw new ArgumentException($"{nameof(perPage)} = {perPage}. Maximum 100.");
            }

            var allPullRequests = new List<PullRequestDto>();
            uint pageNumber = 1;
            var gitHubApiHttpClient = new GitHubApiClient(_accessToken);
            while (true)
            {
                var requestUri = gitHubApiHttpClient.BuildGetPullRequestsUri(_ownerName, repositoryName, pageNumber, perPage, state);
                var response = await gitHubApiHttpClient.GetAsync(requestUri);
                var content = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Call to GIT HUB is failed. " +
                                        $"HTTP status code = {response.StatusCode}. " +
                                        $"Message = {content}");
                }

                var contentStream = await response.Content.ReadAsStreamAsync();
                var pullRequests = await JsonSerializer.DeserializeAsync<List<PullRequestDto>>(contentStream);
                if (!pullRequests.Any())
                {
                    break;
                }

                allPullRequests.AddRange(pullRequests);
                pageNumber++;
            }

            return allPullRequests;
        }
    }
}
