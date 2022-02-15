using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Response {
	internal class GetClient : IResponse {
		public string Name { get; set; }
		public byte[] GetBytes() {
			byte[] buffer = new byte[1 + (Name.Length + 1) * sizeof(char)];
			buffer[0] = (byte)Common.ResponseType.GetClient;
			var raw = System.Text.Encoding.UTF8.GetBytes(Name);
			Array.Copy(raw, 0, buffer, 1, raw.Length);
			return buffer;
			//todo: obsługa błędów
		}
	}
}
