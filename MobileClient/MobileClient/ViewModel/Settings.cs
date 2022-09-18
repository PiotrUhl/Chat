using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class Settings : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		#region Properties
		private string notificationSelected;
		public string NotificationSelected {
			get => notificationSelected;
			set {
				if (notificationSelected != value) {
					notificationSelected = value;
					NotifyPropertyChanged("NotificationSelected");
					if (value == "Enabled") {
						((App)Application.Current).NotificationsEnabled = true;
					}
					else {
						((App)Application.Current).NotificationsEnabled = false;
					}
					if (value == "TempDisabled") {
						TemporaryDisableTime = DateTime.Now.TimeOfDay;
						TimePickerVisibility = 1;
					}
					else
						TimePickerVisibility = 0;
				}
			}
		}
		private TimeSpan temporaryDisableTime = DateTime.Now.TimeOfDay;
		public TimeSpan TemporaryDisableTime {
			get => temporaryDisableTime;
			set {
				if (temporaryDisableTime != value) {
					temporaryDisableTime = value;
					NotifyPropertyChanged("TemporaryDisableTime");
				}
			}
		}
		private byte timePickerVisibility = 0;
		public byte TimePickerVisibility {
			get => timePickerVisibility;
			set {
				if (timePickerVisibility != value) {
					timePickerVisibility = value;
					NotifyPropertyChanged("TimePickerVisibility");
				}
			}
		}

		private string displayName;
		public string DisplayName {
			get => displayName;
			set {
				if (displayName != value) {
					displayName = value;
					NotifyPropertyChanged("DisplayName");
				}
			}
		}
		private string email;
		public string Email {
			get => email;
			set {
				if (email!= value) {
					email = value;
					NotifyPropertyChanged("Email");
				}
			}
		}
		private string login;
		public string Login {
			get => login;
			set {
				if (login != value) {
					login = value;
					NotifyPropertyChanged("Login");
				}
			}
		}
		private string password;
		public string Password {
			get => password;
			set {
				if (password != value) {
					password = value;
					NotifyPropertyChanged("Password");
				}
			}
		}
		#endregion

		#region Commands
		public ICommand LogoutCommand { get; private set; }

		public ICommand ChangeDisplayNameCommand { get; private set; }
		public ICommand ChangeEmailCommand { get; private set; }
		public ICommand ChangeLoginCommand { get; private set; }
		public ICommand ChangePasswordCommand { get; private set; }
		#endregion

		public Settings() {
			notificationSelected = ((App)Application.Current).NotificationsEnabled ? "Enabled" : "Disabled";
			displayName = ((App)Application.Current).User.DisplayName;

			LogoutCommand = makeLogoutCommand();

			ChangeDisplayNameCommand = makePromptCommand(() => DisplayName, value => DisplayName = value, "Zmień nazwę wyświetlaną", "Wprowadź nową nazwę wyświetlaną");
			ChangeEmailCommand = makePromptCommand(() => Email, value => Email = value, "Zmień email", "Wprowadź nowy adres email");
			ChangeLoginCommand = makePromptCommand(() => Login, value => Login = value, "Zmień login", "Wprowadź nowy login");
			ChangePasswordCommand = makePromptCommand(() => Password, value => Password = value, "Zmień hasło", "Wprowadź nowe hasło");
		}

		private Command makeLogoutCommand() {
			return new Command(
				execute: async () => {
					using (var context = new Model.Context()) {
						context.GlobalSettings.First().LoggedUserId = 0;
						context.SaveChanges();
					}
					((App)Application.Current).User = null;
					Application.Current.MainPage = new NavigationPage(new View.ServerSelectPage());
				},
				canExecute: () => {
					return true;
				}
			);
		}
		private Command makePromptCommand(Func<string> getter, Action<string> setter, string title, string message) {
			return new Command(
				execute: async () => {
					string result = await App.Current.MainPage.DisplayPromptAsync(
						title: title,
						message: message,
						initialValue: getter.Invoke()
					);
					if (result != null)
						setter(result);
				},
				canExecute: () => {
					return true;
				}
			);
		}
	}
}
