using GoogleSeoRanking.Scraper;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GoogleSeoRanking.UI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly ISeoRankingService _seoRankingService;

		public MainWindow(ISeoRankingService seoRankingService)
		{
			_seoRankingService = seoRankingService;
			
			InitializeComponent();
		}



		public async void RankingSearchButtonClickHandler(object sender, RoutedEventArgs e)
		{
			try
			{

				if (!string.IsNullOrEmpty(Searchurl.Text) && !string.IsNullOrEmpty(SearchTermInput.Text))
				{

					//hide previous rank
					RankingResultDisplay.Visibility = Visibility.Collapsed;


					//Set loading indicator to true
					LoadingIcon.Visibility = Visibility.Visible;


					// submit to SeoRankingService
					var ranking = await _seoRankingService.GetUrlRankingAsync(Searchurl.Text, SearchTermInput.Text);

					//hide loading indicator
					LoadingIcon.Visibility = Visibility.Collapsed;

					//set
					if (ranking > 0)
					{

						RankingResultDisplay.Content = $"You are ranked {ranking}";

						//if they are in the top 10 lets make the text greeen
						if (ranking <= 10)
						{
							RankingResultDisplay.Foreground = new SolidColorBrush(Colors.Green);
						}
						else
						{
							RankingResultDisplay.Foreground = new SolidColorBrush(Colors.Red);

						}

					}
					else
					{
						RankingResultDisplay.Content = "Sorry we could not find the ranking of your site";
						RankingResultDisplay.Foreground = new SolidColorBrush(Colors.Black);
					}
					RankingResultDisplay.Visibility = Visibility.Visible;

				}
				else
				{
					MessageBox.Show(messageBoxText: "Please enter search url and seach terms");
				}
			}
			catch (Exception ex)
			{
				LoadingIcon.Visibility = Visibility.Visible;

				//TODO: in production we probably would log the ex message and show a more generic error
				MessageBox.Show(ex.Message, "Error finding ranking", MessageBoxButton.OK, MessageBoxImage.Error);

			}
		}
	}
}