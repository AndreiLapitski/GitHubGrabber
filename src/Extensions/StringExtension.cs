namespace GitHubGrabber.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class StringExtension
    {
        public static Uri ToUri(this string str)
        {
            return new Uri(str);
        }
    }
}
