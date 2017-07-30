using BlockExplorer;
using MySql.Data.MySqlClient;
using NBitcoin;
using System;

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

		}

		public Block GetBlock(int height)
		{
			string query = "SELECT * FROM block WHERE height = " + height + ";";

			//Create Command
			MySqlCommand cmd = new MySqlCommand(query, connection);
			//Create a data reader and Execute the command

			MySqlDataReader dataReader = cmd.ExecuteReader();

			//Read the data and store them in the Block
			Block block = new Block();
// if (dataReader.Depth > 1) TODO: Break if there is more than 1 block with the same height.
			while (dataReader.Read())
			{
				BlockHeader b = new BlockHeader();
				b.Bits = new Target(new uint256(dataReader["bits"].ToString()));
			}

			dataReader.Close();

			return null;
		}
	}
}