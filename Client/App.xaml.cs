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
		protected override void OnStartup(StartupEventArgs e) {
			base.OnStartup(e);
			var window = new View.MainWindow();
			var viewmodel = new ViewModel.Main();
			window.DataContext = viewmodel;
			window.Show();
			var worker = new Logic.Worker(viewmodel);
			Task checkingTask = Task.Run(() => worker.RunChecking());
		}
	}
}
