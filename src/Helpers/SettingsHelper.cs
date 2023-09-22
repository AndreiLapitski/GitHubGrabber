namespace GitHubGrabber.Helpers
{
    public class SettingsHelper
    {
        public static void CheckAppConfigs()
        {
            var settingKeys = Enum.GetValues(typeof(SettingKey));
            foreach (SettingKey settingKey in settingKeys)
            {
                Get(settingKey);
            }

            GetPullRequestStatus();
        }

        public static string Get(SettingKey key)
        {
            var value = ConfigurationManager.AppSettings.Get(key.ToString());
            if (string.IsNullOrEmpty(value))
            {
                throw new ConfigurationException($"{key} is empty or not found in the App.config file.");
            }

            return value;
        }

        public static string GetJiraTeamName()
        {
            var jiraTeamBaseUrl = Get(SettingKey.JiraTeamBaseUrl);
            return jiraTeamBaseUrl.ToUri().Host.Split(Separator.Dot).First();
        }

        public static string[] GetTicketNamePartsForExclude()
        {
            return Get(SettingKey.TicketNamePartsForExclude)
                .Split(Separator.Comma)
                .Select(str => str.ToLowerInvariant())
                .ToArray();
        }

        public static PullRequestStatus GetPullRequestStatus()
        {
            var status = Get(SettingKey.PullRequestStatus);
            Enum.TryParse(typeof(PullRequestStatus), status, true, out var result);
            if (result == null)
            {
                throw new ConfigurationException($"Invalid {SettingKey.PullRequestStatus}. " +
                                                 "Please, enter one of acceptable values into App.config. " +
                                                 "Acceptable values: All, Open, Closed");
            }

            return (PullRequestStatus)result;
        }
    }
}
