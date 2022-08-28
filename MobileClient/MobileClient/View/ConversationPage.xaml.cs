using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient.View {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConversationPage : ContentPage {
		public ConversationPage() {
			InitializeComponent();
		}

		private async void MenuButton_Clicked(object sender, EventArgs e) {
			await Navigation.PushAsync(new ContactSettingsPage());
		}
	}
}