namespace GitHubGrabber.Helpers
{
    public class CollectionHelper
    {
        public static List<List<T>> SplitIntoChunks<T>(IEnumerable<T> items, int chunkSize)
        {
            return items
                .Select((item, index) => new { item, index })
                .GroupBy(x => x.index / chunkSize)
                .Select(group => group.Select(x => x.item).ToList())
                .ToList();
        }

        public static string ConvertToJql(Uri baseUri, IEnumerable<string> strings, char separator)
        {
            var ticketNamesAsString = string.Join(separator, strings);
            return $"{baseUri}issues/?jql=key%20in({ticketNamesAsString})";
        }

        public static IEnumerable<Uri> ConvertToUrls(List<string>? strings)
        {
            if (strings == null || !strings.Any())
            {
                return Enumerable.Empty<Uri>();
            }

            return (from string str in strings
                where Uri.IsWellFormedUriString(str, UriKind.Absolute)
                select str.ToUri()).ToList();
        }
    }
}
