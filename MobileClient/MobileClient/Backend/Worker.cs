using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Xamarin.Forms;

namespace MobileClient.Backend {
	public class Worker {

		INotificationManager notificationManager;
		private App app;


		public Worker(App app) {
			this.app = app;

			notificationManager = DependencyService.Get<INotificationManager>();
		}

		public void Run() {
			while (true) {
				try {
					if (app.Network == null || app.Server == null || app.User.Id == 0) {
						return;
					}
					//get list of contact with new messages
					var checkResult = app.Network.Check(app.User.Id, app.User.LastMessageId);
					//update contact list
					if (checkResult != null) {
						using (var context = new Model.Context()) {
							foreach (var id in checkResult) {
								var contact = context.Contacts.Where(_ => _.Id == id).SingleOrDefault();
								if (contact == default) {
									var name = app.Network.GetClient(id);
									contact = new Model.Contact() { Id = id, DisplayName = name, New = true };
									context.Contacts.Add(contact);
									//update contact list
									try {
										((ViewModel.List)App.Current.MainPage.Navigation.NavigationStack.First().BindingContext).ReflilContactList();
									}
									catch (Exception e) {; }
									//send notification
									if (((App)Application.Current).NotificationsEnabled == true)
										notificationManager.SendNotification(contact.DisplayName, $"Nowa wiadomość od kontaktu {contact.DisplayName}", DateTime.Now);
								}
								else {
									contact.New = true;
									context.SaveChanges();
									try {
										((ViewModel.List)App.Current.MainPage.Navigation.NavigationStack.First().BindingContext).SetContactNew(contact.Id);
									} catch (Exception e) {; }
									//send notification
									if (((App)Application.Current).NotificationsEnabled == true)
										notificationManager.SendNotification(contact.DisplayName, $"Nowa wiadomość od kontaktu {contact.DisplayName}", DateTime.Now);
								}
								
								GetMessages(id);
							}
							//context.SaveChanges();
						}
					}
					Thread.Sleep(1000);
				}
				catch (Exception e) {
					;//todo: obsługa błędów
				}
			}
		}
		private void GetMessages(int clientId) {
			long lastMsgId = 0;
			using (var context = new Model.Context()) {
				var msgs = context.Messages.Where(_ => _.UserId == app.User.Id &&_.ContactId == clientId).OrderBy(_ => _.Id);
				if (msgs.Any()) {
					lastMsgId = msgs.Last().Id;
				}
				var messages = app.Network.GetNew(app.User.Id, clientId, lastMsgId);
				foreach (var message in messages.Item1)
					message.UserId = app.User.Id;
				context.Messages.AddRange(messages.Item1);
				context.SaveChanges(); //todo: try remove
				lastMsgId = messages.Item1.First().Id;
				if (app.User.LastMessageId < lastMsgId) {
					context.Users.Where(u => u.Id == app.User.Id).Single().LastMessageId = lastMsgId;
					app.User.LastMessageId = lastMsgId;
				}
				try {
					((ViewModel.Conversation)App.Current.MainPage.Navigation.NavigationStack.Last().BindingContext).InsertMessages(messages.Item1);
					context.Contacts.Single(c => c.Id == clientId).New = false;
				}
				catch (Exception e) {; }
				context.SaveChanges();
			}
		}
	}
}
