namespace GitHubGrabber
{
    internal class Program
    {
        static async Task Main()
        {
            SettingsHelper.CheckAppConfigs();
            ConsoleWriter.WriteLine("Please, enter an absolute path to a csv file:");
            var pathToInputFile = Console.ReadLine();
            var fileValidationResult = Validator.ValidateInputFile(pathToInputFile);
            while (!fileValidationResult.IsSuccess)
            {
                Console.WriteLine(fileValidationResult.ErrorMessage);
                pathToInputFile = Console.ReadLine();
                fileValidationResult = Validator.ValidateInputFile(pathToInputFile);
            }

            var timer = new Stopwatch();
            timer.Start();

            ConsoleWriter.WriteLine("List of repositories to be processed:", NewLineIn.Start);
            var inputFileHelper = new InputFileHelper(pathToInputFile);
            var repositoryNames = inputFileHelper.GetRepositoryNames();
            ConsoleWriter.WriteLines(repositoryNames);

            ConsoleWriter.WriteLine("Processing...", NewLineIn.Start);
            var accessToken = SettingsHelper.Get(SettingKey.AccessToken);
            var ownerName = SettingsHelper.Get(SettingKey.OwnerName);
            var gitHubHelper = new GitHubHelper(accessToken, ownerName);
            var repositories =
                await gitHubHelper.GetRepositoriesAsync(
                    repositoryNames,
                    100,
                    PullRequestStatus.All);

            var outputFileName = SettingsHelper.Get(SettingKey.OutputFileName);
            var outputFileHelper = new OutputFileHelper(outputFileName);
            var outputFilePath = outputFileHelper.GetAbsoluteOutputFilePath();
            var chunkSize = SettingsHelper.Get(SettingKey.ChunkSize);
            await WorksheetHelper.SaveOutputFileAsync(repositories, int.Parse(chunkSize), outputFilePath);
            ConsoleWriter.WriteLine($"Output file has been created. You can find it here: {outputFilePath}", NewLineIn.Start);

            timer.Stop();
            ConsoleWriter.WriteLine($"Time: {timer.Elapsed:m\\:ss\\.fffff}");
        }
    }
}