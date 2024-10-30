using GoogleSeoRanking.Scraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GoogleSeoRanking.Scraper.Tests
{
	public class LinkSelectorTests
	{
		private readonly LinkSelector _xpathSelector;

		public LinkSelectorTests()
		{
			_xpathSelector = new LinkSelector();
		}

		[Fact]
		public void SelectElements_ShouldReturnElements_WhenValidXPath()
		{
			// Arrange
			string xml = "<root><item>Value1</item><item>Value2</item></root>";
			string xpath = "//item";

			// Act
			IEnumerable<string> result = _xpathSelector.SelectAttributesToString(xml, xpath);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
		}

		[Fact]
		public void SelectElements_ShouldReturnHrefLink_WhenValidXPath()
		{
			// Arrange
			string xml = "<root><item><a href='https://example.com'>Value1</a></item><item>Value2</item></root>";
			string xpath = "//item/a";

			// Act
			IEnumerable<string> result = _xpathSelector.SelectAttributesToString(xml, xpath);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(1, result.Count());
			Assert.Equal("https://example.com", result.First());
		}


		[Fact]
		public void SelectElements_ShouldReturnEmpty_WhenNoMatchingElements()
		{
			// Arrange
			string xml = "<root><item>Value1</item></root>";
			string xpath = "//nonexistent";

			// Act
			IEnumerable<string> result = _xpathSelector.SelectAttributesToString(xml, xpath);

			// Assert
			Assert.NotNull(result);
			Assert.Empty(result);
		}

		[Fact]
		public void SelectElements_ShouldThrowException_WhenEmptyXPath()
		{
			// Arrange
			string xml = "<root><item>Value1</item></root>";
			string invalidXPath = ""; // Invalid XPath


			// Act & Assert
			Assert.Throws<XPathException>(() => _xpathSelector.SelectAttributesToString(xml, invalidXPath).ToList());
		}

		

		[Fact]
		public void SelectElements_ShouldReturnElements_WhenValidXDocumentInput()
		{
			// Arrange
			var document ="<root><item>Value1</item><item>Value2</item></root>";
			string xpath = "//item";

			// Act
			IEnumerable<string> result = _xpathSelector.SelectAttributesToString(document, xpath);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(2, result.Count());
		}


	}
}
