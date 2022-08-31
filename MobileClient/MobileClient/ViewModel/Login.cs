using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		public Login() {
			LoginCommand = makeLoginCommand();
			RegisterCommand = makeRegisterCommand();
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

		private void LogUserIn(string login, string password) {
			var page = new View.ListPage();
			((ViewModel.List)page.BindingContext).LoggedUser = new Model.User() { Id = 0, Name = login };
			Application.Current.MainPage = new NavigationPage(page);
		}

	}
}
