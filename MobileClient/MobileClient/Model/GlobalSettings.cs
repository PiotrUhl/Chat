using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MobileClient.Model {
	public class GlobalSettings : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		[Key]
		public int Id { get; set; }

		public Server connectedServer = null;
		public Server ConnectedServer {
			get => connectedServer;
			set {
				if (connectedServer != value) {
					connectedServer = value;
					NotifyPropertyChanged("ConnectedServer");
				}
			}
		}

		public User loggedUser = null;
		public User LoggedUser {
			get => loggedUser;
			set {
				if (loggedUser != value) {
					loggedUser = value;
					NotifyPropertyChanged("LoggedUser");
				}
			}
		}
	}
}
