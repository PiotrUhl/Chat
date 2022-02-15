using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Server {
	internal class Message {

		private NetworkStream stream;
		private byte[] buffer;

		public Message(NetworkStream stream) {
			this.stream = stream;
			this.buffer = new byte[256];
		}

		public byte GetByte() {
			if (stream.Read(buffer, 0, 1) < 1)
				; //todo: error
			return buffer[0];
		}

		public int GetInt() {
			if (stream.Read(buffer, 0, 4) < 4)
				; //todo: error
			return BitConverter.ToInt32(buffer, 0);
			//todo: conversion error
		}

		public long GetLong() {
			if (stream.Read(buffer, 0, 8) < 8)
				; //todo: error
			return BitConverter.ToInt64(buffer, 0);
			//todo: conversion error
		}

		public string GetString() {
			var builder = new StringBuilder();
			int read;
			while (stream.DataAvailable) {
				read = stream.Read(buffer, 0, 256);
				builder.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, read));
			}
			return builder.ToString();
		}

		public void SendResponse(IResponse response) {
			stream.Write(response.GetBytes());
		}
	}
}
