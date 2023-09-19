namespace GitHubGrabberTests
{
    public class CollectionHelperTests
    {
        [Test]
        [TestCase(new[] { "a", "b", "c", "d" }, 2, 2)]
        [TestCase(new[]{ "a", "b", "c", "d", "e" }, 2, 3)]
        public void SplitIntoChunks_Success(
            string[] strings,
            int chunkSize,
            int expectedChunkSize)
        {
            // Arrange & Act
            var result =
                CollectionHelper.SplitIntoChunks(strings, chunkSize);
            var resultChunksCount = result.Count;

            // Assert
            Assert.That(expectedChunkSize, Is.EqualTo(resultChunksCount));
        }

        [Test]
        [TestCase("https://test.com", new[] { "a" }, ',', "https://test.com/issues/?jql=key%20in(a)")]
        [TestCase("https://test.com", new[] { "a", "b" }, ',', "https://test.com/issues/?jql=key%20in(a,b)")]
        public void ConvertToJql_Success(Uri baseUrl, string[] strings, char separator, string expectedResult)
        {
            // Arrange & Act
            var result = CollectionHelper.ConvertToJql(baseUrl, strings, separator);

            // Assert
            Assert.That(expectedResult, Is.EqualTo(result));
        }

        [Test]
        [TestCase(null, null)]
        [TestCase(new[] { "https://test.com", "https://test2.com" }, new[] { "https://test.com", "https://test2.com" })]
        public void ConvertToUrls_Success(string[] strings, string[] expectedStrings)
        {
            // Arrange
            var stringList = new List<string>();
            if (strings != null)
            {
                stringList = strings.ToList();
            }

            var expectedResult = new List<Uri>();
            if (expectedStrings != null)
            {
                expectedResult.AddRange(expectedStrings.Select(str => new Uri(str)));
            }

            // Act
            var result = CollectionHelper.ConvertToUrls(stringList);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}
