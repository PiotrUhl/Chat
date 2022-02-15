using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Chat.Common;

namespace Client.Logic {
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
						ResponseType type = (ResponseType)stream.ReadByte();
						if (type == ResponseType.CheckNoNew) {
							return new();
						}
						else if (type == ResponseType.CheckNew) {
							var readBuffer = new byte[sizeof(int)];
							var list = new List<int>();
							while (stream.DataAvailable) {
								stream.Read(readBuffer);
								list.Add(BitConverter.ToInt32(readBuffer, 0));
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
						ResponseType type = (ResponseType)stream.ReadByte();
						if (type == ResponseType.GetClient) {
							byte[] readBuffer = new byte[256];
							var builder = new StringBuilder();
							int read;
							while ((read = stream.Read(readBuffer, 0, 256)) > 0) {
								builder.Append(System.Text.Encoding.UTF8.GetString(readBuffer, 0, read));
							}
							return builder.ToString();
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
	}
}
