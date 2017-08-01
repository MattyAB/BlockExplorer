using BlockExplorer;
using MySql.Data.MySqlClient;
using NBitcoin;
using NBitcoin.RPC;
using System;

namespace NodeConnector
{
	internal class BlockHolder
	{
		public int height;
		public bool wasOnSQL = true;
		public uint256 hash;
		SqlConnector SQC;
		RPCClient RPCC;

		public BlockHolder(int height, SqlConnector SQC, RPCClient RPCC)
		{
			this.height = height;
			this.SQC = SQC;
			this.RPCC = RPCC;

			Block SQLBlock = SQC.GetBlock(height);
			Block RPCBlock = GetFromRPC();
			//Console.Write("Got RPC block " + height + ": ");

			hash = RPCBlock.GetHash();

			if(SQLBlock == null)
			{
				wasOnSQL = false;
				//Console.Write("Not on SQL! ");
				SQC.InsertBlock(BlockS.toBlockS(RPCBlock, RPCC));
				//Console.WriteLine("Inserted.");
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