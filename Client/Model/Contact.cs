using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model {
	public class Contact : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
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
				displayName = value;
				NotifyPropertyChanged("DisplayName");
			}
		}
		private bool neww;
		public bool New {
			get => neww;
			set {
				neww = value;
				NotifyPropertyChanged("New");
			}
		}

		public virtual List<Message> Messages { get; set; }
	}
}
