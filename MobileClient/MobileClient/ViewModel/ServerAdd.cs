using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class ServerAdd : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		public delegate void AddServerDelegate(Model.Server server);
		public AddServerDelegate AddServerCallback;

		private string ipText;
		public string IpText {
			get => ipText;
			set {
				if (ipText != value) {
					ipText = value;
					NotifyPropertyChanged("IpText");
					AddServerCommand.ChangeCanExecute();
				}
			}
		}

		private string portText;
		public string PortText {
			get => portText;
			set {
				if (portText != value) {
					portText = value;
					NotifyPropertyChanged("PortText");
					AddServerCommand.ChangeCanExecute();
				}
			}
		}

		private string nameText;
		public string NameText {
			get => nameText;
			set {
				if (nameText != value) {
					nameText = value;
					NotifyPropertyChanged("NameText");
					AddServerCommand.ChangeCanExecute();
				}
			}
		}

		private string errorText;
		public string ErrorText {
			get => errorText;
			set {
				if (errorText != value) {
					errorText = value;
					NotifyPropertyChanged("ErrorText");
				}
			}
		}

		private byte errorOpacity = 0;
		public byte ErrorOpacity {
			get => errorOpacity;
			set {
				if (errorOpacity != value) {
					errorOpacity = value;
					NotifyPropertyChanged("ErrorOpacity");
				}
			}
		}

		public Command AddServerCommand { get; private set; }

		public ServerAdd() {
			AddServerCommand = makeAddServerCommand();
		}

		private Command makeAddServerCommand() {
			return new Command(
				execute: () => {
					AddServer(IpText, PortText, NameText);
				},
				canExecute: () => {
					return (IpText != null && IpText != "") && (PortText != null && PortText != "") && (NameText != null && NameText != "");
				}
			);
		}

		private async void AddServer(string ip, string port, string name) {
			var server = new Model.Server() { Ip = ip, Port = UInt16.Parse(port), DisplayName = name }; //todo: obsługa błędów (parse)
			using (var context = new Model.Context()) {
				context.Servers.Add(server);
				context.SaveChanges();
			}
			AddServerCallback(server);
			await Application.Current.MainPage.Navigation.PopAsync();
			//await Application.Current.MainPage.Navigation.PushAsync(new View.LoginPage());
		}
	}
}
