using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Client.ViewModel {
	public class Login : INotifyPropertyChanged {

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string name) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
		#endregion

		public delegate void Continue(int userId);
		private Backend.Network network;
		//private Continue ccontinue;

		public string LoginText { get; set; }
		private System.Windows.Visibility errorVisibility;
		public System.Windows.Visibility ErrorVisibility {
			get => errorVisibility;
			set {
				errorVisibility = value;
				NotifyPropertyChanged("ErrorVisibility");
			}
		}

		public ICommand LogInCommand { get; set; }
		public View.Login window { get; set; }

		public int UserId { get; set; }

		public Login(Backend.Network network, View.Login window) {
			this.network = network;
			this.window = window;
			LogInCommand = new RelayCommand(__ => TryLogIn(), _ => true);
			ErrorVisibility = System.Windows.Visibility.Hidden;
		}

		private void TryLogIn() {
			byte[] passhash = null;
			using (SHA256 mySHA256 = SHA256.Create()) {
				passhash = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(window.PasswordBox.Password));
			}
			var user = network.LogIn(LoginText, passhash);
			if (user > 0) {
				UserId = user;
				window.Close();
			}
			else {
				window.PasswordBox.Password = "";
				ErrorVisibility = System.Windows.Visibility.Visible;
			}
		}
	}
}
