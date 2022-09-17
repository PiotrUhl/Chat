using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class Conversation : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		public Model.Contact contact;
		public Model.Contact Contact {
			get => contact;
			set {
				if (contact != value) {
					contact = value;
					NotifyPropertyChanged("Contact");
					loadMessages();
				}
			}
		}

		public ObservableCollection<Model.Message> MessageList { get; set; }

		private string messageText;
		public string MessageText {
			get => messageText;
			set {
				if (messageText != value) {
					messageText = value;
					NotifyPropertyChanged("MessageText");
					SendCommand.ChangeCanExecute();
				}
			}
		}

		public Command SettingsCommand { get; set; }
		public Command SendCommand { get; set; }

		public Conversation() {
			SettingsCommand = makeSettingsCommand();
			SendCommand = makeSendCommand();

			MessageList = new ObservableCollection<Model.Message>();
		}

		private Command makeSettingsCommand() {
			return new Command(
				execute: async () => {
					var page = new View.ContactSettingsPage();
					((ViewModel.ContactSettings)page.BindingContext).Contact = Contact;
					await Application.Current.MainPage.Navigation.PushAsync(page);
				},
				canExecute: () => {
					return true;
				}
			);
		}

		private Command makeSendCommand() {
			return new Command(
				execute: () => {
					sendMessage(MessageText);
				},
				canExecute: () => {
					return MessageText != null && MessageText != "";
				}
			);
		}

		private void sendMessage(string text) {
			Backend.Network network = ((App)Application.Current).Network;

			var message = new Model.Message() {
				Text = text,
				Recieved = false,
				ContactId = Contact.Id
			};
			message.Id = network.New(((App)Application.Current).User.Id, Contact.Id, text);
			if (message.Id > 0) {
				MessageList.Add(message);
			}
			MessageText = "";
			//todo: manage send message database
		}

		private void loadMessages() {
			Backend.Network network = ((App)Application.Current).Network;

			long lastMsgId = 0;

			if (MessageList.Any()) {
				lastMsgId = MessageList.Last().Id;
			}
			var messages = network.GetNew(((App)Application.Current).User.Id, contact.Id, lastMsgId);
			foreach (var message in messages.Item1) {
				MessageList.Insert(0, message);
			}
			network.Read(messages.Item1.Last().Id);
		}
	}
}
