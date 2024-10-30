using GoogleSeoRanking.Scraper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;

namespace GoogleSeoRanking.UI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		public IServiceProvider ServiceProvider { get; private set; }

		public IConfiguration Configuration { get; private set; }

		protected override void OnStartup(StartupEventArgs e)
		{
			var builder = new ConfigurationBuilder()
			 .SetBasePath(Directory.GetCurrentDirectory())
			 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

			Configuration = builder.Build();

			var serviceCollection = new ServiceCollection();
			serviceCollection.AddScoped<IConfiguration>(_ => Configuration);

			ConfigureServices(serviceCollection);

			ServiceProvider = serviceCollection.BuildServiceProvider();

			var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
			mainWindow.Show();
		}

		private void ConfigureServices(IServiceCollection services)
		{
			services.AddHttpClient();
			services.AddTransient<ISeoRankingService, SeoRankingService>();
			services.AddTransient<IUrlDownloader, UrlDownloader>();
			services.AddTransient<iLinkSelector, LinkSelector>();


			services.AddTransient(typeof(MainWindow));
		}

	}

}
