using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileClient.View {
	public partial class ServerSelectPage : ContentPage {
		public ServerSelectPage() {
			InitializeComponent();
		}

		private async void Button_Clicked(object sender, EventArgs e) {
			await Navigation.PushAsync(new LoginPage());
		}
	}
}
