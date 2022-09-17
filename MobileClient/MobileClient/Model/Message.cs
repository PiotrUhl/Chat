using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileClient.Model {
	public class Message {
		public long Id { get; set; }
		public string Text { get; set; }
		public bool Recieved { get; set; }

		public int UserId { get; set; }

		public int ContactId { get; set; }
		public virtual Contact Contact { get; set; }
	}
}
