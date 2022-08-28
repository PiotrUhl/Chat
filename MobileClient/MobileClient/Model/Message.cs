using System;
using System.Collections.Generic;
using System.Text;

namespace MobileClient.Model {
	public class Message {
		public long Id { get; set; }
		public string Text { get; set; }
		public bool Recieved { get; set; }
	}
}
