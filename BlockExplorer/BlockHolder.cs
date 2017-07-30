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

			if(SQLBlock == null)
			{
				SQC.InsertBlock(RPCBlock, height);
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