namespace GitHubGrabber.Helpers
{
    public class WorksheetHelper
    {
        public static async Task SaveOutputFileAsync(IEnumerable<RepositoryModel> repositories, int chunkSize, string pathToFile)
        {
            if (repositories == null || string.IsNullOrEmpty(pathToFile))
            {
                throw new ArgumentException($"{nameof(repositories)} or {pathToFile} are invalid.");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using var package = new ExcelPackage();
            foreach (var repository in repositories)
            {
                var worksheet = package.Workbook.Worksheets.Add(repository.Name);
                worksheet.SetStyles(chunkSize);

                var id = 1;
                var row = 2;
                foreach (var pullRequest in repository.PullRequests)
                {
                    var jiraTicketLinks = pullRequest.GetJiraTicketNumbersFromDescription().ToList();
                    var linksCount = jiraTicketLinks.Count;
                    switch (linksCount)
                    {
                        case 0:
                            worksheet.SetValue(row, OutputWorksheetConstants.ColumnId, id);
                            worksheet.SetLinkValue(row, OutputWorksheetConstants.ColumnPullRequest, pullRequest.Uri);
                            worksheet.SetValue(row, OutputWorksheetConstants.ColumnJiraTitle, pullRequest.Title);
                            worksheet.SetValue(row, OutputWorksheetConstants.ColumnJiraLink, OutputWorksheetConstants.ValueNotAvailable);
                            break;

                        case 1:
                            worksheet.SetValue(row, OutputWorksheetConstants.ColumnId, id);
                            worksheet.SetLinkValue(row, OutputWorksheetConstants.ColumnPullRequest, pullRequest.Uri);
                            worksheet.SetValue(row, OutputWorksheetConstants.ColumnJiraTitle, pullRequest.Title);
                            worksheet.SetLinkValue(row, OutputWorksheetConstants.ColumnJiraLink, jiraTicketLinks.First());
                            break;

                        default:
                            var fromRow = row;
                            var toRow = fromRow + linksCount - 1;

                            worksheet.Merge(fromRow, OutputWorksheetConstants.ColumnId, toRow, OutputWorksheetConstants.ColumnId);
                            worksheet.Merge(fromRow, OutputWorksheetConstants.ColumnPullRequest, toRow, OutputWorksheetConstants.ColumnPullRequest);
                            worksheet.Merge(fromRow, OutputWorksheetConstants.ColumnJiraTitle, toRow, OutputWorksheetConstants.ColumnJiraTitle);

                            worksheet.SetValue(fromRow, OutputWorksheetConstants.ColumnId, id);
                            worksheet.SetLinkValue(row, OutputWorksheetConstants.ColumnPullRequest, pullRequest.Uri);
                            worksheet.SetValue(row, OutputWorksheetConstants.ColumnJiraTitle, pullRequest.Title);
                            for (var i = 0; i < linksCount; i++)
                            {
                                worksheet.SetLinkValue(row + i, OutputWorksheetConstants.ColumnJiraLink, jiraTicketLinks[i]);
                            }

                            row = toRow;
                            break;
                    }

                    id++;
                    row++;
                }

                var jqlQueries = repository.GetTicketsAsJqlQueries(chunkSize);
                row = 2;
                foreach (var jqlQuery in jqlQueries)
                {
                    worksheet.SetLinkValue(
                        row,
                        OutputWorksheetConstants.ColumnJqlLink,
                        jqlQuery.ToUri());
                    row++;
                }
            }

            var excelFile = new FileInfo(pathToFile);
            await package.SaveAsAsync(excelFile);
        }
    }
}
