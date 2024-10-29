using edutico.Data;
using MySql.Data.MySqlClient;
using edutico.Models;
using System.ComponentModel.DataAnnotations;

namespace edutico.Models
{
    public class Cliente : Login
    {

        public int codCli { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório")]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$", ErrorMessage = "O CPF deve estar no formato correto.")]
        public decimal cpf { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [MaxLength(200)]
        public string nome { get; set; }

        [Required(ErrorMessage = "O sombrenome é obrigatório")]
        [MaxLength(200)]
        public string sobrenome { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório")]
        [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}-?\d{4}$", ErrorMessage = "O número de telefone não é válido")]
        public string telefone { get; set; }

        [Required(ErrorMessage = "O email é obrigatório")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "O email não é válido")]
        [MaxLength(200)]
        public string email { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório")]
        [MaxLength(200)]
        public string logradouro { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório")]
        [MaxLength(200)]
        public string bairro { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatório")]
        [MaxLength(200)]
        public string cidade { get; set; }

        [Required(ErrorMessage = "O UF é obrigatório")]
        [MaxLength(2)]
        public string uf { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        [RegularExpression(@"^\d{5}-\d{3}$|^\d{8}$", ErrorMessage = "O CEP não é válido")]
        public string cep { get; set; }

        public int numEnd { get; set; }
        public string compEnd { get; set; }
    }
}
