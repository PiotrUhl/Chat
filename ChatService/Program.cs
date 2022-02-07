using System;
using Topshelf;

namespace Chat.Service {
	class Program {
		static void Main(string[] args) {
            HostFactory.Run(_ => {
                _.Service<Service>();
                _.SetServiceName("ChatServerService");
                _.SetDescription("Service of server for chat application.");
                _.SetDisplayName("Chat Server service");
                _.StartAutomaticallyDelayed();
                _.EnableServiceRecovery(r => r.RestartService(TimeSpan.FromSeconds(10)));
            });
        }
	}
}