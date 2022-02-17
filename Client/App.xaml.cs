using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Client {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {

		private Backend.Network network = null;

		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);
			network = new Backend.Network();
			var window = new View.MainWindow();
			var viewmodel = new ViewModel.Main(network);
			window.DataContext = viewmodel;
			window.Show();
			{
				var loginWindow = new View.Login();
				var loginViewModel = new ViewModel.Login(network, loginWindow);
				loginWindow.DataContext = loginViewModel;
				loginWindow.ShowDialog();
				viewmodel.LoggedUserId = loginViewModel.UserId;
			}
			viewmodel.Init();
			var worker = new Backend.Worker(viewmodel, network);
			Task checkingTask = Task.Run(() => worker.Run());
		}
	}
}
