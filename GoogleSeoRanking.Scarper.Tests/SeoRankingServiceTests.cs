using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace GoogleSeoRanking.Scraper.Tests
{
	public class SeoRankingServiceTests
	{
		private readonly Mock<IConfiguration> _configurationMock;
		private readonly Mock<IUrlDownloader> _urlDownloaderMock;
		private readonly iLinkSelector _xPathSelector;
		private readonly SeoRankingService _seoRankingService;

		public SeoRankingServiceTests()
		{
			_configurationMock = new Mock<IConfiguration>();
			_urlDownloaderMock = new Mock<IUrlDownloader>();
			_xPathSelector = new LinkSelector();

			_seoRankingService = new SeoRankingService(
				_configurationMock.Object,
				_urlDownloaderMock.Object,
				_xPathSelector
			);

			// Default configuration settings
			_configurationMock.Setup(config => config["MaxRankingsToCheck"]).Returns("10");
			_configurationMock.Setup(config => config["SearchResultXpath"]).Returns("/html/body/a/@href");
			_configurationMock.Setup(config => config["NextPageXPath"]).Returns("//html/body/a[2]/@href");
			_configurationMock.Setup(config => config["SearchUrl"]).Returns("https://google.com");

		}

		[Fact]
		public async Task GetUrlRankingAsync_ReturnsRanking_WhenUrlIsFoundOnFirstPage()
		{
			// Arrange
			string url = "https://findme.com";
			string keyword = "example keyword";

			_urlDownloaderMock.Setup(d => d.DownloadUrlAsync(_configurationMock.Object["SearchUrl"]))
				.ReturnsAsync((
				@"
<html>
<body>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://findme.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>

</body>
</html>", HttpStatusCode.OK));


			// Act
			var ranking = await _seoRankingService.GetUrlRankingAsync(url, keyword);

			// Assert
			Assert.Equal(3, ranking);
		}



		[Fact]
		public async Task GetUrlRankingAsync_ReturnsNegativeRanking_WhenNoRecordFound()
		{
			// Arrange
			string url = "https://findme.com";
			string keyword = "example keyword";

			_urlDownloaderMock.Setup(d => d.DownloadUrlAsync(_configurationMock.Object["SearchUrl"]))
				.ReturnsAsync((
				@"
<html>
<body>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>

</body>
</html>", HttpStatusCode.OK));


			// Act
			var ranking = await _seoRankingService.GetUrlRankingAsync(url, keyword);

			// Assert
			Assert.Equal(-1, ranking);
		}

		[Fact]
		public async Task GetUrlRankingAsync_ReturnsException_WhenStatusCodeNot200()
		{
			// Arrange
			string url = "https://findme.com";
		string keyword = "example keyword";

		_urlDownloaderMock.Setup(d => d.DownloadUrlAsync(_configurationMock.Object["SearchUrl"]))
				.ReturnsAsync((
				@"
<html>
<body>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>

</body>
</html>", HttpStatusCode.NotFound));


			// Act

			// Assert
			await Assert.ThrowsAsync<Exception>(  () =>  _seoRankingService.GetUrlRankingAsync(url, keyword));
		}

		[Fact]
		public async Task GetUrlRankingAsync_ReturnsRanking_WhenOnSecondPage()
		{
			// Arrange
			string url = "https://findme.com";
			string keyword = "example keyword";

			_urlDownloaderMock
				.Setup(d => d.DownloadUrlAsync(_configurationMock.Object["SearchUrl"]))
				.ReturnsAsync((
				@"
<html>
<body>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>


</body>
</html>", HttpStatusCode.OK));



			_urlDownloaderMock
		.Setup(d => d.DownloadUrlAsync("https://example.com"))
		.ReturnsAsync(
				(
				@"
<html>
<body>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://findme.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>
<a href='https://example.com'>Example</a>


</body>
</html>", HttpStatusCode.OK));


			// Act
			var ranking = await _seoRankingService.GetUrlRankingAsync(url, keyword);

			// Assert
			Assert.Equal(8, ranking);
		}
	}






}


