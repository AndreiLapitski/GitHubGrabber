namespace GitHubGrabberTests
{
    public class GitHubApiClientTests
    {
        private GitHubApiClient _gitHubApiClient;

        [Test]
        [TestCase("https://test.com", "ownerName", "repositoryName", "1", "100", PullRequestStatus.All, "https://test.com/repos/ownerName/repositoryName/pulls?page=1&per_page=100&state=all")]
        public void BuildGetPullRequestsUri_Success(
            string baseAddress,
            string ownerName,
            string repositoryName,
            uint startPageNumber,
            uint itemsPerPage,
            PullRequestStatus pullRequestStatus,
            string expected)
        {
            // Arrange
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _gitHubApiClient = new GitHubApiClient(httpClient);
            var expectedResult = new Uri(expected);

            // Act
            var result = _gitHubApiClient.BuildGetPullRequestsUri(
                ownerName,
                repositoryName,
                startPageNumber,
                itemsPerPage,
                pullRequestStatus);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("https://test.com", null, "repositoryName", "1", "100", PullRequestStatus.All)]
        [TestCase("https://test.com", "", "repositoryName", "1", "100", PullRequestStatus.All)]
        [TestCase("https://test.com", "ownerName", null, "1", "100", PullRequestStatus.All)]
        [TestCase("https://test.com", "ownerName", "", "1", "100", PullRequestStatus.All)]
        [TestCase("https://test.com", "ownerName", "repositoryName", "1", "101", PullRequestStatus.All)]
        public void BuildGetPullRequestsUri_Fail(
            string baseAddress,
            string ownerName,
            string repositoryName,
            uint startPageNumber,
            uint itemsPerPage,
            PullRequestStatus pullRequestStatus)
        {
            // Arrange
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            _gitHubApiClient = new GitHubApiClient(httpClient);

            // Act
            TestDelegate action = () =>
                _gitHubApiClient.BuildGetPullRequestsUri(
                    ownerName,
                    repositoryName,
                    startPageNumber,
                    itemsPerPage,
                    pullRequestStatus);

            // Assert
            Assert.Throws<ArgumentException>(action);
        }
    }
}