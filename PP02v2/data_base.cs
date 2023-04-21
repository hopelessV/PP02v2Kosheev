using System.Collections.Generic;
using System.Data.SqlClient;

namespace PP02v2
{
    internal class data_base
    {
        public static string MyCon = $@"Data Source=DESKTOP-E3QKFVD\HOPELESS;Initial Catalog=PP02v2;Integrated Security=True";
        SqlConnection con = new SqlConnection(MyCon);

        public void OpenCon() { if(con.State == System.Data.ConnectionState.Closed) con.Open(); }
        public void CloseCon() { if(con.State == System.Data.ConnectionState.Open) con.Close(); }
        public SqlConnection GetCon() { return con; }

        public static string[] ExitColumn(string table, string column)
        {
            List<string> columns = new List<string>();

            SqlConnection sqlConnection = new SqlConnection(MyCon);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"select {column} as 'n' from {table}";
            SqlDataReader sqlDataReaderreader = sqlCommand.ExecuteReader();
            while (sqlDataReaderreader.Read())
            {
                columns.Add("" + sqlDataReaderreader["n"]);
            }

            string[] arr = new string[columns.Count];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = columns[i];
            }
            return arr;
        }
    }
}
