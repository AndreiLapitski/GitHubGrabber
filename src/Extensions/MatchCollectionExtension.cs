namespace GitHubGrabber.Extensions
{
    public static class MatchCollectionExtension
    {
        public static List<string> Filter(this MatchCollection matches, string[] forbiddenStrings)
        {
            var filteredStrings = new List<string>();
            foreach (Match match in matches)
            {
                var matchString = match.Value;
                var isForbiddenString = forbiddenStrings.Any(forbiddenString => matchString.Contains(forbiddenString));
                if (!isForbiddenString)
                {
                    filteredStrings.Add(matchString);
                }
            }

            return filteredStrings;
        }
    }
}
