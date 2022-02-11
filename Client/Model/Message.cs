using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model {
	public class Message {
		[Key]
		public long Id { get; set; }
		public string Text { get; set; }

		public int ContactId { get; set; }
		public virtual Contact Contact { get; set; }
	}
}
