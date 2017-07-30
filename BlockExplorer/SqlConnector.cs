using BlockExplorer;
using MySql.Data.MySqlClient;
using NBitcoin;
using System;
using System.Text;

namespace NodeConnector
{
	class SqlConnector
	{
		MySqlConnection connection;

		public SqlConnector(string server, string database, string uid, string password)
		{
			connection = connect(server, database, uid, password);
		}

		MySqlConnection connect(string server, string database, string uid, string password)
		{
			string connectionString;
			connectionString = "SERVER=" + server + ";" + "DATABASE=" +
			database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

			MySqlConnection connection = new MySqlConnection(connectionString);
			try
			{
				//connection.Open();
			}
			catch (Exception e)
			{
				Console.WriteLine("Error opening MySQL connection: " + e);
			}
			return connection;
		}

		internal void InsertBlock(BlockS b)
		{
			connection.Open();

			string query = "INSERT INTO block (" +
				"height, hash, version, merkleroot, time, nonce, bits, " +
				"size, strippedsize, weight, chainwork, nextblockhash, versionhex" +
				") VALUES('" +
				b.height + "', " +
				"'" + b.hash + "', '" +
				b.version + "', " +
				"'" + b.merkleroot + "', '" +
				b.time + "', '" +
				b.nonce + "', " +
				"'" + String.Format("{0:x}", b.bits) + "', '" +
				b.size + "', '" +
				b.strippedsize + "', '" +
				b.weight + "', " +
				"'" + b.chainwork + "', " +
				"'" + b.nextblockhash + "', " +
				"'" + String.Format("{0:x}", b.versionhex) + "');";

			//Create Command
			MySqlCommand cmd = new MySqlCommand(query, connection);
			//Execute the command

			cmd.ExecuteNonQuery();

			connection.Close();
		}

		public Block GetBlock(int height)
		{
			string query = "SELECT * FROM blockchain.block WHERE height = " + height + ";";

			//Create Command
			MySqlCommand cmd = new MySqlCommand(query, connection);
			//Create a data reader and Execute the command

			//Create default block
			Block block = default(Block);

			connection.Open();

			using (MySqlDataReader dataReader = cmd.ExecuteReader())
			{

				//Read the data and store them in the Block
				while (dataReader.Read())
				{
					block = new Block();
					BlockS b = new BlockS();

					b.height = Convert.ToInt32(dataReader.GetString("height"));
					b.hash = new uint256(dataReader.GetString("hash"));
					b.version = Convert.ToInt32(dataReader.GetString("version"));
					b.merkleroot = new uint256(dataReader.GetString("merkleroot"));
					b.time = Convert.ToUInt32(dataReader.GetString("time"));
					b.nonce = Convert.ToUInt32(dataReader.GetString("nonce"));
					b.bits = uint.Parse(dataReader.GetString("bits"), System.Globalization.NumberStyles.HexNumber);
					b.size = Convert.ToUInt32(dataReader.GetString("size"));
					b.strippedsize = Convert.ToUInt32(dataReader.GetString("strippedsize"));
					b.weight = Convert.ToUInt32(dataReader.GetString("weight"));
					b.chainwork = new uint256(dataReader.GetString("chainwork"));
					b.nextblockhash = new uint256(dataReader.GetString("nextblockhash"));
					b.versionhex = uint.Parse(dataReader.GetString("versionhex"), System.Globalization.NumberStyles.HexNumber);

					block = b.toBlock();
				}
			}

			connection.Close();

			if (block == default(Block))
				return null;
			else
				return block;
		}

		private String ToHex(String i_asciiText)
		{
			StringBuilder sBuffer = new StringBuilder();
			for (int i = 0; i < i_asciiText.Length; i++)
			{
				sBuffer.Append(Convert.ToInt32(i_asciiText[i]).ToString("x"));
			}
			return sBuffer.ToString().ToUpper();
		}
	}
}