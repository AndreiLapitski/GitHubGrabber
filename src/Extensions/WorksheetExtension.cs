namespace GitHubGrabber.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class WorksheetExtension
    {
        public static void Merge(
            this ExcelWorksheet worksheet,
            int fromRow,
            int fromColumn,
            int toRow,
            int toColumn)
        {
            worksheet.Cells[fromRow, fromColumn, toRow, toColumn].Merge = true;
        }

        public static void SetLinkValue(
            this ExcelWorksheet worksheet,
            int row,
            int column,
            Uri link)
        {
            worksheet.Cells[row, column].Style.Font.UnderLine = true;
            worksheet.Cells[row, column].Style.Font.Color.SetColor(Color.Blue);
            worksheet.Cells[row, column].SetHyperlink(link);
        }

        public static void SetStyles(this ExcelWorksheet worksheet, int chunkSize)
        {
            worksheet.Cells[OutputWorksheetConstants.HeaderRange].LoadFromArrays(new List<string[]>
            {
                new[]
                {
                    OutputWorksheetConstants.ColumnNameId,
                    OutputWorksheetConstants.ColumnNamePullRequest,
                    OutputWorksheetConstants.ColumnNameJiraLink,
                    OutputWorksheetConstants.ColumnNameJiraTitle,
                    $"{OutputWorksheetConstants.ColumnNameJqlLink} ({chunkSize} tickets per row)"
                }
            });

            worksheet.Cells[OutputWorksheetConstants.HeaderRange].Style.Font.Bold = true;
            worksheet.Cells[OutputWorksheetConstants.HeaderRange].Style.Font.Size = 18;
            worksheet.Cells[OutputWorksheetConstants.HeaderRange].Style.Font.Color.SetColor(Color.Black);

            worksheet.Column(OutputWorksheetConstants.ColumnId).Width = 10;
            worksheet.Column(OutputWorksheetConstants.ColumnPullRequest).Width = 60;
            worksheet.Column(OutputWorksheetConstants.ColumnJiraLink).Width = 50;
            worksheet.Column(OutputWorksheetConstants.ColumnJiraTitle).Width = 100;
            worksheet.Column(OutputWorksheetConstants.ColumnJqlLink).Width = 200;
        }
    }
}
