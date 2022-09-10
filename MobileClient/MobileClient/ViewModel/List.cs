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

		public ObservableCollection<Model.Contact> ContactList { get; set; }

		public Command SettingsCommand { get; private set; }
		public Command ContactCommand { get; private set; }

		public List() {
			SettingsCommand = makeSettingsCommand();
			ContactCommand = makeContactCommand();

			flilContactList();
		}

		private void flilContactList() {
			ContactList = new ObservableCollection<Model.Contact>();

			Backend.Network network = ((App)Application.Current).Network;

			//get list of contact with new messages
			var checkResult = network.Check(((App)Application.Current).User.Id, NewestMessageId);
			//updated contact list
			foreach (var id in checkResult) {
				var contact = ContactList.Where(_ => _.Id == id).SingleOrDefault();
				if (contact == default) {
					var name = network.GetClient(id);
					contact = new Model.Contact() { Id = id, DisplayName = name, New = true };
					ContactList.Add(contact);
				}
				else {
					contact.New = true;
					//ContactList.Remove(contact);
					//ContactList.Insert(0, contact);
				}
			}
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
