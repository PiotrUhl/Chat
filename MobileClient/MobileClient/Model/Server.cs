using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MobileClient.Model {
	public class Server : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		[Key]
		public int Id { get; set; }

		private string displayName;
		public string DisplayName {
			get => displayName;
			set {
				if (displayName != value) {
					displayName = value;
					NotifyPropertyChanged("DisplayName");
				}
			}
		}

		private string ip;
		public string Ip {
			get => ip;
			set {
				if (ip != value) {
					ip = value;
					NotifyPropertyChanged("Ip");
				}
			}
		}

		private ushort port;
		public ushort Port {
			get => port;
			set {
				if (port != value) {
					port = value;
					NotifyPropertyChanged("Port");
				}
			}
		}

		public DateTime LastConnected { get; set; }
	}
}
