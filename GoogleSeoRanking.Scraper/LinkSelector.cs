using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GoogleSeoRanking.Scraper
{
	public class LinkSelector : iLinkSelector
	{

		public IEnumerable<string> SelectAttributesToString(string xml, string xpath)
		{
			var htmlDoc = new HtmlDocument();

			htmlDoc.LoadHtml(xml);

			var links = htmlDoc.DocumentNode.SelectNodes(xpath);

			if (links != null)
			{
				foreach (var item in links)
				{
					var attribute = item.Attributes["href"]?.Value ?? "";
					yield return attribute;
				}
			}

			yield break;
		}

		
	  


	}
}
