using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Response {
	internal class LogIn : IResponse {
		public int Id { get; set; }

		public byte[] GetBytes() {
			byte[] buffer = new byte[1 + sizeof(int)];
			buffer[0] = (byte)Common.ResponseType.LogIn;
			BitConverter.TryWriteBytes(new Span<byte>(buffer, 1, sizeof(int)), Id);
			return buffer;
			//todo: obsługa błędów
		}
	}
}
