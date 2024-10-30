using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

namespace GoogleSeoRanking.Scraper.Tests
{
	public class UrlDownloaderTests
	{
		private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
		private readonly UrlDownloader _urlDownloader;

	

		[Fact]
		public async Task DownloadUrlAsync_ShouldReturnContentAndStatusCode_WhenRequestIsSuccessful()
		{
			var url = "http://example.com";

			// ARRANGE
			var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
			handlerMock
			   .Protected()
			   // Setup the PROTECTED method to mock
			   .Setup<Task<HttpResponseMessage>>(
				  "SendAsync",
				  ItExpr.IsAny<HttpRequestMessage>(),
				  ItExpr.IsAny<CancellationToken>()
			   )
			   // prepare the expected response of the mocked http call
			   .ReturnsAsync(new HttpResponseMessage()
			   {
				   StatusCode = HttpStatusCode.OK,
				   Content = new StringContent("[{'id':1,'value':'1'}]"),
			   })
			   .Verifiable();

			// use real http client with mocked handler here
			var httpClient = new HttpClient(handlerMock.Object)
			{
				BaseAddress = new Uri(url),
			};

			var urlDownloader = new UrlDownloader(httpClient);

			// Act
			var (content, statusCode) = await urlDownloader.DownloadUrlAsync("TestUrl");

			// Assert
			Assert.Equal("[{'id':1,'value':'1'}]", content);
			Assert.Equal(HttpStatusCode.OK, statusCode);
		}

		
	}
}
