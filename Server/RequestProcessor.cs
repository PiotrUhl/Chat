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
				MessageType type = (MessageType)message.GetByte();
				switch (type) {
					case MessageType.Check:
						ProcessCheck(message);
						break;
					case MessageType.GetNew:
						ProcessGetNew(message);
						break;
					case MessageType.GetOld:
						ProcessGetOld(message);
						break;
					case MessageType.New:
						ProcessNew(message);
						break;
					case MessageType.Read:
						ProcessRead(message);
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
					response = new Response.CheckNew() { Clients = query.Select(_ => _.RecipentId).ToList() };
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
				response = new Response.Get() { Client = clientId, Messages = query.ToList(), More = more };
			}
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
				msg.Displayed = true;
				context.SaveChanges();
				response = new Response.Read();
			}
			message.SendResponse(response);
		}
	}
}
