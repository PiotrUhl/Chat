using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Response {
	internal class GetClient : IResponse {
		public string Name { get; set; }
		public byte[] GetBytes() {
			var raw = System.Text.Encoding.UTF8.GetBytes(Name);
			byte[] buffer = new byte[1 + raw.Length];
			buffer[0] = (byte)Common.ResponseType.GetClient;
			Array.Copy(raw, 0, buffer, 1, raw.Length);
			return buffer;
			//todo: obsługa błędów
		}
	}
}
