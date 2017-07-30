using NBitcoin;
using NBitcoin.RPC;
using System;

namespace BlockExplorer
{
	public class BlockS
	{
		public int height;
		public uint256 hash;
		public int version;
		public uint256 merkleroot;
		public uint time;
		public uint nonce;
		public UInt32 bits;
		public uint size;
		public uint strippedsize;
		public uint weight;
		public uint256 chainwork;
		public uint256 nextblockhash;
		public UInt32 versionhex;

		public static BlockS toBlockS(Block input, RPCClient RPCC)
		{
			BlockS b = new BlockS();

			RPCResponse RawBlockResp = RPCC.SendCommand("getblock", input.Header.GetHash().ToString());

			b.height = Convert.ToInt32(RawBlockResp.Result["height"]);
			b.hash = input.Header.GetHash();
			b.version = input.Header.Version;
			b.merkleroot = input.Header.HashMerkleRoot;
			b.time = Utils.DateTimeToUnixTime(input.Header.BlockTime);
			b.nonce = input.Header.Nonce;
			b.bits = input.Header.Bits;
			b.size = Convert.ToUInt32(RawBlockResp.Result["size"]);
			b.strippedsize = Convert.ToUInt32(RawBlockResp.Result["strippedsize"]);
			b.weight = Convert.ToUInt32(RawBlockResp.Result["weight"]);
			b.chainwork = new uint256(Convert.ToString(RawBlockResp.Result["chainwork"]));
			b.nextblockhash = new uint256(Convert.ToString(RawBlockResp.Result["nextblockhash"]));
			b.versionhex = Convert.ToUInt32(input.Header.Version);

			return b;
		}
	}
}