using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileClient.View {
	public partial class ListPage : ContentPage {
		public ListPage() {
			InitializeComponent();
		}

		private async void SettingsButton_Clicked(object sender, EventArgs e) {
			await Navigation.PushAsync(new SettingsPage());
		}
		private async void ContactButton_Clicked(object sender, EventArgs e) {
			await Navigation.PushAsync(new ConversationPage());
		}
	}
}