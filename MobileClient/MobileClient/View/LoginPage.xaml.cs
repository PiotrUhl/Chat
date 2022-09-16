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

		protected override void OnAppearing() {
			((ViewModel.Login)BindingContext).OnAppearing();
			base.OnAppearing();
		}
	}
}