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
		static MySqlConnection MSC;
		static int height;

		static void Main(string[] args)
		{
			XmlDocument PassXML = new XmlDocument();
			PassXML.Load("PassStore.xml");

			// Object for connecting to SQL database
			SqlConnector SQC = new SqlConnector("192.168.1.146", "blockchain", PassXML["credentials"]["SQLCreds"]["username"].ToString(), PassXML["credentials"]["SQLCreds"]["password"].ToString());

			// Object for connecting to Bitcoin node
			RPCClient RPCC = new RPCClient(
				PassXML["credentials"]["SQLCreds"]["username"].ToString() + ":" + PassXML["credentials"]["SQLCreds"]["password"].ToString()
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
	}
}
