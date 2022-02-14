using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace Chat.Service {
	internal class Service : ServiceControl {

		private Server.Server server = null;
		private Thread thread = null;

		public Service() {
			server = new();
		}

		public bool Start(HostControl hostControl) {
			//todo: log
			try {
				server = new();
				thread = new(() => server.Run());
				thread.Start();
				//todo: log
				return true;
			}
			catch (Exception e) {
				//todo: log
				return false;
			}
		}
		public bool Stop(HostControl hostControl) {
			//todo: log
			server?.Stop();
			//todo: check if stopped, if not kill thread
			return true;
		}
	}
}
