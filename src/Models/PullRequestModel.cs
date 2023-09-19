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
