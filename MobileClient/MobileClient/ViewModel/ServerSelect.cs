﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
		#endregion

		#region Commands
		public Command ConnectCommand { get; private set; }
		public Command AddServerCommand { get; private set; }
		#endregion

		public ServerSelect() {
			ConnectCommand = makeConnectCommand();
			AddServerCommand = makeAddServerCommand();

			ServerList = new ObservableCollection<Model.Server>();

			//debug
			ServerList.Add(new Model.Server() {
				DisplayName = "Server 1",
				Ip = "1.1.1.1",
				Port = 1
			});
			ServerList.Add(new Model.Server() {
				DisplayName = "Server 2",
				Ip = "2.2.2.2",
				Port = 2
			});
			ServerList.Add(new Model.Server() {
				DisplayName = "Server 3",
				Ip = "3.3.3.3",
				Port = 3
			});
			ServerList.Add(new Model.Server() {
				DisplayName = "Server 4",
				Ip = "4.4.4.4",
				Port = 4
			});
			ServerList.Add(new Model.Server() {
				DisplayName = "Server 5",
				Ip = "5.5.5.5",
				Port = 5
			});
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
					await Application.Current.MainPage.Navigation.PushAsync(new View.ServerAddPage(), false);
				},
				canExecute: () => {
					return true;
				}
			);
		}

		private async void ConnectToServer(Model.Server server) {
			await Application.Current.MainPage.Navigation.PushAsync(new View.LoginPage(), false);
		}
	}
}