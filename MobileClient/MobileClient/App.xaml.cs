﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileClient {
	public partial class App : Application {
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
