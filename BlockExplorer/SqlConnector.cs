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
				connection.Open();
			}
			catch (Exception e)
			{
				Console.WriteLine("Error opening MySQL connection: " + e);
			}
			return connection;
		}

		internal void InsertBlock(BlockS b)
		{
			string query = "INSERT INTO block (" +
				"height, hash, version, merkleroot, time, nonce, bits, " +
				"size, strippedsize, weight, chainwork, nextblockhash, versionhex" +
				") VALUES('" +
				b.height + "', " +
				"unhex('" + b.hash + "'), '" +
				b.version + "', " +
				"unhex('" + b.merkleroot + "'), '" +
				b.time + "', '" +
				b.nonce + "', " +
				"unhex('" + b.bits + "'), '" +
				b.size + "', '" +
				b.strippedsize + "', '" +
				b.weight + "', " +
				"unhex('" + b.chainwork + "'), " +
				"unhex('" + b.nextblockhash + "'), " +
				"unhex('" + b.versionhex + "'));";

				//Create Command
				MySqlCommand cmd = new MySqlCommand(query, connection);
				//Execute the command

				 cmd.ExecuteReader();
		}

		public Block GetBlock(int height)
		{
			string query = "SELECT * FROM blockchain.block WHERE height = " + height + ";";

			//Create Command
			MySqlCommand cmd = new MySqlCommand(query, connection);
			//Create a data reader and Execute the command

			MySqlDataReader dataReader = cmd.ExecuteReader();

			//Read the data and store them in the Block
			Block block = new Block();
			while (dataReader.Read())
			{
				BlockS b = new BlockS();
				
				b.height = Convert.ToInt32(dataReader.GetString("height"));
				Console.WriteLine(ToHex(dataReader.GetString("hash")));
				Console.WriteLine(dataReader.GetString("hash").Length);
				var v = ToHex(dataReader.GetString("hash"));
				b.hash = new uint256(ToHex(dataReader.GetString("hash")));
				b.version = Convert.ToInt32(dataReader.GetString("version"));
				b.merkleroot = new uint256(ToHex(dataReader.GetString("merkleroot")));
				b.time = Convert.ToUInt32(dataReader.GetString("time"));
				b.nonce = Convert.ToUInt32(dataReader.GetString("nonce"));
				b.bits = Convert.ToUInt32(ToHex(dataReader.GetString("bits")));
				b.size = Convert.ToUInt32(dataReader.GetString("size"));
				b.strippedsize = Convert.ToUInt32(dataReader.GetString("strippedsize"));
				b.weight = Convert.ToUInt32(dataReader.GetString("weight"));
				b.chainwork = new uint256(ToHex(dataReader.GetString("chainwork")));
				b.nextblockhash = new uint256(ToHex(dataReader.GetString("nextblockhash")));
				b.versionhex = Convert.ToUInt32(ToHex(dataReader.GetString("versionhex")));
			}

			dataReader.Close();

			return null;
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