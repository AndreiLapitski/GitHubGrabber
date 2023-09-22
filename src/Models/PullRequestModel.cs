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
            var linksFromTitle = GetJiraTicketNumberLinksFromTitle();
            var linksFromDescription = GetJiraTicketNumberLinksFromDescription();
            var unitedLinks = linksFromDescription.Union(linksFromTitle);
            var linksToTickets = new List<Uri>();
            linksToTickets.AddRange(unitedLinks);

            return linksToTickets.Distinct();
        }

        public List<string> GetJiraTicketNumbersFromTitle()
        {
            var linkFromTitleRegex = new Regex(OutputWorksheetConstants.LinkFromTitleRegex);
            var forbiddenStrings = SettingsHelper.GetTicketNamePartsForExclude();
            var filteredTicketNames = linkFromTitleRegex.Matches(Title).Filter(forbiddenStrings);

            return filteredTicketNames;
        }

        public List<Uri> GetJiraTicketNumberLinksFromTitle()
        {
            var ticketNumbers = GetJiraTicketNumbersFromTitle();
            if (!ticketNumbers.Any())
            {
                return new List<Uri>();
            }

            var jiraLinks = ticketNumbers.Select(ticketNumber =>
                $"{SettingsHelper.Get(SettingKey.JiraTeamBaseUrl)}/browse/{ticketNumber}".ToUri());

            return jiraLinks.ToList();
        }

        public List<Uri> GetJiraTicketNumberLinksFromDescription()
        {
            if (string.IsNullOrEmpty(Description))
            {
                return new List<Uri>();
            }

            var forbiddenStrings = SettingsHelper.GetTicketNamePartsForExclude();
            var pattern = string.Format(
                OutputWorksheetConstants.LinksFromDescriptionRegex,
                SettingsHelper.GetJiraTeamName());
            var linksFromDescriptionRegex = new Regex(pattern);
            var linksAsStrings = linksFromDescriptionRegex.Matches(Description)
                .Filter(forbiddenStrings)
                .Distinct()
                .ToList();

            return CollectionHelper.ConvertToUrls(linksAsStrings).ToList();
        }
    }
}
