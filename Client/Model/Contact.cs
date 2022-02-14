using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Model {
	public class Contact {
		[Key]
		public int Id { get; set; }
		public string DisplayName { get; set; }
		public bool New { get; set; } = false;

		public virtual List<Message> Messages { get; set; }
	}
}
