using System;
using System.Collections.Generic;
using System.Text;

namespace MobileClient.Model {
	public class Contact : User {
		public string DisplayName { get; set; }
		public bool New { get; set; }
	}
}
