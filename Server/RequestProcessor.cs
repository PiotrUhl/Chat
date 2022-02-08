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
				var buffer = new byte[256];
				int read = 0;
				read = stream.Read(buffer, 0, 1);
				if (read < 1)
					; //todo: error
				MessageType type = (MessageType)buffer[0];
				switch (type) {
					case MessageType.Check:
						ProcessCheck();
						break;
					case MessageType.Get:
						ProcessGet();
						break;
					case MessageType.New:
						ProcessNew();
						break;
					case MessageType.Read:
						ProcessRead();
						break;
					default:
						;//todo: error
						break;
				}
			}
		}
		private void ProcessCheck() {

		}
		private void ProcessGet() {

		}
		private void ProcessNew() {

		}
		private void ProcessRead() {

		}
	}
}
