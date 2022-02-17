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
		public int LoggedUserId { get; set; }
		public long NewestMessageId { get; set; }

		//ContactList
		public ObservableCollection<Model.Contact> Contacts { get; set; }
		public Model.Contact SelectedContact {
			get => selectedContact;
			set {
				selectedContact = value;
				RefreshMessageBox();
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
			SendCommand = new RelayCommand(__ => SendMessage(), _ => MessageInput?.Length > 0);
		}
		public void Init() {
			using (var db = new Model.Context()) {
				Contacts = new(db.Contacts.ToList());
				NewestMessageId = db.Messages.OrderBy(_ => _.Id).LastOrDefault()?.Id ?? 0;
				NotifyPropertyChanged("Contacts");
			}
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
			if (NewestMessageId < message.Id)
				NewestMessageId = message.Id;
			MessageInput = "";
			RefreshMessageBox();
		}

		//Refill MessageBox
		public void RefreshMessageBox() {
			if (SelectedContact == null) {
				ActiveMessageBox = null;
			}
			else {
				SelectedContact.New = false;
				using (var db = new Model.Context()) {
					db.Contacts.Where(_ => _.Id == SelectedContact.Id).Single().New = false;
					ActiveMessageBox = new(db.Messages.Where(_ => _.Contact == SelectedContact).ToList());
					NotifyMessagesRead(ActiveMessageBox.Last().Id);
				}
			}
			NotifyPropertyChanged("ActiveMessageBox");
		}

		//Notify messages read
		private void NotifyMessagesRead(long message) {
			network.Read(message);
		}
	}
}
