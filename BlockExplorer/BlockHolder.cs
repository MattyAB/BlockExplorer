using BlockExplorer;
using MySql.Data.MySqlClient;
using NBitcoin;
using NBitcoin.RPC;
using System;

namespace NodeConnector
{
	internal class BlockHolder
	{
		int height;
		SqlConnector SQC;
		RPCClient RPCC;

		public BlockHolder(int height, SqlConnector SQC, RPCClient RPCC)
		{
			this.height = height;
			this.SQC = SQC;
			this.RPCC = RPCC;

			Block SQLBlock = SQC.GetBlock(height);
			Block RPCBlock = GetFromRPC();
			Console.Write("Got RPC block " + height + ": ");

			if(SQLBlock == null)
			{
				Console.Write("Not on SQL! ");
				SQC.InsertBlock(BlockS.toBlockS(RPCBlock, RPCC));
				Console.WriteLine("Inserted.");
			}
		}

		Block GetFromRPC()
		{
			uint256 hash = RPCC.GetBlockHash(height);
			Block rawBlock = RPCC.GetBlock(hash);
			return rawBlock;
		}
	}
}