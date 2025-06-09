using TextReader.Services;
using TextReader.Models;

namespace TextReader.Tests
{
    public class TextFilterServiceTests
    {
        private class MockTextProvider : ITextProvider
        {
            public IReadOnlyList<string> Lines { get; }

            public MockTextProvider(IReadOnlyList<string> lines)
            {
                Lines = lines;
            }
        }

        [Fact]
        public void ApplyFilter_EmptyFilter_ShowsAllLines()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "line1", "line2", "line3" });
            var searchService = new TextSearchService(provider);
            var service = new TextFilterService(provider);

            // Act
            service.ApplyFilter("");

            // Assert
            Assert.Equal(3, service.FilteredLines);
            Assert.Equal(0, service.GetOriginalIndex(0));
            Assert.Equal(1, service.GetOriginalIndex(1));
            Assert.Equal(2, service.GetOriginalIndex(2));
        }

        [Fact]
        public void ApplyFilter_WithMatchingFilter_ShowsOnlyMatchingLines()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "other", "test2", "test3" });
            var searchService = new TextSearchService(provider);
            var service = new TextFilterService(provider);

            // Act
            service.ApplyFilter("test");

            // Assert
            Assert.Equal(3, service.FilteredLines);
            Assert.Equal(0, service.GetOriginalIndex(0));
            Assert.Equal(2, service.GetOriginalIndex(1));
            Assert.Equal(3, service.GetOriginalIndex(2));
        }

        [Fact]
        public void ApplyFilter_CaseInsensitive_FindsMatches()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "TEST1", "test2", "Test3" });
            var searchService = new TextSearchService(provider);
            var service = new TextFilterService(provider);

            // Act
            service.ApplyFilter("test");

            // Assert
            Assert.Equal(3, service.FilteredLines);
        }

        [Fact]
        public void ApplyFilter_WithNoMatches_ShowsNoLines()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "line1", "line2", "line3" });
            var searchService = new TextSearchService(provider);
            var service = new TextFilterService(provider);

            // Act
            service.ApplyFilter("nonexistent");

            // Assert
            Assert.Equal(0, service.FilteredLines);
        }

        [Fact]
        public void ResetFilter_ShowsAllLines()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "other", "test2" });
            var searchService = new TextSearchService(provider);
            var service = new TextFilterService(provider);
            service.ApplyFilter("test");

            // Act
            service.ResetFilter();

            // Assert
            Assert.Equal(3, service.FilteredLines);
            Assert.Equal(0, service.GetOriginalIndex(0));
            Assert.Equal(1, service.GetOriginalIndex(1));
            Assert.Equal(2, service.GetOriginalIndex(2));
        }

        [Fact]
        public void GetOriginalIndex_WithFilteredLines_ReturnsCorrectIndex()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "other", "test2", "test3" });
            var searchService = new TextSearchService(provider);
            var service = new TextFilterService(provider);
            service.ApplyFilter("test");

            // Act & Assert
            Assert.Equal(0, service.GetOriginalIndex(0));
            Assert.Equal(2, service.GetOriginalIndex(1));
            Assert.Equal(3, service.GetOriginalIndex(2));
        }

        [Fact]
        public void GetOriginalIndex_WithInvalidIndex_ReturnsMinusOne()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "line1", "line2" });
            var searchService = new TextSearchService(provider);
            var service = new TextFilterService(provider);

            // Act & Assert
            Assert.Equal(-1, service.GetOriginalIndex(-1));
            Assert.Equal(-1, service.GetOriginalIndex(2));
        }
    }
} 