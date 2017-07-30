using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockExplorer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBitcoin;
using NBitcoin.RPC;

namespace BlockExplorer.Tests
{
	[TestClass()]
	public class BlockSTests
	{
		[TestMethod()]
		public void toBlockSTest()
		{
			// Object for connecting to Bitcoin node
			RPCClient RPCC = new RPCClient("mattyab:N1ceMeme!", "192.168.1.146", Network.Main);
			height = RPCC.GetBlockCount();

			Block blck = RPCC.GetBlock(0);

			BlockS b = BlockS.toBlockS(blck, RPCC);
		}
	}
}