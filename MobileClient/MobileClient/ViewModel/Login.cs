using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class Login : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		private string serverName = "";
		public string ServerName {
			get => serverName;
			set {
				if (serverName != value) {
					serverName = value;
					NotifyPropertyChanged("ServerName");
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

		private string loginText;
		public string LoginText {
			get => loginText;
			set {
				if (loginText != value) {
					loginText = value;
					NotifyPropertyChanged("LoginText");
					LoginCommand.ChangeCanExecute();
				}
			}
		}

		private string passwordText;
		public string PasswordText {
			get => passwordText;
			set {
				if (passwordText != value) {
					passwordText = value;
					NotifyPropertyChanged("PasswordText");
					LoginCommand.ChangeCanExecute();
				}
			}
		}

		public Command LoginCommand { get; private set; }
		public Command RegisterCommand { get; private set; }
		public Command BackCommand { get; private set; }

		public Login() {
			ServerName = ((App)Application.Current).Server.DisplayName;
			LoginCommand = makeLoginCommand();
			RegisterCommand = makeRegisterCommand();
			BackCommand = makeBackCommand();
		}

		public void OnAppearing() {
			using (var context = new Model.Context()) {
				var loggedUser = context.Users.Where(u => u.Id == context.GlobalSettings.FirstOrDefault().LoggedUserId).FirstOrDefault();
				if (loggedUser != null)
					InternalLogIn(loggedUser);
			}
		}

		private Command makeLoginCommand() {
			return new Command(
				execute: () => {
					LogUserIn(LoginText, PasswordText);
				},
				canExecute: () => {
					return (LoginText != null && LoginText != "") && (PasswordText != null && PasswordText != "");
				}
			);
		}

		private Command makeRegisterCommand() {
			return new Command(
				execute: async () => {
					await Application.Current.MainPage.Navigation.PushAsync(new View.RegisterPage());
				},
				canExecute: () => {
					return true;
				}
			);
		}

		private Command makeBackCommand() {
			return new Command(
				execute: async () => {
					((App)Application.Current).Network = null;
					((App)Application.Current).Server = null;
					using (var context = new Model.Context()) {
						var settings = context.GlobalSettings.First().ConnectedServerId = 0;
						context.SaveChanges();
					}
					await Application.Current.MainPage.Navigation.PopAsync();
				},
				canExecute: () => {
					return true;
				}
			);
		}

		private void LogUserIn(string login, string password) {
			//BusyVisible = true;
			byte[] passhash = null;
			using (SHA256 mySHA256 = SHA256.Create()) {
				passhash = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
			var userId = ((App)Application.Current).Network.LogIn(login, passhash);
			if (userId > 0) {
				string userName = ((App)Application.Current).Network.GetClient(userId);
				using (var context = new Model.Context()) {
					var user = context.Users.Where(u => u.Id == userId).SingleOrDefault();
					if (user == default) {
						user = new Model.User() { ServerId = ((App)Application.Current).Server.Id, Id = userId, Login = login, DisplayName = userName };
						context.Users.Add(user);
						context.SaveChanges();
					}
					((App)Application.Current).User = user;
					ErrorText = "";
					context.GlobalSettings.First().LoggedUserId = user.Id;
					context.SaveChanges();
				}
				//BusyVisible = false;
				Application.Current.MainPage = new NavigationPage(new View.ListPage());
			}
			else {
				PasswordText = "";
				ErrorText = "Błąd logowania";
				//BusyVisible = false;
			}
		}

		private void InternalLogIn(Model.User user) {
			((App)Application.Current).User = user;
			ErrorText = "";
			using (var context = new Model.Context()) {
				context.GlobalSettings.First().LoggedUserId = user.Id;
				context.SaveChanges();
			}
			//BusyVisible = false;
			Application.Current.MainPage = new NavigationPage(new View.ListPage());
		}
	}
}
