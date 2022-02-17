using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Common {
	public enum ResponseType {
		Unknown = 0,
		CheckNoNew,
		CheckNew,
		New,
		Read,
		GetNew,
		GetClient,
		LogIn
	}
}
