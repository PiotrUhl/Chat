using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: ExportFont("MaterialIcons-Regular.ttf", Alias = "Material")]

namespace MobileClient {
	public partial class App : Application {

		public Backend.Network Network { get; set; } = null;
		public Model.Server Server { get; set; } = null;
		public Model.User User { get; set; } = null;
		public bool NotificationsEnabled { get; set; } = true;

		public App() {
			InitializeComponent();

			MainPage = new NavigationPage(new View.ServerSelectPage());
		}

		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
