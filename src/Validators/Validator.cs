namespace GitHubGrabber.Validators
{
    public class Validator
    {
        public static ValidationResultModel ValidateInputFile(string? path)
        {
            var validationResult = new ValidationResultModel();
            if (string.IsNullOrEmpty(path))
            {
                validationResult.ErrorMessage = ValidationMessages.PathIsNullOrEmptyError;
                return validationResult;
            }

            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                validationResult.ErrorMessage = ValidationMessages.FileDoesNotExistsError;
                return validationResult;
            }

            if (fileInfo.Extension != ".csv")
            {
                validationResult.ErrorMessage = ValidationMessages.InvalidFileFormatError;
                return validationResult;
            }

            validationResult.IsSuccess = true;
            return validationResult;
        }
    }
}
