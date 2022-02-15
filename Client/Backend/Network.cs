using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Chat.Common;

namespace Client.Backend {
	public class Network {
		public List<int> Check(int senderId, long lastMessageId) {
			byte[] buffer = new byte[1 + sizeof(int) + sizeof(long)];
			buffer[0] = (byte)Chat.Common.RequestType.Check;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), senderId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int), sizeof(long)), lastMessageId);
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Config.ServerIp, Config.ServerPort));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.CheckNoNew) {
							return new();
						}
						else if (type == ResponseType.CheckNew) {
							var list = new List<int>();
							while (stream.DataAvailable) {
								list.Add(response.GetInt());
							}
							return list;
						}
						else {
							return null;
						}
					}
				}
			}
			catch (Exception e) {
				//todo: obsługa błędów
				return null;
			}
		}

		public string GetClient(int clientId) {
			byte[] buffer = new byte[1 + sizeof(int)];
			buffer[0] = (byte)Chat.Common.RequestType.GetClient;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), clientId);
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Config.ServerIp, Config.ServerPort));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.GetClient) {
							return response.GetString();
						}
						else {
							return null;
						}
					}
				}
			}
			catch (Exception e) {
				//todo: obsługa błędów
				return null;
			}
		}

		public (List<Model.Message>, bool) GetNew(int callerId, int partnerId, long lastMessageId) {
			byte[] buffer = new byte[2 + 2*sizeof(int) + sizeof(long)];
			buffer[0] = (byte)Chat.Common.RequestType.GetNew;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), callerId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int), sizeof(int)), partnerId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + 2*sizeof(int), sizeof(long)), lastMessageId);
			buffer[1 + 2 * sizeof(int) + sizeof(long)] = 10;
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Config.ServerIp, Config.ServerPort));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.GetNew) {
							var messages = new List<Model.Message>();
							while (true) {
								Model.Message message = new() { ContactId = partnerId, Recieved = false };
								byte control = response.GetByte();
								switch(control) {
									case 3:
										message.Recieved = true;
										goto case 1;
									case 1:
										message.Id = response.GetLong();
										message.Text = response.GetNullTerminatedString();
										messages.Add(message);
										break;
									case 0:
										return (messages, false);
									case 4:
										return (messages, true);
									default:
										return (null, false);
								}
							}
						}
						else {
							return (null, false);
						}
					}
				}
			}
			catch (Exception e) {
				//todo: obsługa błędów
				return (null, false);
			}
		}
	}
}
