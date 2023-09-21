namespace GitHubGrabber.Constants
{
    public class OutputWorksheetConstants
    {
        public const string HeaderRange = "A1:E1";

        public const string ColumnNameId = "ID";
        public const string ColumnNamePullRequest = "Pull request";
        public const string ColumnNameJiraLink = "JIRA link";
        public const string ColumnNameJiraTitle = "JIRA title";
        public const string ColumnNameJqlLink = "Jql link";

        public const int ColumnId = 1;
        public const int ColumnPullRequest = 2;
        public const int ColumnJiraLink = 3;
        public const int ColumnJiraTitle = 4;
        public const int ColumnJqlLink = 5;

        public const string ValueNotAvailable = "N/A";

        public const string LinkFromTitleRegex = @"(?<=^|\s)[A-Za-z\d]+[-]\d+(?=[\s:]|$)";
        public const string LinksFromDescriptionRegex = "https:\\/\\/{0}\\.atlassian\\.net\\/browse\\/\\w+-\\d+";
    }
}
