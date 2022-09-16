using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MobileClient.Model {
	public class GlobalSettings {

		[Key]
		public int Id { get; set; }

		//[ForeignKey("ConnectedServer")]
		public int ConnectedServerId { get; set; }
		//public Server ConnectedServer { get; set; }

		public int LoggedUserId { get;set; }
	}
}
