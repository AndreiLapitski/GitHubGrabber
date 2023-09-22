namespace GitHubGrabber.Extensions
{
    public static class MatchCollectionExtension
    {
        public static List<string> Filter(
            this MatchCollection matches,
            string[] forbiddenStrings,
            char toChar)
        {
            var validStrings = new List<string>();
            foreach (Match match in matches)
            {
                var matchString = match.Value;
                var isForbiddenString = forbiddenStrings.Any(forbiddenString =>
                    matchString.Contains($"{forbiddenString}{toChar}"));
                if (!isForbiddenString)
                {
                    validStrings.Add(matchString);
                }
            }

            return validStrings;
        }
    }
}
