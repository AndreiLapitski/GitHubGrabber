namespace GitHubGrabber.Models
{
    public class RepositoryModel
    {
        public string Name { get; }

        public List<PullRequestModel> PullRequests { get; }

        public RepositoryModel(
            string name,
            List<PullRequestModel> pullRequests)
        {
            Name = name;
            PullRequests = pullRequests;
        }

        public IEnumerable<Uri> GetAllUniqueJiraLinks()
        {
            var linksToTickets = new List<Uri>();
            foreach (var pullRequest in PullRequests)
            {
                var linksFromTitle = pullRequest.GetJiraTicketNumberLinksFromTitle();
                var linksFromDescription = pullRequest.GetJiraTicketNumberLinksFromDescription();
                var unitedLinks = linksFromDescription.Union(linksFromTitle);
                linksToTickets.AddRange(unitedLinks);
            }

            return linksToTickets.Distinct();
        }

        public IEnumerable<string> GetTicketsAsJqlQueries(int chunkSize)
        {
            var uris = GetAllUniqueJiraLinks().ToList();
            var chunks = CollectionHelper.SplitIntoChunks(uris, chunkSize);
            var baseUrl = SettingsHelper.Get(SettingKey.JiraTeamBaseUrl).ToUri();

            return chunks.Select(chunk =>
                    chunk.Select(uri => uri.Segments.LastOrDefault()))
                .Select(ticketNames =>
                    CollectionHelper.ConvertToJql(baseUrl, ticketNames, Separator.Comma));
        }
    }
}
