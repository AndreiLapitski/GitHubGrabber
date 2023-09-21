namespace GitHubGrabber.Models
{
    public class PullRequestModel
    {
        public Uri Uri { get; }

        public string Title { get; }

        public string Description { get; }

        public PullRequestModel(
            Uri uri,
            string title,
            string description)
        {
            Uri = uri;
            Title = title;
            Description = description;
        }

        public IEnumerable<Uri> GetAllUniqueJiraLinks()
        {
            var linksToTickets = new List<Uri>();
            var linksFromDescription = GetJiraTicketNumbersFromDescription().ToList();
            var linkFromTitle = GetJiraTicketNumberLinkFromTitle();
            if (linkFromTitle != null && !linksFromDescription.Contains(linkFromTitle))
            {
                linksFromDescription.Add(linkFromTitle);
            }

            linksToTickets.AddRange(linksFromDescription);

            return linksToTickets.Distinct();
        }

        public string? GetJiraTicketNumberFromTitle()
        {
            var linkFromTitleRegex = new Regex(OutputWorksheetConstants.LinkFromTitleRegex);
            var match = linkFromTitleRegex.Matches(Title).FirstOrDefault();
            return match?.Value;
        }

        public Uri? GetJiraTicketNumberLinkFromTitle()
        {
            var ticketNumber = GetJiraTicketNumberFromTitle();
            return string.IsNullOrEmpty(ticketNumber)
                ? null
                : $"{SettingsHelper.Get(SettingKey.JiraTeamBaseUrl)}/browse/{ticketNumber}".ToUri();
        }

        public IEnumerable<Uri> GetJiraTicketNumbersFromDescription()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return new List<Uri>();
            }

            var pattern = string.Format(
                OutputWorksheetConstants.LinksFromDescriptionRegex,
                SettingsHelper.GetJiraTeamName());
            var linksFromDescriptionRegex = new Regex(pattern);
            var linksAsStrings = linksFromDescriptionRegex.Matches(Description)
                .Select(match => match.Value)
                .Distinct()
                .ToList();

            return CollectionHelper.ConvertToUrls(linksAsStrings);
        }
    }
}
