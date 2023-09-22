﻿namespace GitHubGrabberTests
{
    public class MatchCollectionExtensionTests
    {
        [Test]
        [TestCase("CORE-12, ATS-8: qwe test-1, release-18",
            new[] { "test", "release" },
            new[] { "CORE-12", "ATS-8" })]
        [TestCase("CORE-12, ATS-8: qwe test-1, release-18",
            new[] { "tes" },
            new[]{ "CORE-12", "ATS-8", "test-1", "release-18" })]
        [TestCase("CoRE-12, AtS-8: qwe Test-1, releasE-18",
            new[] { "TesT", "Release" },
            new[] { "CORE-12", "ATS-8" })]
        public void Filter_Success(
            string str,
            string[] forbiddenStrings,
            string[] expectedStrings)
        {
            // Arrange
            var pattern = OutputWorksheetConstants.LinkFromTitleRegex;
            var regex = new Regex(pattern);

            // Act
            var resultStrings = regex.Matches(str).Filter(forbiddenStrings, Separator.Dash);

            // Assert
            Assert.That(resultStrings, Is.EqualTo(expectedStrings).IgnoreCase);
        }
    }
}
