using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

		public Command SendCommand { get; set; }

		public Conversation() {
			SendCommand = makeSendCommand();

			MessageList = new ObservableCollection<Model.Message> {
				new Model.Message {
					Id = 1,
					Text = "Treść pierwszej odebranej wiadomości",
					Recieved = true
				},
				new Model.Message {
					Id = 2,
					Text = "Treść drugiej odebranej wiadomości",
					Recieved = true
				},
				new Model.Message {
					Id = 3,
					Text = "Treść pierwszej wysłanej wiadomości",
					Recieved = false
				},
				new Model.Message {
					Id = 4,
					Text = "Treść trzeciej odebranej wiadomości",
					Recieved = true
				},
			};
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

		private void sendMessage(string message) {
			;
		}
	}
}
