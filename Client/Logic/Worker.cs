using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Client.Logic {
	public class Worker {
		private ViewModel.Main viewmodel { get; set; }

		private Network network;

		public Worker(ViewModel.Main viewmodel) {
			this.viewmodel = viewmodel;
			this.network = new();
		}

		public void RunChecking() {
			var checkResult = network.Check(viewmodel.LoggedUserId, viewmodel.NewestMessageId);
			foreach (var id in checkResult) {
				var contact = viewmodel.Contacts.Where(_ => _.Id == id).SingleOrDefault();
				if (contact == default) {
					var name = network.GetClient(id);
					var newContact = new Model.Contact() { Id = id, DisplayName = name, New = true };
					using (var context = new Model.Context()) {
						context.Add(newContact);
						context.SaveChanges();
					}
					Application.Current.Dispatcher.Invoke(() =>
					{
						viewmodel.Contacts.Add(newContact);
					});
				}
				else {
					contact.New = true;
					viewmodel.Contacts.Remove(contact);
					viewmodel.Contacts.Insert(0, contact);
				}
			}
		}
	}
}
