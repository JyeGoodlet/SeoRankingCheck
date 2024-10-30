using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSeoRanking.Scraper
{
	public class SeoRankingService : ISeoRankingService
	{
		private readonly IConfiguration _configuration;
		private readonly IUrlDownloader _urlDownloader;
		private readonly iLinkSelector _xPathSelector;

		public SeoRankingService(IConfiguration configuration, IUrlDownloader urlDownloader, iLinkSelector xPathSelector)
		{
			_configuration = configuration;
			_urlDownloader = urlDownloader;
			_xPathSelector = xPathSelector;
		}


		public async Task<int> GetUrlRankingAsync(string url, string keyword)
		{
			var maxRankingsToCheck = int.Parse(_configuration["MaxRankingsToCheck"]);
			var searchResultXPath = _configuration["SearchResultXpath"];
			var nextPageXPath = _configuration["NextPageXPath"];
			var searchUrl = _configuration["SearchUrl"];
			var rankingsChecked = 1;

			searchUrl = string.Format(searchUrl, keyword); 
			while ( rankingsChecked <= maxRankingsToCheck)
			{
				//Download site
				var result = await _urlDownloader.DownloadUrlAsync(searchUrl);
				if (result.StatusCode != HttpStatusCode.OK)
				{
					//This could be handled better.
					throw new Exception($"Expection '{HttpStatusCode.OK}' but recieved '{result.StatusCode}'");
				}


				// Use xpath to get rankings
				var rankings = _xPathSelector.SelectAttributesToString(result.Content, searchResultXPath);
				
				foreach(var ranking in rankings)
				{
					if (ranking.Contains(url))
						return rankingsChecked;
					
					rankingsChecked++;

				}

				if (rankingsChecked <= maxRankingsToCheck)
				{
					//Check if there is a next page
					var nextPageUrl = _xPathSelector.SelectAttributesToString(result.Content, nextPageXPath);
					if (string.IsNullOrWhiteSpace(nextPageUrl.FirstOrDefault()))
					{
						return -1;
					}

					searchUrl = nextPageUrl.FirstOrDefault();

				}
				


			}

			//we didn't find the result so return -1
			return -1;




		}
	}


}
