using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileClient.Model {
	public class User : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		public int ServerId { get; set; }
		public int Id { get; set; }

		private string login;
		public string Login {
			get => login;
			set {
				if (login != value) {
					login = value;
					NotifyPropertyChanged("Login");
				}
			}
		}

		private string displayName;
		public string DisplayName {
			get => displayName;
			set {
				if (displayName != value) {
					displayName = value;
					NotifyPropertyChanged("Name");
				}
			}
		}

		public virtual Server Server { get; set; }
	}
}
