using edutico.Data;
using MySql.Data.MySqlClient;
using edutico.Models;

namespace edutico.Models
{
    public class Cliente : Login
    {
        public int codCli { get; set; }
        public decimal cpf { get; set; }
        public string nome { get; set; }
        public string sobrenome { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public int numEnd { get; set; }
        public string compEnd { get; set; }
    }
}
