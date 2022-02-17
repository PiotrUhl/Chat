using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server {
	internal class RequestProcessor {
		private TcpClient client;
		public RequestProcessor(TcpClient client) {
			this.client = client;
		}
		public void ProcessRequest() {
			var endpoint = client.Client.RemoteEndPoint;
			//log  client endpoint
			using (var stream = client.GetStream()) {
				var message = new Message(stream);
				Common.RequestType type = (Common.RequestType)message.GetByte();
				switch (type) {
					case Common.RequestType.Check:
						ProcessCheck(message);
						break;
					case Common.RequestType.GetNew:
						ProcessGetNew(message);
						break;
					case Common.RequestType.GetOld:
						ProcessGetOld(message);
						break;
					case Common.RequestType.New:
						ProcessNew(message);
						break;
					case Common.RequestType.Read:
						ProcessRead(message);
						break;
					case Common.RequestType.GetClient:
						ProcessGetClient(message);
						break;
					case Common.RequestType.LogIn:
						ProcessLogIn(message);
						break;
					default:
						;//todo: error
						break;
				}
			}
		}

		private void ProcessCheck(Message message) {
			IResponse response;
			int clientId = message.GetInt(); //caller
			long messageId = message.GetLong(); //newest message client has
			using (var context = new Database.Context()) {
				var query = context.Messages.Where(_ => _.SenderId == clientId && _.Id > messageId);
				if (query.Any() == false)
					response = new Response.CheckNoNew();
				else {
					response = new Response.CheckNew() { Clients = query.Select(_ => _.RecipentId).Distinct().ToList() };
				}
			}
			message.SendResponse(response);
			//todo: obsługa błędów
		}

		private void ProcessGetNew(Message message) {
			IResponse response;
			int clientId = message.GetInt(); //caller
			int client2Id = message.GetInt(); //partner
			long messageId = message.GetLong(); //newest message client has
			byte limit = message.GetByte(); //messages limit
			using (var context = new Database.Context()) {
				bool more = false;
				var query = context.Messages.Where(_ => ((_.SenderId == clientId && _.RecipentId == client2Id) || (_.RecipentId == clientId && _.SenderId == client2Id)) && _.Id > messageId).OrderByDescending(_ => _.Id);
				if (query.Count() > limit) {
					more = true;
					query.Take(limit);
				}
				foreach (var msg in query) {
					msg.Delivered = true;
				}
				response = new Response.Get() { Client = clientId, Messages = query.ToList(), More = more };
				message.SendResponse(response);
				context.SaveChanges();
			}
			//todo: obsługa błędów
		}

		private void ProcessGetOld (Message message) {
			IResponse response;
			int clientId = message.GetInt(); //caller
			int client2Id = message.GetInt(); //partner
			long messageNewId = message.GetLong(); //newest message border
			long messageOldId = message.GetLong(); //older message border
			byte limit = message.GetByte(); //messages limit

			using (var context = new Database.Context()) {
				bool more = false;
				var query = context.Messages.Where(_ => ((_.SenderId == clientId && _.RecipentId == client2Id) || (_.RecipentId == clientId && _.SenderId == client2Id)) && _.Id > messageOldId && _.Id < messageNewId).OrderByDescending(_ => _.Id);
				if (query.Count() > limit) {
					more = true;
					query.Take(limit);
				}
				response = new Response.Get() { Client = clientId, Messages = query.ToList(), More = more };
			}
			message.SendResponse(response);
			//todo: obsługa błędów
		}

		private void ProcessNew(Message message) {
			IResponse response;
			int clientId = message.GetInt(); //caller
			int partnerId = message.GetInt(); //partner
			string text = message.GetString();
			using (var context = new Database.Context()) {
				var newMessage = new Database.Model.Message {
					SenderId = clientId,
					RecipentId = partnerId,
					Text = text
				};
				context.Messages.Add(newMessage);
				context.SaveChanges();
				response = new Response.New() { ClientId = clientId, PartnerId = partnerId, MessageId = newMessage.Id };
			}
			message.SendResponse(response);
			//todo: obsługa błędów
		}

		private void ProcessRead(Message message) {
			IResponse response;
			long messageId = message.GetLong(); //message
			using (var context = new Database.Context()) {
				var msg = context.Messages.Where(_ => _.Id == messageId).Single();
				var query = context.Messages.Where(_ => _.Id <= messageId && ((_.SenderId == msg.SenderId && _.RecipentId == msg.RecipentId) || (_.SenderId == msg.RecipentId && _.RecipentId == msg.SenderId)) && _.Displayed == false);
				foreach (var m in query) {
					m.Displayed = true;
				}
				context.SaveChanges();
				response = new Response.Read();
			}
			message.SendResponse(response);
		}

		private void ProcessGetClient(Message message) {
			IResponse response;
			int clientId = message.GetInt();
			using (var context = new Database.Context()) {
				var client = context.Clients.Where(_ => _.Id == clientId).Single();
				response = new Response.GetClient() { Name = client.DisplayName };
			}
			message.SendResponse(response);
			//todo: obsługa błędów
		}

		private void ProcessLogIn(Message message) {
			IResponse response;
			byte[] passhash = message.GetBytes(32);
			string login = message.GetString();
			using (var context = new Database.Context()) {
				var query = context.Clients.Where(_ => _.Login == login);
				if (query.Any()) {
					var client = query.Single();
					if (client.Password.SequenceEqual(passhash))
						response = new Response.LogIn() { Id = client.Id };
					else
						response = new Response.LogIn() { Id = -1 };
				}
				else
					response = new Response.LogIn() { Id = -1 };
			}
			message.SendResponse(response);
			//todo: obsługa błędów
		}
	}
}
