using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client {
	public static class Config {
		public static readonly IPAddress ServerIp;
		public static readonly ushort ServerPort;
		static Config() {
			ServerIp = IPAddress.Parse(ConfigurationManager.AppSettings["ServerIp"]);
			ServerPort = UInt16.Parse(ConfigurationManager.AppSettings["ServerPort"]);
		}
	}
}
