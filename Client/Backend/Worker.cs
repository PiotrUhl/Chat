using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Client.Backend {
	public class Worker {
		private ViewModel.Main viewmodel { get; set; }

		private Network network;

		public Worker(ViewModel.Main viewmodel, Network network) {
			this.viewmodel = viewmodel;
			this.network = network;
		}

		public void Run() {
			while (true) {
				try {
					//get list of contact with new messages
					var checkResult = network.Check(viewmodel.LoggedUserId, viewmodel.NewestMessageId);
					//updated contact list
					foreach (var id in checkResult) {
						var contact = viewmodel.Contacts.Where(_ => _.Id == id).SingleOrDefault();
						if (contact == default) {
							var name = network.GetClient(id);
							contact = new Model.Contact() { Id = id, DisplayName = name, New = true };
							using (var context = new Model.Context(viewmodel.LoggedUserId)) {
								context.Add(contact);
								context.SaveChanges();
							}
							Application.Current.Dispatcher.Invoke(() => {
								viewmodel.Contacts.Add(contact);
							});
						}
						else {
							contact.New = true;
							//viewmodel.Contacts.Remove(contact);
							//viewmodel.Contacts.Insert(0, contact);
						}
						GetMessages(contact.Id);
					}
					Thread.Sleep(5000);
				}
				catch (Exception e) {
					;//todo: obsługa błędów
				}
			}
		}

		private void GetMessages(int clientId) {
			long lastMsgId = 0;
			using (var db = new Model.Context(viewmodel.LoggedUserId)) {
				var msgs = db.Messages.Where(_ => _.ContactId == clientId).OrderBy(_ => _.Id);
				if (msgs.Any()) {
					lastMsgId = msgs.Last().Id;
				}
				var messages = network.GetNew(viewmodel.LoggedUserId, clientId, lastMsgId);
				db.Contacts.Where(_ => _.Id == clientId).Single().New = false;
				db.Messages.AddRange(messages.Item1);
				db.SaveChanges();
				lastMsgId = messages.Item1.First().Id;
			}
			if (viewmodel.NewestMessageId < lastMsgId)
				viewmodel.NewestMessageId = lastMsgId;
			if (viewmodel.SelectedContact != null && viewmodel.SelectedContact.Id == clientId)
				viewmodel.RefreshMessageBox();
		}
	}
}
