using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Chat.Server {
	public class Server {

		private Config config;

		private TcpListener listener = null;

		public Server() {
			//read config
		}

		public void Run() {
			try {
				listener = new(config.Ip ?? IPAddress.Any, config.Port);
				listener.Start();
				while (true) {
					var client = listener.AcceptTcpClient();
					var processor = new RequestProcessor(client);
					Task.Run(() => processor.ProcessRequest());
				}
			}
			catch (Exception e) {
				//todo: obsługa błędów
			}
		}

		public void Stop() {
			listener?.Stop();
		}
	}
}
