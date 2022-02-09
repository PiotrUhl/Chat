using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server {
	internal interface IResponse {
		public byte[] GetBytes();
	}
}
