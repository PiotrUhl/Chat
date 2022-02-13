using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Response {
	internal class CheckNew : IResponse {
		public List<int> Clients { get; set; }
		public byte[] GetBytes() {
			byte[] buffer = new byte[1 + sizeof(int) * (Clients.Count + 1)];
			buffer[0] = (byte)Common.ResponseType.CheckNew;
			for (int i = 0; i < Clients.Count; i++) {
				BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int) * i, sizeof(int)), Clients[i]);
			}
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int) * Clients.Count, sizeof(int)), (int)0);
			return buffer;
			//todo: obsługa błędów
		}
	}
}
