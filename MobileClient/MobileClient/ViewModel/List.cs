using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class List : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		public Model.User loggedUser;
		public Model.User LoggedUser {
			get => loggedUser;
			set {
				if (loggedUser != value) {
					loggedUser = value;
					NotifyPropertyChanged("LoggedUser");
				}
			}
		}

		public ObservableCollection<Model.User> ContactList { get; set; }

		public Command SettingsCommand { get; private set; }
		public Command ContactCommand { get; private set; }

		public List() {
			SettingsCommand = makeSettingsCommand();
			ContactCommand = makeContactCommand();

			ContactList = new ObservableCollection<Model.User> {
				new Model.User {
					Id = 1,
					Name = "Kontakt 1"
				},
				new Model.User {
					Id = 2,
					Name = "Kontakt 2"
				},
				new Model.User {
					Id = 3,
					Name = "Kontakt 3"
				},
				new Model.User {
					Id = 4,
					Name = "Kontakt 4"
				},
				new Model.User {
					Id = 5,
					Name = "Kontakt 5"
				}
			};
		}

		private Command makeSettingsCommand() {
			return new Command(
				execute: async () => {
					await Application.Current.MainPage.Navigation.PushAsync(new View.SettingsPage());
				},
				canExecute: () => {
					return true;
				}
			);
		}

		private Command makeContactCommand() {
			return new Command<Model.User>(
				execute: async (Model.User contact) => {
					var page = new View.ConversationPage();
					((ViewModel.Conversation)page.BindingContext).Contact = contact;
					await Application.Current.MainPage.Navigation.PushAsync(page);
				},
				canExecute: (Model.User contact) => {
					return true;
				}
			);
		}
	}
}
