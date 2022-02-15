using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server.Response {
	internal class Get : IResponse {
		public int Client { get; set; }
		public List<Database.Model.Message> Messages { get; set; }
		public bool More { get; set; } = false;

		public byte[] GetBytes() {
			byte[] buffer = new byte[1 + CalculateSize() + 1];
			buffer[0] = (byte)Common.ResponseType.GetNew;
			int offset = 1;
			for (int i = 0; i < Messages.Count; i++) {
				//control byte (1 - sent, 3 - recieved)
				buffer[offset++] = (byte)(Messages[i].SenderId == Client ? 1 : 3);
				//write id
				BitConverter.TryWriteBytes(new Span<byte>(buffer, offset, sizeof(long)), Messages[i].Id);
				offset += sizeof(long);
				//write text
				var raw = System.Text.Encoding.UTF8.GetBytes(Messages[i].Text);
				Array.Copy(raw, 0, buffer, offset, raw.Length);
				offset += raw.Length;
				//string termination byte
				buffer[offset++] = (byte)0;
			}
			//control byte (0 - end, 4 - more pending)
			buffer[offset++] = (byte)0;
			return buffer;
			//todo: obsługa błędów
		}

		private uint CalculateSize() {
			uint size = 0;
			foreach (var message in Messages) {
				size += sizeof(long);
				size += 1;
				size += (uint)(message.Text.Length + 1) * sizeof(char);
			}
			return size;
		}
	}
}
