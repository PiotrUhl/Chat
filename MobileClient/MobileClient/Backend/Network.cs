﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chat.Common;

namespace MobileClient.Backend {
	public class Network {

		public IPAddress Ip { get; set; }
		public ushort Port { get; set; }

		public Network(string ip, ushort port) {
			Ip = IPAddress.Parse(ip);
			Port = port;
		}

		public async Task<bool> TryConnectAsync() {
			try {
				using TcpClient client = new TcpClient();
				var ca = client.ConnectAsync(Ip, Port);
				await Task.WhenAny(ca, Task.Delay(10000));
				client.Close();
				if (ca.IsFaulted || !ca.IsCompleted)
					return false;
				else
					return true;
			}
			catch (Exception e) {
				return false;
			}
		}

		public int LogIn(string login, byte[] passhash) {
			var raw = System.Text.Encoding.UTF8.GetBytes(login);
			byte[] buffer = new byte[1 + raw.Length + passhash.Length];
			buffer[0] = (byte)Chat.Common.RequestType.LogIn;
			Array.Copy(passhash, 0, buffer, 1, passhash.Length);
			Array.Copy(raw, 0, buffer, 1 + passhash.Length, raw.Length);
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Ip, Port));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.LogIn) {
							int userId = response.GetInt();
							return userId;
						}
						else {
							return -1;
						}
					}
				}
			}
			catch (Exception e) {
				//todo: obsługa błędów
				return -1;
			}
		}

		public List<int> Check(int senderId, long lastMessageId) {
			byte[] buffer = new byte[1 + sizeof(int) + sizeof(long)];
			buffer[0] = (byte)Chat.Common.RequestType.Check;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), senderId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int), sizeof(long)), lastMessageId);
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Ip, Port));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.CheckNoNew) {
							return new List<int>();
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
					tcpClient.Connect(new IPEndPoint(Ip, Port));
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
			byte[] buffer = new byte[2 + 2 * sizeof(int) + sizeof(long)];
			buffer[0] = (byte)Chat.Common.RequestType.GetNew;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), callerId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int), sizeof(int)), partnerId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + 2 * sizeof(int), sizeof(long)), lastMessageId);
			buffer[1 + 2 * sizeof(int) + sizeof(long)] = 10;
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Ip, Port));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.GetNew) {
							var messages = new List<Model.Message>();
							while (true) {
								Model.Message message = new Model.Message() { ContactId = partnerId, Recieved = false };
								byte control = response.GetByte();
								switch (control) {
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

		public long New(int senderId, int recipentId, string message) {
			var raw = System.Text.Encoding.UTF8.GetBytes(message);
			byte[] buffer = new byte[1 + 2 * sizeof(int) + raw.Length];
			buffer[0] = (byte)Chat.Common.RequestType.New;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), senderId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int), sizeof(int)), recipentId);
			Array.Copy(raw, 0, buffer, 1 + 2 * sizeof(int), raw.Length);
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Ip, Port));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.New) {
							int responseSenderId = response.GetInt();
							int responseRecipentId = response.GetInt();
							long messageId = response.GetLong();
							if (responseSenderId == senderId && responseRecipentId == recipentId)
								return messageId;
							else
								return -1;
						}
						else {
							return -1;
						}
					}
				}
			}
			catch (Exception e) {
				//todo: obsługa błędów
				return -1;
			}
		}

		public bool Read(long messageId) {
			byte[] buffer = new byte[1 + sizeof(long)];
			buffer[0] = (byte)Chat.Common.RequestType.Read;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(long)), messageId);
			try {
				using (var tcpClient = new TcpClient()) {
					tcpClient.Connect(new IPEndPoint(Ip, Port));
					using (var stream = tcpClient.GetStream()) {
						stream.Write(buffer);
						var response = new Response(stream);
						ResponseType type = (ResponseType)response.GetByte();
						if (type == ResponseType.Read) {
							return true;
						}
						else {
							return false;
						}
					}
				}
			}
			catch (Exception e) {
				//todo: obsługa błędów
				return false;
			}
		}
	}
}
