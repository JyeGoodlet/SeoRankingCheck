using System.Net;

namespace GoogleSeoRanking.Scraper
{
	public interface IUrlDownloader
	{

		Task<(string Content, HttpStatusCode StatusCode)> DownloadUrlAsync(string url);


	}
}