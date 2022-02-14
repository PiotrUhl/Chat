using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public int LoggedUserId { get; set; }
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
		public ObservableCollection<Model.Message> ActiveMessageBox {
			get => activeMessageBox;
			set {
				activeMessageBox = value;
				SelectedMessage = null;
			}
		}
		private ObservableCollection<Model.Message> activeMessageBox;
		public Model.Message SelectedMessage { get; set; }

		//Constructor
		public Main() {
			SelectedContact = null;
			SelectedMessage = null;
			using (var db = new Model.Context()) {
				Contacts = new(db.Contacts.ToList());
			}
		}

		//Refill MessageBox
		private void SelectedContactChanged() {
			if (SelectedContact == null) {
				ActiveMessageBox = null;
			}
			else {
				using (var db = new Model.Context()) {
					ActiveMessageBox = new(db.Messages.Where(_ => _.Contact == SelectedContact).ToList());
				}
			}
			NotifyPropertyChanged("ActiveMessageBox");
		}
	}
}
