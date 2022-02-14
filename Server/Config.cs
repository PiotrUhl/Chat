using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server {
	public static class Config {
		public static readonly IPAddress ServerIp;
		public static readonly ushort ServerPort;
		static Config() {
			var ip = ConfigurationManager.AppSettings["ServerIp"];
			if (ip == null)
				ServerIp = null;
			else
				ServerIp = IPAddress.Parse(ip);
			var port = ConfigurationManager.AppSettings["ServerPort"];
			ServerPort = UInt16.Parse(port);
		}
	}
}
