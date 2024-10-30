namespace GoogleSeoRanking.Scraper
{
	public interface ISeoRankingService
	{

		Task<int> GetUrlRankingAsync(string url, string keyword);


	}
}