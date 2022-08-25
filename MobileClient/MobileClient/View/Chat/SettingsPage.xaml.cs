using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient.View {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage {
		public SettingsPage() {
			InitializeComponent();
		}

		private void LogoutButton_Clicked(object sender, EventArgs e) {
			Application.Current.MainPage = new NavigationPage(new ServerSelectPage());
		}
	}
}