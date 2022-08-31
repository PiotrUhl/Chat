using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileClient.ViewModel {
	public class ContactSettings : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		public Model.User Contact { get; set; }

		#region Properties
		private string notificationSelected = "Enabled";
		public string NotificationSelected {
			get => notificationSelected;
			set {
				if (notificationSelected != value) {
					notificationSelected = value;
					NotifyPropertyChanged("NotificationSelected");
					if (value == "TempDisabled") {
						TemporaryDisableTime = DateTime.Now.TimeOfDay;
						TimePickerVisibility = 1;
					}
					else
						TimePickerVisibility = 0;
				}
			}
		}
		private TimeSpan temporaryDisableTime = DateTime.Now.TimeOfDay;
		public TimeSpan TemporaryDisableTime {
			get => temporaryDisableTime;
			set {
				if (temporaryDisableTime != value) {
					temporaryDisableTime = value;
					NotifyPropertyChanged("TemporaryDisableTime");
				}
			}
		}
		private byte timePickerVisibility = 0;
		public byte TimePickerVisibility {
			get => timePickerVisibility;
			set {
				if (timePickerVisibility != value) {
					timePickerVisibility = value;
					NotifyPropertyChanged("TimePickerVisibility");
				}
			}
		}
		#endregion
	}
}
