using System.Xml.Linq;

namespace GoogleSeoRanking.Scraper
{
	public interface iLinkSelector
	{

		IEnumerable<string> SelectAttributesToString(string xml, string xpath);

	}
}