using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Response {
	internal class Read : IResponse {
		public byte[] GetBytes() {
			byte[] buffer = new byte[1];
			buffer[0] = (byte)Common.ResponseType.Read;
			return buffer;
			//todo: obsługa błędów
		}
	}
}