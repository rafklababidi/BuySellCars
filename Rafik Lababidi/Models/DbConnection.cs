using System.Data.SqlClient;

namespace Rafik_Lababidi.Models
{
    public class DbConnection
    {
        SqlConnection con = new SqlConnection("Server = .; DataBase = rafikLababidi; Integrated Security = True");
        SqlCommand cmd;

        public void openConnection()
        {
            if (con.State != System.Data.ConnectionState.Open && con != null)
                con.Open();
        }

        public void closeConnection()
        {
            if (con.State != System.Data.ConnectionState.Closed && con != null)
                con.Close();
        }

        public bool executeNonQuery(string query)
        {
            try
            {
                openConnection();
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                closeConnection();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string executeScalar(string query)
        {
            try
            {
                openConnection();
                cmd = new SqlCommand(query, con);
                object result = cmd.ExecuteScalar();
                closeConnection();

                string data = "";
                if (result != null)
                    data = (string)result;
                return data;
            }
            catch
            {
                return "Error";
            }
        }

        public bool executeBool(string query)
        {
            try
            {
                openConnection();
                cmd = new SqlCommand(query, con);
                object result = cmd.ExecuteScalar();
                closeConnection();

                return Convert.ToBoolean(result);
            }
            catch
            {
                return false;
            }
        }

        public SqlDataReader executeDataReader(string query)
        {
            try
            {
                openConnection();
                cmd = new SqlCommand(query, con);
                SqlDataReader dr = cmd.ExecuteReader();
                closeConnection();

                return dr;
            }
            catch
            {
                return null;
            }
        }
    }
}
