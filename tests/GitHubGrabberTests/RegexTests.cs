namespace GitHubGrabberTests
{
    public class RegexTests
    {
        [Test]
        [TestCase("ABCD-123, ABC-987 hello", "ABCD-123,ABC-987")]
        [TestCase("ABCD-123,ABC-987 hello", "ABCD-123,ABC-987")]
        [TestCase("ABCD-123 ABC-987 hello", "ABCD-123,ABC-987")]
        [TestCase("ABCD-123, hello", "ABCD-123")]
        [TestCase("ABCD-123 hello", "ABCD-123")]
        [TestCase("A23D-123: test abw", "A23D-123")]
        [TestCase("awe A23D-123: test abw", "A23D-123")]
        [TestCase("awe 555 A23D-123: test abw", "A23D-123")]
        [TestCase(" A23D-123: test abw", "A23D-123")]
        [TestCase("A23D-123 ", "A23D-123")]
        public void LinkFromTitle_Success(string title, string expectedResult)
        {
            // Arrange
            var linkFromTitleRegex = new Regex(OutputWorksheetConstants.LinkFromTitleRegex);

            // Act
            var matches = linkFromTitleRegex.Matches(title);
            var result = string.Join(Separator.Comma, matches);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("# [CORE-23989](https://myteam.atlassian.net/browse/CORE-23989)\r\n\r\n## Screen Shots\r\n![image](https://github.com/test/MyRepo/assets/83586919/94297d0f-5add-4928-9756-0fe7eedb1354)\r\n\r\n## Testing Notes\r\n> What was done to ensure that the updates do not break things?",
            "https://myteam.atlassian.net/browse/CORE-23989")]
        [TestCase("### Note for code review\r\n* Verify if a unit test is here for every new method\r\n* Ask for screenshot for any UI change\r\n* JIRA ticket + Description in commit names and PR Title\r\n* Run autotest on local branch(smoke suit) and attach results (screenshots/ReportPortal links) before and after changes. (not required) \r\n  how to run autotests on local fansight: https://github.com/testowner/SQE-Test-Ticket-Purchase/tree/master/gridlastic\r\n\r\n### Changes (What specifically changed?)\r\n* [PR](https://github.com/testowner/axs-b2b-ssm/pull/318)\r\n* [PR](https://github.com/testowner/axs-b2b-ssm/pull/322)\r\n\r\n### Possible Drawbacks/Concerns (What should reviewers look out for?)\r\n*\r\n\r\n### Testing Notes (How do we know this works & doesn't break other things)\r\n* see PRs\r\n\r\n\r\n### Related Issues\r\n* [EOMC-17146](https://myteam.atlassian.net/browse/EOMC-17146)\r\n* [EOMC-17164](https://myteam.atlassian.net/browse/EOMC-17164)\r\n* [EOMC-17168](https://myteam.atlassian.net/browse/EOMC-17168)\r\n* [EOMC-17169](https://myteam.atlassian.net/browse/EOMC-17169)\r\n\r\n\r\n[EOMC-17146]: https://myteam.atlassian.net/browse/EOMC-17146?atlOrigin=eyJpIjoiNWRkNTljNzYxNjVmNDY3MDlhMDU5Y2ZhYzA5YTRkZjUiLCJwIjoiZ2l0aHViLWNvbS1KU1cifQ\r\n[EOMC-17164]: https://myteam.atlassian.net/browse/EOMC-17164?atlOrigin=eyJpIjoiNWRkNTljNzYxNjVmNDY3MDlhMDU5Y2ZhYzA5YTRkZjUiLCJwIjoiZ2l0aHViLWNvbS1KU1cifQ\r\n[EOMC-17168]: https://myteam.atlassian.net/browse/EOMC-17168?atlOrigin=eyJpIjoiNWRkNTljNzYxNjVmNDY3MDlhMDU5Y2ZhYzA5YTRkZjUiLCJwIjoiZ2l0aHViLWNvbS1KU1cifQ\r\n[EOMC-17169]: https://myteam.atlassian.net/browse/EOMC-17169?atlOrigin=eyJpIjoiNWRkNTljNzYxNjVmNDY3MDlhMDU5Y2ZhYzA5YTRkZjUiLCJwIjoiZ2l0aHViLWNvbS1KU1cifQ",
            "https://myteam.atlassian.net/browse/EOMC-17146,https://myteam.atlassian.net/browse/EOMC-17164,https://myteam.atlassian.net/browse/EOMC-17168,https://myteam.atlassian.net/browse/EOMC-17169")]
        public void LinkFromDescription_Success(string description, string expectedResult)
        {
            // Arrange
            var teamName = "myteam";
            var pattern = string.Format(
                OutputWorksheetConstants.LinksFromDescriptionRegex,
                teamName);
            var linkFromDescriptionRegex = new Regex(pattern);

            // Act
            var matches = linkFromDescriptionRegex.Matches(description)
                .Select(match => match.Value)
                .Distinct()
                .ToList();
            var result = string.Join(Separator.Comma, matches);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
