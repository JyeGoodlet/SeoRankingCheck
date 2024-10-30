
using HtmlAgilityPack;
using System.Net;
using System.Net.Http;

namespace GoogleSeoRanking.Scraper
{
	public class UrlDownloader : IUrlDownloader
	{
		private readonly HttpClient _httpClient;


		public UrlDownloader(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<(string Content, HttpStatusCode StatusCode)> DownloadUrlAsync(string url)
		{
			var web = new HtmlWeb();


			var response = await _httpClient.GetAsync(url);
			return  (await response.Content.ReadAsStringAsync(), response.StatusCode );

		}
	}
}
