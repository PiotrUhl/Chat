using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server {
	internal enum MessageType : byte {
		Unknown = 0,
		Check,
		Get,
		New,
		Read
	}
}
