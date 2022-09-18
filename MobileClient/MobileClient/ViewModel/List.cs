using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

		private int NewestMessageId = 0;

		public Model.User loggedUser = ((App)Application.Current).User;
		public Model.User LoggedUser {
			get => loggedUser;
			set {
				if (loggedUser != value) {
					loggedUser = value;
					NotifyPropertyChanged("LoggedUser");
				}
			}
		}

		private ObservableCollection<Model.Contact> contactList;
		public ObservableCollection<Model.Contact> ContactList {
			get => contactList;
			set {
				if (contactList != value) {
					contactList = value;
					NotifyPropertyChanged("ContactList");
				}
			}
		}

		public Command SettingsCommand { get; private set; }
		public Command ContactCommand { get; private set; }

		public List() {
			SettingsCommand = makeSettingsCommand();
			ContactCommand = makeContactCommand();

			ReflilContactList();
		}

		public void ReflilContactList() {
			using var context = new Model.Context();
			ContactList = new ObservableCollection<Model.Contact>(context.Contacts);
		}

		public void SetContactNew(int id) {
			ContactList.Single(c => c.Id == id).New = true;
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
			return new Command<Model.Contact>(
				execute: async (Model.Contact contact) => {
					contact.New = false;

					using (var context = new Model.Context()) {
						context.Contacts.Single(c => c.Id == contact.Id).New = false;
						context.SaveChanges();
					}

					var page = new View.ConversationPage();
					((ViewModel.Conversation)page.BindingContext).Contact = contact;
					await Application.Current.MainPage.Navigation.PushAsync(page);
				},
				canExecute: (Model.Contact contact) => {
					return true;
				}
			);
		}
	}
}
