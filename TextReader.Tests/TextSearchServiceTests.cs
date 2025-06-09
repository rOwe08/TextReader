using TextReader.Services;
using TextReader.Models;

namespace TextReader.Tests
{
    public class TextSearchServiceTests
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
        public void Search_EmptyQuery_ClearsMatches()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "test2" });
            var service = new TextSearchService(provider);
            service.Search("test");

            // Act
            service.Search("");

            // Assert
            Assert.Empty(service.Matches);
        }

        [Fact]
        public void Search_ValidQuery_FindsAllMatches()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "test2", "test3", "other" });
            var service = new TextSearchService(provider);

            // Act
            service.Search("test");

            // Assert
            Assert.Equal(3, service.Matches.Count);
            Assert.Equal(new[] { 0, 1, 2 }, service.Matches);
        }

        [Fact]
        public void FindNext_WithMatches_ReturnsNextMatch()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "test2", "test3" });
            var service = new TextSearchService(provider);
            service.Search("test");

            // Act & Assert
            Assert.Equal(0, service.FindNext());
            Assert.Equal(1, service.FindNext());
            Assert.Equal(2, service.FindNext());
            Assert.Null(service.FindNext()); // No more matches
        }

        [Fact]
        public void FindPrevious_WithMatches_ReturnsPreviousMatch()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "test2", "test3" });
            var service = new TextSearchService(provider);
            service.Search("test");
            
            // Move to last match
            service.FindNext(); // 0
            service.FindNext(); // 1
            service.FindNext(); // 2
            service.FindNext(); // null, but current index stays at 2

            // Act & Assert
            var result1 = service.FindPrevious();
            Assert.Equal(1, result1);
            var result2 = service.FindPrevious();
            Assert.Equal(0, result2);
            var result3 = service.FindPrevious();
            Assert.Null(result3); // No more matches
        }

        [Fact]
        public void Search_CaseInsensitive_FindsMatches()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "TEST1", "test2", "Test3" });
            var service = new TextSearchService(provider);

            // Act
            service.Search("test");

            // Assert
            Assert.Equal(3, service.Matches.Count);
        }

        [Fact]
        public void CurrentMatchIndex_ReflectsCurrentPosition()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "test2", "test3" });
            var service = new TextSearchService(provider);
            service.Search("test");

            // Act & Assert
            Assert.Equal(-1, service.CurrentMatchIndex); // Initial state
            service.FindNext();
            Assert.Equal(0, service.CurrentMatchIndex);
            service.FindNext();
            Assert.Equal(1, service.CurrentMatchIndex);
        }

        [Fact]
        public void TotalMatches_ReflectsTotalMatches()
        {
            // Arrange
            var provider = new MockTextProvider(new[] { "test1", "test2", "test3", "other" });
            var service = new TextSearchService(provider);

            // Act
            service.Search("test");

            // Assert
            Assert.Equal(3, service.TotalMatches);
        }
    }
} 