using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.RPC;
using MySql.Data.MySqlClient;
using System.Xml;

namespace NodeConnector
{
	class Program
	{
		static int height;

		static void Main(string[] args)
		{
			#region ConnectionCode
			XmlDocument PassXML = new XmlDocument();
			PassXML.Load(@"C:\Users\matth\onedrive\documents\visual studio 2017\Projects\BlockExplorer\BlockExplorer\PassStore.xml");

			// Object for connecting to SQL database
			SqlConnector SQC = new SqlConnector("192.168.1.146", "blockchain", PassXML["credentials"]["SQLCreds"]["username"].InnerText, PassXML["credentials"]["SQLCreds"]["password"].InnerText);

			// Object for connecting to Bitcoin node
			RPCClient RPCC = new RPCClient(
				PassXML["credentials"]["RPCCreds"]["username"].InnerText + ":" + PassXML["credentials"]["RPCCreds"]["password"].InnerText
				, "192.168.1.146", Network.Main);
			#endregion

			height = RPCC.GetBlockCount();

			int loc = getCommandLocation(args, "startpos");
			int startPosition = 0; //by default, starts at 0
			if (loc != 999)
			{
				startPosition = Convert.ToInt32(args[loc + 1]);
			}

			List<DateTime> blockTimes = new List<DateTime>();

			// Iterate through every block so far since the start location
			for(int currentBlockHeight = startPosition; currentBlockHeight <= height; currentBlockHeight++)
			{
				blockTimes.Insert(0, DateTime.Now);
				BlockHolder b = new BlockHolder(currentBlockHeight, SQC, RPCC);
				if (blockTimes.Count >= 10000)
					blockTimes.RemoveAt(blockTimes.Count - 1);

				if(b.height % 100 == 0)
					displayStats(blockTimes, b, height);
			}

			Console.ReadLine();
		}

		static void displayStats(List<DateTime> blockTimes, BlockHolder lastBlock, int height)
		{
			TimeSpan timeSince = blockTimes[0].Subtract(blockTimes[blockTimes.Count - 1]);
			//int secondsPerBlock = Convert.ToInt32(Math.Round(timeSince.TotalSeconds / blockTimes.Count));
			double blocksPerSecond = blockTimes.Count / timeSince.TotalSeconds;

			Console.Clear();

			Console.WriteLine("Current Block: " + lastBlock.height);

			if (lastBlock.wasOnSQL)
				Console.WriteLine("Was on SQL!");
			else
				Console.WriteLine("Wasn't on SQL!");

			Console.WriteLine("Current Block hash: " + lastBlock.hash.ToString());
			
			Console.WriteLine("Currently doing " + blocksPerSecond + " blocks / second");

			TimeSpan timeLeft = new TimeSpan(0, 0, Convert.ToInt32((height-lastBlock.height)/blocksPerSecond));
			Console.WriteLine("Time until done: " + timeLeft.ToString(@"dd\.hh\:mm\:ss"));
		}

		static int getCommandLocation(string[] args, string command)
		{
			int location = 999; //reader understands this as a command not existing in the args.

			command = "-" + command;

			for (int i = 0; i < args.Length; i++)
			{
				if(args[i] == command | "-" + args[i] == command)
				{
					location = i;
					break;
				}
			}

			return location;
		}
	}
}
