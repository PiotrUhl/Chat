using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class ServerSelect : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		#region Properties
		public ObservableCollection<Model.Server> ServerList { get; set; }

		private Model.Server selectedServer = null;
		public Model.Server SelectedServer {
			get => selectedServer;
			set {
				if (selectedServer != value) {
					selectedServer = value;
					NotifyPropertyChanged("ServerSelected");
					ConnectCommand?.ChangeCanExecute();
				}
			}
		}

		private string errorText = "";
		public string ErrorText {
			get => errorText;
			set {
				if (errorText != value) {
					errorText = value;
					NotifyPropertyChanged("ErrorText");
				}
			}
		}

		private bool busyVisible = false;
		public bool BusyVisible {
			get => busyVisible;
			set {
				if (busyVisible != value) {
					busyVisible = value;
					NotifyPropertyChanged("BusyVisible");
				}
			}
		}
		#endregion

		#region Commands
		public Command ConnectCommand { get; private set; }
		public Command AddServerCommand { get; private set; }
		#endregion

		public ServerSelect() {
			ConnectCommand = makeConnectCommand();
			AddServerCommand = makeAddServerCommand();

			using var context = new Model.Context();
			ServerList = new ObservableCollection<Model.Server>(context.Servers.OrderByDescending(s => s.LastConnected));
		}

		public void OnAppearing() {
			using var context = new Model.Context();
			var connectedServer = context.Servers.Where(s => s.Id == context.GlobalSettings.First().ConnectedServerId).FirstOrDefault();
			if (connectedServer != null)
				ConnectToServer(connectedServer);
		}

		private Command makeConnectCommand() {
			return new Command(
				execute: () => {
					ConnectToServer(SelectedServer);
				},
				canExecute: () => {
					return SelectedServer != null;
				}
			);
		}
		private Command makeAddServerCommand() {
			return new Command(
				execute: async () => {
					ErrorText = "";
					var page = new View.ServerAddPage();
					((ServerAdd)page.BindingContext).AddServerCallback = (Model.Server server) => ServerList.Add(server);
					await Application.Current.MainPage.Navigation.PushAsync(page, false);
				},
				canExecute: () => {
					return true;
				}
			);
		}

		private async void ConnectToServer(Model.Server server) {
			((App)Application.Current).Network = new Backend.Network(server.Ip, server.Port);
			BusyVisible = true;
			if (await ((App)Application.Current).Network.TryConnectAsync() == true) {
				((App)Application.Current).Server = server;
				using (var context = new Model.Context()) {
					server.LastConnected = DateTime.Now;
					context.GlobalSettings.First().ConnectedServerId = server.Id;
					context.SaveChanges();
				}
				BusyVisible = false;
				var page = new View.LoginPage();
				await Application.Current.MainPage.Navigation.PushAsync(page, false);
			}
			else {
				((App)Application.Current).Network = null;
				((App)Application.Current).Server = null;
				ErrorText = "Błąd połączenia z serwerem";
				BusyVisible = false;
			}
		}
	}
}
