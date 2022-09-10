using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MobileClient.Model {
	public class Contact : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		[Key]
		private int id;
		public int Id {
			get => id;
			set {
				if (id != value) {
					id = value;
					NotifyPropertyChanged("Id");
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

		private bool neww;
		public bool New {
			get => neww;
			set {
				if (neww != value) {
					neww = value;
					NotifyPropertyChanged("New");
				}
			}
		}
	}
}
