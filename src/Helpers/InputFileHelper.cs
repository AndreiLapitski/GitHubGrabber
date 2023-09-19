namespace GitHubGrabber.Helpers
{
    public class InputFileHelper
    {
        private readonly string _path;

        public InputFileHelper(string path)
        {
            _path = path;
        }

        public List<string> GetRepositoryNames()
        {
            if (string.IsNullOrEmpty(_path))
            {
                throw new ArgumentNullException(nameof(_path));
            }

            using var reader = new StreamReader(_path);
            var lineIndex = 0;
            var hasFileInvalidLinks = false;
            var repositoryLinks = new List<Uri>();
            var errorMessage = new StringBuilder("Document has invalid links. Please, fix the following lines:");
            while (!reader.EndOfStream)
            {
                lineIndex++;
                var line = reader.ReadLine();
                var isLink = Uri.IsWellFormedUriString(line, UriKind.Absolute);
                if (isLink)
                {
                    repositoryLinks.Add(line.ToUri());
                }
                else
                {
                    hasFileInvalidLinks = true;
                    errorMessage.AppendLine($"Line: {lineIndex}. Line value: {line}.");
                }
            }

            if (hasFileInvalidLinks)
            {
                throw new ArgumentException(errorMessage.ToString());
            }

            return repositoryLinks.Select(repositoryLink => repositoryLink.Segments.Last()).ToList();
        }
    }
}
