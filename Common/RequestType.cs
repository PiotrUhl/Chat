using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Common {
	public enum RequestType : byte {
		Unknown = 0,
		Check,
		GetNew,
		GetOld,
		New,
		Read,
		GetClient
	}
}
