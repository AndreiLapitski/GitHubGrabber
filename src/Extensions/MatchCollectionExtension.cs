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
                var isValidString = true;
                foreach (var forbiddenString in forbiddenStrings)
                {
                    var matchStringFirstSplitPart = matchString.Split(toChar).First();
                    if (matchStringFirstSplitPart.Equals(forbiddenString, StringComparison.InvariantCultureIgnoreCase))
                    {
                        isValidString = false;
                        break;
                    }
                }

                if (isValidString)
                {
                    validStrings.Add(matchString);
                }
            }

            return validStrings;
        }
    }
}
