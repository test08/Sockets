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
	class Client
	{
				           				    
		public Client (TcpClient Client)
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
	}

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

				//ThreadPool.QueueUserWorkItem (new WaitCallback (ClientThread), 
				  //		                        Listener.AcceptTcpClient ());


			}
								
		}

		 static void ClientThread (Object client)
		{
			new Client ((TcpClient)client);
		}

		~Server ()
		{
			if (Listener != null) {
				Listener.Stop ();
			}
		}

		static void Main (string[] args)
		{

			//ThreadPool.SetMaxThreads (10, 10);
			//ThreadPool.SetMinThreads (4, 4);
			new Server (8001);
		}
	}
}