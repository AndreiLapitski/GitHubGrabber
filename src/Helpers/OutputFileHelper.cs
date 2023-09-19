namespace GitHubGrabber.Helpers
{
    public class OutputFileHelper
    {
        public readonly string Filename;

        public OutputFileHelper(string filename)
        {
            Filename = filename;
        }

        public string GetAbsoluteOutputFilePath()
        {
            return Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Downloads", Filename);
        }
    }
}
