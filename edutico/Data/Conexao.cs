using MySql.Data.MySqlClient;

namespace edutico.Data
{
    public class Conexao
    {
        private MySqlConnection con = new MySqlConnection("Server=localhost;Database=dbEdutico;User ID=root;Password=12345678;");
        public static string msg;

        public MySqlConnection ConectarBD()
        {
            try
            {
                con.Open();
            }
            catch (Exception erro)
            {
                msg = "Ocorreu um erro ao se conectar: " + erro.Message;
            }
            return con;
        }

        public void DesconectarBD()
        {
            try
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }
            }
            catch (Exception erro)
            {
                msg = "Ocorreu um erro ao se desconectar: " + erro.Message;
            }
        }
    }
}
