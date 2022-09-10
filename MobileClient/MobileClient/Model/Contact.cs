using System;
using System.Collections.Generic;
using System.Text;

namespace MobileClient.Model {
	public class Contact : User {
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
