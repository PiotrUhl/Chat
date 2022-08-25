using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient.View {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage {
		public LoginPage() {
			InitializeComponent();
		}
		private void LoginButton_Clicked(object sender, EventArgs e) {
			Application.Current.MainPage = new NavigationPage(new ListPage());
		}
		private async void RegisterButton_Clicked(object sender, EventArgs e) {
			await Navigation.PushAsync(new RegisterPage());
		}
	}
}