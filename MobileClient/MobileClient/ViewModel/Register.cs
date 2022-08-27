using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class Register : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		private string emailText;
		public string EmailText {
			get => emailText;
			set {
				if (emailText != value) {
					emailText = value;
					NotifyPropertyChanged("EmailText");
					RegisterCommand.ChangeCanExecute();
				}
			}
		}

		private string displayNameText;
		public string DisplayNameText {
			get => displayNameText;
			set {
				if (displayNameText != value) {
					displayNameText = value;
					NotifyPropertyChanged("DisplayNameText");
					RegisterCommand.ChangeCanExecute();
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
					RegisterCommand.ChangeCanExecute();
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
					RegisterCommand.ChangeCanExecute();
				}
			}
		}

		private string repeatPasswordText;
		public string RepeatPasswordText {
			get => repeatPasswordText;
			set {
				if (repeatPasswordText != value) {
					repeatPasswordText = value;
					NotifyPropertyChanged("RepeatPasswordText");
					RegisterCommand.ChangeCanExecute();
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

		public Command RegisterCommand { get; private set; }
		public Command LoginCommand { get; private set; }
		
		public Register() {
			RegisterCommand = makeRegisterCommand();
			LoginCommand = makeLoginCommand();
		}

		private Command makeRegisterCommand() {
			return new Command(
				execute: () => {
					if (PasswordText == RepeatPasswordText)
						RegisterUser(EmailText, DisplayNameText, LoginText, PasswordText);
					else {
						ErrorText = "Hasła nie są takie same";
						ErrorOpacity = 1;
					}
				},
				canExecute: () => {
					return (EmailText != null && EmailText != "") && (DisplayNameText != null && DisplayNameText != "") && (LoginText != null && LoginText != "") && (PasswordText != null && PasswordText != "");
				}
			);
		}

		private Command makeLoginCommand() {
			return new Command(
				execute: async () => {
					await Application.Current.MainPage.Navigation.PushAsync(new View.LoginPage());
				},
				canExecute: () => {
					return true;
				}
			);
		}

		private void RegisterUser(string email, string displayName, string login, string passowrd) {
			Application.Current.MainPage = new NavigationPage(new View.ListPage());
		}
	}
}
