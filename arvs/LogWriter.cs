using System;
using System.IO;

namespace arvs
{
	public static class LogWriter
	{
	static	StreamWriter _writer;
	static	String fileName = "log.txt";
		static  LogWriter ()
		{
			_writer = new StreamWriter (fileName,true);
			_writer.AutoFlush = true;

		}

	public	static void writeLog(String text)
		{
			lock (_writer) 
				_writer.WriteLine (text);
		}
		 


	}
}

