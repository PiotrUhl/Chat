using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Response {
	internal class New : IResponse { 
		public int ClientId { get; set; }
		public int PartnerId { get; set; }
		public long MessageId { get; set; }

		public byte[] GetBytes() {
			byte[] buffer = new byte[1 + 2 * sizeof(int) + sizeof(long)];
			buffer[0] = (byte)Common.ResponseType.New;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), ClientId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + sizeof(int), sizeof(int)), PartnerId);
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1 + 2 * sizeof(int), sizeof(long)), MessageId);
			return buffer;
			//todo: obsługa błędów
		}
	}
}
