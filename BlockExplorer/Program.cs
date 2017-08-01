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
			XmlDocument PassXML = new XmlDocument();
			PassXML.Load(@"C:\Users\matth\onedrive\documents\visual studio 2017\Projects\BlockExplorer\BlockExplorer\PassStore.xml");

			// Object for connecting to SQL database
			SqlConnector SQC = new SqlConnector("192.168.1.146", "blockchain", PassXML["credentials"]["SQLCreds"]["username"].InnerText, PassXML["credentials"]["SQLCreds"]["password"].InnerText);

			// Object for connecting to Bitcoin node
			RPCClient RPCC = new RPCClient(
				PassXML["credentials"]["RPCCreds"]["username"].InnerText + ":" + PassXML["credentials"]["RPCCreds"]["password"].InnerText
				, "192.168.1.146", Network.Main);
			height = RPCC.GetBlockCount();

			// Iterate through every block so far since the genesis
			for(int currentBlockHeight = 0; currentBlockHeight <= height; currentBlockHeight++)
			{
				BlockHolder b = new BlockHolder(currentBlockHeight, SQC, RPCC);
			}
			Console.WriteLine(RPCC.GetBlockHash(1));
			Console.ReadLine();
		}

		int getCommandLocation(string[] args, string command)
		{
			int location = 999; //reader understands this as a command not existing in the args.

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
