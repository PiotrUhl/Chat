﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

		private string name;
		public string Name {
			get => name;
			set {
				if (name != value) {
					name = value;
					NotifyPropertyChanged("Name");
				}
			}
		}
	}
}