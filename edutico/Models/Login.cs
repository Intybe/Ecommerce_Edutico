using edutico.Data;
using MySql.Data.MySqlClient;
using System;

namespace edutico.Models
{
    public class Login
    {
        public int codLogin { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; } 
        public int nivelAcesso { get; set; }

        // Método de verificação do login
        public bool login(string usuario, string senha)
        {
            Conexao con = new Conexao();
            MySqlConnection conexao = con.ConectarBD();
            try
            {
                string sql = "CALL spSelectTbLogin(@usuario, @senha)";

                MySqlCommand cmd = new MySqlCommand(sql, conexao);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@senha", senha);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)  // Verifica se há resultados
                    {
                        while (reader.Read())
                        {
                            // Verifica se as colunas realmente existem no resultado
                            if (!reader.IsDBNull(reader.GetOrdinal("codLogin")) && !reader.IsDBNull(reader.GetOrdinal("nivelAcesso")))
                            {
                                this.codLogin = Convert.ToInt32(reader["codLogin"]);
                                this.nivelAcesso = Convert.ToInt32(reader["nivelAcesso"]);
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Logar ou lidar com a exceção de alguma forma
                Console.WriteLine("Erro ao realizar login: " + ex.Message);
            }
            finally
            {
                con.DesconectarBD();
            }

            return false;  // Retorna falso caso não encontre um login válido
        }
    }
}
