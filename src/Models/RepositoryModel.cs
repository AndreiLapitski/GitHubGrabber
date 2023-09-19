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
                var linksFromDescription = pullRequest.GetJiraTicketNumbersFromDescription().ToList();
                var linkFromTitle = pullRequest.GetJiraTicketNumberLinkFromTitle();
                if (linkFromTitle != null && !linksFromDescription.Contains(linkFromTitle))
                {
                    linksFromDescription.Add(linkFromTitle);
                }

                linksToTickets.AddRange(linksFromDescription);
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
