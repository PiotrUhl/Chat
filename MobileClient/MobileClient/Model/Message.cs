using System;
using System.Collections.Generic;
using System.Text;

namespace MobileClient.Model {
	public class Message {
		public long Id { get; set; }
		public string Text { get; set; }
		public bool Recieved { get; set; }

		public int ContactId { get; set; }
		public virtual User Contact { get; set; }
	}
}
