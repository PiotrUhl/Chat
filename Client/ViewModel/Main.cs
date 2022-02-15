using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModel {
	public class Main : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		//Global
		private Backend.Network network;
		public int LoggedUserId { get; set; } = 1;
		public long NewestMessageId { get; set; }

		//ContactList
		public ObservableCollection<Model.Contact> Contacts { get; set; }
		public Model.Contact SelectedContact {
			get => selectedContact;
			set {
				selectedContact = value;
				SelectedContactChanged();
			}
		}
		private Model.Contact selectedContact;

		//MessageBox
		private ObservableCollection<Model.Message> activeMessageBox;
		public ObservableCollection<Model.Message> ActiveMessageBox {
			get => activeMessageBox;
			set {
				activeMessageBox = value;
				SelectedMessage = null;
			}
		}
		public Model.Message SelectedMessage { get; set; }
		private string messageInput;
		public string MessageInput {
			get => messageInput;
			set {
				messageInput = value;
				NotifyPropertyChanged("MessageInput");
			}
		}
		public ICommand SendCommand { get; set; }

		//Constructor
		public Main(Backend.Network network) {
			this.network = network;
			SelectedContact = null;
			SelectedMessage = null;
			using (var db = new Model.Context()) {
				Contacts = new(db.Contacts.ToList());
				NewestMessageId = db.Messages.FirstOrDefault()?.Id ?? 0;
			}
			SendCommand = new RelayCommand(__ => SendMessage(), _ => MessageInput?.Length > 0);
		}

		//Send message
		private void SendMessage() {
			var message = new Model.Message() {
				Text = MessageInput,
				Recieved = false,
				ContactId = SelectedContact.Id
			};
			message.Id = network.New(LoggedUserId, SelectedContact.Id, MessageInput);
			if (message.Id > 0) {
				using (var db = new Model.Context()) {
					db.Messages.Add(message);
					db.SaveChanges();
				}
			}
			MessageInput = "";
			SelectedContactChanged();
		}

		//Refill MessageBox
		private void SelectedContactChanged() {
			if (SelectedContact == null) {
				ActiveMessageBox = null;
			}
			else {
				(List<Model.Message>, bool) newMessages = new();
				if (SelectedContact.New == true) {
					newMessages = network.GetNew(LoggedUserId, SelectedContact.Id, NewestMessageId);
				}
				using (var db = new Model.Context()) {
					if (newMessages.Item1 != null) {
						SelectedContact.New = false;
						db.Contacts.Where(_ => _.Id == SelectedContact.Id).Single().New = false;
						db.Messages.AddRange(newMessages.Item1);
						db.SaveChanges();
					}
					ActiveMessageBox = new(db.Messages.Where(_ => _.Contact == SelectedContact).ToList());
				}
			}
			NotifyPropertyChanged("ActiveMessageBox");
		}
	}
}
