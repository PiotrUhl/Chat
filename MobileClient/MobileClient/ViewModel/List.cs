using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
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

		public ObservableCollection<Model.Contact> ContactList { get; set; }
		private Model.Server selectedContact = null;
		public Model.Server SelectedContact {
			get => selectedContact;
			set {
				if (selectedContact != value) {
					selectedContact = value;
					NotifyPropertyChanged("SelectedContact");
				}
			}
		}

		public Command SettingsCommand { get; private set; }
		public Command<int> ContactCommand { get; private set; }

		public List() {
			SettingsCommand = makeSettingsCommand();
			ContactCommand = makeContactCommand();

			ContactList = new ObservableCollection<Model.Contact>();
			ContactList.Add(new Model.Contact {
				Id = 1,
				Name = "Kontakt 1"
			});
			ContactList.Add(new Model.Contact {
				Id = 2,
				Name = "Kontakt 2"
			});
			ContactList.Add(new Model.Contact {
				Id = 3,
				Name = "Kontakt 3"
			});
			ContactList.Add(new Model.Contact {
				Id = 4,
				Name = "Kontakt 4"
			});
			ContactList.Add(new Model.Contact {
				Id = 5,
				Name = "Kontakt 5"
			});
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

		private Command<int> makeContactCommand() {
			return new Command<int>(
				execute: async (int id) => {
					await Application.Current.MainPage.Navigation.PushAsync(new View.ConversationPage());
				},
				canExecute: (int id) => {
					return true;
				}
			);
		}
	}
}
