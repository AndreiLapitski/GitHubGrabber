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
    }
}
