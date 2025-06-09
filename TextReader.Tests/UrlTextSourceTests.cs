using System.Net;
using Moq.Protected;
using TextReader.Sources;
using TextReader.Models;

namespace TextReader.Tests
{
    public class UrlTextSourceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly UrlTextSource _source;

        public UrlTextSourceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _source = new UrlTextSource("https://example.com/test.txt", _httpClient);
        }

        [Fact]
        public async Task GetLinesAsync_PlainText_ReturnsCorrectLines()
        {
            // Arrange
            var content = "line1\nline2\nline3";
            SetupMockResponse(content);

            // Act
            var lines = await _source.GetLinesAsync();
            
            // Assert
            Assert.NotNull(lines);
            Assert.Equal(3, lines.Count());
            Assert.Equal("line1", lines.ElementAt(0));
            Assert.Equal("line2", lines.ElementAt(1));
            Assert.Equal("line3", lines.ElementAt(2));
        }

        [Fact]
        public async Task GetLinesAsync_HtmlContent_RemovesHtmlTags()
        {
            // Arrange
            var content = "<html><body><p>line1</p><div>line2</div><span>line3</span></body></html>";
            SetupMockResponse(content);

            // Act
            var lines = await _source.GetLinesAsync();
            
            // Assert
            Assert.NotNull(lines);
            foreach (var line in lines)
            {
                Assert.False(line.Contains("<") && line.Contains(">"), "HTML tags should be removed");
            }
        }

        [Fact]
        public async Task GetLinesAsync_InvalidUrl_ThrowsException()
        {
            // Arrange
            SetupMockResponse(HttpStatusCode.NotFound);

            // Act & Assert
            await Assert.ThrowsAsync<TextSourceException>(() => _source.GetLinesAsync());
        }

        [Fact]
        public async Task GetLinesAsync_CancellationRequested_ThrowsTaskCanceledException()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .Returns<HttpRequestMessage, CancellationToken>((_, token) =>
                {
                    token.ThrowIfCancellationRequested();
                    return Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });
                });

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() => 
                _source.GetLinesAsync(cts.Token));
        }

        [Fact]
        public void Name_ReturnsCorrectFormat()
        {
            // Arrange
            var url = "https://example.com/test.txt";
            var source = new UrlTextSource(url, _httpClient);

            // Act
            var name = source.Name;

            // Assert
            Assert.Equal($"URL: {url}", name);
        }

        private void SetupMockResponse(string content)
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(content)
                });
        }

        private void SetupMockResponse(HttpStatusCode statusCode)
        {
            _mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode
                });
        }
    }
} 