using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.IO;
using arvs;

namespace arvs
{

	class Server
	{
		TcpListener Listener;

		public Server (int Port)
		{
			Listener = new TcpListener (IPAddress.Any, Port); 
			Listener.Start (); 
					    
			while (true) {
				TcpClient Client = Listener.AcceptTcpClient();
				Thread Thread = new Thread(new ParameterizedThreadStart(ClientThread));
				Thread.Start(Client);
			}
								
		}

		 static void ClientThread (Object client)
		{
			processClient((TcpClient)client);
		}

		private static void processClient (TcpClient Client)
		{
			NetworkStream networkStream = Client.GetStream ();
			StringBuilder request = new StringBuilder ();
			byte[] Buffer = new byte[100];
			do {
				int count = networkStream.Read (Buffer, 0, Buffer.Length);

				request.AppendFormat ("{0}", Encoding.UTF8.GetString (Buffer, 0, count));

			} while(networkStream.DataAvailable);

			String requestString = request.ToString ();

			LogWriter.writeLog (requestString);
			Console.WriteLine (requestString);	


			byte[] AnswerBuffer = new byte[11];
			AnswerBuffer = Encoding.UTF8.GetBytes (request.Length.ToString ());
			Thread.Sleep (10000);
			networkStream.Write (AnswerBuffer, 0, AnswerBuffer.Length);
			Client.Close ();
			networkStream.Close ();

		}

		~Server ()
		{
			if (Listener != null) {
				Listener.Stop ();
			}
		}

		static void Main (string[] args)
		{


			new Server (8001);
		}
	}
}